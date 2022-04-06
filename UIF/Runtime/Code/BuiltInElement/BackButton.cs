using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIF
{
    public class BackButton : UIPageElement
    {
        Button btn;
        protected internal override void OnInitElement(UIPage owner)
        {
            base.OnInitElement(owner);
            btn = GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                owner.Close();
            });
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            btn.interactable = true;
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            btn.interactable = false;
        }
    }
}