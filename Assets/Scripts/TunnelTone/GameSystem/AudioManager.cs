using System.Collections;
using TunnelTone.Core;
using TunnelTone.Events;
using TunnelTone.ScriptableObjects;
using TunnelTone.Singleton;
using TunnelTone.UI.SongList;
using UnityEngine;

namespace TunnelTone.GameSystem
{
    public class AudioManager : Singleton<AudioManager>
    {
        private SongData loadedSongData;
        
        [SerializeField] internal AudioSource audioSource;
        
        private static SongListEvent SongListEvent => SongListEvent.Instance;
        private static ChartEventReference ChartEvent => ChartEventReference.Instance;
        
        private void Start()
        {
            SystemEvent.OnAudioSystemReset.AddListener(o =>
            {
                StartCoroutine(ResetAudioSystem((AudioConfiguration)o[0]));
            });
            
            SongListItem.SelectItem += SongListItemOnSelectCallback;
            SongListManager.SongStart += StopPreview;
            
            // Fade out the audio when the the chart ends
            ChartEvent.OnSongEnd.AddListener(delegate
            {
                LeanTween.value(audioSource.gameObject,
                    f => { audioSource.volume = f; }, audioSource.volume, 0, 1.5f);
            });
        }

        private void SongListItemOnSelectCallback(SongData songData)
        {
            LeanTween.cancel(gameObject);
            loadedSongData = songData;
            audioSource.clip = loadedSongData.music;
            StartCoroutine(Preview());
        }

        private void FadePreview()
        { 
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, f =>
            {
                audioSource.volume = f;
            }, .2f, 0f, 1.2f)
            .setOnComplete(() =>
            {
                audioSource.Stop();
                StartCoroutine(Preview());
            });
        }

        private IEnumerator Preview()
        {
            audioSource.volume = .2f;
            
            audioSource.time = loadedSongData.previewStart * 0.001f;
            audioSource.Play();
            
            yield return new WaitUntil(() => audioSource.time >= (loadedSongData.previewStart + loadedSongData.previewDuration) * 0.001f);
            FadePreview();
        }
        
        private void StopPreview(ref MusicPlayDescription _)
        {
            LeanTween.value(gameObject, f =>
                {
                    audioSource.volume = f;
                }, audioSource.volume, 0f, 1.2f)
                .setOnComplete(() =>
                {
                    audioSource.Stop();
                    StopAllCoroutines();
                });
        }

        private IEnumerator ResetAudioSystem(AudioConfiguration config)
        {
            LeanTween.cancel(gameObject);
            yield return AudioSettings.Reset(config);
            StartCoroutine(Preview());
        }
    }
}