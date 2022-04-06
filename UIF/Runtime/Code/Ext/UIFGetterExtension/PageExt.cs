using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    public static class PageExt
    {
        #region ChildPages
        public static T GetChildPage<T>(this UIPage page) where T : UIPage
        {
            return page.childPages.GetExactTypedScript<T, UIPage>();
        }

        public static List<T> GetChildPages<T>(this UIPage page) where T : UIPage
        {
            return page.childPages.GetExactTypedScripts<T, UIPage>();
        }

        public static T GetChildPageByTag<T>(this UIPage page, UITag tag) where T : UIPage
        {
            var lst = new List<ITagable>();
            lst.AddRange(page.childPages);
            return lst.GetExactTypedScriptByTag<T>(tag);
        }

        public static T GetChildPageByTags<T>(this UIPage page, params UITag[] tags) where T : UIPage
        {
            var lst = new List<ITagable>();
            lst.AddRange(page.childPages);
            var ts = new List<ScriptableObject>();
            ts.AddRange(tags);
            return lst.GetExactTypedScriptByTags<T>(ts);
        }

        public static List<T> GetChildPagesByTag<T>(this UIPage page, UITag tag) where T : UIPage
        {
            var lst = new List<ITagable>();
            lst.AddRange(page.childPages);
            return lst.GetExactTypedScriptsByTag<T>(tag);
        }

        public static List<T> GetChildPagesByTags<T>(this UIPage page, params UITag[] tags) where T : UIPage
        {
            var lst = new List<ITagable>();
            lst.AddRange(page.childPages);
            var ts = new List<ScriptableObject>();
            ts.AddRange(tags);
            return lst.GetExactTypedScriptsByTags<T>(ts);
        }
        #endregion

        #region Elements
        public static List<T> GetAllElementsAndBelow<T>(this UIPage page) where T : UIPageElement
        {
            List<T> result = new List<T>();
            AddTo(ref result, page);
            return result;
            void AddTo(ref List<T> lst, UIPage _p)
            {
                var features = _p.GetElements<T>();
                lst.AddUniquely(features);

                var childPages = _p.childPages;
                if (childPages != null && childPages.Count > 0)
                {
                    for (int i = 0; i < childPages.Count; i++)
                    {
                        var ch = childPages[i];
                        if (ch == null) { continue; }
                        AddTo(ref lst, ch);
                    }
                }
            }
        }

        public static List<T> GetAllElementsAndBelowByTag<T>(this UIPage page, UITag tag) where T : UIPageElement
        {
            List<T> result = new List<T>();
            AddTo(ref result, page);
            return result;
            void AddTo(ref List<T> lst, UIPage _p)
            {
                var elements = _p.GetElementsByTag<T>(tag);
                lst.AddUniquely(elements);

                var childPages = _p.childPages;
                if (childPages != null && childPages.Count > 0)
                {
                    for (int i = 0; i < childPages.Count; i++)
                    {
                        var ch = childPages[i];
                        if (ch == null) { continue; }
                        AddTo(ref lst, ch);
                    }
                }
            }
        }

        public static List<T> GetAllElementsAndBelowByTag<T>(this UIPage page, params UITag[] tags) where T : UIPageElement
        {
            List<T> result = new List<T>();
            AddTo(ref result, page);
            return result;
            void AddTo(ref List<T> lst, UIPage _p)
            {
                var elements = _p.GetElementsByTags<T>(tags);
                lst.AddUniquely(elements);

                var childPages = _p.childPages;
                if (childPages != null && childPages.Count > 0)
                {
                    for (int i = 0; i < childPages.Count; i++)
                    {
                        var ch = childPages[i];
                        if (ch == null) { continue; }
                        AddTo(ref lst, ch);
                    }
                }
            }
        }

        public static T GetElement<T>(this UIPage page) where T : UIPageElement
        {
            return page.elements.GetExactTypedScript<T, UIPageElement>();
        }

        public static List<T> GetElements<T>(this UIPage page) where T : UIPageElement
        {
            return page.elements.GetExactTypedScripts<T, UIPageElement>();
        }

        public static T GetElementByTag<T>(this UIPage page, UITag tag) where T : UIPageElement
        {
            var lst = new List<ITagable>();
            lst.AddRange(page.elements);
            return lst.GetExactTypedScriptByTag<T>(tag);
        }

        public static T GetElementByTags<T>(this UIPage page, params UITag[] tags) where T : UIPageElement
        {
            var lst = new List<ITagable>();
            lst.AddRange(page.elements);
            var ts = new List<ScriptableObject>();
            ts.AddRange(tags);
            return lst.GetExactTypedScriptByTags<T>(ts);
        }

        public static List<T> GetElementsByTag<T>(this UIPage page, UITag tag) where T : UIPageElement
        {
            var lst = new List<ITagable>();
            lst.AddRange(page.elements);
            return lst.GetExactTypedScriptsByTag<T>(tag);
        }

        public static List<T> GetElementsByTags<T>(this UIPage page, params UITag[] tags) where T : UIPageElement
        {
            var lst = new List<ITagable>();
            lst.AddRange(page.elements);
            var ts = new List<ScriptableObject>();
            ts.AddRange(tags);
            return lst.GetExactTypedScriptsByTags<T>(ts);
        }
        #endregion

        #region Features
        public static List<T> GetAllFeaturesAndBelow<T>(this UIPage page) where T : UIPageElementFeature
        {
            List<T> result = new List<T>();
            AddTo(ref result, page);
            return result;
            void AddTo(ref List<T> lst, UIPage _p)
            {
                var elements = _p.elements;
                if (elements != null && elements.Count > 0)
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        var element = elements[i];
                        if (element == null) { continue; }
                        var features = element.features.GetExactTypedScripts<T, UIPageElementFeature>();
                        lst.AddUniquely(features);
                    }
                }

                var childPages = _p.childPages;
                if (childPages != null && childPages.Count > 0)
                {
                    for (int i = 0; i < childPages.Count; i++)
                    {
                        var ch = childPages[i];
                        if (ch == null) { continue; }
                        AddTo(ref lst, ch);
                    }
                }
            }
        }

        public static List<T> GetAllFeaturesAndBelowByTag<T>(this UIPage page, UITag tag) where T : UIPageElementFeature
        {
            List<T> result = new List<T>();
            AddTo(ref result, page);
            return result;
            void AddTo(ref List<T> lst, UIPage _p)
            {
                var features = _p.GetFeaturesByTag<T>(tag);
                lst.AddUniquely(features);

                var childPages = _p.childPages;
                if (childPages != null && childPages.Count > 0)
                {
                    for (int i = 0; i < childPages.Count; i++)
                    {
                        var ch = childPages[i];
                        if (ch == null) { continue; }
                        AddTo(ref lst, ch);
                    }
                }
            }
        }

        public static List<T> GetAllFeaturesAndBelowByTags<T>(this UIPage page, params UITag[] tags) where T : UIPageElementFeature
        {
            List<T> result = new List<T>();
            AddTo(ref result, page);
            return result;
            void AddTo(ref List<T> lst, UIPage _p)
            {
                var features = _p.GetFeaturesByTags<T>(tags);
                lst.AddUniquely(features);

                var childPages = _p.childPages;
                if (childPages != null && childPages.Count > 0)
                {
                    for (int i = 0; i < childPages.Count; i++)
                    {
                        var ch = childPages[i];
                        if (ch == null) { continue; }
                        AddTo(ref lst, ch);
                    }
                }
            }
        }

        public static T GetFeature<T>(this UIPage page) where T : UIPageElementFeature
        {
            T result = null;
            var elements = page.elements;
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];
                    if (element == null) { continue; }
                    result = element.features.GetExactTypedScript<T, UIPageElementFeature>();
                    if (result != null) { break; }
                }
            }
            return result;
        }

        public static List<T> GetFeatures<T>(this UIPage page) where T : UIPageElementFeature
        {
            List<T> result = new List<T>();
            var elements = page.elements;
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];
                    if (element == null) { continue; }
                    var features = element.features.GetExactTypedScripts<T, UIPageElementFeature>();
                    result.AddUniquely(features);
                }
            }
            return result;
        }

        public static T GetFeatureByTag<T>(this UIPage page, UITag tag) where T : UIPageElementFeature
        {
            T result = null;
            var elements = page.elements;
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];
                    if (element == null) { continue; }
                    var lst = new List<ITagable>();
                    lst.AddRange(element.features);
                    result = lst.GetExactTypedScriptByTag<T>(tag);
                    if (result != null) { break; }
                }
            }
            return result;
        }

        public static T GetFeatureByTags<T>(this UIPage page, params UITag[] tags) where T : UIPageElementFeature
        {
            T result = null;
            var elements = page.elements;
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];
                    if (element == null) { continue; }
                    var lst = new List<ITagable>();
                    lst.AddRange(element.features);
                    var ts = new List<ScriptableObject>();
                    ts.AddRange(tags);
                    result = lst.GetExactTypedScriptByTags<T>(ts);
                    if (result != null) { break; }
                }
            }
            return result;
        }

        public static List<T> GetFeaturesByTag<T>(this UIPage page, UITag tag) where T : UIPageElementFeature
        {
            List<T> result = new List<T>();
            var elements = page.elements;
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];
                    if (element == null) { continue; }
                    var lst = new List<ITagable>();
                    lst.AddRange(element.features);

                    var features = lst.GetExactTypedScriptsByTag<T>(tag);
                    result.AddUniquely(features);
                }
            }
            return result;
        }

        public static List<T> GetFeaturesByTags<T>(this UIPage page, params UITag[] tags) where T : UIPageElementFeature
        {
            List<T> result = new List<T>();
            var elements = page.elements;
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];
                    if (element == null) { continue; }
                    var lst = new List<ITagable>();
                    lst.AddRange(element.features);

                    var ts = new List<ScriptableObject>();
                    ts.AddRange(tags);

                    var features = lst.GetExactTypedScriptsByTags<T>(ts);
                    result.AddUniquely(features);
                }
            }
            return result;
        }
        #endregion
    }
}