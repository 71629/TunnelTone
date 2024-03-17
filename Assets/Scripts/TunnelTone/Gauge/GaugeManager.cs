using TunnelTone.Charts;
using TunnelTone.Core;
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
            JsonScanner.ChartLoadFinish += Initialize;
        }
        
        internal void Initialize()
        {
            gauge = Settings.Gauge.Initialize(musicPlayGauge, resultScreenGauge);
        }
    }
}