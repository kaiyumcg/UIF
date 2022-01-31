using DG.Tweening.Core;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace UIF
{
    [DisallowMultipleComponent]
    [AddComponentMenu("UIF/Page UI Element")]
    public class UIPageElement : MonoBehaviour
    {
        [SerializeField] List<UITag> tags = null;
        [SerializeField] bool waitForPageOpen = false, waitForPageClose = false;
        [SerializeField] float appearenceTweenDuration = 0.3f;
        [SerializeField] List<UIPageElementFeature> features = null;
        [SerializeField] bool useAnimationWhenPageOpen = true, useAnimationWhenPageClose = true;
        [SerializeField] bool overrideDefaultTween = false;
        [SerializeField, HideInInspector] internal bool editDefaultSetting = false, eventEdit = false;
        [SerializeField] public UnityEvent onInit, onOwnerPageOpen, onOwnerPageClose, onAppear, onHide, onActivate, onDeactivate;
        CompletionHandle appeared = null, hidden = null;
        Transform _transform = null;
        GameObject _gameObject = null;
        UIPage owner = null;
        Vector3 initLocalScale = Vector3.one;
        TweenerCore<Vector3, Vector3, VectorOptions> tween = null;
        
        protected internal bool WaitForPageOpen { get { return waitForPageOpen; } }
        protected internal bool WaitForPageClose { get { return waitForPageClose; } }
        public UIPage Owner { get { return owner; } }
        public Transform _Transform { get { return _transform; } }
        public GameObject _GameObject { get { return _gameObject; } }
        public Vector3 InitLocalScale { get { return initLocalScale; } }
        public CompletionHandle Appeared { get { return appeared; } }
        public CompletionHandle Hidden { get { return hidden; } }
        internal List<UITag> Tags { get { return tags; } }

        #region Getter
        public List<T> GetFeaturesFast<T>() where T : UIPageElementFeature
        {
            List<T> result = new List<T>();
            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    if (ft == null) { continue; }
                    if (ft.GetType() == typeof(T))
                    {
                        T tElem = (T)(UIPageElementFeature)ft;
                        result.Add(tElem);
                    }
                }
            }
            return result;
        }

        public T GetFeatureByTag<T>(UITag tag) where T : UIPageElementFeature
        {
            T result = null;
            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    if (ft == null || ft.Tags == null || ft.Tags.Count == 0) { continue; }
                    if (ft.GetType() == typeof(T) && ft.Tags.Contains(tag))
                    {
                        T tElem = (T)(UIPageElementFeature)ft;
                        result = tElem;
                        break;
                    }
                }
            }
            return result;
        }

        public T GetFeatureByTags<T>(params UITag[] tags) where T : UIPageElementFeature
        {
            T result = null;
            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    if (ft == null) { continue; }
                    if (ft.GetType() == typeof(T) && HasFoundTags(tags, ft))
                    {
                        T tElem = (T)(UIPageElementFeature)ft;
                        result = tElem;
                        break;
                    }
                }
            }
            return result;
        }

        public List<T> GetFeaturesByTags<T>(params UITag[] tags) where T : UIPageElementFeature
        {
            List<T> result = new List<T>();
            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    if (ft == null) { continue; }
                    if (ft.GetType() == typeof(T) && HasFoundTags(tags, ft))
                    {
                        T tElem = (T)(UIPageElementFeature)ft;
                        result.Add(tElem);
                    }
                }
            }
            return result;
        }

        bool HasFoundTags(UITag[] tags_arg, UIPageElementFeature elem)
        {
            bool found = true;
            if (elem.Tags != null && elem.Tags.Count > 0)
            {
                if (tags_arg != null && tags_arg.Length > 0)
                {
                    for (int i = 0; i < tags_arg.Length; i++)
                    {
                        if (tags_arg[i] == null) { continue; }
                        if (elem.Tags.Contains(tags_arg[i]) == false)
                        {
                            found = false;
                            break;
                        }
                    }
                }
                else
                {
                    found = false;
                }
            }
            else
            {
                found = false;
            }
            return found;
        }

        #endregion


        protected virtual IEnumerator DefineOverridenTweenForAppear(Action OnDone) { yield return null; }
        protected virtual IEnumerator DefineOverridenTweenForHide(Action OnDone) { yield return null; }
        public void SetActiveUI(bool activate)
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

        public void AppearElement(bool useAnimation = true)
        {
            AppearElement(null, useAnimation);
        }

        public void AppearElement(Action OnComplete, bool useAnimation = true)
        {
            Cleanup();
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
                if (overrideDefaultTween)
                {
                    StartCoroutine(DefineOverridenTweenForAppear(() =>
                    {
                        appeared.completed = true;
                        onAppear?.Invoke();
                        _transform.localScale = initLocalScale;
                        StartCoroutine(DoFeatureAppear());
                    }));
                }
                else
                {
                    tween = _transform.DOScale(initLocalScale, appearenceTweenDuration).OnComplete(() =>
                    {
                        appeared.completed = true;
                        onAppear?.Invoke();
                        _transform.localScale = initLocalScale;
                        StartCoroutine(DoFeatureAppear());
                    });
                }
            }
            else
            {
                appeared.completed = true;
                onAppear?.Invoke();
                StartCoroutine(DoFeatureAppear());
            }

            IEnumerator DoFeatureAppear()
            {
                if (features != null && features.Count > 0)
                {
                    for (int i = 0; i < features.Count; i++)
                    {
                        var ft = features[i];
                        if (ft == null) { continue; }
                        ft.OnAppearOwnerElement();
                        if (ft.WaitForElementAppear)
                        {
                            yield return ft.StartCoroutine(ft.OnAppearOwnerElementAsync());
                        }
                        else
                        {
                            ft.StartCoroutine(ft.OnAppearOwnerElementAsync());
                        }
                    }
                }
                OnComplete?.Invoke();
            }
        }

        public void HideElement(bool useAnimation = true)
        {
            HideElement(null, useAnimation);
        }

        public void HideElement(Action OnComplete = null, bool useAnimation = true)
        {
            Cleanup();
            if (tween != null && tween.IsPlaying()) { tween.Kill(); }
            if (appeared == null) { appeared = new CompletionHandle(); }
            if (hidden == null) { hidden = new CompletionHandle(); }
            appeared.completed = false;
            hidden.completed = false;
            if (useAnimation)
            {
                if (overrideDefaultTween)
                {
                    StartCoroutine(DoFeatureHide(() =>
                    {
                        if (_gameObject.activeInHierarchy == false)
                        {
                            _gameObject.SetActive(true);
                        }
                        StartCoroutine(DefineOverridenTweenForHide(() =>
                        {
                            if (_gameObject.activeInHierarchy)
                            {
                                _gameObject.SetActive(false);
                            }
                            _transform.localScale = initLocalScale;
                            hidden.completed = true;
                            onHide?.Invoke();
                            OnComplete?.Invoke();
                        }));

                    }));
                   
                }
                else
                {
                    StartCoroutine(DoFeatureHide(() =>
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
                            onHide?.Invoke();
                            OnComplete?.Invoke();
                        });
                    }));
                }
            }
            else
            {
                StartCoroutine(DoFeatureHide(() =>
                {
                    if (_gameObject.activeInHierarchy)
                    {
                        _gameObject.SetActive(false);
                    }
                    _transform.localScale = initLocalScale;
                    hidden.completed = true;
                    onHide?.Invoke();
                    OnComplete?.Invoke();
                }));
            }

            IEnumerator DoFeatureHide(Action OnCompleteCB)
            {
                if (features != null && features.Count > 0)
                {
                    for (int i = 0; i < features.Count; i++)
                    {
                        var ft = features[i];
                        if (ft == null) { continue; }
                        ft.OnHideOwnerElement();
                        if (ft.WaitForElementHide)
                        {
                            yield return ft.StartCoroutine(ft.OnHideOwnerElementAsync());
                        }
                        else
                        {
                            ft.StartCoroutine(ft.OnHideOwnerElementAsync());
                        }
                    }
                }
                OnCompleteCB?.Invoke();
            }
        }

        public List<T> GetFeatures<T>() where T : UIPageElementFeature
        {
            return this.GetComps<T, UIPageElement>();
        }

        internal void Cleanup()
        {
            StopAllCoroutines();
            if (features != null && features.Count > 0)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    var ft = features[i];
                    ft.Cleanup();
                }
            }
        }

        internal void LoadFeatures()
        {
            var curFeatures = this.GetFeatures<UIPageElementFeature>();
            Util.AddAndMakeOverallUnique(curFeatures, ref features);
        }
        protected internal virtual void OnInit(UIPage owner)
        {
            this.owner = owner;
            _transform = transform;
            _gameObject = gameObject;
            initLocalScale = _transform.localScale;
            LoadFeatures();
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
            onInit?.Invoke();
        }
        protected internal virtual void OnOpenOwnerPage()
        {
            onOwnerPageOpen?.Invoke();
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
            yield return StartCoroutine(DoIt());
            IEnumerator DoIt()
            {
                var done = false;
                AppearElement(() =>
                {
                    done = true;
                }, useAnimationWhenPageOpen);
                while (!done) { yield return null; }

                if (features != null && features.Count > 0)
                {
                    for (int i = 0; i < features.Count; i++)
                    {
                        var ft = features[i];
                        if (ft == null) { continue; }
                        if (ft.WaitForPageOpen)
                        {
                            yield return ft.StartCoroutine(ft.OnOpenOwnerPageAsync());
                        }
                        else
                        {
                            ft.StartCoroutine(ft.OnOpenOwnerPageAsync());
                        }
                    }
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
            onOwnerPageClose?.Invoke();
        }
        protected internal virtual IEnumerator OnCloseOwnerPageAsync() 
        {
            yield return StartCoroutine(DoIt());
            IEnumerator DoIt()
            {
                if (features != null && features.Count > 0)
                {
                    for (int i = 0; i < features.Count; i++)
                    {
                        var ft = features[i];
                        if (ft == null) { continue; }
                        if (ft.WaitForPageClose)
                        {
                            yield return ft.StartCoroutine(ft.OnCloseOwnerPageAsync());
                        }
                        else
                        {
                            ft.StartCoroutine(ft.OnCloseOwnerPageAsync());
                        }
                    }
                }
                var done = false;
                HideElement(() =>
                {
                    done = true;

                }, useAnimationWhenPageClose);
                while (!done) { yield return null; }
            }
        }
        protected virtual void OnActivate() { onActivate?.Invoke(); }
        protected virtual void OnDeactivate() { onDeactivate?.Invoke(); }
    }
}