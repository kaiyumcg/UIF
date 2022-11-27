using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace UIF
{
    [AddComponentMenu("UIF/Page UI Element Feature")]
    public class UIPageElementFeature : UIFAttachableObject
    {
        UIPage ownerPage;
        UIPageElement ownerElement;
        public UIPage OwnerPage { get { return ownerPage; } }
        public UIPageElement OwnerElement { get { return ownerElement; } }

        protected internal virtual void OnInitFeature(UIPage ownerPage, UIPageElement ownerElement)
        {
            this.ownerPage = ownerPage;
            this.ownerElement = ownerElement;
            var ld = (ILoadReference)this;
            ld.LoadReference();
            ld.InitIfRequired();
        }
    }
}