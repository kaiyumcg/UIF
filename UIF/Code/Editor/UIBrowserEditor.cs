using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIF
{
    [CustomEditor(typeof(UIBrowser), true)]
    [CanEditMultipleObjects]
    public class UIBrowserEditor : Editor
    {
        SerializedProperty shouldTick_prop, homePage_prop, pages_prop, deltaSmoothnessBufferSize_prop, onInit_prop, onOpenHomePage_prop;
        UIBrowser browser;
        GUIStyle headerLabelStyle;
        protected UIBrowser Browser { get { return browser; } }
        protected virtual void OnStartUIF() { }
        protected virtual void OnUpdateUIF() { }
        string[] defaultProps;
        void OnEnable()
        {
            browser = (UIBrowser)target;
            headerLabelStyle = new GUIStyle(EditorStyles.label);
            headerLabelStyle.fontStyle = FontStyle.Bold;
            headerLabelStyle.normal.textColor = Color.green;
            pages_prop = serializedObject.FindProperty("pages");
            deltaSmoothnessBufferSize_prop = serializedObject.FindProperty("deltaSmoothnessBufferSize");
            homePage_prop = serializedObject.FindProperty("homePage");
            shouldTick_prop = serializedObject.FindProperty("shouldTick");
            homePage_prop = serializedObject.FindProperty("homePage");
            onInit_prop = serializedObject.FindProperty("onInit");
            onOpenHomePage_prop = serializedObject.FindProperty("onOpenHomePage");
            OnStartUIF();
            defaultProps = new string[] { "shouldTick", "homePage", "pages", "deltaSmoothnessBufferSize", "onInit", "onOpenHomePage" };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Label("Game UI browser", headerLabelStyle);
            browser.editDefaultSetting = EditorGUILayout.Toggle("Edit Default", browser.editDefaultSetting);
            if (browser.editDefaultSetting)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(shouldTick_prop);
                EditorGUILayout.PropertyField(homePage_prop);
                EditorGUILayout.PropertyField(pages_prop);
                EditorGUILayout.PropertyField(deltaSmoothnessBufferSize_prop);
                EditorGUILayout.HelpBox("We fetch pages at runtime for you though :)", MessageType.Info);
                EditorGUILayout.HelpBox("You can edit for execution order", MessageType.Info);
                if (GUILayout.Button("Fetch pages"))
                {
                    browser.LoadInputSpecificReferences();
                    browser.LoadAllPages(ref browser.pages);
                }
                EditorGUI.indentLevel--;
            }

            browser.eventEdit = EditorGUILayout.Toggle("Open Events", browser.eventEdit);
            if(browser.eventEdit)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(onInit_prop);
                EditorGUILayout.PropertyField(onOpenHomePage_prop);
                EditorGUI.indentLevel--;
            }

            OnUpdateUIF();
            DrawPropertiesExcluding(serializedObject, defaultProps);
            serializedObject.ApplyModifiedProperties();
        }
    }
}