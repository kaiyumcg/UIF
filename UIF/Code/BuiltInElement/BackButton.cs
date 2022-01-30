using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIF
{
    public class BackButton : UIPageElementFeature
    {
        Button btn;
        [SerializeField] bool closeWithAnimation = true;
        protected internal override void OnInit(UIPage owner)
        {
            base.OnInit(owner);
            btn = GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                owner.Close(closeWithAnimation);
            });
        }
    }
}