using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIF
{
    public enum SelectionAction { ShowHideGameObject = 0, ModifyColor = 1, EnableDisableComponent = 2 }

    [System.Serializable]
    internal class ColoredSelections
    {
        [SerializeField] List<ColoredSel> coloredUIs;
        [SerializeField] float appearTweenDuration = 0.3f;
        internal void Init()
        {
            if (coloredUIs != null && coloredUIs.Count > 0)
            {
                for (int i = 0; i < coloredUIs.Count; i++)
                {
                    var colUI = coloredUIs[i];
                    if (colUI == null) { continue; }
                    colUI.Init();
                }
            }
        }

        internal void Apply(UIFState state)
        {
            if (coloredUIs != null && coloredUIs.Count > 0)
            {
                for (int i = 0; i < coloredUIs.Count; i++)
                {
                    var colUI = coloredUIs[i];
                    if (colUI == null) { continue; }
                    colUI.Apply(state, appearTweenDuration);
                }
            }
        }

        [System.Serializable]
        internal class ColoredSel
        {
            [SerializeField] MaskableGraphic graphic;
            [SerializeField]
            Color selectedColor = Color.green,
                lockedColor = Color.gray, hiddenColor = Color.clear;
            Color originalColor;
            internal void Init()
            {
                if (graphic == null) { return; }
                originalColor = graphic.color;
            }

            internal void Apply(UIFState state, float fadeDuration)
            {
                if (graphic == null) { return; }
                Color col = default;
                if (state == UIFState.Selected)
                {
                    col = selectedColor;
                }
                else if (state == UIFState.Locked)
                {
                    col = lockedColor;
                }
                else if (state == UIFState.Hidden)
                {
                    col = hiddenColor;
                }
                else if (state == UIFState.Normal)
                {
                    col = originalColor;
                }
                graphic.CrossFadeColor(col, fadeDuration, true, true);
            }
        }
    }

    [System.Serializable]
    internal class ComponentSelections
    {
        [SerializeField] List<ComponentSel> components;

        internal void Apply(UIFState state)
        {
            if (components != null && components.Count > 0)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    var comp = components[i];
                    if (comp == null) { continue; }
                    comp.Apply(state);
                }
            }
        }

        [System.Serializable]
        internal class ComponentSel
        {
            [SerializeField]
            Behaviour componentOnSelection, originalComponent,
                componentWhenLocked, componentWhenHidden;

            internal void Apply(UIFState state)
            {
                if (componentOnSelection != null) { componentOnSelection.enabled = false; }
                if (originalComponent != null) { originalComponent.enabled = false; }
                if (componentWhenLocked != null) { componentWhenLocked.enabled = false; }
                if (componentWhenHidden != null) { componentWhenHidden.enabled = false; }

                if (state == UIFState.Selected)
                {
                    if (componentOnSelection != null) { componentOnSelection.enabled = true; }
                }
                else if (state == UIFState.Locked)
                {
                    if (componentWhenLocked != null) { componentWhenLocked.enabled = true; }
                }
                else if (state == UIFState.Normal)
                {
                    if (originalComponent != null) { originalComponent.enabled = true; }
                }
                else if (state == UIFState.Hidden)
                {
                    if (componentWhenHidden != null) { componentWhenHidden.enabled = true; }
                }
            }
        }
    }

    [System.Serializable]
    internal class GameObjectSelections
    {
        [SerializeField] List<GameObjectSel> objects;
        internal void Apply(UIFState state)
        {
            if (objects != null && objects.Count > 0)
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    var comp = objects[i];
                    if (comp == null) { continue; }
                    comp.Apply(state);
                }
            }
        }

        [System.Serializable]
        internal class GameObjectSel
        {
            [SerializeField] GameObject selectedGameobject, normalGameobject, lockedGameobject, hiddenGameobject;

            internal void Apply(UIFState state)
            {
                if (selectedGameobject != null) { selectedGameobject.SetActive(false); }
                if (normalGameobject != null) { normalGameobject.SetActive(false); }
                if (lockedGameobject != null) { lockedGameobject.SetActive(false); }
                if (hiddenGameobject != null) { hiddenGameobject.SetActive(false); }

                if (state == UIFState.Selected)
                {
                    if (selectedGameobject != null) { selectedGameobject.SetActive(true); }
                }
                else if (state == UIFState.Locked)
                {
                    if (lockedGameobject != null) { lockedGameobject.SetActive(true); }
                }
                else if (state == UIFState.Normal)
                {
                    if (normalGameobject != null) { normalGameobject.SetActive(true); }
                }
                else if (state == UIFState.Hidden)
                {
                    if (hiddenGameobject != null) { hiddenGameobject.SetActive(true); }
                }
            }
        }
    }

    [System.Serializable]
    public class UIFStateRenderDesc
    {
        [SerializeField, EnumFlags] protected SelectionAction actionToDo = SelectionAction.ShowHideGameObject;
        [SerializeField] GameObjectSelections gameobjectSelections;
        [SerializeField] ColoredSelections selectableGraphics;
        [SerializeField] ComponentSelections componentSelections;

        internal void Init()
        {
            if (selectableGraphics != null) { selectableGraphics.Init(); }
        }
        internal void ApplyChangeOnVisual(UIFState state)
        {
            var actions = EnumFlagsAttribute.GetSelectedItems(actionToDo);
            if (actions != null && actions.Count > 0)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    var action = actions[i];
                    if (action == SelectionAction.ModifyColor)
                    {
                        if (selectableGraphics != null) { selectableGraphics.Apply(state); }
                    }
                    if (action == SelectionAction.ShowHideGameObject)
                    {
                        if (gameobjectSelections != null) { gameobjectSelections.Apply(state); }
                    }
                    if (action == SelectionAction.EnableDisableComponent)
                    {
                        if (componentSelections != null) { componentSelections.Apply(state); }
                    }
                }
            }
        }
    }
}