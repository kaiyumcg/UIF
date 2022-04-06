using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UIF
{
    [CustomPropertyDrawer(typeof(CannotEditAttribute))]
    public class CannotEditAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position,
                                   SerializedProperty property,
                                   GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}