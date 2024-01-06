using System;
using TunnelTone.Core;
using TunnelTone.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.Gauge
{
    public class GaugeManager : MonoBehaviour
    {
        internal static Gauge gauge;
        
        [SerializeField] private Slider musicPlayGauge;
        [SerializeField] private Slider resultScreenGauge;

        private void Start()
        {
            SystemEvent.OnChartLoadFinish.AddListener(delegate
            {
                Initialize(Settings.Gauge);
            });
        }
        
        internal void Initialize(Gauge newGauge)
        {
            gauge = newGauge.Initialize(musicPlayGauge, resultScreenGauge);
        }
    }
}