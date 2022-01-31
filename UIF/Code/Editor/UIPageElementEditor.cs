using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIF
{
    [CustomEditor(typeof(UIPageElement), true)]
    [CanEditMultipleObjects]
    public class UIPageElementEditor : Editor
    {
        SerializedProperty tags_prop, waitForPageOpen_prop, waitForPageClose_prop,
            overrideDefaultTween_prop, appearenceTweenDuration_prop, 
            features_prop, useAnimationWhenPageOpen_prop, useAnimationWhenPageClose_prop, onInit_prop,
            onOwnerPageOpen_prop, onOwnerPageClose_prop, onAppear_prop, onHide_prop, onActivate_prop, onDeactivate_prop;

        UIPageElement element;
        GUIStyle headerLabelStyle;
        protected UIPageElement Element { get { return element; } }
        protected virtual void OnStartUIF() { }
        protected virtual void OnUpdateUIF() { }
        string[] defaultPropList;
        void OnEnable()
        {
            element = (UIPageElement)target;
            headerLabelStyle = new GUIStyle(EditorStyles.label);
            headerLabelStyle.fontStyle = FontStyle.Bold;
            headerLabelStyle.normal.textColor = Color.yellow;
            waitForPageOpen_prop = serializedObject.FindProperty("waitForPageOpen");
            waitForPageClose_prop = serializedObject.FindProperty("waitForPageClose");
            overrideDefaultTween_prop = serializedObject.FindProperty("overrideDefaultTween");
            appearenceTweenDuration_prop = serializedObject.FindProperty("appearenceTweenDuration");
            features_prop = serializedObject.FindProperty("features");
            useAnimationWhenPageOpen_prop = serializedObject.FindProperty("useAnimationWhenPageOpen");
            useAnimationWhenPageClose_prop = serializedObject.FindProperty("useAnimationWhenPageClose");

            tags_prop = serializedObject.FindProperty("tags");
            onInit_prop = serializedObject.FindProperty("onInit");
            onOwnerPageOpen_prop = serializedObject.FindProperty("onOwnerPageOpen");
            onOwnerPageClose_prop = serializedObject.FindProperty("onOwnerPageClose");
            onAppear_prop = serializedObject.FindProperty("onAppear");
            onHide_prop = serializedObject.FindProperty("onHide");
            onActivate_prop = serializedObject.FindProperty("onActivate");
            onDeactivate_prop = serializedObject.FindProperty("onDeactivate");
            OnStartUIF();

            defaultPropList = new string[] {  "tags", "waitForPageOpen", "waitForPageClose", "appearenceTweenDuration",
                "features", "useAnimationWhenPageOpen", "useAnimationWhenPageClose", "overrideDefaultTween",
                "onInit", "onOwnerPageOpen", "onOwnerPageClose", "onAppear", "onHide", "onActivate", "onDeactivate"};
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Label("Page UI Element", headerLabelStyle);
            element.editDefaultSetting = EditorGUILayout.Toggle("Edit Default", element.editDefaultSetting);
            if (element.editDefaultSetting)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(tags_prop);
                EditorGUILayout.PropertyField(waitForPageOpen_prop);
                EditorGUILayout.PropertyField(waitForPageClose_prop);
                EditorGUILayout.PropertyField(overrideDefaultTween_prop);
                EditorGUILayout.PropertyField(appearenceTweenDuration_prop);
                EditorGUILayout.PropertyField(useAnimationWhenPageOpen_prop);
                EditorGUILayout.PropertyField(useAnimationWhenPageClose_prop);
                EditorGUILayout.PropertyField(features_prop);
                EditorGUILayout.HelpBox("We fetch features at runtime for you though :)", MessageType.Info);
                EditorGUILayout.HelpBox("You can edit for execution order", MessageType.Info);
                if (GUILayout.Button("Fetch Features"))
                {
                    element.LoadFeatures();
                }
                EditorGUI.indentLevel--;
            }

            element.eventEdit = EditorGUILayout.Toggle("Open Events", element.eventEdit);
            if (element.eventEdit)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(onInit_prop);
                EditorGUILayout.PropertyField(onOwnerPageOpen_prop);
                EditorGUILayout.PropertyField(onOwnerPageClose_prop);
                EditorGUILayout.PropertyField(onAppear_prop);
                EditorGUILayout.PropertyField(onHide_prop);
                EditorGUILayout.PropertyField(onActivate_prop);
                EditorGUILayout.PropertyField(onDeactivate_prop);
                EditorGUI.indentLevel--;
            }

            OnUpdateUIF();
            DrawPropertiesExcluding(serializedObject, defaultPropList);
            serializedObject.ApplyModifiedProperties();
        }
    }
}