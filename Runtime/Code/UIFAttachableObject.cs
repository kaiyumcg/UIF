using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UIF
{
    public abstract class UIFAttachableObject : MonoBehaviour, ILoadReference, IBuiltinEditorSetting, IBuiltInAPICall, IActivation, ITagable
    {
        #region BaseFramework Fields and Methods
        [SerializeField] protected List<UITag> tags = null;
        [SerializeField] protected UIFStateInfo stateInfo;
        [SerializeField, HideInInspector] protected Transform _transform;
        [SerializeField, HideInInspector] protected GameObject _gameObject;
        [HideInInspector] protected Vector3 initPosition;
        [HideInInspector] protected Vector3 initLocalPosition;
        [HideInInspector] protected Quaternion initRotation;
        [HideInInspector] protected Quaternion initLocalRotation;
        [HideInInspector] protected Vector3 initAngle;
        [HideInInspector] protected Vector3 initLocalAngle;
        [HideInInspector] protected Vector3 initScale;
        [HideInInspector] protected Vector3 initLocalScale;
        public UIFStateInfo StateInfo { get { return stateInfo; } }
        public Transform _Transform { get { return _transform; } }
        public GameObject _GameObject { get { return _gameObject; } }
        public Vector3 InitPosition { get { return initPosition; } }
        public Vector3 InitLocalPosition { get { return initLocalPosition; } }
        public Quaternion InitRotation { get { return initRotation; } }
        public Quaternion InitLocalRotation { get { return initLocalRotation; } }
        public Vector3 InitAngle { get { return initAngle; } }
        public Vector3 InitLocalAngle { get { return initLocalAngle; } }
        public Vector3 InitScale { get { return initScale; } }
        public Vector3 InitLocalScale { get { return initLocalScale; } }
        protected virtual string OverrideUniqueName() { return ""; }

        [SerializeField, HideInInspector] internal bool validateEveryInspectorFrame = false;
        [SerializeField, HideInInspector] internal string configErrorMessage = "";
        [SerializeField, HideInInspector] internal bool configError = false;
        bool IBuiltinEditorSetting.ValidateEveryInspectorFrame { get => validateEveryInspectorFrame; set => validateEveryInspectorFrame = value; }
        bool IBuiltinEditorSetting.ConfigError { get => configError; set => configError = value; }
        string IBuiltinEditorSetting.ConfigErrorMessage { get => configErrorMessage; set => configErrorMessage = value; }

        void ILoadReference.LoadReference()
        {
            OnLoadReference();
            _transform = transform;
            _gameObject = gameObject;
        }

        protected virtual void OnLoadReference() { }

        [SerializeField, HideInInspector] public UnityEvent onInit, onCleanup;
        void ILoadReference.InitIfRequired()
        {
            if (stateInfo == null)
            {
                stateInfo = new UIFStateInfo();
            }

            if (stateInfo != null)
            {
                stateInfo.Init(_transform, OverrideUniqueName());
            }

            initPosition = _transform.position;
            initLocalPosition = _transform.localPosition;
            initRotation = _transform.rotation;
            initLocalRotation = _transform.localRotation;
            initAngle = _transform.eulerAngles;
            initLocalAngle = _transform.localEulerAngles;
            initScale = _transform.lossyScale;
            initLocalScale = _transform.localScale;

            if (_gameObject.activeInHierarchy == false)
            {
                _gameObject.SetActive(true);
            }

            onInit?.Invoke();
            OnInitBaseFW();
        }

        internal virtual void OnInitBaseFW() { }
        internal void Cleanup()
        {
            OnCleanup();
            onCleanup?.Invoke();
            if (_gameObject.activeInHierarchy == false) { _gameObject.SetActive(true); }
        }
        protected virtual void OnCleanup() { }

        List<UnityEngine.ScriptableObject> ITagable.Tags
        {
            get
            {
                var lst = new List<UnityEngine.ScriptableObject>();
                lst.AddRange(tags);
                return lst;
            }
        }

        [SerializeField, HideInInspector] public UnityEvent onActivateUI, onDeactivateUI;
        protected virtual void OnActivate() { onActivateUI?.Invoke(); }
        protected virtual void OnDeactivate() { onDeactivateUI?.Invoke(); }
        protected virtual void OnSetActive(bool active) { }
        public void SetActiveUI(bool activate)
        {
            ThrowIfInvalidConfig("Invalid configuration of UIF object. " +
                "Check inspector of affected gameobject: " + gameObject.name);
            OnSetActive(activate);
            if (activate) { OnActivate(); }
            else { OnDeactivate(); }
        }

        internal void ThrowIfInvalidConfig(string message)
        {
            if (UIFSetting.safetyCheck && configError)
            {
                var ex = new System.InvalidOperationException(message);
                ULog.PrintException(ex);
                ULog.PrintError(message);
                throw ex;
            }
        }
        #endregion
    }
}