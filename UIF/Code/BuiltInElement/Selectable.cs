using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UIF
{
    public class Selectable : UIPageElement, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] protected SelectionMode mode = SelectionMode.OnlyOneCanbeSelected;
        [SerializeField] protected SelectionInputAction inputMode = SelectionInputAction.Click;
        [SerializeField] protected SelectionAction actionToDo = SelectionAction.ShowHideGameObject;
        [SerializeField] List<GameObject> objectVisibilityForSelection;
        [SerializeField] List<ColoredSelection> selectableGraphics;
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
            else
            {
                objectVisibilityForSelection.SetActiveAll(isSelected);
            }
        }

        protected internal override void OnInit(UIPage owner)
        {
            base.OnInit(owner);
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
            if (mode == SelectionMode.OnlyOneCanbeSelected)
            {
                var currentlySelected = Owner.selectedElement;
                if (currentlySelected != null)
                {
                    currentlySelected.ClearSelection();
                }
                Owner.selectedElement = this;
            }
            else
            {
                if (Owner.multiSelectedElements == null) { Owner.multiSelectedElements = new List<Selectable>(); }
                if (Owner.multiSelectedElements.Contains(this) == false)
                {
                    Owner.multiSelectedElements.Add(this);
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