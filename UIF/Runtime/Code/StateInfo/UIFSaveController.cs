using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

namespace UIF
{
    public delegate void OnDoAnythingFunc();
    public class UIFSaveController : MonoBehaviour
    {
        static UIFMasterSave saveData = null;
        static UIFSaveController instance = null;
        static bool writeOnEveryModification = false;
        bool registered = false;
        bool writeOnSceneLoad = true;

        public static void SetWriteOnModification(bool set)
        {
            writeOnEveryModification = set;
        }

        void Awake()
        {
            TryUpdateReferenceOfModule();
        }

        void TryUpdateReferenceOfModule()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnLoadNewScene;
                registered = true;
                writeOnSceneLoad = true;
            }
            else if (instance != this)
            {
                DestroyImmediate(this);
            }
        }

        void OnDestroy()
        {
            if (registered)
            {
                SceneManager.sceneLoaded -= OnLoadNewScene;
            }
        }

        void OnLoadNewScene(Scene scene, LoadSceneMode loadMode)
        {
            if (writeOnSceneLoad == false || scene.name == UIFSetting.BootSceneName) { return; }
            StopAllCoroutines();
            StartCoroutine(WriteDataCOR());
            IEnumerator WriteDataCOR()
            {
                yield return null;
                CreateModuleIfReq();
                WriteDataToDevice();
            }
        }

        static void CreateModuleIfReq()
        {
            if (instance == null)
            {
                var g = new GameObject(UIFSetting.UIFFileName + "_obj");
                var clonedInstance = g.AddComponent<UIFSaveController>();
                clonedInstance.TryUpdateReferenceOfModule();
            }
        }

        internal static UIFMasterSave SaveData 
        { 
            get 
            {
                CreateModuleIfReq();
                if (saveData == null) { LoadDataFromDevice(); }
                return saveData; 
            } 
        }

        internal static void OnModifyInternalData(string uniqueKey, UIFStateInfoInternal data)
        {
            CreateModuleIfReq();
            var foundAndUpdated = false;
            if (saveData != null && saveData.data != null && saveData.data.Length > 0)
            {
                for(int i = 0; i < saveData.data.Length;i++)
                {
                    var s = saveData.data[i];
                    if (s == null) { continue; }
                    if (s.uniqueKey == uniqueKey)
                    {
                        saveData.data[i] = data;
                        foundAndUpdated = true;
                        break;
                    }
                }
            }

            if(foundAndUpdated == false)
            {
                var lst = new List<UIFStateInfoInternal>();
                lst.AddRange(saveData.data);
                lst.Add(data);
                saveData.data = lst.ToArray();
            }

            if (writeOnEveryModification)
            {
                WriteDataToDevice();
            }
        }

        public static void LoadDataFromDevice()
        {
            try
            {
                CreateModuleIfReq();
                var fName = UIFSetting.UIFFileName;

                var fPath = Path.Combine(Application.persistentDataPath, fName + UIFSetting.UIFFileExtension);
                if (File.Exists(fPath))
                {
                    var content = File.ReadAllText(fPath);
                    if (UIFSetting.encryptContent)
                    {
                        content = Crypto.DecryptStringAES(content, UIFSetting.encoding, UIFSetting.sharedSecret, UIFSetting.salt);
                    }
                    saveData = JsonUtility.FromJson<UIFMasterSave>(content);
                    if (saveData == null)
                    {
                        saveData = new UIFMasterSave { data = new UIFStateInfoInternal[0] };
                    }
                }
                else
                {
                    saveData = new UIFMasterSave { data = new UIFStateInfoInternal[0] };
                }
                OnLoadFromDevice?.Invoke();
            }
            catch (Exception ex)
            {
                var msg = "Could not read UIF save data from device. Error: " + ex.Message;
                ULog.PrintError(msg);
            }
        }

        public static void WriteDataToDevice()
        {
            try
            {
                CreateModuleIfReq();
                if (saveData == null)
                {
                    var msg = "No valid UIF save data in the memory, yet the system is trying to write into device. " +
                        "Are you sure you have loaded from device in the first place? " +
                        "You must call 'UIFSaveController.LoadDataFromDevice()' before writing or actually doing any operation on UIF save data!!";
                    var ex = new System.InvalidOperationException(msg);
                    ULog.PrintException(ex);
                    ULog.PrintError(msg);
                    throw ex;
                }

                var toJson = JsonUtility.ToJson(saveData);
                var fName = UIFSetting.UIFFileName;

                var fPath = Path.Combine(Application.persistentDataPath, fName + UIFSetting.UIFFileExtension);
                var content = toJson;
                if (UIFSetting.encryptContent)
                {
                    content = Crypto.EncryptStringAES(content, UIFSetting.encoding, UIFSetting.sharedSecret, UIFSetting.salt);
                }
                File.WriteAllText(fPath, content);
                OnWriteToDevice?.Invoke();
            }
            catch (System.Exception ex)
            {
                var msg = "Could not write UIF save data to device. Error: " + ex.Message;
                ULog.PrintError(msg);
            }
        }

        public static event OnDoAnythingFunc OnWriteToDevice, OnLoadFromDevice;

        public static void SetDataWritingOnLevelLoad(bool write)
        {
            CreateModuleIfReq();
            instance.writeOnSceneLoad = write;
        }
    }
}