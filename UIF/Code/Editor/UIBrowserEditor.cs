using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIF
{
    [CustomEditor(typeof(UIBrowser))]
    [CanEditMultipleObjects]
    internal class UIBrowserEditor : Editor
    {
        UIBrowser controller;
        void OnEnable()
        {
            controller = (UIBrowser)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            if (GUILayout.Button("Fetch enabled pages inside"))
            {
                controller.LoadPagesifReq();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}