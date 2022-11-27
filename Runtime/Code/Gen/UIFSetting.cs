using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    public static class UIFSetting
    {
        public const bool useLogs = true;
        public const bool safetyCheck = true;
        public const bool encryptContent = true;
        public const string sharedSecret = "o7x8y6";
        public const string salt = "o6806642kbM7c5";
        public const EncodingType encoding = EncodingType.UTF_8;
        public const string UIFFileName = "uif";
        public const string BootSceneName = "boot";
        public const string UIFFileExtension = ".styx";
    }
}