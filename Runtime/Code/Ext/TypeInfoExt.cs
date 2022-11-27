using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    internal static class TypeInfoExt
    {
        //https://stackoverflow.com/questions/2742276/how-do-i-check-if-a-type-is-a-subtype-or-the-type-of-an-object
        internal static bool IsSameOrSubclass(this Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }

        internal static bool IsSameOrSubOrSuper(this Type type1, Type type2)
        {
            return type1.IsSameOrSubclass(type2) || type2.IsSameOrSubclass(type1);
        }
    }
}