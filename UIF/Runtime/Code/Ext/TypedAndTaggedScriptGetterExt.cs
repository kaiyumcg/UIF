using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    internal interface ITagable
    {
        internal List<UnityEngine.ScriptableObject> Tags { get; }
    }

    internal static class TypedAndTaggedScriptGetterExt
    {
        internal static ReturnType GetExactTypedScript<ReturnType, ListType>(this List<ListType> scripts)
            where ReturnType : MonoBehaviour
            where ListType : MonoBehaviour
        {
            ReturnType result = null;
            if (scripts != null && scripts.Count > 0)
            {
                for (int i = 0; i < scripts.Count; i++)
                {
                    var scr = scripts[i];
                    if (scr == null) { continue; }
                    if (scr.GetType() == typeof(ReturnType))
                    {
                        ReturnType tElem = (ReturnType)(MonoBehaviour)scr;
                        result = tElem;
                        break;
                    }
                }
            }
            return result;
        }

        internal static List<ReturnType> GetExactTypedScripts<ReturnType, ListType>(this List<ListType> scripts)
            where ReturnType : MonoBehaviour
            where ListType : MonoBehaviour
        {
            List<ReturnType> result = new List<ReturnType>();
            if (scripts != null && scripts.Count > 0)
            {
                for (int i = 0; i < scripts.Count; i++)
                {
                    var scr = scripts[i];
                    if (scr == null) { continue; }
                    if (scr.GetType() == typeof(ReturnType))
                    {
                        ReturnType tElem = (ReturnType)(MonoBehaviour)scr;
                        result.Add(tElem);
                    }
                }
            }
            return result;
        }

        internal static T GetExactTypedScriptByTag<T>(this List<ITagable> scripts, UnityEngine.ScriptableObject tag) where T : ITagable
        {
            T result = default;
            if (scripts != null && scripts.Count > 0)
            {
                for (int i = 0; i < scripts.Count; i++)
                {
                    var script = scripts[i];
                    if (script == null || script.Tags == null || script.Tags.Count == 0) { continue; }
                    if (script.GetType() == typeof(T) && script.Tags.Contains(tag))
                    {
                        T tElem = (T)(ITagable)script;
                        result = tElem;
                        break;
                    }
                }
            }
            return result;
        }

        internal static T GetExactTypedScriptByTags<T>(this List<ITagable> scripts, List<UnityEngine.ScriptableObject> tags) where T : ITagable
        {
            T result = default;
            if (scripts != null && scripts.Count > 0)
            {
                for (int i = 0; i < scripts.Count; i++)
                {
                    var script = scripts[i];
                    if (script == null) { continue; }
                    if (script.GetType() == typeof(T) && script.Tags.ContainsAll(tags))
                    {
                        T tElem = (T)(ITagable)script;
                        result = tElem;
                        break;
                    }
                }
            }
            return result;
        }

        internal static List<T> GetExactTypedScriptsByTag<T>(this List<ITagable> scripts, UnityEngine.ScriptableObject tag) where T : ITagable
        {
            List<T> result = new List<T>();
            if (scripts != null && scripts.Count > 0)
            {
                for (int i = 0; i < scripts.Count; i++)
                {
                    var script = scripts[i];
                    if (script == null) { continue; }
                    if (script.GetType() == typeof(T) && script.Tags.Contains(tag))
                    {
                        T tElem = (T)(ITagable)script;
                        result.Add(tElem);
                    }
                }
            }
            return result;
        }

        internal static List<T> GetExactTypedScriptsByTags<T>(this List<ITagable> scripts, List<UnityEngine.ScriptableObject> tags) where T : ITagable
        {
            List<T> result = new List<T>();
            if (scripts != null && scripts.Count > 0)
            {
                for (int i = 0; i < scripts.Count; i++)
                {
                    var script = scripts[i];
                    if (script == null) { continue; }
                    if (script.GetType() == typeof(T) && script.Tags.ContainsAll(tags))
                    {
                        T tElem = (T)(ITagable)script;
                        result.Add(tElem);
                    }
                }
            }
            return result;
        }
    }
}