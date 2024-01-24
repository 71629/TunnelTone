using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TunnelTone.Charts;
using TunnelTone.Core;
using TunnelTone.Events;
using TunnelTone.PlayArea;
using TunnelTone.Singleton;
using TunnelTone.UI.Reference;
using UnityEngine;

// TODO: Refactor

namespace TunnelTone.Elements
{
    public class NoteRenderer : Singleton<NoteRenderer>
    {
        internal static readonly GameEvent OnDestroyChart = new();
        
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private Transform noteContainer;
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

        public float chartSpeedModifier;

        public float universalOffset;
        public float offsetTime;
        public const float StartDelay = 2500f;
        public static float dspSongStartTime, dspSongEndTime;

        public float currentBpm;
        public static float CurrentTime => (float)AudioSettings.dspTime - dspSongStartTime;
        public static bool isPlaying = false;

        public GameObject Fill;
        public RectTransform FillArea;
        public Vector2 fillVector2;

        // Debug
        private float _currentTime;

        private void Update()
        {
            _currentTime = CurrentTime;
        }
        
        private void Start()
        {
            fillVector2 = Fill.transform.position;
            transform.localPosition = Vector3.zero;
            ChartEventReference.Instance.OnQuit.AddListener(delegate { StopCoroutine(PlayChart()); });
            SystemEvent.OnChartLoadFinish.AddListener(delegate
            {
                Resume();
            });
            SystemEvent.OnSettingsChanged.AddListener(delegate
            {
                var settings = Settings.instance;
                chartSpeedModifier = settings.chartSpeed;
                universalOffset = settings.universalOffset;
                var main = particleSystem.main;
                main.startSpeed = new ParticleSystem.MinMaxCurve(1000 * (chartSpeedModifier / 6.5f), 2500 * (chartSpeedModifier / 6.5f));
            });
            OnDestroyChart.AddListener(delegate
            {
                StopAllCoroutines();
                ResetContainer();
            });
        }

        internal void ResetContainer()
        {
            isPlaying = false;
            transform.localPosition = Vector3.zero;
            foreach (var gb in GetComponentsInChildren<Transform>())
            {
                if (gb.gameObject == gameObject) continue;
                Destroy(gb.gameObject);
            }
        }
        
        private IEnumerator PlayChart()
        {
            Debug.Log(ScoreManager.totalCombo);
            isPlaying = true;
            while (true)
            {
                if (transform.childCount == 0)
                {
                    ChartEventReference.Instance.OnSongEnd.Trigger();
                    yield break;
                }
                yield return transform.localPosition = new Vector3(0, 0, -chartSpeedModifier * (1000 * CurrentTime + offsetTime + StartDelay + universalOffset).TranslateTiming());
            }
        }

        public void Pause()
        {
            ChartEventReference.Instance.OnPause.Trigger();
            UIElementReference.Instance.pause.SetActive(true);
            AudioListener.pause = true;

            LeanTween.pause(Fill);

            particleSystem.Pause();
        }
        
        public void Resume()
        {
            ChartEventReference.Instance.OnResume.Trigger();
            UIElementReference.Instance.pause.SetActive(false);

            LeanTween.resume(Fill);

            AudioListener.pause = false;
            particleSystem.Play();
        }

        private void ResumeForListener(object param)
        {
            Resume();
            SystemEvent.OnChartLoadFinish.RemoveListener(ResumeForListener);
        }
        
        public void Retry()
        {
            //LeanTween.cancel(Fill);
            //Fill.transform.position = new Vector2(-86.15f, 47.60f);
            LeanTween.move(Fill, fillVector2, 0.1f);
            isPlaying = false;
            StopAllCoroutines();
            transform.position = Vector3.zero;
            foreach (var gb in GetComponentsInChildren<Transform>())
            {
                if (gb.gameObject == gameObject) continue;
                Destroy(gb.gameObject);
            }
            Fill.transform.position = new Vector2(0,Fill.transform.position.z);

            ChartEventReference.Instance.OnRetry.Trigger();
            SystemEvent.OnChartLoadFinish.AddListener(ResumeForListener);
        }
        
        public void Quit()
        {
            ChartEventReference.Instance.OnQuit.Trigger();
            LeanTween.move(Fill, fillVector2, 0.1f);
        }

        public void StartSong()
        {
            Debug.Log(AudioSettings.dspTime);
            
            // Set up song start and end times
            dspSongStartTime = (float)AudioSettings.dspTime + StartDelay / 1000f;
            dspSongEndTime = (float)AudioSettings.dspTime + audioSource.clip.length * 1000;
            Debug.Log($"Start time: {dspSongStartTime}\nEnd time: {dspSongEndTime}");
            
            LeanTween.moveX(Fill, 85.5f , (dspSongEndTime / 1000) + 2.8f);
            audioSource.time = 0;
            audioSource.volume = .2f;
            audioSource.PlayDelayed(StartDelay / 1000f);
            StartCoroutine(PlayChart());
        }
    }
}
