using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIF
{
    [CustomEditor(typeof(UIPage), true)]
    [CanEditMultipleObjects]
    public class UIPageEditor : UIFAttachableObjectEditor
    {
        SerializedProperty pageName_prop, elements_prop, onOpen_prop, onClose_prop, childPages_prop;
        UIPage page;
        protected UIPage Page { get { return page; } }
        static bool editSetting = false;

        protected override void OnStartUIF()
        {
            base.OnStartUIF();
            page = (UIPage)target;

            pageName_prop = serializedObject.FindProperty("pageName");
            elements_prop = serializedObject.FindProperty("elements");
            childPages_prop = serializedObject.FindProperty("childPages");
            onOpen_prop = serializedObject.FindProperty("onOpen");
            onClose_prop = serializedObject.FindProperty("onClose");

            var propList = new List<string>();
            propList.AddRange(defaultProps);
            var additionals = new string[] { "pageName", "elements", "childPages", "onOpen", "onClose" };
            propList.AddRange(additionals);
            defaultProps = propList.ToArray();
        }

        protected override void OnUpdateUIF()
        {
            base.OnUpdateUIF();
            editSetting = EditorGUILayout.Foldout(editSetting, "Page Setting");
            if (editSetting)
            {
                EditorGUI.indentLevel++;
                DrawUILine(Color.gray);
                EditorGUILayout.PropertyField(pageName_prop);
                EditorGUILayout.PropertyField(elements_prop);
                EditorGUILayout.PropertyField(childPages_prop);
                EditorGUILayout.PropertyField(onOpen_prop);
                EditorGUILayout.PropertyField(onClose_prop);
                EditorGUI.indentLevel--;
            }
        }
    }
}