using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UIF
{
    public static class Ext
    {
        public static void SetAllActiveUI<T>(this List<T> ui, bool activate) where T : IActivation
        {
            if (ui != null && ui.Count > 0)
            {
                for (int i = 0; i < ui.Count; i++)
                {
                    var u = ui[i];
                    if (u == null) { continue; }
                    u.SetActiveUI(activate);
                }
            }
        }

        internal static List<string> GetPagesNameFromUrl(this string url)
        {
            if (url.Contains("http://")) { url = url.Replace("http://", ""); }
            if (url.Contains("ui://")) { url = url.Replace("ui://", ""); }
            if (url.Contains(@"\")) { url = url.Replace(@"\", "/"); }
            var parts = Regex.Split(url, "/");
            List<string> pageNames = new List<string>();
            if (parts != null && parts.Length > 0)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];
                    if (part.IsStringValid() == false) { continue; }
                    pageNames.Add(part);
                }
            }
            return pageNames;
        }

        static Coroutine randomSelectionHandle = null;
        static TweenerCore<float, float, FloatOptions> tween = null;
        const float delayStart = 0.3f;
        const float delayEnd = 0.02f;
        const float delayLerpTime = 2.5f;
        public static void ChoseSelectableRandom(this UIPage page, List<Selectable> selectables, Action<Selectable> OnFinallySelected)
        {
            page.Cleanup();
            if (selectables == null) { selectables = new List<Selectable>(); }
            selectables.RemoveAll((sel) => { return sel == null; });
            if (selectables == null) { selectables = new List<Selectable>(); }

            for (int i = 0; i < selectables.Count; i++)
            {
                var sel = selectables[i];
                if (sel == null) { continue; }
                sel.FreezeInput = true;
            }

            var timeOfEffect = (4f / 6f) * selectables.Count * 0.9f;
            var allElements = page.elements;
            foreach (var elem in allElements)
            {
                if (elem == null) { continue; }
                if (elem.StateInfo.State == UIFState.Selected)
                {
                    elem.StateInfo.State = UIFState.Normal;
                }
            }

            if (randomSelectionHandle != null) { page.StopCoroutine(randomSelectionHandle); }
            randomSelectionHandle = page.StartCoroutine(RandomSelect());
            IEnumerator RandomSelect()
            {
                var delayTimer = 0.0f;
                var totalTimer = 0.0f;
                float delay = delayStart;

                if (tween != null && tween.IsPlaying()) { tween.Kill(); }
                tween = DOTween.To(() => delay, x => delay = x, delayEnd, delayLerpTime);

                Selectable selected = null;
                while (true)
                {
                    if (selectables.Count == 1)
                    {
                        selected = selectables[0];
                        break;
                    }

                    var delta = Time.deltaTime;
                    delayTimer += delta;
                    totalTimer += delta;
                    if (delayTimer >= delay)
                    {
                        delayTimer = 0.0f;
                        selected = selectables[UnityEngine.Random.Range(0, selectables.Count)];
                        selected.OwnerElement.StateInfo.StateChangeVisual.ApplyChangeOnVisual(UIFState.Selected);
                        foreach (var s in selectables)
                        {
                            if (s != null && s != selected)
                            {
                                s.OwnerElement.StateInfo.StateChangeVisual.ApplyChangeOnVisual(UIFState.Locked);
                            }
                        }

                        var selElem = selected.OwnerElement;
                        var selPage = selElem.Owner;
                        var masterPage = selPage.OwnerPage;
                    }
                    if (totalTimer >= timeOfEffect)
                    {
                        break;
                    }
                    yield return null;
                }
                
                selected.Select();
                for (int i = 0; i < selectables.Count; i++)
                {
                    var sel = selectables[i];
                    if (sel == null) { continue; }
                    sel.FreezeInput = false;
                }

                yield return null;
                OnFinallySelected?.Invoke(selected);
            }
        }
    }
}