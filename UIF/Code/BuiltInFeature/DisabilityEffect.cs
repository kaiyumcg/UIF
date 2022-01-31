using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UIF
{
    public class DisabilityEffect : UIPageElementFeature
    {
        [SerializeField] Color disableColor = Color.gray;
        [SerializeField] float tweenDuration = 0.2f;
        MaskableGraphic ui_graphic = null;
        Color enabledColor;

        protected internal override void OnInit(UIPage owner, UIPageElement ownerElement)
        {
            base.OnInit(owner, ownerElement);
            ui_graphic = GetComponent<MaskableGraphic>();
            if (ui_graphic != null)
            {
                enabledColor = ui_graphic.color;
            }

            if (ui_graphic == null)
            {
                throw new Exception("'PageElementDisability' is valid for maskable graphic Components. Misused Framework error!");
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            ui_graphic.CrossFadeColor(enabledColor, tweenDuration, true, true);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            ui_graphic.CrossFadeColor(disableColor, tweenDuration, true, true);
        }
    }
}