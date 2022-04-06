using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    internal static class TypedHierarchyExt
    {
        internal static void GetOrUpdateChilds<ChildType, ParentType>
            (
            this ParentType script,
            bool childParentCoexist_Sameobject,
            bool multiChildOkOn_SameObject,
            bool parentHasNoOtherChildInstanceInTree,
            bool disallowAfterFirstLevelChildOfChildType,
            ref List<ChildType> result,
            ref bool error,
            ref string errorMsg
            )
            where ChildType : MonoBehaviour
            where ParentType : MonoBehaviour
        {
            if (typeof(ParentType) == typeof(ChildType))
            {
                error = true;
                errorMsg = "Wrong method for child update operation. " +
                    "If both types are same then please use single typed generic method instead of this one.";
                result = new List<ChildType>();
                return;
            }

            List<ChildType> toAdd = new List<ChildType>();
            var pScrs = script.GetComponents<ParentType>();
            if (pScrs.Length > 1)
            {
                error = true;
                errorMsg = "Multiple " + typeof(ParentType).Name + " instances on the same gameObject named: " + script.gameObject.name;
                result = new List<ChildType>();
                return;
            }

            if (parentHasNoOtherChildInstanceInTree)
            {
                ParentType[] childsOfParentType = script.GetComponentsInChildren<ParentType>();
                if (childsOfParentType != null && childsOfParentType.Length > 1)
                {
                    error = true;
                    errorMsg = "Multiple " + typeof(ParentType).Name + " instances found in the tree of gameObject named: "
                        + script.gameObject.name + " This is NOT allowed! You can only have one master type";
                    result = new List<ChildType>();
                    return;
                }
            }

            if (childParentCoexist_Sameobject)
            {
                var childs = script.GetComponents<ChildType>();
                if (multiChildOkOn_SameObject)
                {
                    if (childs != null && childs.Length > 0)
                    {
                        for (int i = 0; i < childs.Length; i++)
                        {
                            toAdd.Add(childs[i]);
                        }
                    }
                }
                else
                {
                    if (childs != null && childs.Length > 1)
                    {
                        error = true;
                        errorMsg = "Multiple " + typeof(ChildType).Name + " instances on the same gameObject named: " + script.gameObject.name;
                        result = new List<ChildType>();
                        return;
                    }
                    else
                    {
                        var child = script.GetComponent<ChildType>();
                        if (child != null)
                        {
                            toAdd.Add(child);
                        }
                    }
                }
            }
            else
            {
                var childScripts = script.GetComponents<ChildType>();
                if (childScripts.Length > 0)
                {
                    error = true;
                    errorMsg = typeof(ChildType).Name + " Type can not coexist with " + typeof(ParentType).Name + " on the same gameObject.";
                    result = new List<ChildType>();
                    return;
                }
            }

            if (!(disallowAfterFirstLevelChildOfChildType && toAdd.Count > 0))
            {
                UpdateList(script.transform, ref toAdd, ref error, ref errorMsg);
            }
            if (toAdd == null) { toAdd = new List<ChildType>(); }
            if (result == null) { result = new List<ChildType>(); }
            result.RemoveAll((item) => { return item == null || toAdd.Contains(item) == false; });
            if (result == null) { result = new List<ChildType>(); }
            for (int i = 0; i < toAdd.Count; i++)
            {
                var it = toAdd[i];
                if (it == null) { continue; }
                if (result.Contains(it) == false)
                {
                    result.Add(it);
                }
            }
            CollectionExt.EnforceUniqueness(ref result);

            void UpdateList(Transform t, ref List<ChildType> list, ref bool error, ref string errorMsg)
            {
                if (error) { return; }
                var count = t.childCount;
                for (int i = 0; i < count; i++)
                {
                    var tChild = t.GetChild(i);
                    var parentScript = tChild.GetComponent<ParentType>();
                    var parentScripts = tChild.GetComponents<ParentType>();
                    if (parentScripts.Length > 1)
                    {
                        error = true;
                        errorMsg += "Multiple " + typeof(ParentType).Name + " instances on the same gameObject named: "
                            + tChild.gameObject.name + " Current Child-Parent relationship allows only one!"
                            + System.Environment.NewLine;
                        list = new List<ChildType>();
                        return;
                    }

                    if (parentScript == null)
                    {
                        var childScripts = tChild.GetComponents<ChildType>();
                        if (multiChildOkOn_SameObject)
                        {
                            if (childScripts != null && childScripts.Length > 0)
                            {
                                for (int c = 0; c < childScripts.Length; c++)
                                {
                                    var child = childScripts[c];
                                    if (list.Contains(child) == false)
                                    {
                                        list.Add(child);
                                    }
                                }
                            }
                            
                            if (!(disallowAfterFirstLevelChildOfChildType && childScripts.Length > 0))
                            {
                                UpdateList(tChild, ref list, ref error, ref errorMsg);
                            }
                        }
                        else
                        {
                            if (childScripts != null && childScripts.Length > 1)
                            {
                                error = true;
                                errorMsg += "Multiple " + typeof(ChildType).Name + " instances on the same gameObject named: "
                                    + tChild.gameObject.name + " Current Child-Parent relationship allows only one!"
                                    + System.Environment.NewLine;
                                return;
                            }
                            else
                            {
                                var child = tChild.GetComponent<ChildType>();
                                if (child != null && list.Contains(child) == false)
                                {
                                    list.Add(child);
                                }
                                if (!(disallowAfterFirstLevelChildOfChildType && child != null))
                                {
                                    UpdateList(tChild, ref list, ref error, ref errorMsg);
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static void GetOrUpdateChilds<T>
            (
            this T script,
            ref List<T> result,
            ref bool error,
            ref string errorMsg
            )
            where T : MonoBehaviour
        {
            List<T> toAdd = new List<T>();
            var pScrs = script.GetComponents<T>();
            if (pScrs.Length > 1)
            {
                error = true;
                errorMsg = "Multiple " + typeof(T).Name + " instances on the root gameObject named: " + script.gameObject.name;
                result = new List<T>();
                return;
            }

            UpdateList(script.transform, ref toAdd, ref error, ref errorMsg);

            if (toAdd == null) { toAdd = new List<T>(); }
            if (result == null) { result = new List<T>(); }
            result.RemoveAll((item) => { return item == null || toAdd.Contains(item) == false; });
            if (result == null) { result = new List<T>(); }
            for (int i = 0; i < toAdd.Count; i++)
            {
                var it = toAdd[i];
                if (it == null) { continue; }
                if (result.Contains(it) == false)
                {
                    result.Add(it);
                }
            }
            CollectionExt.EnforceUniqueness(ref result);

            void UpdateList(Transform t, ref List<T> list, ref bool error, ref string errorMsg)
            {
                if (error) { return; }
                var count = t.childCount;
                for (int i = 0; i < count; i++)
                {
                    var tChild = t.GetChild(i);
                    var childScript = tChild.GetComponent<T>();
                    var childScripts = tChild.GetComponents<T>();
                    if (childScripts.Length > 1)
                    {
                        error = true;
                        errorMsg += "Multiple " + typeof(T).Name + " instances on the same gameObject named: "
                            + tChild.gameObject.name + " Current Child-Parent relationship allows only one!"
                            + System.Environment.NewLine;
                        list = new List<T>();
                        return;
                    }

                    if(childScript == null)
                    {
                        UpdateList(tChild, ref list, ref error, ref errorMsg);
                    }
                    else
                    {
                        if (list.Contains(childScript) == false)
                        {
                            list.Add(childScript);
                        }
                    }
                }
            }
        }
    }
}