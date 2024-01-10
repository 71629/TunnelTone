using TunnelTone.PlayArea;
using TunnelTone.UI.PlayResult;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.Gauge
{
    
    internal class CompletionGauge : Gauge
    {
        internal const float CompletionRequirement = 70;
        
        protected readonly Color normalColor = new(0.62f, 0.59f, 1f);
        protected readonly Color clearColor = new(0.02f, 0.85f, 1f);

        private int totalCombo => ScoreManager.totalCombo;

        internal override Gauge Initialize(Slider musicPlayGauge, Slider resultScreenGauge)
        {
            var ret = base.Initialize(musicPlayGauge, resultScreenGauge);
            
            clearType = ClearType.NormalClear;
            
            gainRate = (70f + 0.35f * (totalCombo - 1)) / totalCombo;
            // gainRate = .35f + 69.65f / totalCombo;
            lossRate = 4.54f * gainRate;
            
            musicPlayGauge.maxValue = 100;
            musicPlayGauge.minValue = 0;
            resultScreenGauge.maxValue = 100;
            resultScreenGauge.minValue = 0;

            currentValue = 0;
            
            return ret;
        }

        protected override void OnPerfectHit()
        {
            currentValue += gainRate;
        }

        protected override void OnGreatHit()
        {
            currentValue += gainRate * .5f;
        }

        protected override void OnMiss()
        {
            currentValue -= lossRate;
        }

        protected override void UpdateGauge()
        {
            base.UpdateGauge();
            
            // >=70 => clearColor | <70 => normalColor
            musicPlayGaugeFill.color = currentValue >= CompletionRequirement ? clearColor : normalColor;
        }

        protected override void OnEmpty() {  } // Do nothing
        protected override void OnMaxOut() {  } // Do nothing

        protected override void OnSongEnd(params object[] param)
        {
            ResultScreen.playResult.clearType = currentValue >= CompletionRequirement ? ClearType.NormalClear : ClearType.Failed;
            
            base.OnSongEnd();
        }
    }
}