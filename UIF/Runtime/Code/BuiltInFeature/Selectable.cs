using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UIF
{
    public enum SelectionInputAction { TouchOrEnter = 0, Click = 1 }
    public class Selectable : UIPageElementFeature, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] protected UIPage holderPage = null;
        [SerializeField] protected SelectionInputAction inputMode = SelectionInputAction.Click;
        bool isInputFrozen = false;
        public bool FreezeInput { get { return isInputFrozen; } set { isInputFrozen = value; } }

        protected internal override void OnInitFeature(UIPage owner, UIPageElement ownerElement)
        {
            base.OnInitFeature(owner, ownerElement);
            isInputFrozen = false;
        }
        
        public void Select(bool multiSelect = false)
        {
            if (multiSelect == false)
            {
                DeselectAnySelectedPageSelectables();
            }
            OwnerElement.StateInfo.State = UIFState.Selected;
        }

        public void DeselectAnySelectedPageSelectables()
        {
            var allFeaturesOfPage = holderPage.GetAllFeaturesAndBelow<Selectable>();
            if (allFeaturesOfPage != null && allFeaturesOfPage.Count > 0)
            {
                for (int i = 0; i < allFeaturesOfPage.Count; i++)
                {
                    var feature = allFeaturesOfPage[i];
                    if (feature == null) { continue; }
                    if (feature.OwnerElement.StateInfo.State == UIFState.Selected)
                    {
                        feature.OwnerElement.StateInfo.State = UIFState.Normal;
                    }
                }
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (isInputFrozen) { return; }
            if (inputMode == SelectionInputAction.Click && 
                (OwnerElement.StateInfo.State == UIFState.Normal)) { Select(); }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (isInputFrozen) { return; }
            if (inputMode == SelectionInputAction.TouchOrEnter &&
                (OwnerElement.StateInfo.State == UIFState.Normal)) { Select(); }
        }
    }
}