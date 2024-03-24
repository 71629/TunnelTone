using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TunnelTone.Charts;
using TunnelTone.Core;
using TunnelTone.Events;
using TunnelTone.PlayArea;
using TunnelTone.Singleton;
using TunnelTone.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace TunnelTone.Elements
{
    public class NoteRenderer : Singleton<NoteRenderer>
    {
        internal static readonly GameEvent OnDestroyChart = new();
        
        [FormerlySerializedAs("particleSystem")] 
        [SerializeField] private ParticleSystem musicPlayParticleSystem;
        
        [SerializeField] private Transform noteContainer;
        #region Element Container
        
        public static readonly List<Trail> TrailList = new();
        public static readonly List<GameObject> TapList = new();
        public static List<GameObject> FlickList = new();
        public static Trail TrailReference => TrailList.Last();
        
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
        
        public delegate void RetryHandler();
        public delegate void OnBeginHandler();
        public static event RetryHandler Retry;
        public static event OnBeginHandler BeginSong;
        
        
        private void Start()
        {
            //Fill = GameObject.Find("Music Play/UI/Progress/Fill Area/Fill");
            //fillVector2 = Fill.transform.position;
            transform.localPosition = Vector3.zero;
            ChartEventReference.Instance.OnQuit.AddListener(delegate { StopCoroutine(PlayChart()); });
            JsonScanner.ChartLoadFinish += Resume;
            PauseMenu.GamePause += PauseAudioAndEffect;
            SystemEvent.OnSettingsChanged.AddListener(delegate
            {
                var settings = Settings.instance;
                chartSpeedModifier = settings.chartSpeed;
                universalOffset = settings.universalOffset;
                var main = musicPlayParticleSystem.main;
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
            BeginSong?.Invoke();
            InteractionManager.Instance.Enable();
            while (true)
            {
                if (transform.childCount == 0)
                {
                    ChartEventReference.Instance.OnSongEnd.Trigger();
                    InteractionManager.Instance.Disable();
                    yield break;
                }

                var timestamp = 1000 * CurrentTime + offsetTime + StartDelay + universalOffset;
                timestamp = timestamp.TranslateTiming();
                yield return transform.localPosition = new Vector3(0, 0, -chartSpeedModifier * timestamp);
            }
        }

        public void Pause()
        {
            PauseMenu.Pause();
            PauseMenu.GameResume += Resume;
            PauseMenu.GameQuit += Quit;
            PauseMenu.GameRetry += RetryCallback;
        }

        private void PauseAudioAndEffect()
        {
            AudioListener.pause = true;
            musicPlayParticleSystem.Pause();
        }
        
        public void Resume()
        {
            PauseMenu.GameResume -= Resume;
            PauseMenu.GameQuit -= Quit;
            PauseMenu.GameRetry -= RetryCallback;
            
            AudioListener.pause = false;
            musicPlayParticleSystem.Play();
        }

        private void ResumeForListener()
        {
            Resume();
            JsonScanner.ChartLoadFinish -= ResumeForListener;
        }
        
        public void RetryCallback()
        {
            PauseMenu.GameResume -= Resume;
            PauseMenu.GameQuit -= Quit;
            PauseMenu.GameRetry -= RetryCallback;
            
            isPlaying = false;
            StopAllCoroutines();
            transform.position = Vector3.zero;
            foreach (var gb in GetComponentsInChildren<Transform>())
            {
                if (gb.gameObject == gameObject) continue;
                Destroy(gb.gameObject);
            }

            Retry?.Invoke();
            JsonScanner.ChartLoadFinish += ResumeForListener;
        }
        
        public void Quit()
        {
            PauseMenu.GameResume -= Resume;
            PauseMenu.GameQuit -= Quit;
            PauseMenu.GameRetry -= RetryCallback;
            
            MusicPlayDescription.instance.module.Quit();
            
            ChartEventReference.Instance.OnQuit.Trigger();
        }

        public void StartSong()
        {
            // Set up song start and end times
            dspSongStartTime = (float)AudioSettings.dspTime + StartDelay / 1000f;
            dspSongEndTime = (float)AudioSettings.dspTime + audioSource.clip.length * 1000;
            
            audioSource.time = 0;
            audioSource.volume = .2f;
            audioSource.PlayScheduled(AudioSettings.dspTime + StartDelay / 1000);
            StartCoroutine(PlayChart());
        }
    }
}
