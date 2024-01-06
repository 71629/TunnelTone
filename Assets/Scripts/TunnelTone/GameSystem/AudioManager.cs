using System.Collections;
using TunnelTone.Events;
using TunnelTone.Singleton;
using TunnelTone.UI.SongList;
using UnityEngine;

namespace TunnelTone.GameSystem
{
    public class AudioManager : Singleton<AudioManager>
    {
        private SongListItem current;
        
        [SerializeField] private AudioSource audioSource;
        
        private static SongListEventReference SongListEvent => SongListEventReference.Instance;
        private static ChartEventReference ChartEvent => ChartEventReference.Instance;
        
        private void Start()
        {
            SystemEvent.OnAudioSystemReset.AddListener(o =>
            {
                StartCoroutine(ResetAudioSystem((AudioConfiguration)o[0]));
            });
            SongListEvent.OnSelectItem.AddListener(o =>
            {
                LeanTween.cancel(gameObject);
                current = (SongListItem)o[0];
                audioSource.clip = current.previewAudio;
                StartCoroutine(Preview());
            });
            SongListEvent.OnSongStart.AddListener(StopPreview);
            
            // Fade out the audio when the the chart ends
            ChartEvent.OnSongEnd.AddListener(delegate
            {
                LeanTween.value(audioSource.gameObject,
                    f => { audioSource.volume = f; }, audioSource.volume, 0, 1.5f);
            });
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
            audioSource.Play();
            
            audioSource.time = current.previewStart * 0.001f;
            yield return new WaitUntil(() => audioSource.time >= (current.previewStart + current.previewDuration) * 0.001f);
            FadePreview();
        }
        
        private void StopPreview(params object[] param)
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