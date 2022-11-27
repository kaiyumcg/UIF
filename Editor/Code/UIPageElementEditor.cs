using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIF
{
    [CustomEditor(typeof(UIPageElement), true)]
    [CanEditMultipleObjects]
    public class UIPageElementEditor : UIFAttachableObjectEditor
    {
        UIPageElement element;
        GUIStyle headerLabelStyle;
        protected UIPageElement Element { get { return element; } }

        SerializedProperty elementName_prop, features_prop, onAppearUI_prop, onHideUI_prop;
        static bool editSetting = false;

        protected override void OnStartUIF()
        {
            base.OnStartUIF();
            element = (UIPageElement)target;
            headerLabelStyle = new GUIStyle();
            headerLabelStyle.fontStyle = FontStyle.Bold;
            headerLabelStyle.normal.textColor = Color.green;

            elementName_prop = serializedObject.FindProperty("elementName");
            features_prop = serializedObject.FindProperty("features");
            onAppearUI_prop = serializedObject.FindProperty("onAppearUI");
            onHideUI_prop = serializedObject.FindProperty("onHideUI");

            var propList = new List<string>();
            propList.AddRange(defaultProps);
            var additionals = new string[] { "elementName", "features", "onAppearUI", "onHideUI" };
            propList.AddRange(additionals);
            defaultProps = propList.ToArray();
        }

        protected override void OnUpdateUIF()
        {
            base.OnUpdateUIF();
            editSetting = EditorGUILayout.Foldout(editSetting, "Element Setting");
            if (editSetting)
            {
                EditorGUI.indentLevel++;
                DrawUILine(Color.gray);
                EditorGUILayout.PropertyField(elementName_prop);
                EditorGUILayout.PropertyField(features_prop);
                EditorGUILayout.PropertyField(onAppearUI_prop);
                EditorGUILayout.PropertyField(onHideUI_prop);
                EditorGUI.indentLevel--;
            }
        }
    }
}
