using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace UIF
{
    public class UIPageElement : MonoBehaviour
    {
        protected internal virtual void OnInit(UIPage owner)
        {
            this.owner = owner;
            _transform = transform;
            _gameObject = gameObject;
            initLocalScale = _transform.localScale;
        }
        protected internal virtual void OnOpenOwnerPage() { }
        protected internal virtual IEnumerator OnOpenOwnerPageAsync() 
        {
            if (useTweenOnPageOpen)
            {
                AppearElement(true);
            }
            else
            {
                yield return null;
            }
        }
        protected internal virtual void OnCloseOwnerPage() { }
        protected virtual void OnActivate() { }
        protected virtual void OnDeactivate() { }
        Transform _transform;
        GameObject _gameObject;
        UIPage owner;
        Vector3 initLocalScale;
        TweenerCore<Vector3, Vector3, VectorOptions> tween = null;
        [SerializeField] bool waitForOpen = false;
        [SerializeField] bool useTweenOnPageOpen = true;
        [SerializeField] float appearenceTweenDuration = 0.3f;
        internal bool WaitForOpen { get { return waitForOpen; } }
        protected UIPage Owner { get { return owner; } }
        protected Transform _Transform { get { return _transform; } }
        protected GameObject _GameObject { get { return _gameObject; } }
        protected Vector3 InitLocalScale { get { return initLocalScale; } }

        internal void SetActiveUI(bool activate)
        {
            if (activate) { OnActivate(); }
            else { OnDeactivate(); }
        }

        public void AppearElement(bool useAnimation)
        {
            if (tween != null && tween.IsPlaying()) { tween.Kill(); }
            if (_gameObject.activeInHierarchy == false)
            {
                _gameObject.SetActive(true);
            }
            _transform.localScale = initLocalScale;

            if (useAnimation)
            {
                _transform.localScale = Vector3.zero;
                tween = _transform.DOScale(initLocalScale, appearenceTweenDuration);
            }
        }

        public void HideElement(bool useAnimation)
        {
            if (tween != null && tween.IsPlaying()) { tween.Kill(); }
            if (useAnimation)
            {
                if (_gameObject.activeInHierarchy == false)
                {
                    _gameObject.SetActive(true);
                }
                tween = _transform.DOScale(Vector3.zero, appearenceTweenDuration).OnComplete(() =>
                {
                    if (_gameObject.activeInHierarchy)
                    {
                        _gameObject.SetActive(false);
                    }
                    _transform.localScale = initLocalScale;
                });
            }
            else
            {
                if (_gameObject.activeInHierarchy)
                {
                    _gameObject.SetActive(false);
                }
                _transform.localScale = initLocalScale;
            }
        }
    }
}