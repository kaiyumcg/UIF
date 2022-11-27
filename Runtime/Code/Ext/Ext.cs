using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UIF
{
    public static class Ext
    {
        public static void SetAllActiveUI<T>(this List<T> ui, bool activate) where T : IActivation
        {
            if (ui != null && ui.Count > 0)
            {
                for (int i = 0; i < ui.Count; i++)
                {
                    var u = ui[i];
                    if (u == null) { continue; }
                    u.SetActiveUI(activate);
                }
            }
        }
        internal static List<string> GetPagesNameFromUrl(this string url)
        {
            if (url.Contains("http://")) { url = url.Replace("http://", ""); }
            if (url.Contains("ui://")) { url = url.Replace("ui://", ""); }
            if (url.Contains(@"\")) { url = url.Replace(@"\", "/"); }
            var parts = Regex.Split(url, "/");
            List<string> pageNames = new List<string>();
            if (parts != null && parts.Length > 0)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];
                    if (part.IsStringValid() == false) { continue; }
                    pageNames.Add(part);
                }
            }
            return pageNames;
        }
    }
}