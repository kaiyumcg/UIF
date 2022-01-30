using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace UIF
{
    public class UIPageElementFeature : MonoBehaviour
    {
        protected internal virtual void OnInit(UIPage owner, UIPageElement element)
        {
            this.ownerPage = owner;
            this.ownerElement = element;
            _transform = transform;
            _gameObject = gameObject;
            initLocalScale = _transform.localScale;
        }
        protected internal virtual void OnOpenOwnerPage() { }
        protected internal virtual IEnumerator OnOpenOwnerPageAsync() { yield return null; }
        protected internal virtual void OnAppearOwnerElement() { }
        protected internal virtual IEnumerator OnAppearOwnerElementAsync() { yield return null; }

        protected internal virtual void OnCloseOwnerPage() { }
        protected internal virtual void OnHideOwnerElement() { }
        protected virtual void OnActivate() { }
        protected virtual void OnDeactivate() { }
        Transform _transform;
        GameObject _gameObject;
        UIPage ownerPage;
        UIPageElement ownerElement;
        Vector3 initLocalScale;
        public UIPage OwnerPage { get { return ownerPage; } }
        public UIPageElement OwnerElement { get { return ownerElement; } }
        public Transform _Transform { get { return _transform; } }
        public GameObject _GameObject { get { return _gameObject; } }
        public Vector3 InitLocalScale { get { return initLocalScale; } }

        internal void SetActiveUI(bool activate)
        {
            if (activate) { OnActivate(); }
            else { OnDeactivate(); }
        }
    }
}