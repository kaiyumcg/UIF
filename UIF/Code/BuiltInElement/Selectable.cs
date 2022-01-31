using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UIF
{
    public class Selectable : UIPageElementFeature, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] protected SelectionMode mode = SelectionMode.OnlyOneCanBeSelected;
        [SerializeField] protected SelectionInputAction inputMode = SelectionInputAction.Click;
        [SerializeField] protected SelectionAction actionToDo = SelectionAction.ShowHideGameObject;
        [SerializeField] List<GameObject> objectVisibilityForSelection;
        [SerializeField] List<ColoredSelection> selectableGraphics;
        [SerializeField] ComponentSelection selectionComponents;
        [SerializeField] float appearTweenDuration = 0.3f;
        public UnityEvent onSelect, onDeselect;
        bool isSelected = false;
        bool isInputFrozen = false;
        public bool FreezeInput { get { return isInputFrozen; } set { isInputFrozen = value; } }
        protected virtual void OnSelect() { UpdateGraphicBasedOnSelection(); onSelect?.Invoke(); }
        protected virtual void OnDelect() { UpdateGraphicBasedOnSelection(); onDeselect?.Invoke(); }
        void UpdateGraphicBasedOnSelection()
        {
            if (actionToDo == SelectionAction.ModifyColor)
            {
                selectableGraphics.ApplyColorAll(isSelected, appearTweenDuration);
            }
            else if (actionToDo == SelectionAction.ShowHideGameObject)
            {
                objectVisibilityForSelection.SetActiveAll(isSelected);
            }
            else if (actionToDo == SelectionAction.EnableDisableComponent)
            {
                if (selectionComponents != null) { selectionComponents.Apply(isSelected); }
            }
        }

        protected internal override void OnInit(UIPage owner, UIPageElement ownerElement)
        {
            base.OnInit(owner, ownerElement);
            isSelected = false;
            isInputFrozen = false;
            if (selectableGraphics == null) { selectableGraphics = new List<ColoredSelection>(); }
            selectableGraphics.RemoveAll((ui) => { return ui == null || ui.Graphic == null; });
            if (selectableGraphics == null) { selectableGraphics = new List<ColoredSelection>(); }

            if (objectVisibilityForSelection == null) { objectVisibilityForSelection = new List<GameObject>(); }
            objectVisibilityForSelection.RemoveAll((ui) => { return ui == null; });
            if (objectVisibilityForSelection == null) { objectVisibilityForSelection = new List<GameObject>(); }

            if (selectableGraphics != null && selectableGraphics.Count > 0)
            {
                for (int i = 0; i < selectableGraphics.Count; i++)
                {
                    selectableGraphics[i].Init();
                }
            }
        }
        
        public void Select()
        {
            if (mode == SelectionMode.OnlyOneCanBeSelected)
            {
                var currentlySelected = OwnerPage.selectedFeature;
                if (currentlySelected != null)
                {
                    currentlySelected.ClearSelection();
                }
                OwnerPage.selectedFeature = this;
            }
            else
            {
                if (OwnerPage.multiSelectedFeatures == null) { OwnerPage.multiSelectedFeatures = new List<Selectable>(); }
                if (OwnerPage.multiSelectedFeatures.Contains(this) == false)
                {
                    OwnerPage.multiSelectedFeatures.Add(this);
                }
            }

            isSelected = true;
            OnSelect();
        }

        public void ClearSelection()
        {
            isSelected = false;
            OnDelect();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (isInputFrozen) { return; }
            if (inputMode == SelectionInputAction.Click) { Select(); }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (isInputFrozen) { return; }
            if (inputMode == SelectionInputAction.TouchOrEnter) { Select(); }
        }
    }
}