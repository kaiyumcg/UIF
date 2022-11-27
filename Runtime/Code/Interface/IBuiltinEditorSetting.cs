using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    public interface IBuiltinEditorSetting
    {
        bool ValidateEveryInspectorFrame { get; set; }
        bool ConfigError { get; set; }
        string ConfigErrorMessage { get; set; }
    }
}