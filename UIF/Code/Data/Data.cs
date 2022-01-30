using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIF
{
    public enum SelectionMode { OnlyOneCanBeSelected = 0, MultiSelection = 1 }
    public enum SelectionInputAction { TouchOrEnter = 0, Click = 1 }
    public enum SelectionAction { ShowHideGameObject = 0, ModifyColor = 1, EnableDisableComponent = 2 }

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

    [System.Serializable]
    internal class ComponentSel
    {
        [SerializeField] Behaviour component;
        [SerializeField] bool enableWhenSelected = true;

        internal void Apply(bool selected)
        {
            component.enabled = enableWhenSelected ? selected : !selected;
        }
    }

    [System.Serializable]
    internal class ComponentSelection
    {
        [SerializeField] List<ComponentSel> components;

        internal void Apply(bool selected)
        {
            if (components != null && components.Count > 0)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    var comp = components[i];
                    if (comp == null) { continue; }
                    comp.Apply(selected);
                }
            }
        }
    }
}