using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIF
{
    [CustomEditor(typeof(UIPage), true)]
    [CanEditMultipleObjects]
    public class UIPageEditor : Editor
    {
        SerializedProperty tags_prop, pageName_prop, pageWideTweenTime_prop, 
            dynamicPage_prop, elements_prop,
            overrideDefaultTween_prop, onInit_prop, onOpen_prop, onClose_prop, onActivate_prop, onDeactivate_prop;

        UIPage page;
        GUIStyle headerLabelStyle;
        protected UIPage Page { get { return page; } }
        protected virtual void OnStartUIF() { }
        protected virtual void OnUpdateUIF() { }
        string[] defaultProps;
        void OnEnable()
        {
            page = (UIPage)target;
            headerLabelStyle = new GUIStyle(EditorStyles.label);
            headerLabelStyle.fontStyle = FontStyle.Bold;
            headerLabelStyle.normal.textColor = Color.cyan;
            pageName_prop = serializedObject.FindProperty("pageName");
            pageWideTweenTime_prop = serializedObject.FindProperty("pageWideTweenTime");
            dynamicPage_prop = serializedObject.FindProperty("dynamicPage");
            elements_prop = serializedObject.FindProperty("elements");
            overrideDefaultTween_prop = serializedObject.FindProperty("overrideDefaultTween");

            tags_prop = serializedObject.FindProperty("tags");
            onInit_prop = serializedObject.FindProperty("onInit");
            onOpen_prop = serializedObject.FindProperty("onOpen");
            onClose_prop = serializedObject.FindProperty("onClose");
            onActivate_prop = serializedObject.FindProperty("onActivate");
            onDeactivate_prop = serializedObject.FindProperty("onDeactivate");
            OnStartUIF();
            defaultProps = new string[] { "tags", "pageName", "pageWideTweenTime", "dynamicPage", "overrideDefaultTween",
                "elements", "onInit", "onOpen", "onClose", "onActivate", "onDeactivate"};
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Label("Game UI Page", headerLabelStyle);
            page.editDefaultSetting = EditorGUILayout.Toggle("Edit Default", page.editDefaultSetting);
            if (page.editDefaultSetting)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(tags_prop);
                EditorGUILayout.PropertyField(pageName_prop);
                EditorGUILayout.PropertyField(pageWideTweenTime_prop);
                EditorGUILayout.PropertyField(dynamicPage_prop);
                EditorGUILayout.PropertyField(overrideDefaultTween_prop);
                EditorGUILayout.PropertyField(elements_prop);
                EditorGUILayout.HelpBox("We fetch elements at runtime for you though :)", MessageType.Info);
                EditorGUILayout.HelpBox("You can edit for execution order", MessageType.Info);
                if (GUILayout.Button("Fetch Elements"))
                {
                    page.LoadElements();
                }
                EditorGUI.indentLevel--;
            }

            page.eventEdit = EditorGUILayout.Toggle("Open Events", page.eventEdit);
            if (page.eventEdit)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(onInit_prop);
                EditorGUILayout.PropertyField(onOpen_prop);
                EditorGUILayout.PropertyField(onClose_prop);
                EditorGUILayout.PropertyField(onActivate_prop);
                EditorGUILayout.PropertyField(onDeactivate_prop);
                EditorGUI.indentLevel--;
            }

            OnUpdateUIF();
            DrawPropertiesExcluding(serializedObject, defaultProps);
            serializedObject.ApplyModifiedProperties();
        }
    }
}