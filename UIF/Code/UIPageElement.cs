using DG.Tweening.Core;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    public class UIPageElement : MonoBehaviour
    {
        [SerializeField, HideInInspector] List<UIPageElementFeature> features = null;

        CompletionHandle appeared = null, hidden = null;
        public CompletionHandle Appeared { get { return appeared; } }
        public CompletionHandle Hidden { get { return hidden; } }

        protected internal virtual void OnInit(UIPage owner)
        {
            this.owner = owner;
            _transform = transform;
            _gameObject = gameObject;
            initLocalScale = _transform.localScale;
            features = this.GetFeatures<UIPageElementFeature>();
            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    if (ft == null) { continue; }
                    var ftObj = ft.gameObject;
                    var activFlag = ftObj.activeInHierarchy;
                    ftObj.SetActive(true);
                    ft.OnInit(owner, this);
                    ftObj.SetActive(activFlag);
                }
            }
        }
        protected internal virtual void OnOpenOwnerPage() 
        {
            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    if (ft == null) { continue; }
                    ft.OnOpenOwnerPage();
                }
            }
        }
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

            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    if (ft == null) { continue; }
                    StartCoroutine(ft.OnOpenOwnerPageAsync());
                }
            }
        }
        protected internal virtual void OnCloseOwnerPage() 
        {
            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    if (ft == null) { continue; }
                    ft.OnCloseOwnerPage();
                }
            }
        }
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
        public UIPage Owner { get { return owner; } }
        public Transform _Transform { get { return _transform; } }
        public GameObject _GameObject { get { return _gameObject; } }
        public Vector3 InitLocalScale { get { return initLocalScale; } }

        internal void SetActiveUI(bool activate)
        {
            if (activate) { OnActivate(); }
            else { OnDeactivate(); }

            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    if (ft == null) { continue; }
                    ft.SetActiveUI(activate);
                }
            }
        }

        public void AppearElement(bool useAnimation)
        {
            if (tween != null && tween.IsPlaying()) { tween.Kill(); }
            if (appeared == null) { appeared = new CompletionHandle(); }
            if (hidden == null) { hidden = new CompletionHandle(); }
            hidden.completed = false;
            appeared.completed = false;

            if (_gameObject.activeInHierarchy == false)
            {
                _gameObject.SetActive(true);
            }
            _transform.localScale = initLocalScale;

            if (useAnimation)
            {
                _transform.localScale = Vector3.zero;
                tween = _transform.DOScale(initLocalScale, appearenceTweenDuration).OnComplete(() =>
                {
                    appeared.completed = true;
                });
            }
            else
            {
                appeared.completed = true;
            }

            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    if (ft == null) { continue; }
                    ft.OnAppearOwnerElement();
                    StartCoroutine(ft.OnAppearOwnerElementAsync());
                }
            }
        }

        public void HideElement(bool useAnimation)
        {
            if (tween != null && tween.IsPlaying()) { tween.Kill(); }
            if (appeared == null) { appeared = new CompletionHandle(); }
            if (hidden == null) { hidden = new CompletionHandle(); }
            appeared.completed = false;
            hidden.completed = false;
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
                    hidden.completed = true;
                });
            }
            else
            {
                if (_gameObject.activeInHierarchy)
                {
                    _gameObject.SetActive(false);
                }
                _transform.localScale = initLocalScale;
                hidden.completed = true;
            }

            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    if (ft == null) { continue; }
                    ft.OnHideOwnerElement();
                }
            }
        }
    }
}