using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;

namespace UIF
{
    [DisallowMultipleComponent]
    public class UIPage : MonoBehaviour
    {
        [SerializeField] string pageName = "";
        [SerializeField] float pageWideTweenTime = 0.35f;
        [SerializeField] bool dynamicPage = false;
        List<UIPageElement> elements = null;
        TweenerCore<Vector3, Vector3, VectorOptions> showTween = null, hideTween = null;
        Vector3 initLocalScale;
        Transform _transform = null;
        GameObject _gameObject = null;
        bool opened = false, inTransition = false;

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
            elements = this.GetElements<UIPageElement>();
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
        }

        protected internal virtual void OnOpen(bool withAnimation) { }
        protected internal virtual IEnumerator OnOpenAsync(bool withAnimation) { yield return null; }

        protected internal virtual void OnClose(bool withAnimation) { }
        protected internal virtual IEnumerator OnCloseAsync(bool withAnimation) { yield return null; }

        protected internal virtual void OnActivate() { }
        protected internal virtual void OnDeactivate() { }

        protected internal virtual void OnTick(Vector2 deltaInputPosition, Vector2 smoothDeltaInputPosition) { }

        public Transform _Transform { get { return _transform; } }
        public GameObject _GameObject { get { return _gameObject; } }
        public Vector3 PageLocalScale { get { return initLocalScale; } }
        public bool IsOpened { get { return opened; } }
        public bool IsInTransition { get { return inTransition; } }
        public bool IsPageDynamic { get { return dynamicPage; } }
        public string PageName { get { return pageName; } }

        public void ClearAllSelectedItems()
        {
            if (selectedElement != null) { selectedElement.ClearSelection(); }
            if (multiSelectedElements != null && multiSelectedElements.Count > 0)
            {
                for (int i = 0; i < multiSelectedElements.Count; i++)
                {
                    var elem = multiSelectedElements[i];
                    if (elem == null) { continue; }
                    elem.ClearSelection();
                }
            }
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
                    elem.StopAllCoroutines();
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

        Coroutine randomSelectionHandle = null;
        public void ChoseSelectableRandom(List<Selectable> selectables, 
            Action<Selectable> OnFinallySelected, float delayAmount = 0.2f, float within = 2f)
        {
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
            if (opened || inTransition) { return; }
            opened = true;
            inTransition = true;
            Cleaup();
            _gameObject.SetActive(true);
            if (withAnimation)
            {
                _transform.localScale = Vector3.zero;
                showTween = _transform.DOScale(1.0f, pageWideTweenTime).OnComplete(() =>
                {
                    ShowInternal(() =>
                    {
                        inTransition = false;
                    });
                });
            }
            else
            {
                _transform.localScale = initLocalScale;
                ShowInternal(() =>
                {
                    inTransition = false;
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
                            if (elem.WaitForOpen)
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
                hideTween = _transform.DOScale(0.0f, pageWideTweenTime).OnComplete(() =>
                {
                    HideInternal(() =>
                    {
                        inTransition = false;
                    });
                });
            }
            else
            {
                HideInternal(() =>
                {
                    inTransition = false;
                });
            }

            void HideInternal(Action OnComplete)
            {
                StartCoroutine(DoHideCB_(OnComplete));
                IEnumerator DoHideCB_(Action OnComplete)
                {
                    OnClose(withAnimation);
                    yield return StartCoroutine(OnCloseAsync(withAnimation));
                    OnComplete?.Invoke();
                    if (elements != null && elements.Count > 0)
                    {
                        for (int i = 0; i < elements.Count; i++)
                        {
                            var elem = elements[i];
                            elem.OnCloseOwnerPage();
                        }
                    }
                    if (_gameObject.activeInHierarchy)
                    {
                        _gameObject.SetActive(false);
                    }
                    _transform.localScale = initLocalScale;
                }
            }
        }
    }
}