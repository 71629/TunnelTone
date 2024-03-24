using UnityEngine;

namespace TunnelTone.Charts
{
    public static class TimingManager
    {
        internal static AnimationCurve timingSheet;
        
        // Modifies caller according to timingSheet
        internal static float TranslateTiming(this float timestamp) => timingSheet.Evaluate(timestamp);
    }
}