using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UIF
{
    [DisallowMultipleComponent]
    [AddComponentMenu("UIF/UI Page")]
    public class UIPage : UIFAttachableObject
    {
        [SerializeField, HideInInspector] protected string pageName = "";
        [SerializeField, HideInInspector, CannotEdit] internal protected List<UIPageElement> elements = null;
        [SerializeField, HideInInspector, CannotEdit] internal protected List<UIPage> childPages = null;

        bool opened = false;
        UIPage ownerPage = null;
        public bool IsOpened { get { return opened; } }
        public string PageName { get { return pageName; } }
        public UIPage OwnerPage { get { return ownerPage; } }
        
        protected override void OnLoadReference()
        {
            this.GetOrUpdateChilds<UIPage>(ref childPages, ref configError, ref configErrorMessage);
            if (childPages != null && childPages.Count > 0)
            {
                for (int i = 0; i < childPages.Count; i++)
                {
                    var child = childPages[i];
                    if (child == null) { continue; }
                    child.ownerPage = this;
                }
            }
            if (configError == false)
            {
                this.GetOrUpdateChilds<UIPageElement, UIPage>(false, false, false, false, ref elements, ref configError, ref configErrorMessage);
            }
        }

        internal override void OnInitBaseFW()
        {
            base.OnInitBaseFW();
            for (int i = 0; i < elements.Count; i++)
            {
                var elem = elements[i];
                elem.OnInitElement(this);
            }
        }

        protected internal virtual void OnInitPage(UIBrowser browser)
        {
            var ld = (ILoadReference)this;
            if (ld != null) { ld.LoadReference(); }
            if (ld != null) { ld.InitIfRequired(); }
            opened = false;
            _transform.localScale = Vector3.zero;
        }
        protected internal virtual void OnTick() { }
        protected override void OnCleanup()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                var elem = elements[i];
                elem.Cleanup();
            }
            base.OnCleanup();
        }

        protected override void OnSetActive(bool active)
        {
            base.OnSetActive(active);
            for (int i = 0; i < elements.Count; i++)
            {
                var elem = elements[i];
                elem.SetActiveUI(active);
            }
        }

        [SerializeField, HideInInspector] public UnityEvent onOpen, onClose;
        [SerializeField, HideInInspector] internal UnityEvent _onOpen, _onClose;
        protected internal virtual void OnOpen() { onOpen?.Invoke(); _onOpen?.Invoke(); }
        protected internal virtual void OnClose() { onClose?.Invoke(); _onClose?.Invoke(); }
        public void Open()
        {
            ThrowIfInvalidConfig("Invalid configuration of UIPage. Can not open. " +
               "Check inspector of affected gameobject: " + gameObject.name);
            Cleanup();
            opened = true;
            _transform.localScale = initLocalScale;
            for (int k = 0; k < childPages.Count; k++)
            {
                var ch = childPages[k];
                ch.Open();
            }
            OnOpen();
        }

        public void Close()
        {
            ThrowIfInvalidConfig("Invalid configuration of UIPage. Can not close. " +
               "Check inspector of affected gameobject: " + gameObject.name);
            Cleanup();
            opened = false;
            for (int k = 0; k < childPages.Count; k++)
            {
                var ch = childPages[k];
                ch.Close();
            }
            _transform.localScale = Vector3.zero;
            OnClose();
        }
    }
}