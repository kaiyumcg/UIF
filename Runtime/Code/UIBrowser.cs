using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AttributeExt2;

/// <summary>
/// Todos
/// Solution for builtin tween animation(KTween?)
/// Child class Script template for framework components
/// UIF setting window and code generation for UIFSetting.cs
/// UIF Debugger for showing current UIF related activities, information etc If applicable
/// </summary>
namespace UIF
{
    [DisallowMultipleComponent]
    [AddComponentMenu("UIF/UI Browser")]
    public class UIBrowser : UIFAttachableObject
    {
        [SerializeField, HideInInspector] protected bool shouldTick = true;
        [SerializeField, HideInInspector] protected string homePage;
        [SerializeField, HideInInspector] public UnityEvent onOpenHomePage;
        [SerializeField, ReadOnly, HideInInspector] internal protected List<UIPage> pages;

        protected override void OnLoadReference()
        {
            this.GetOrUpdateChilds<UIPage, UIBrowser>(false, false, true, false, ref pages, ref configError, ref configErrorMessage);
        }

        internal override void OnInitBaseFW()
        {
            base.OnInitBaseFW();
            if (pages != null && pages.Count > 0)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    pages[i].OnInitPage(this);
                }
            }
        }

        private void Awake()
        {
            var ld = (ILoadReference)this;
            if (ld != null) { ld.LoadReference(); }
            if (ld != null) { ld.InitIfRequired(); }
        }
        protected virtual void OnTick() { }

        public void OpenBrowser()
        {
            OpenUIURL(homePage);
            onOpenHomePage?.Invoke();
        }

        public void CloseAllOpenedPage()
        {
            for (int i = 0; i < pages.Count; i++)
            {
                var page = pages[i];
                page.Close();
            }
        }

        public void OpenUIURL(string url)
        {
            var pNames = url.GetPagesNameFromUrl();
            if (pNames != null && pNames.Count > 0)
            {
                for (int i = 0; i < pNames.Count; i++)
                {
                    var pageName = pNames[i];
                    OpenPage(pageName);
                }
            }
        }

        public void OpenPage(string pageName)
        {
            UIPage page = this.GetPageByName(pageName);
            page.Open();
        }

        void Update()
        {
            if (shouldTick == false) { return; }

            for (int i = 0; i < pages.Count; i++)
            {
                var page = pages[i];
                if (page.IsOpened)
                {
                    pages[i].OnTick();
                }
            }
            OnTick();
        }
    }
}