using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace UIF
{
    [AddComponentMenu("UIF/Page UI Element Feature")]
    public class UIPageElementFeature : MonoBehaviour
    {
        [SerializeField]
        bool waitForPageOpen = false, waitForPageClose = false,
            waitForElementAppear = false, waitForElementHide = false;
        [SerializeField] List<UITag> tags = null;
        [SerializeField, HideInInspector] internal bool editDefaultSetting = false, eventEdit = false;
        [SerializeField] public UnityEvent onInit, onOwnerPageOpen, onOwnerPageClose, 
            onOwnerElementAppear, onOwnerElementHide, onActivate, onDeactivate;
        Transform _transform;
        GameObject _gameObject;
        UIPage ownerPage;
        UIPageElement ownerElement;
        Vector3 initLocalScale;
        
        internal List<UITag> Tags { get { return tags; } }

        protected internal virtual void OnInit(UIPage owner, UIPageElement element)
        {
            this.ownerPage = owner;
            this.ownerElement = element;
            _transform = transform;
            _gameObject = gameObject;
            initLocalScale = _transform.localScale;
            onInit?.Invoke();
        }
        protected internal virtual void OnOpenOwnerPage() { onOwnerPageOpen?.Invoke(); }
        protected internal virtual IEnumerator OnOpenOwnerPageAsync() { yield return null; }
        protected internal virtual void OnAppearOwnerElement() { onOwnerElementAppear?.Invoke(); }
        protected internal virtual IEnumerator OnAppearOwnerElementAsync() { yield return null; }

        protected internal virtual void OnCloseOwnerPage() { onOwnerPageClose?.Invoke(); }
        protected internal virtual IEnumerator OnCloseOwnerPageAsync() { yield return null; }
        protected internal virtual void OnHideOwnerElement() { onOwnerElementHide?.Invoke(); }
        protected internal virtual IEnumerator OnHideOwnerElementAsync() { yield return null; }
        protected virtual void OnActivate() { onActivate?.Invoke(); }
        protected virtual void OnDeactivate() { onDeactivate?.Invoke(); }
        
        public UIPage OwnerPage { get { return ownerPage; } }
        public UIPageElement OwnerElement { get { return ownerElement; } }
        public Transform _Transform { get { return _transform; } }
        public GameObject _GameObject { get { return _gameObject; } }
        public Vector3 InitLocalScale { get { return initLocalScale; } }
        protected internal bool WaitForPageOpen { get { return waitForPageOpen; } }
        protected internal bool WaitForPageClose { get { return waitForPageClose; } }
        protected internal bool WaitForElementAppear { get { return waitForElementAppear; } }
        protected internal bool WaitForElementHide { get { return waitForElementHide; } }

        public void SetActiveUI(bool activate)
        {
            if (activate) { OnActivate(); }
            else { OnDeactivate(); }
        }

        internal void Cleanup()
        {
            StopAllCoroutines();
        }
    }
}