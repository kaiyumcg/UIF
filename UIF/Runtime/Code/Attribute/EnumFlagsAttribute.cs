using UnityEngine;
using System;
using System.Collections.Generic;

namespace UIF
{
    //https://forum.unity.com/threads/multiple-enum-select-from-inspector.184729/
    public sealed class EnumFlagsAttribute : PropertyAttribute
    {
        public EnumFlagsAttribute() { }

        public static List<int> GetSelectedIndexes<T>(T val) where T : IConvertible
        {
            List<int> selectedIndexes = new List<int>();
            for (int i = 0; i < System.Enum.GetValues(typeof(T)).Length; i++)
            {
                int layer = 1 << i;
                if ((Convert.ToInt32(val) & layer) != 0)
                {
                    selectedIndexes.Add(i);
                }
            }
            return selectedIndexes;
        }

        public static List<T> GetSelectedItems<T>(T val) where T : IConvertible
        {
            List<T> items = new List<T>();
            for (int i = 0; i < System.Enum.GetValues(typeof(T)).Length; i++)
            {
                int layer = 1 << i;
                if ((Convert.ToInt32(val) & layer) != 0)
                {
                    var item = (T)(object)i;
                    items.Add(item);
                }
            }
            return items;
        }

        public static List<string> GetSelectedStrings<T>(T val) where T : IConvertible
        {
            List<string> selectedStrings = new List<string>();
            for (int i = 0; i < Enum.GetValues(typeof(T)).Length; i++)
            {
                int layer = 1 << i;
                if ((Convert.ToInt32(val) & layer) != 0)
                {
                    selectedStrings.Add(Enum.GetValues(typeof(T)).GetValue(i).ToString());
                }
            }
            return selectedStrings;
        }
    }
}