using TunnelTone.Core;
using TunnelTone.Events;
using TunnelTone.UI.PlayResult;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.Gauge
{
    public class IntegrityGauge : Gauge
    {
        internal static readonly GameEvent OnSuddenDeath = new();
        
        protected readonly Color normalColor = new(1f, 0f, 0.4f);
        protected readonly Color integrityCriticalColor = new(1f, 0.53f, 0f);

        internal override Gauge Initialize(Slider musicPlayGauge, Slider resultScreenGauge)
        {
            var ret = base.Initialize(musicPlayGauge, resultScreenGauge);
            
            gaugeMode = GaugeMode.Integrity;
            clearType = ClearType.HardClear;

            gainRate = (15f + 0.28f * (totalCombo - 1)) / totalCombo;
            lossRate = 9;
            
            musicPlayGauge.maxValue = 100;
            musicPlayGauge.minValue = 0;
            resultScreenGauge.maxValue = 100;
            resultScreenGauge.minValue = 0;

            currentValue = 100;
            musicPlayGauge.value = currentValue;
            
            ResultScreen.playResult.clearType = ClearType.HardClear;
            
            UpdateGauge();
            
            return ret;
        }
        
        protected override void OnPerfectHit()
        {
            if (!isTrackingUpdate) return;
            currentValue += gainRate;
        }

        protected override void OnGreatHit()
        {
            if (!isTrackingUpdate) return;
            currentValue += gainRate / .35f;
        }

        protected override void OnMiss()
        {
            if (!isTrackingUpdate) return;
            currentValue -= lossRate;
            lossRate = 5 * (currentValue / 100) + 5;
        }

        protected override void OnEmpty()
        {
            isTrackingUpdate = false;
            ResultScreen.playResult.clearType = ClearType.Failed;
            OnSuddenDeath.Trigger();
            ChartEventReference.Instance.OnSongEnd.Trigger();
        }

        protected override void OnMaxOut() { }
        
        protected override void UpdateGauge()
        {
            if (!isTrackingUpdate) return;
            base.UpdateGauge();
            
            // >=30 => normal | <30 => integrityCritical
            if (currentValue < 30)
            {
                LeanTween.cancel(musicPlayGaugeFill.gameObject);
                LeanTween.value(musicPlayGaugeFill.gameObject, c => { musicPlayGaugeFill.color = c; },
                    musicPlayGaugeFill.color, integrityCriticalColor, .45f);
            }
            else
            {
                LeanTween.cancel(musicPlayGaugeFill.gameObject);
                LeanTween.value(musicPlayGaugeFill.gameObject, c => { musicPlayGaugeFill.color = c; },
                    musicPlayGaugeFill.color, normalColor, .45f);
            }
        }
    }
}