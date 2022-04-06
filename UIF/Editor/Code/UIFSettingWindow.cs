using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace UIF
{
    public class UIFSettingWindow : EditorWindow
    {
        [MenuItem("Edit/Clear UIF Save Data")]
        static void DoSomething(MenuCommand command)
        {
            var result = EditorUtility.DisplayDialog("Warning", "UIF save data will be wiped out. \n Are you are?", "Ok", "cancel");
            if (!result) { return; }
            var fPath = Path.Combine(Application.persistentDataPath, UIFSetting.UIFFileName + UIFSetting.UIFFileExtension);
            if (File.Exists(fPath))
            {
                File.Delete(fPath);
            }
            Debug.Log("UIF save data deleted from: " + fPath);
        }
    }
}