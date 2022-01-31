using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine.Events;

namespace UIF
{
    [DisallowMultipleComponent]
    [AddComponentMenu("UIF/UI Page")]
    public class UIPage : MonoBehaviour
    {
        [SerializeField] string pageName = "";
        [SerializeField] List<UITag> tags = null;
        [SerializeField] float pageWideTweenTime = 0.35f;
        [SerializeField] bool dynamicPage = false;
        [SerializeField] List<UIPageElement> elements = null;
        [SerializeField] bool overrideDefaultTween = false;

        [SerializeField, HideInInspector] internal Selectable selectedFeature = null;
        [SerializeField, HideInInspector] internal List<Selectable> multiSelectedFeatures = null;
        [SerializeField, HideInInspector] internal bool editDefaultSetting = false, eventEdit = false;
        [SerializeField] public UnityEvent onInit, onOpen, onClose, onActivate, onDeactivate;
        TweenerCore<Vector3, Vector3, VectorOptions> showTween = null, hideTween = null;
        Vector3 initLocalScale;
        Transform _transform = null;
        GameObject _gameObject = null;
        bool opened = false, inTransition = false;

        public Transform _Transform { get { return _transform; } }
        public GameObject _GameObject { get { return _gameObject; } }
        public Vector3 PageLocalScale { get { return initLocalScale; } }
        public bool IsOpened { get { return opened; } }
        public bool IsInTransition { get { return inTransition; } }
        public bool IsPageDynamic { get { return dynamicPage; } }
        public string PageName { get { return pageName; } }
        internal List<UITag> Tags { get { return tags; } }

        public void ClearAllSelectedFeatures()
        {
            if (selectedFeature != null) { selectedFeature.ClearSelection(); }
            if (multiSelectedFeatures != null && multiSelectedFeatures.Count > 0)
            {
                for (int i = 0; i < multiSelectedFeatures.Count; i++)
                {
                    var elem = multiSelectedFeatures[i];
                    if (elem == null) { continue; }
                    elem.ClearSelection();
                }
            }
        }

        internal void LoadElements()
        {
            var curElems = this.GetElements<UIPageElement>();
            Util.AddAndMakeOverallUnique(curElems, ref elements);
        }

        public List<T> GetElements<T>() where T : UIPageElement
        {
            return this.GetComps<T, UIPage>();
        }

        #region Getter
        public List<T> GetElementsFast<T>() where T : UIPageElement
        {
            List<T> result = new List<T>();
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var elem = elements[i];
                    if (elem == null) { continue; }
                    if (elem.GetType() == typeof(T))
                    {
                        T tElem = (T)(UIPageElement)elem;
                        result.Add(tElem);
                    }
                }
            }
            return result;
        }

        public T GetElementByTag<T>(UITag tag) where T : UIPageElement
        {
            T result = null;
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var elem = elements[i];
                    if (elem == null || elem.Tags == null || elem.Tags.Count == 0) { continue; }
                    if (elem.GetType() == typeof(T) && elem.Tags.Contains(tag))
                    {
                        T tElem = (T)(UIPageElement)elem;
                        result = tElem;
                        break;
                    }
                }
            }
            return result;
        }

        public T GetElementByTags<T>(params UITag[] tags) where T : UIPageElement
        {
            T result = null;
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var elem = elements[i];
                    if (elem == null) { continue; }
                    if (elem.GetType() == typeof(T) && HasFoundTags(tags, elem))
                    {
                        T tElem = (T)(UIPageElement)elem;
                        result = tElem;
                        break;
                    }
                }
            }
            return result;
        }

        public List<T> GetElementsByTags<T>(params UITag[] tags) where T : UIPageElement
        {
            List<T> result = new List<T>();
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var elem = elements[i];
                    if (elem == null) { continue; }
                    if (elem.GetType() == typeof(T) && HasFoundTags(tags, elem))
                    {
                        T tElem = (T)(UIPageElement)elem;
                        result.Add(tElem);
                    }
                }
            }
            return result;
        }

        bool HasFoundTags(UITag[] tags_arg, UIPageElement elem)
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

        protected internal virtual void OnInit() 
        {
            _transform = transform;
            _gameObject = gameObject;
            initLocalScale = transform.localScale;
            Cleaup();
            if (_gameObject.activeInHierarchy == false)
            {
                _gameObject.SetActive(true);
            }
            LoadElements();
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var elem = elements[i];
                    var elemObj = elem.gameObject;
                    var activFlag = elemObj.activeInHierarchy;
                    elemObj.SetActive(true);
                    elem.OnInit(this);
                    elemObj.SetActive(activFlag);
                }
            }
            _gameObject.SetActive(false);
            opened = inTransition = false;
            onInit?.Invoke();
        }
        protected internal virtual void OnOpen(bool withAnimation) { onOpen?.Invoke(); }
        protected internal virtual IEnumerator OnOpenAsync(bool withAnimation) { yield return null; }
        protected internal virtual void OnClose(bool withAnimation) { onClose?.Invoke(); }
        protected internal virtual IEnumerator OnCloseAsync(bool withAnimation) { yield return null; }
        protected internal virtual void OnActivate() { onActivate?.Invoke(); }
        protected internal virtual void OnDeactivate() { onDeactivate?.Invoke(); }
        protected internal virtual void OnTick(Vector2 deltaInputPosition, Vector2 smoothDeltaInputPosition) { }
        protected virtual IEnumerator DefineOverridenTweenForOpen(Action OnDone) 
        { 
            yield return null; 
        }
        protected virtual IEnumerator DefineOverridenTweenForClose(Action OnDone) 
        { 
            yield return null;
        }

        void Cleaup()
        {
            StopAllCoroutines();
            if (showTween != null && showTween.IsPlaying()) { showTween.Kill(); }
            if (hideTween != null && hideTween.IsPlaying()) { hideTween.Kill(); }
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var elem = elements[i];
                    elem.Cleanup();
                }
            }
        }

        public void SetActivePage(bool activate)
        {
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var elem = elements[i];
                    elem.SetActiveUI(activate);
                }
            }

            if (activate) { OnActivate(); }
            else { OnDeactivate(); }
        }

        #region DynamicUI
        public void AddItemToLayoutGroupFromPrefab(LayoutGroup group, LayoutElement elementPrefab)
        {
            var elemGObject = Instantiate(elementPrefab.gameObject) as GameObject;
            elemGObject.transform.SetParent(group.transform, true);
            elements = this.GetElements<UIPageElement>();
            dynamicPage = true;
            //todo animation
        }

        public void RemoveItemFromLayoutGroup(LayoutGroup group, LayoutElement elementOnGroup, bool deactivate = true)
        {
            //todo
        }

        public void ActivateItemInLayoutGroup(LayoutGroup group, LayoutGroup elementOnGroup)
        {
            //todo
        }
        #endregion

        Coroutine randomSelectionHandle = null;
        public void ChoseSelectableRandom(List<Selectable> selectables, 
            Action<Selectable> OnFinallySelected, float delayAmount = 0.2f, float within = 2f)
        {
            Cleaup();
            if (selectables == null) { selectables = new List<Selectable>(); }
            selectables.RemoveAll((sel) => { return sel == null; });
            if (selectables == null) { selectables = new List<Selectable>(); }

            for (int i = 0; i < selectables.Count; i++)
            {
                var sel = selectables[i];
                if (sel == null) { continue; }
                sel.FreezeInput = true;
            }

            if (randomSelectionHandle != null) { StopCoroutine(randomSelectionHandle); }
            randomSelectionHandle = StartCoroutine(RandomSelect());
            IEnumerator RandomSelect()
            {
                var delayTimer = 0.0f;
                var totalTimer = 0.0f;
                Selectable selected = null;
                while (true)
                {
                    var delta = Time.deltaTime;
                    delayTimer += delta;
                    totalTimer += delta;
                    if (delayTimer >= delayAmount)
                    {
                        delayTimer = 0.0f;
                        selected = selectables[UnityEngine.Random.Range(0, selectables.Count)];
                        selected.Select();
                    }
                    
                    if (totalTimer >= within)
                    {
                        break;
                    }
                    
                    yield return null;
                }
                OnFinallySelected?.Invoke(selected);
                for (int i = 0; i < selectables.Count; i++)
                {
                    var sel = selectables[i];
                    if (sel == null) { continue; }
                    sel.FreezeInput = false;
                }
            }
        }

        public void Open(bool withAnimation = false)
        {
            Open(null, withAnimation);
        }

        public void Open(Action OnComplete = null, bool withAnimation = false)
        {
            if (opened || inTransition) { return; }
            opened = true;
            inTransition = true;
            Cleaup();
            _gameObject.SetActive(true);
            if (withAnimation)
            {
                _transform.localScale = Vector3.zero;
                if (overrideDefaultTween)
                {
                    StartCoroutine(DefineOverridenTweenForOpen(() =>
                    {
                        ShowInternal(() =>
                        {
                            inTransition = false;
                            OnComplete?.Invoke();
                        });
                    }));
                }
                else
                {
                    showTween = _transform.DOScale(1.0f, pageWideTweenTime).OnComplete(() =>
                    {
                        ShowInternal(() =>
                        {
                            inTransition = false;
                            OnComplete?.Invoke();
                        });
                    });
                }
            }
            else
            {
                _transform.localScale = initLocalScale;
                ShowInternal(() =>
                {
                    inTransition = false;
                    OnComplete?.Invoke();
                });
            }

            void ShowInternal(Action OnComplete)
            {   
                StartCoroutine(DoShowCB_(OnComplete));
                IEnumerator DoShowCB_(Action OnComplete)
                {
                    OnOpen(withAnimation);
                    yield return StartCoroutine(OnOpenAsync(withAnimation));
                    if (elements != null && elements.Count > 0)
                    {
                        for (int i = 0; i < elements.Count; i++)
                        {
                            var elem = elements[i];
                            elem.OnOpenOwnerPage();
                            if (elem.WaitForPageOpen)
                            {
                                yield return elem.StartCoroutine(elem.OnOpenOwnerPageAsync());
                            }
                            else
                            {
                                elem.StartCoroutine(elem.OnOpenOwnerPageAsync());
                            }
                        }
                    }
                    OnComplete?.Invoke();
                }
            }
        }

        public void Close(bool withAnimation = false)
        {
            Close(null, withAnimation);
        }

        public void Close(Action OnComplete = null, bool withAnimation = false)
        {
            if (!opened || inTransition) { return; }
            opened = false;
            inTransition = true;
            Cleaup();
            if (withAnimation)
            {
                if (_gameObject.activeInHierarchy == false)
                {
                    _gameObject.SetActive(true);
                }
                if (overrideDefaultTween)
                {
                    StartCoroutine(DefineOverridenTweenForClose(() =>
                    {
                        HideInternal(() =>
                        {
                            inTransition = false;
                            OnComplete?.Invoke();
                        });
                    }));
                }
                else
                {
                    hideTween = _transform.DOScale(0.0f, pageWideTweenTime).OnComplete(() =>
                    {
                        HideInternal(() =>
                        {
                            inTransition = false;
                            OnComplete?.Invoke();
                        });
                    });
                }
            }
            else
            {
                HideInternal(() =>
                {
                    inTransition = false;
                    OnComplete?.Invoke();
                });
            }

            void HideInternal(Action OnComplete)
            {
                StartCoroutine(DoHideCB_(OnComplete));
                IEnumerator DoHideCB_(Action OnCompleteArg)
                {
                    if (elements != null && elements.Count > 0)
                    {
                        for (int i = 0; i < elements.Count; i++)
                        {
                            var elem = elements[i];
                            elem.OnCloseOwnerPage();
                        }
                    }
                    OnClose(withAnimation);
                    yield return StartCoroutine(OnCloseAsync(withAnimation));
                    if (_gameObject.activeInHierarchy)
                    {
                        _gameObject.SetActive(false);
                    }
                    _transform.localScale = initLocalScale;
                    OnCompleteArg?.Invoke();
                }
            }
        }
    }
}