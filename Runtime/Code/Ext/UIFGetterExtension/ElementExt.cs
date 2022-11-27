using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    public static class ElementExt
    {
        public static T GetFeature<T>(this UIPageElement element) where T : UIPageElementFeature
        {
            return element.features.GetExactTypedScript<T, UIPageElementFeature>();
        }

        public static List<T> GetFeatures<T>(this UIPageElement element) where T : UIPageElementFeature
        {
            return element.features.GetExactTypedScripts<T, UIPageElementFeature>();
        }

        public static T GetFeatureByTag<T>(this UIPageElement element, UITag tag) where T : UIPageElementFeature
        {
            var lst = new List<ITagable>();
            lst.AddRange(element.features);
            return lst.GetExactTypedScriptByTag<T>(tag);
        }

        public static T GetFeatureByTags<T>(this UIPageElement element, params UITag[] tags) where T : UIPageElementFeature
        {
            var lst = new List<ITagable>();
            lst.AddRange(element.features);
            var ts = new List<ScriptableObject>();
            ts.AddRange(tags);
            return lst.GetExactTypedScriptByTags<T>(ts);
        }

        public static List<T> GetFeaturesByTag<T>(this UIPageElement element, UITag tag) where T : UIPageElementFeature
        {
            var lst = new List<ITagable>();
            lst.AddRange(element.features);
            return lst.GetExactTypedScriptsByTag<T>(tag);
        }

        public static List<T> GetFeaturesByTags<T>(this UIPageElement element, params UITag[] tags) where T : UIPageElementFeature
        {
            var lst = new List<ITagable>();
            lst.AddRange(element.features);
            var ts = new List<ScriptableObject>();
            ts.AddRange(tags);
            return lst.GetExactTypedScriptsByTags<T>(ts);
        }
    }
}