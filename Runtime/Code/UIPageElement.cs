using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using AttributeExt;

namespace UIF
{
    [DisallowMultipleComponent]
    [AddComponentMenu("UIF/Page UI Element")]
    public class UIPageElement : UIFAttachableObject
    {
        [SerializeField, HideInInspector] protected string elementName = "";
        [SerializeField, HideInInspector, CanNotEdit] internal protected List<UIPageElementFeature> features = null;
        [SerializeField, HideInInspector] public UnityEvent onAppearUI, onHideUI;

        UIPage owner = null;
        UnityEngine.UI.Selectable ut_selectable;
        public UIPage Owner { get { return owner; } }

        protected override void OnLoadReference()
        {
            this.GetOrUpdateChilds<UIPageElementFeature, UIPageElement>(true, true, false, false, ref features, ref configError, ref configErrorMessage);
        }

        internal override void OnInitBaseFW()
        {
            base.OnInitBaseFW();
            for (int i = 0; i < features.Count; i++)
            {
                var ft = features[i];
                ft.OnInitFeature(owner, this);
            }
        }

        protected internal virtual void OnInitElement(UIPage owner)
        {
            this.owner = owner;
            ut_selectable = GetComponent<UnityEngine.UI.Selectable>();
            var ld = (ILoadReference)this;
            if (ld != null) { ld.LoadReference(); }
            if (ld != null) { ld.InitIfRequired(); }
            
            _transform.localScale = Vector3.zero;
            owner._onOpen.AddListener(() =>
            {
                AppearElement();
            });

            owner._onClose.AddListener(() =>
            {
                HideElement();
            });
        }

        protected override void OnCleanup()
        {
            for (int i = 0; i < features.Count; i++)
            {
                var ft = features[i];
                ft.Cleanup();
            }
            base.OnCleanup();
        }

        public void AppearElement()
        {
            ThrowIfInvalidConfig("Invalid configuration of UIPageElement. Can not appear. " +
                "Check inspector of affected gameobject: " + gameObject.name);
            Cleanup();
            _transform.localScale = initLocalScale;
            onAppearUI?.Invoke();
        }

        public void HideElement()
        {
            ThrowIfInvalidConfig("Invalid configuration of UIPageElement. Can not disappear. " +
               "Check inspector of affected gameobject: " + gameObject.name);
            Cleanup();
            _transform.localScale = Vector3.zero;
            onHideUI?.Invoke();
        }

        protected override void OnSetActive(bool active)
        {
            base.OnSetActive(active);
            if (ut_selectable != null) { ut_selectable.interactable = active; }
            for (int i = 0; i < features.Count; i++)
            {
                var ft = features[i];
                ft.SetActiveUI(active);
            }
        }
    }
}