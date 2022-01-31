using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIF
{
    [CustomEditor(typeof(UIPageElementFeature), true)]
    [CanEditMultipleObjects]
    public class UIPageElementFeatureEditor : Editor
    {
        SerializedProperty tags_prop, waitForPageOpen_prop, waitForPageClose_prop, 
            waitForElementAppear_prop, waitForElementHide_prop, onInit_prop, onOwnerPageOpen_prop, 
            onOwnerPageClose_prop, onOwnerElementAppear_prop, onOwnerElementHide_prop, onActivate_prop, onDeactivate_prop;
        UIPageElementFeature feature;
        GUIStyle headerLabelStyle;
        protected UIPageElementFeature Feature { get { return feature; } }
        protected virtual void OnStartUIF() { }
        protected virtual void OnUpdateUIF() { }
        string[] defaultProps;
        void OnEnable()
        {
            feature = (UIPageElementFeature)target;
            headerLabelStyle = new GUIStyle(EditorStyles.label);
            headerLabelStyle.fontStyle = FontStyle.Bold;
            headerLabelStyle.normal.textColor = Color.magenta;
            waitForPageOpen_prop = serializedObject.FindProperty("waitForPageOpen");
            waitForPageClose_prop = serializedObject.FindProperty("waitForPageClose");
            waitForElementAppear_prop = serializedObject.FindProperty("waitForElementAppear");
            waitForElementHide_prop = serializedObject.FindProperty("waitForElementHide");

            tags_prop = serializedObject.FindProperty("tags");
            onInit_prop = serializedObject.FindProperty("onInit");
            onOwnerPageOpen_prop = serializedObject.FindProperty("onOwnerPageOpen");
            onOwnerPageClose_prop = serializedObject.FindProperty("onOwnerPageClose");
            onOwnerElementAppear_prop = serializedObject.FindProperty("onOwnerElementAppear");
            onOwnerElementHide_prop = serializedObject.FindProperty("onOwnerElementHide");
            onActivate_prop = serializedObject.FindProperty("onActivate");
            onDeactivate_prop = serializedObject.FindProperty("onDeactivate");
            OnStartUIF();
            defaultProps = new string[] { "tags", "waitForPageOpen",
                "waitForPageClose", "waitForElementAppear", "waitForElementHide", "onInit", "onOwnerPageOpen",
                "onOwnerPageClose", "onOwnerElementAppear", "onOwnerElementHide", "onActivate", "onDeactivate" };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Label("Page UI Element Feature", headerLabelStyle);
            feature.editDefaultSetting = EditorGUILayout.Toggle("Edit Default", feature.editDefaultSetting);
            if (feature.editDefaultSetting)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(tags_prop);
                EditorGUILayout.PropertyField(waitForPageOpen_prop);
                EditorGUILayout.PropertyField(waitForPageClose_prop);
                EditorGUILayout.PropertyField(waitForElementAppear_prop);
                EditorGUILayout.PropertyField(waitForElementHide_prop);
                EditorGUI.indentLevel--;
            }

            feature.eventEdit = EditorGUILayout.Toggle("Open Events", feature.eventEdit);
            if (feature.eventEdit)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(onInit_prop);
                EditorGUILayout.PropertyField(onOwnerPageOpen_prop);
                EditorGUILayout.PropertyField(onOwnerPageClose_prop);
                EditorGUILayout.PropertyField(onOwnerElementAppear_prop);
                EditorGUILayout.PropertyField(onOwnerElementHide_prop);
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