using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TunnelTone.Singleton;
using UnityEngine;

// TODO: Refactor

namespace TunnelTone.Elements
{
    public class NoteRenderer : Singleton<NoteRenderer>
    {
        #region Element Container

        public static readonly List<GameObject> TrailList = new();
        public static readonly List<GameObject> TapList = new();
        public static List<GameObject> FlickList = new();
        public static GameObject TrailReference => TrailList.Last();
        
        #endregion
        
        #region References
        public GameObject gameArea;
        public AudioSource audioSource;
        public Material left, right, none;
        #endregion
        
        private Transform _transform;

        public float chartSpeedModifier;
        
        public float offsetTime;
        public const float StartDelay = 2500f;
        public static float dspSongStartTime, dspSongEndTime;

        public float currentBpm;
        public static float currentTime => (float)AudioSettings.dspTime - dspSongStartTime;

        private bool isPlaying = true;

        private void Start()
        {
            _transform = GetComponent<Transform>();
            _transform.localPosition = Vector3.zero;
        }

        IEnumerator PlayChart()
        {
            yield return new WaitUntil(() => isPlaying);
            
            while (isPlaying)
            {
                yield return _transform.localPosition = new Vector3(0, 0, chartSpeedModifier * (-1000 * currentTime + offsetTime + StartDelay));
            }

            StartCoroutine(PlayChart());
        }

        public void StartSong()
        {
            Debug.Log(AudioSettings.dspTime);
            
            // Set up song start and end times
            dspSongStartTime = (float)AudioSettings.dspTime + StartDelay / 1000f;
            dspSongEndTime = (float)AudioSettings.dspTime + audioSource.clip.length * 1000;
            Debug.Log($"Start time: {dspSongStartTime}\nEnd time: {dspSongEndTime}");

            audioSource.time = 0;
            audioSource.volume = 1;
            audioSource.PlayDelayed(StartDelay / 1000f);
            StartCoroutine(PlayChart());
        }
    }
}
