using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIF
{
    [CustomEditor(typeof(UIBrowser), true)]
    [CanEditMultipleObjects]
    public class UIBrowserEditor : UIFAttachableObjectEditor
    {
        SerializedProperty shouldTick_prop, homePage_prop, pages_prop, onOpenHomePage_prop;
        UIBrowser browser;
        protected UIBrowser Browser { get { return browser; } }
        static bool editSetting = false;

        protected override void OnStartUIF()
        {
            base.OnStartUIF();
            browser = (UIBrowser)target;

            shouldTick_prop = serializedObject.FindProperty("shouldTick");
            homePage_prop = serializedObject.FindProperty("homePage");
            pages_prop = serializedObject.FindProperty("pages");
            onOpenHomePage_prop = serializedObject.FindProperty("onOpenHomePage");

            var propList = new List<string>();
            propList.AddRange(defaultProps);
            var additionals = new string[] { "shouldTick", "homePage", "pages", "onOpenHomePage" };
            propList.AddRange(additionals);
            defaultProps = propList.ToArray();
        }

        protected override void OnUpdateUIF()
        {
            base.OnUpdateUIF();
            editSetting = EditorGUILayout.Foldout(editSetting, "Browser Setting");
            if (editSetting)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(shouldTick_prop);
                EditorGUILayout.PropertyField(homePage_prop);
                EditorGUILayout.PropertyField(pages_prop);
                EditorGUILayout.PropertyField(onOpenHomePage_prop);
                EditorGUI.indentLevel--;
                DrawUILine(Color.gray);
            }
        }
    }
}