using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TunnelTone.Events;
using TunnelTone.Gauge;
using TunnelTone.UI.PlayResult;
using UnityEngine;

namespace TunnelTone.Elements
{
    public class ComboPoint : MonoBehaviour
    {
        public float time;

        private void Start()
        {
            IntegrityGauge.OnSuddenDeath.AddListener(AssumeMiss);
            ChartEventReference.Instance.OnSongEnd.AddListener(OnSongEnd);
        }
        
        private void Update()
        {
            float currentTime = NoteRenderer.CurrentTime * 1000;
            
            if (!NoteRenderer.isPlaying) return;
            if (currentTime < time)
            {
                return;
            }
            if (currentTime >= time + 120)
            {
                ChartEventReference.Instance.OnNoteMiss.Trigger();
                Destroy(gameObject);
                return;
            }
            if (currentTime >= time && GetComponentInParent<Trail>().isTracking)
            {
                ChartEventReference.Instance.OnNoteHit.Trigger(0f);
                Destroy(gameObject);
            }
        }

        private static void AssumeMiss(params object[] param)
        {
            ResultScreen.playResult.miss++;
            IntegrityGauge.OnSuddenDeath.RemoveListener(AssumeMiss);
        }

        private void OnSongEnd(params object[] param)
        {
            enabled = false;
        }

        private void OnDestroy()
        {
            ChartEventReference.Instance.OnSongEnd.RemoveListener(OnSongEnd);            
            IntegrityGauge.OnSuddenDeath.RemoveListener(AssumeMiss);
        }
    }
}