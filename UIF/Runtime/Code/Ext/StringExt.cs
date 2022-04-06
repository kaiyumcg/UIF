using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    public static class StringExt
    {
        public static bool IsStringValid(this string str)
        {
            return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        }
    }
}