using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    internal enum TransformUniqueNameMode { TransformTreeForFixed = 0, ManualUniqueName = 1 }

    internal static class TransformExt
    {
        internal static string GetUniqueNameForTransform(this Transform transform,
            TransformUniqueNameMode keyingMode, string defaultUniqueName, string overridableUniqueName)
        {
            var key = "";
            if (keyingMode == TransformUniqueNameMode.TransformTreeForFixed) { key = GetTransformPath(transform); }
            else if (keyingMode == TransformUniqueNameMode.ManualUniqueName)
            {
                var fromOverride = overridableUniqueName;
                if (string.IsNullOrEmpty(fromOverride) || string.IsNullOrWhiteSpace(fromOverride))
                {
                    key = defaultUniqueName;
                }
                else
                {
                    key = fromOverride;
                }

                if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                {
                    throw new Exception("You intent to save the transform state to device but the key for it is empty!. " +
                        "Consider switching to TransformTree or provide an unique name.");
                }
            }
            return key;
            string GetTransformPath(Transform root)
            {
                string result = "";
                var tr = root;
                result += ("~" + root.name);
                while (tr != null)
                {
                    tr = tr.parent;
                    Canvas canvas = null;
                    if (tr != null)
                    {
                        canvas = tr.GetComponent<Canvas>();
                    }
                    if (canvas != null)
                    {
                        tr = null;
                    }
                    if (tr != null)
                    {
                        result += ("~" + tr.name);
                    }
                }
                return result;
            }
        }
    }
}