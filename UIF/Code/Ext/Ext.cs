using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    public static class Ext
    {
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

        internal static List<Comp> GetComps<Comp, Scrpt>(this Scrpt script) where Comp : MonoBehaviour where Scrpt : MonoBehaviour
        {
            List<Comp> result = new List<Comp>();
            UpdateList(script.transform, ref result);
            return result;
            void UpdateList(Transform t, ref List<Comp> result)
            {
                var count = t.childCount;
                for (int i = 0; i < count; i++)
                {
                    var tChild = t.GetChild(i);
                    var tcom = tChild.GetComponent<Comp>();
                    var thisScrpt = tChild.GetComponent<Scrpt>();
                    if (tcom != null)
                    {
                        if (thisScrpt != null && thisScrpt != script)
                        {
                            //other Script ownership, ignore
                        }
                        else
                        {
                            if (result == null) { result = new List<Comp>(); }
                            result.Add(tcom);
                            UpdateList(tChild, ref result);
                        }
                    }
                }
            }
        }
    }
}