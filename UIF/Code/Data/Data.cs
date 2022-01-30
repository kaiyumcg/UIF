using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIF
{
    public enum SelectionMode { OnlyOneCanbeSelected = 0, MultiSelection = 1 }
    public enum SelectionInputAction { TouchOrEnter = 0, Click = 1 }
    public enum SelectionAction { ShowHideGameObject = 0, ModifyColor = 1 }

    [System.Serializable]
    internal class ColoredSelection
    {
        [SerializeField] MaskableGraphic graphic;
        [SerializeField] Color selectedColor;
        Color originalColor;
        internal MaskableGraphic Graphic { get { return graphic; } }

        internal void Init()
        {
            originalColor = graphic.color;
        }

        internal void Apply(bool selected, float fadeDuration = 0.3f)
        {
            graphic.CrossFadeColor(selected ? selectedColor : originalColor, fadeDuration, true, true);
        }
    }
}