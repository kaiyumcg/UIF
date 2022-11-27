using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    public static class BrowserExt
    {
        public static UIPage GetPageByName(this UIBrowser browser, string pageName)
        {
            UIPage page = null;
            for (int i = 0; i < browser.pages.Count; i++)
            {
                var uiPage = browser.pages[i];
                if (uiPage.PageName == pageName)
                {
                    page = uiPage;
                    break;
                }
            }
            return page;
        }

        public static T GetPage<T>(this UIBrowser browser) where T : UIPage
        {
            return browser.pages.GetExactTypedScript<T, UIPage>();
        }

        public static List<T> GetPages<T>(this UIBrowser browser) where T : UIPage
        {
            return browser.pages.GetExactTypedScripts<T, UIPage>();
        }

        public static T GetPageByTag<T>(this UIBrowser browser, UITag tag) where T : UIPage
        {
            var lst = new List<ITagable>();
            lst.AddRange(browser.pages);
            return lst.GetExactTypedScriptByTag<T>(tag);
        }

        public static T GetPageByTags<T>(this UIBrowser browser, params UITag[] tags) where T : UIPage
        {
            var lst = new List<ITagable>();
            lst.AddRange(browser.pages);
            var ts = new List<ScriptableObject>();
            ts.AddRange(tags);
            return lst.GetExactTypedScriptByTags<T>(ts);
        }

        public static List<T> GetPagesByTag<T>(this UIBrowser browser, UITag tag) where T : UIPage
        {
            var lst = new List<ITagable>();
            lst.AddRange(browser.pages);
            return lst.GetExactTypedScriptsByTag<T>(tag);
        }

        public static List<T> GetPagesByTags<T>(this UIBrowser browser, params UITag[] tags) where T : UIPage
        {
            var lst = new List<ITagable>();
            lst.AddRange(browser.pages);
            var ts = new List<ScriptableObject>();
            ts.AddRange(tags);
            return lst.GetExactTypedScriptsByTags<T>(ts);
        }
    }
}