using TunnelTone.Core;
using TunnelTone.Events;
using TunnelTone.PlayArea;
using TunnelTone.UI.PlayResult;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.Gauge
{
    public abstract class Gauge
    {
        protected bool isTrackingUpdate;
        
        // Basic musicPlayGauge value
        internal GaugeMode gaugeMode;
        internal ClearType clearType;
        internal float currentValue;

        // Gauge reference
        private Slider musicPlayGauge;
        private Slider resultScreenGauge;
        
        protected Image musicPlayGaugeFill;
        private Image resultScreenGaugeFill;
        
        protected float gainRate;
        protected float lossRate;
        
        protected int totalCombo => ScoreManager.totalCombo;
        
        internal virtual Gauge Initialize(Slider musicPlayGauge, Slider resultScreenGauge)
        {
            this.musicPlayGauge = musicPlayGauge;
            this.resultScreenGauge = resultScreenGauge;
            
            musicPlayGaugeFill = musicPlayGauge.fillRect.GetComponent<Image>();
            resultScreenGaugeFill = resultScreenGauge.fillRect.GetComponent<Image>();
            
            currentValue = 0;
            this.musicPlayGauge.value = currentValue;
            
            ChartEventReference.Instance.OnNoteHit.AddListener(o =>
            {
                var offset = (float)o[0];
                switch (offset)
                {
                    case <= 50f:
                        OnPerfectHit();
                        break;
                    case <= 100f:
                        OnGreatHit();
                        break;
                    default:
                        OnMiss();
                        break;
                }
                UpdateGauge();
                CheckGaugeValue();
            });
            ChartEventReference.Instance.OnNoteMiss.AddListener(delegate
            {
                OnMiss();
                UpdateGauge();
                CheckGaugeValue();
            });
            ChartEventReference.Instance.OnSongEnd.AddListener(OnSongEnd);
            
            isTrackingUpdate = true;
            
            return this;
        }

        protected abstract void OnPerfectHit();
        protected abstract void OnGreatHit();
        protected abstract void OnMiss();
        protected abstract void OnEmpty();
        protected abstract void OnMaxOut();

        protected virtual void UpdateGauge()
        {
            currentValue = Mathf.Clamp(currentValue, musicPlayGauge.minValue, musicPlayGauge.maxValue);
            LeanTween.cancel(musicPlayGauge.gameObject);
            LeanTween.value(musicPlayGauge.gameObject, f => { musicPlayGauge.value = f; }, musicPlayGauge.value, currentValue, .25f);
        }

        private void CheckGaugeValue()
        {
            if (currentValue == 0) OnEmpty();
            if (currentValue.Equals(musicPlayGauge.maxValue)) OnMaxOut();
        }

        protected virtual void OnSongEnd(params object[] param)
        {
            OnDisplayingResult();
        }

        private void OnDisplayingResult()
        {
            ResultScreen.playResult.gaugeMode = gaugeMode;
            ResultScreen.playResult.gaugeMin = musicPlayGauge.minValue;
            ResultScreen.playResult.gaugeMax = musicPlayGauge.maxValue;
            ResultScreen.playResult.gaugeRemain = currentValue;
        }
    }
}

namespace TunnelTone.Core
{
    public partial struct PlayResult
    {
        public GaugeMode gaugeMode; // Assigned
        public float gaugeMin; // Assigned
        public float gaugeRemain; // Assigned
        public float gaugeMax; // Assigned
        
        public ClearType clearType; // Assigned
    }
}