using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    internal static class CollectionExt
    {
        internal static bool ContainsAll<T>(this List<T> actList, T[] list) where T : class
        {
            int tCount = -1, existCount = -1;
            if (actList != null && actList.Count > 0 && list != null && list.Length > 0)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    var t = list[i];
                    if (t == null) { continue; }
                    tCount++;
                    if (actList.Contains(t))
                    {
                        existCount++;
                    }
                }
            }
            return tCount >= 0 && existCount >= 0 && tCount == existCount;
        }

        internal static bool ContainsAll<T>(this T[] actList, List<T> list) where T : class
        {
            int tCount = -1, existCount = -1;
            if (actList != null && actList.Length > 0 && list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var t = list[i];
                    if (t == null) { continue; }
                    tCount++;
                    if (actList.Contains(t))
                    {
                        existCount++;
                    }
                }
            }
            return tCount >= 0 && existCount >= 0 && tCount == existCount;
        }

        internal static bool ContainsAll<T>(this List<T> actList, List<T> list) where T : class
        {
            int tCount = -1, existCount = -1;
            if (actList != null && actList.Count > 0 && list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var t = list[i];
                    if (t == null) { continue; }
                    tCount++;
                    if (actList.Contains(t))
                    {
                        existCount++;
                    }
                }
            }
            return tCount >= 0 && existCount >= 0 && tCount == existCount;
        }

        internal static bool ContainsAll<T>(this T[] actList, T[] list) where T : class
        {
            int tCount = -1, existCount = -1;
            if (actList != null && actList.Length > 0 && list != null && list.Length > 0)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    var t = list[i];
                    if (t == null) { continue; }
                    tCount++;
                    if (actList.Contains(t))
                    {
                        existCount++;
                    }
                }
            }
            return tCount >= 0 && existCount >= 0 && tCount == existCount;
        }

        internal static bool ContainsAny<T>(this List<T> actList, T[] list) where T : class
        {
            bool contains = false;
            if (actList != null && actList.Count > 0 && list != null && list.Length > 0)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    var t = list[i];
                    if (t == null) { continue; }
                    if (actList.Contains(t))
                    {
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }

        internal static bool ContainsAny<T>(this T[] actList, List<T> list) where T : class
        {
            bool contains = false;
            if (actList != null && actList.Length > 0 && list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var t = list[i];
                    if (t == null) { continue; }
                    if (actList.Contains(t))
                    {
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }

        internal static bool ContainsAny<T>(this List<T> actList, List<T> list) where T : class
        {
            bool contains = false;
            if (actList != null && actList.Count > 0 && list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var t = list[i];
                    if (t == null) { continue; }
                    if (actList.Contains(t))
                    {
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }

        internal static bool ContainsAny<T>(this T[] actList, T[] list) where T : class
        {
            bool contains = false;
            if (actList != null && actList.Length > 0 && list != null && list.Length > 0)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    var t = list[i];
                    if (t == null) { continue; }
                    if (actList.Contains(t))
                    {
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }

        internal static bool Contains<T>(this T[] list, T data) where T : class
        {
            var contains = false;
            if (list != null && list.Length > 0)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    var l = list[i];
                    if (l == null) { continue; }
                    if (l == data)
                    {
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }

        internal static void AddUniquely<T>(this List<T> result, List<T> toAdd) where T : class
        {
            if (result == null) { result = new List<T>(); }
            if (toAdd != null && toAdd.Count > 0)
            {
                for (int i = 0; i < toAdd.Count; i++)
                {
                    var t = toAdd[i];
                    if (t == null) { continue; }
                    if (result.Contains(t) == false)
                    {
                        result.Add(t);
                    }
                }
            }
        }

        internal static void AddUniquely<T>(ref T[] result, T[] toAdd) where T : class
        {
            var _result = new List<T>();
            _result.AddRange(result);
            if (toAdd != null && toAdd.Length > 0)
            {
                for (int i = 0; i < toAdd.Length; i++)
                {
                    var t = toAdd[i];
                    if (t == null) { continue; }
                    if (_result.Contains(t) == false)
                    {
                        _result.Add(t);
                    }
                }
            }
            result = _result.ToArray();
        }

        internal static void AddUniquely<T>(ref T[] result, List<T> toAdd) where T : class
        {
            var _result = new List<T>();
            _result.AddRange(result);
            if (toAdd != null && toAdd.Count > 0)
            {
                for (int i = 0; i < toAdd.Count; i++)
                {
                    var t = toAdd[i];
                    if (t == null) { continue; }
                    if (_result.Contains(t) == false)
                    {
                        _result.Add(t);
                    }
                }
            }
            result = _result.ToArray();
        }

        internal static void AddUniquely<T>(this List<T> result, T[] toAdd) where T : class
        {
            if (toAdd != null && toAdd.Length > 0)
            {
                for (int i = 0; i < toAdd.Length; i++)
                {
                    var t = toAdd[i];
                    if (t == null) { continue; }
                    if (result.Contains(t) == false)
                    {
                        result.Add(t);
                    }
                }
            }
        }

        internal static void EnforceUniqueness<T>(ref List<T> input) where T : class
        {
            var result = new List<T>();
            if (input != null && input.Count > 0)
            {
                for (int i = 0; i < input.Count; i++)
                {
                    var t = input[i];
                    if (t == null) { continue; }
                    if (result.Contains(t) == false)
                    {
                        result.Add(t);
                    }
                }
            }
            input = result;
        }
    }
}