using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UIF
{
    public static class Util
    {
        internal static void AddAndMakeOverallUnique<T>(List<T> toAdd, ref List<T> collection) where T : class
        {
            if (collection == null) { collection = new List<T>(); }
            var pgComp = collection;
            collection.RemoveAll((p) =>
            {
                int matchCount = 0;
                if (p != null)
                {
                    for (int i = 0; i < pgComp.Count; i++)
                    {
                        var p2 = pgComp[i];
                        if (p2 == null) { continue; }
                        if (p2 == p)
                        {
                            matchCount++;
                        }
                    }
                }
                return matchCount > 1 || p == null;
            });

            if (collection == null) { collection = new List<T>(); }
            if (toAdd != null && toAdd.Count > 0)
            {
                for (int i = 0; i < toAdd.Count; i++)
                {
                    var p = toAdd[i];
                    if (collection.Contains(p) == false) { collection.Add(p); }
                }
            }
            if (collection == null) { collection = new List<T>(); }
        }

        internal static void AddAndMakeOverallUnique<T>(T[] toAdd, ref List<T> collection) where T : class
        {
            if (collection == null) { collection = new List<T>(); }
            var pgComp = collection;
            collection.RemoveAll((p) =>
            {
                int matchCount = 0;
                if (p != null)
                {
                    for (int i = 0; i < pgComp.Count; i++)
                    {
                        var p2 = pgComp[i];
                        if (p2 == null) { continue; }
                        if (p2 == p)
                        {
                            matchCount++;
                        }
                    }
                }
                return matchCount > 1 || p == null;
            });

            if (collection == null) { collection = new List<T>(); }
            if (toAdd != null && toAdd.Length > 0)
            {
                for (int i = 0; i < toAdd.Length; i++)
                {
                    var p = toAdd[i];
                    if (collection.Contains(p) == false) { collection.Add(p); }
                }
            }
            if (collection == null) { collection = new List<T>(); }
        }

        internal static List<string> GetPagesNameFromUrl(string url)
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
                    if (string.IsNullOrEmpty(part) || string.IsNullOrWhiteSpace(part)) { continue; }
                    pageNames.Add(part);
                }
            }
            return pageNames;
        }
    }
}