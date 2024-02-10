using TunnelTone.Charts;
using TunnelTone.ScriptableObjects;
using UnityEngine;

namespace TunnelTone.Events
{
    public static class SystemEvent
    {
        internal static readonly GameEvent OnChartLoad = new();
        internal static readonly GameEvent OnChartLoadFinish = new();

        internal static readonly GameEvent OnDisplayResult = new();
        
        internal static readonly GameEvent OnDisplayDialog = new();
        internal static readonly GameEvent OnAbortDialog = new();
        
        internal static readonly GameEvent OnEnterSettings = new();
        
        internal static readonly GameEvent OnSettingsChanged = new();
        internal static readonly GameEvent OnAudioSystemReset = new();

        public static event ChartLoadEvent ChartLoad;
        public delegate void ChartLoadEvent(Chart chart, AudioClip audioClip);
        public static void InvokeChartLoad(Chart chart, AudioClip audioClip) => ChartLoad?.Invoke(chart, audioClip);
    }
}