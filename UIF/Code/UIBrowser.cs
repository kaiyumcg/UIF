using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    public sealed class UIBrowser : MonoBehaviour
    {
        [SerializeField] List<UIPage> pages;
        [SerializeField] int deltaSmoothnessBufferSize = 4;
        [SerializeField, HideInInspector] Vector2 lastMousePos;
        [SerializeField, HideInInspector] int curbID = 0;
        [SerializeField, HideInInspector] Vector2[] bDeltas;
        Vector2 curDelta, smoothDelta;

        private void Awake()
        {
            LoadPagesifReq();
            if (pages != null && pages.Count > 0)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    pages[i].OnInit();
                }
            }
        }

        public void OpenUIURL(string url, bool useAnimation)
        {
            var pNames = url.GetPagesName();
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

            page.Open(useAnimation);
        }

        //dynamic hole ki ki hobe. register/unregister? 
        //todo, get element by tag like web
        //r&d on game UI
        //cross, back, forward (UX implication)
        //open homepage?
        //Create UIPage, UIElement scripts by context menu
        //Editor code for "Create UI Browser"
        //Potential for server side execution?
        //Redesign-->Browser runs pages-->page runs "UIActor"-->UIActor runs "UIPageElement"


        internal void LoadPagesifReq()
        {
            var ps = GetComponentsInChildren<UIPage>();
            if (pages == null) { pages = new List<UIPage>(); }
            pages.RemoveAll((p) =>
            {
                int matchCount = 0;
                if (p != null)
                {
                    for (int i = 0; i < pages.Count; i++)
                    {
                        var p2 = pages[i];
                        if (p2 == null) { continue; }
                        if (p2 == p)
                        {
                            matchCount++;
                        }
                    }
                }
                return matchCount > 1 || p == null;
            });
            if (pages == null) { pages = new List<UIPage>(); }
            if (ps != null && ps.Length > 0)
            {
                for (int i = 0; i < ps.Length; i++)
                {
                    var p = ps[i];
                    if (pages.Contains(p) == false) { pages.Add(p); }
                }
            }
            if (pages == null) { pages = new List<UIPage>(); }
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

        // Update is called once per frame
        void Update()
        {
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
        }
    }
}