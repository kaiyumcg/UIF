using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace UIF
{
    public class ScriptTemplateTool
    {
        const string browserName = "_tmpl_uif_browser.cs.txt";
        const string browserEdName = "_tmpl_uif_browser_editor.cs.txt";

        const string uiPageName = "_tmp_uif_Page.cs.txt";
        const string elementName = "_tmpl_uif_element.cs.txt";
        const string elementFeatureName = "_tmp_uif_elementFeature.cs.txt";

        [MenuItem(itemName: "Assets/Create/UIF/Create New Browser Script", isValidateFunction: false, priority: 51)]
        public static void CreateBrowserScript()
        {
            string templateFilePath = GetPath(browserName);
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templateFilePath, "NewBrowser.cs");
        }

        [MenuItem(itemName: "Assets/Create/UIF/Create New UI Page Script", isValidateFunction: false, priority: 52)]
        public static void CreateUIPageScript()
        {
            string templateFilePath = GetPath(uiPageName);
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templateFilePath, "NewUIPage.cs");
        }

        [MenuItem(itemName: "Assets/Create/UIF/Create New UI Element Script", isValidateFunction: false, priority: 53)]
        public static void CreateUIElementScript()
        {
            string templateFilePath = GetPath(elementName);
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templateFilePath, "NewUIElement.cs");
        }

        [MenuItem(itemName: "Assets/Create/UIF/Create New UI Element Feature Script", isValidateFunction: false, priority: 54)]
        public static void CreateUIElementFeatureScript()
        {
            string templateFilePath = GetPath(elementFeatureName);
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templateFilePath, "NewUIElementFeature.cs");
        }

        static string GetPath(string fileName)
        {
            //https://forum.unity.com/threads/how-to-get-list-of-assets-at-asset-path.18898/#post-5181719
            var assets = AssetDatabase.FindAssets("t:TextAsset", new[] { "Assets/", "Library/PackageCache/UIF/" });
            var selGUID = "";
            var selPath = "";
            foreach (var guid in assets)
            {
                var aPath = AssetDatabase.GUIDToAssetPath(guid);
                if (aPath.Contains(fileName))
                {
                    var len = aPath.Length;
                    var c3 = aPath[len - 1];
                    var c2 = aPath[len - 2];
                    var c1 = aPath[len - 3];
                    if (c1 == 't' && c2 == 'x' && c3 == 't')
                    {
                        selGUID = guid;
                        selPath = aPath;
                        break;
                    }
                }
                //Debug.Log(aPath);
                ////var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(AssetDatabase.GUIDToAssetPath(guid));
                ////Debug.Log(clip);
            }
            return selPath;
        }
    }
}