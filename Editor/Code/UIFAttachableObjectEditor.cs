using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIF
{
    [CustomEditor(typeof(UIFAttachableObject), true)]
    [CanEditMultipleObjects]
    public class UIFAttachableObjectEditor : Editor
    {
        SerializedProperty validateEveryInspectorFrame_prop, tags_prop, stateInfo_prop, 
            onInit_prop, onCleanup_prop, onActivateUI_prop, onDeactivateUI_prop;
        static bool editAttachableDefaultSetting = false, editAttachableEvents = false;
        UIFAttachableObject baseObject;
        IBuiltinEditorSetting builtInSetting;
        ILoadReference refLoader;
        protected UIFAttachableObject BaseObject { get { return baseObject; } }
        protected virtual void OnStartUIF() { }
        protected virtual void OnUpdateUIF() { }
        protected string[] defaultProps;
        void OnEnable()
        {
            baseObject = (UIFAttachableObject)target;
            builtInSetting = (IBuiltinEditorSetting)target;
            refLoader = (ILoadReference)target;

            validateEveryInspectorFrame_prop = serializedObject.FindProperty("validateEveryInspectorFrame");
            tags_prop = serializedObject.FindProperty("tags");
            stateInfo_prop = serializedObject.FindProperty("stateInfo");
            onInit_prop = serializedObject.FindProperty("onInit");
            onCleanup_prop = serializedObject.FindProperty("onCleanup");
            onActivateUI_prop = serializedObject.FindProperty("onActivateUI");
            onDeactivateUI_prop = serializedObject.FindProperty("onDeactivateUI");
            defaultProps = new string[] { "tags", "stateInfo", "onInit", "onCleanup", "onActivateUI", "onDeactivateUI" };
            OnStartUIF();
        }

        //https://forum.unity.com/threads/horizontal-line-in-editor-window.520812/#post-3534861
        protected void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, defaultProps);

            editAttachableDefaultSetting = EditorGUILayout.Foldout(editAttachableDefaultSetting, "UIF Default Setting");
            if (editAttachableDefaultSetting)
            {
                DrawUILine(Color.gray);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(validateEveryInspectorFrame_prop);
                EditorGUILayout.PropertyField(tags_prop);
                EditorGUILayout.PropertyField(stateInfo_prop, true);
                
                var validate = false;
                if (builtInSetting.ValidateEveryInspectorFrame)
                {
                    validate = true;
                }
                else if (GUILayout.Button("\n Validate \n"))
                {
                    validate = true;
                }

                if (validate)
                {
                    refLoader.LoadReference();
                }

                if (builtInSetting.ConfigError)
                {
                    EditorGUILayout.HelpBox(builtInSetting.ConfigErrorMessage, MessageType.Error);
                }

                editAttachableEvents = EditorGUILayout.Foldout(editAttachableEvents, "Events");
                if (editAttachableEvents)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(onInit_prop);
                    EditorGUILayout.PropertyField(onCleanup_prop);
                    EditorGUILayout.PropertyField(onActivateUI_prop);
                    EditorGUILayout.PropertyField(onDeactivateUI_prop);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
            OnUpdateUIF();
            serializedObject.ApplyModifiedProperties();
        }
    }
}