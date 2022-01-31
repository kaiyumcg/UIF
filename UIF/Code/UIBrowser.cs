using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UIF
{
    [DisallowMultipleComponent]
    [AddComponentMenu("UIF/UI Browser")]
    public class UIBrowser : MonoBehaviour
    {
        [SerializeField] bool shouldTick = false;
        [SerializeField] string homePage;
        [SerializeField] internal List<UIPage> pages;
        [SerializeField] int deltaSmoothnessBufferSize = 4;//delta positions are also needed for input control, so we are duplicating calculations
        //then for multiple UIBrowser we have multiple of this calculation? fix it
        [SerializeField, HideInInspector] Vector2 lastMousePos;
        [SerializeField, HideInInspector] int curbID = 0;
        [SerializeField, HideInInspector] Vector2[] bDeltas;
        [SerializeField, HideInInspector] internal bool editDefaultSetting = false, eventEdit = false;
        [SerializeField] public UnityEvent onInit, onOpenHomePage;

        Vector2 curDelta, smoothDelta;
        public Vector2 CurrentInputDeltaPosition { get { return curDelta; } }
        public Vector2 CurrentSmoothInputDeltaPosition { get { return smoothDelta; } }

        protected virtual void OnInit() { onInit?.Invoke(); }
        protected virtual void OnTick() { }

        private void Awake()
        {
            LoadInputSpecificReferences();
            LoadAllPages(ref pages);
            if (pages != null && pages.Count > 0)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    pages[i].OnInit();
                }
            }
            OnInit();
        }

        public void OpenBrowser()
        {
            OpenUIURL(homePage, true);
            onOpenHomePage?.Invoke();
        }

        internal void LoadInputSpecificReferences()
        {
            curbID = 0;
            if (deltaSmoothnessBufferSize < 1) { deltaSmoothnessBufferSize = 1; }
            if (bDeltas == null || deltaSmoothnessBufferSize != bDeltas.Length)
            {
                bDeltas = new Vector2[deltaSmoothnessBufferSize];
            }

            for (int i = 0; i < deltaSmoothnessBufferSize; i++)
            {
                bDeltas[i] = Vector3.zero;
            }
            lastMousePos = Input.mousePosition;
        }

        internal void LoadAllPages(ref List<UIPage> pages)
        {
            var ps = transform.GetComponentsInChildren<UIPage>(true);
            Util.AddAndMakeOverallUnique(ps, ref pages);
        }

        public void CloseAllOpenedPage(bool withAnimation = true)
        {
            if (pages != null && pages.Count > 0)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    var page = pages[i];
                    if (page == null) { continue; }
                    page.Close(withAnimation);
                }
            }
        }

        public void SetActiveUI(bool activate)
        {
            if (pages != null && pages.Count > 0)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    var page = pages[i];
                    if (page == null) { continue; }
                    page.SetActivePage(activate);
                }
            }
        }

        public void OpenUIURL(string url, bool useAnimation)
        {
            var pNames = Util.GetPagesNameFromUrl(url);
            if (pNames != null && pNames.Count > 0)
            {
                for (int i = 0; i < pNames.Count; i++)
                {
                    var pageName = pNames[i];
                    OpenPage(pageName, useAnimation);
                }
            }
        }

        public void OpenPage(string pageName, bool useAnimation)
        {
            UIPage page = GetPageByName(pageName);
            page.Open(useAnimation);
        }

        public UIPage GetPageByName(string pageName)
        {
            UIPage page = null;
            if (pages != null && pages.Count > 0)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    var uiPage = pages[i];
                    if (uiPage.PageName == pageName)
                    {
                        page = uiPage;
                        break;
                    }
                }
            }
            return page;
        }

        //Builtin Browser, Page, Element, Feature's editor tool
        //Editor Tool Script template
        //validation? wrong component or setup hole run i korbe na etc
        //dynamic hole ki ki hobe. register/unregister? 
        //r&d on game UI
        //cross, back, forward (UX implication)
        //Potential for server side execution?

        #region Getter
        public List<T> GetFeaturesFast<T>() where T : UIPage
        {
            List<T> result = new List<T>();
            if (pages != null && pages.Count > 0)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    var page = pages[i];
                    if (page == null) { continue; }
                    if (page.GetType() == typeof(T))
                    {
                        T tElem = (T)(UIPage)page;
                        result.Add(tElem);
                    }
                }
            }
            return result;
        }

        public T GetFeatureByTag<T>(UITag tag) where T : UIPage
        {
            T result = null;
            if (pages != null && pages.Count > 0)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    var page = pages[i];
                    if (page == null || page.Tags == null || page.Tags.Count == 0) { continue; }
                    if (page.GetType() == typeof(T) && page.Tags.Contains(tag))
                    {
                        T tElem = (T)(UIPage)page;
                        result = tElem;
                        break;
                    }
                }
            }
            return result;
        }

        public T GetFeatureByTags<T>(params UITag[] tags) where T : UIPage
        {
            T result = null;
            if (pages != null && pages.Count > 0)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    var page = pages[i];
                    if (page == null) { continue; }
                    if (page.GetType() == typeof(T) && HasFoundTags(tags, page))
                    {
                        T tElem = (T)(UIPage)page;
                        result = tElem;
                        break;
                    }
                }
            }
            return result;
        }

        public List<T> GetFeaturesByTags<T>(params UITag[] tags) where T : UIPage
        {
            List<T> result = new List<T>();
            if (pages != null && pages.Count > 0)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    var page = pages[i];
                    if (page == null) { continue; }
                    if (page.GetType() == typeof(T) && HasFoundTags(tags, page))
                    {
                        T tElem = (T)(UIPage)page;
                        result.Add(tElem);
                    }
                }
            }
            return result;
        }

        bool HasFoundTags(UITag[] tags_arg, UIPage page)
        {
            bool found = true;
            if (page.Tags != null && page.Tags.Count > 0)
            {
                if (tags_arg != null && tags_arg.Length > 0)
                {
                    for (int i = 0; i < tags_arg.Length; i++)
                    {
                        if (tags_arg[i] == null) { continue; }
                        if (page.Tags.Contains(tags_arg[i]) == false)
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

        // Update is called once per frame
        void Update()
        {
            if (shouldTick == false) { return; }
            Vector2 delta = Vector2.zero;
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                var touches = Input.touches;
                var tCount = 0;
                if (touches != null && touches.Length > 0)
                {
                    for (int i = 0; i < touches.Length; i++)
                    {
                        var t = touches[i];
                        if (t.phase == TouchPhase.Canceled ||
                            t.phase == TouchPhase.Ended ||
                            t.phase == TouchPhase.Stationary) { continue; }
                        delta += t.deltaPosition;
                        tCount++;
                    }
                }
                delta /= tCount;
            }
            else
            {
                Vector2 mPos = Input.mousePosition;
                delta = mPos - lastMousePos;
            }

            curDelta = delta;

            bDeltas[curbID] = delta;
            curbID++;
            var sDelta = Vector2.zero;
            for (int i = 0; i < deltaSmoothnessBufferSize; i++)
            {
                sDelta += bDeltas[i];
            }
            smoothDelta = sDelta / deltaSmoothnessBufferSize;
            if (curbID > bDeltas.Length - 1) { curbID = 0; }

            for (int i = 0; i < pages.Count; i++)
            {
                var page = pages[i];
                if (page.IsOpened)
                {
                    pages[i].OnTick(curDelta, smoothDelta);
                }   
            }
            OnTick();
        }
    }
}