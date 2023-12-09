using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TunnelTone.Events;
using UnityEngine;

namespace TunnelTone.Elements
{
    public class ComboPoint : MonoBehaviour
    {
        public float time;
        

        private void Update()
        {
            float currentTime = NoteRenderer.currentTime * 1000;
            
            if (!NoteRenderer.IsPlaying) return;
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
    }
}