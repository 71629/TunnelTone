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
        public bool bIsPlaying;
        public bool bIsMissed;
        public float fCurrentTime;

        private void Update()
        {
            bIsPlaying = NoteRenderer.IsPlaying;
            float currentTime = NoteRenderer.currentTime * 1000;
            bIsMissed = !(currentTime < time) && (currentTime >= time + 120);
            fCurrentTime = currentTime;
            
            if (!NoteRenderer.IsPlaying) return;
            if (currentTime < time)
            {
                return;
            }
            Debug.Log("currentTime >= time + 120: "+(currentTime >= time + 120));
            if (currentTime >= time + 120)
            {
                Debug.Log($"Current Time: {currentTime}, Time: {time}, Result: miss");
                ChartEventReference.Instance.OnNoteMiss.Trigger();
                Destroy(gameObject);
                return;
            }
            if (currentTime >= time && GetComponentInParent<Trail>().isTracking)
            {
                Debug.Log($"Current Time: {currentTime}, Time: {time}, Result: hit");
                ChartEventReference.Instance.OnNoteHit.Trigger(0f);
                Destroy(gameObject);
            }
        }
    }
}