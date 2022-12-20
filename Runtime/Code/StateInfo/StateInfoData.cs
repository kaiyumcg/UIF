using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;
using AttributeExt2;

namespace UIF
{
    public enum UIFState { Normal = 0, Locked = 1, Hidden = 2, Selected = 3 }
    public delegate void OnStateChangeFunc(UIFState newState, UIFState oldState);
    public delegate void OnNameStateChangeFunc(string key, string newValue, string oldValue);

    [System.Serializable]
    public class StringState
    {
        [SerializeField, ReadOnly] public string key;
        [SerializeField, ReadOnly] public string value;
    }

    [System.Serializable]
    public class UIFStateInfoInternal
    {
        [SerializeField, ReadOnly] public string uniqueKey;
        [SerializeField, ReadOnly] public UIFState state = UIFState.Hidden;
        [SerializeField, ReadOnly] public StringState[] nameInfo;
    }

    [System.Serializable]
    public class UIFMasterSave
    {
        [SerializeField, ReadOnly] public UIFStateInfoInternal[] data;
    }

    [System.Serializable]
    public class UIFStateInfo
    {
        [SerializeField] UIFState defaultState = UIFState.Locked;
        [SerializeField] TransformUniqueNameMode keyingMode = TransformUniqueNameMode.TransformTreeForFixed;
        [SerializeField] bool deviceSave = false;
        [SerializeField] string uniqueName = "";
        [SerializeField] UIFStateRenderDesc stateChangeVisual;
        [SerializeField] public UnityEvent<UIFState, UIFState> onStateChange;
        [SerializeField] public UnityEvent<string, string, string> onNameStateChange;
        [SerializeField, ReadOnly] UIFStateInfoInternal info = null;
        [SerializeField, ReadOnly] string stateAppliedUniqueKey = "";
        public event OnStateChangeFunc OnStateChange;
        public event OnNameStateChangeFunc OnNameStateChange;
        bool prepared = false;

        internal UIFStateRenderDesc StateChangeVisual { get { return stateChangeVisual; } }
        internal void Init(Transform transform, string overrideableUniqueName)
        {
            stateChangeVisual.Init();
            this.stateAppliedUniqueKey = transform.GetUniqueNameForTransform(keyingMode, uniqueName, overrideableUniqueName);
            var saveData = UIFSaveController.SaveData;
            if (saveData == null)
            {
                var msg = "UIF data read failure!";
                var ex = new System.InvalidOperationException(msg);
                ULog.PrintException(ex);
                ULog.PrintError(msg);
                throw ex;
            }

            info = null;
            if (saveData.data != null && saveData.data.Length > 0)
            {
                for (int i = 0; i < saveData.data.Length; i++)
                {
                    var s = saveData.data[i];
                    if (s == null) { continue; }
                    if(s.uniqueKey == stateAppliedUniqueKey)
                    {
                        info = s;
                        break;
                    }
                }
            }

            if (info == null)
            {
                info = new UIFStateInfoInternal
                {
                    uniqueKey = stateAppliedUniqueKey,
                    nameInfo = new StringState[0],
                    state = defaultState
                };

                if (deviceSave)
                {
                    UIFSaveController.OnModifyInternalData(stateAppliedUniqueKey, info);
                }
            }

            var nms = info.nameInfo;
            if (nms != null && nms.Length > 0)
            {
                for (int i = 0; i < nms.Length; i++)
                {
                    var n = nms[i];
                    if (n.key.IsStringValid() == false) { continue; }
                    var vl = n.value;
                    onNameStateChange?.Invoke(n.key, vl, vl);
                    OnNameStateChange?.Invoke(n.key, vl, vl);
                }
            }

            var st = info.state;
            stateChangeVisual.ApplyChangeOnVisual(st);
            onStateChange?.Invoke(st, st);
            OnStateChange?.Invoke(st, st);
            if (deviceSave)
            {
                UIFSaveController.OnModifyInternalData(stateAppliedUniqueKey, info);
            }
            prepared = true;
        }
        public UIFStateInfo() { prepared = false; }

        void CheckSys()
        {
            if (UIFSetting.safetyCheck == false) { return; }
            if (prepared == false)
            {
                throw new System.Exception("UIF StateInfo Data is not prepared. First call 'Init' to use it subsequently.");
            }

            if (info == null || info.nameInfo == null)
            {
                var msg = "Internal state data is invalid. " +
                    "This can not happen since the data is supposed to exist at class construction. ";
                var ex = new System.InvalidOperationException(msg);
                ULog.PrintException(ex);
                ULog.PrintError(msg);
                throw ex;
            }
        }

        void CheckKey(string keyName)
        {
            if (UIFSetting.safetyCheck == false) { return; }
            if (keyName.IsStringValid() == false)
            {
                var msg = "Given key is null or empty or whitespace. This is not allowed.";
                var ex = new System.InvalidOperationException(msg);
                ULog.PrintException(ex);
                ULog.PrintError(msg);
                throw ex;
            }
        }

        bool DoesKeyExist(string keyName)
        {
            var keyExist = false;
            for (int i = 0; i < info.nameInfo.Length; i++)
            {
                var info = this.info.nameInfo[i];
                if (info.key == keyName)
                {
                    keyExist = true;
                    break;
                }
            }
            return keyExist;
        }

        public bool IsNameNew(string keyName, string updatedValue)
        {
            CheckSys();
            CheckKey(keyName);
            if (info.nameInfo.Length == 0) { return true; }
            if (DoesKeyExist(keyName) == false) { return true; }
            else
            {
                var hasChange = true;
                for (int i = 0; i < info.nameInfo.Length; i++)
                {
                    var st = info.nameInfo[i];
                    if (st == null) { continue; }
                    if (st.key == keyName && st.value == updatedValue)
                    {
                        hasChange = false;
                        break;
                    }
                }
                return hasChange;
            }
        }

        public void ClearNames()
        {
            CheckSys();
            info.nameInfo = new StringState[0];
            if (deviceSave)
            {
                UIFSaveController.OnModifyInternalData(stateAppliedUniqueKey, info);
            }
        }

        public UIFState State
        {
            get { CheckSys(); return info.state; }
            set
            {
                CheckSys();
                var fireChangeEvents = false;
                var oldState = info.state;
                if (info.state != value)
                {
                    fireChangeEvents = true;

                }
                info.state = value;
                if (fireChangeEvents)
                {
                    stateChangeVisual.ApplyChangeOnVisual(value);
                    onStateChange?.Invoke(value, oldState);
                    OnStateChange?.Invoke(value, oldState);
                    if (deviceSave)
                    {
                        UIFSaveController.OnModifyInternalData(stateAppliedUniqueKey, info);
                    }
                }
            }
        }

        public string GetName(string key)
        {
            var result = "";
            CheckSys();
            CheckKey(key);
            if (info.nameInfo.Length == 0) { return result; }
            for (int i = 0; i < info.nameInfo.Length; i++)
            {
                var st = info.nameInfo[i];
                if (st.key == key)
                {
                    result = st.value;
                    break;
                }
            }
            return result;
        }

        public void SetName(string key, string value)
        {
            CheckSys();
            CheckKey(key);
            string oldValue = "";
            bool fireChangeEvents = false;
            if (IsNameNew(key, value))
            {
                oldValue = GetName(key);
                fireChangeEvents = true;
            }
            if (DoesKeyExist(key))
            {
                for (int i = 0; i < info.nameInfo.Length; i++)
                {
                    var st = info.nameInfo[i];
                    if (st.key == key)
                    {
                        info.nameInfo[i].value = value;
                        break;
                    }
                }
            }
            else
            {
                var item = new StringState { key = key, value = value };
                var curNum = info.nameInfo.Length;
                var newLen = curNum + 1;
                var newData = new StringState[newLen];
                for (int i = 0; i < newLen; i++)
                {
                    if (i == newLen - 1)
                    {
                        newData[i] = item;
                    }
                    else
                    {
                        newData[i] = info.nameInfo[i];
                    }
                }
                info.nameInfo = newData;
            }

            if (fireChangeEvents)
            {
                onNameStateChange?.Invoke(key, value, oldValue);
                OnNameStateChange?.Invoke(key, value, oldValue);

                if (deviceSave)
                {
                    UIFSaveController.OnModifyInternalData(stateAppliedUniqueKey, info);
                }
            }
        }
    }
}