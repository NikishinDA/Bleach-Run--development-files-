using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationTapticFix
{
    public static void Vibrate(long milliseconds, int amplitude)
    {
        if (!Taptic.tapticOn || Application.isEditor) return;
        #if UNITY_ANDROID
        AndroidTaptic.AndroidVibrate(milliseconds, amplitude);
        #endif
    }
}
