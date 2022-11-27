using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    internal static class ULog
    {
        internal static void Check(System.Action Code)
        {
#if _KLOG_
            KLog.Check(() =>
            {
                Code?.Invoke();
            });
#else
            try
            {
                Code?.Invoke();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
#endif
        }

        internal static void Print(string message, Color color = default)
        {
            if (UIFSetting.useLogs == false) { return; }
            string resultLog = message;
            if (color != default)
            {
                resultLog = string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(color.r * 255f),
                    (byte)(color.g * 255f), (byte)(color.b * 255f), message);
            }
#if _KLOG_
            KLog.Print(resultLog, color);
#else
            Debug.Log(resultLog);
#endif
        }

        internal static void PrintWarning(string message)
        {
            if (UIFSetting.useLogs == false) { return; }
#if _KLOG_
            KLog.PrintWarning(message);
#else
            Debug.LogWarning(message);
#endif
        }

        internal static void PrintError(string message)
        {
            if (UIFSetting.useLogs == false) { return; }
#if _KLOG_
            KLog.PrintError(message);
#else
            Debug.LogError(message);
#endif
        }

        internal static void PrintException(System.Exception exception)
        {
            if (UIFSetting.useLogs == false) { return; }
#if _KLOG_
            KLog.PrintException(exception);
#else
            Debug.LogException(exception);
#endif
        }
    }
}