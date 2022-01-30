using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace UIF
{
    public static class Ext
    {
        internal static List<string> GetPagesName(this string url)
        {
            if (url.Contains("http://")) { url = url.Replace("http://", ""); }
            if (url.Contains("ui://")) { url = url.Replace("ui://", ""); }
            if (url.Contains(@"\")) { url = url.Replace(@"\","/"); }
            var parts = Regex.Split(url, "/");
            List<string> pageNames = new List<string>();
            if (parts != null && parts.Length > 0)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];
                    if (string.IsNullOrEmpty(part) || string.IsNullOrWhiteSpace(part)) { continue; }
                    pageNames.Add(part);
                }
            }
            return pageNames;
        }

        internal static void SetActiveAll(this List<GameObject> gameObjects, bool active)
        {
            if (gameObjects != null && gameObjects.Count > 0)
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    var g = gameObjects[i];
                    if (g == null) { continue; }
                    if (g.activeInHierarchy == !active)
                    {
                        g.SetActive(active);
                    }
                }
            }
        }

        internal static void ApplyColorAll(this List<ColoredSelection> coloredUIs, bool active, float tweenDuration)
        {
            if (coloredUIs != null && coloredUIs.Count > 0)
            {
                for (int i = 0; i < coloredUIs.Count; i++)
                {
                    var colUI = coloredUIs[i];
                    if (colUI == null) { continue; }
                    colUI.Apply(active, tweenDuration);
                }
            }
        }

        //todo, get element by tag like web
        public static List<T> GetElement<T>(this UIPage page) where T : UIPageElement
        {
            List<T> result = new List<T>();
            UpdateList(page._Transform, ref result);
            return result;
            void UpdateList<T>(Transform t, ref List<T> result) where T : Behaviour
            {
                var count = t.childCount;
                for (int i = 0; i < count; i++)
                {
                    var tChild = t.GetChild(i);
                    var tcom = tChild.GetComponent<T>();
                    var uiPage = tChild.GetComponent<UIPage>();
                    if (tcom != null)
                    {
                        if (uiPage != null && uiPage != page)
                        {
                            //other page, ignore
                        }
                        else
                        {
                            if (result == null) { result = new List<T>(); }
                            result.Add(tcom);
                            UpdateList(tChild, ref result);
                        }
                    }
                }
            }
        }
    }
}