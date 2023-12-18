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
        
        [Header("This GameObject")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Animator animator;
        
        private static readonly int FadeOut = Animator.StringToHash("FadeOut");
        private static readonly int IsPreviewing = Animator.StringToHash("isPreviewing");
        private static readonly int EnterSong = Animator.StringToHash("EnterSong");
        private static SongListEventReference SongListEvent => SongListEventReference.Instance;
        private static ChartEventReference ChartEvent => ChartEventReference.Instance;
        private void Start()
        {
            SongListEvent.OnSelectItem.AddListener(Preview);
            SongListEvent.OnSongStart.AddListener(StopPreview);
            
            // Fade out the audio when the the chart ends
            ChartEvent.OnSongEnd.AddListener(delegate
            {
                LeanTween.value(audioSource.gameObject,
                    f => { audioSource.volume = f; }, audioSource.volume, 0, 1.5f);
            });
        }

        private void Preview(params object[] param)
        {
            current = (SongListItem)param[0];
            StopAllCoroutines();
            StartCoroutine(PlayPreview());
        }

        private IEnumerator PlayPreview()
        {
            audioSource.clip = current.previewAudio;
            audioSource.Play();

            while(true)
            {
                audioSource.time = current.previewStart * 0.001f;
                yield return new WaitForSecondsRealtime(current.previewDuration * 0.001f);
                animator.SetTrigger(FadeOut);
                yield return new WaitForSecondsRealtime(1.2f);
            }
        }
        
        private void StopPreview(params object[] param)
        {
            StopAllCoroutines();
            animator.SetBool(IsPreviewing, false);
            animator.SetTrigger(EnterSong);
            Invoke(nameof(DisableAnimator), 1.1f);
        }

        private void DisableAnimator()
        {
            animator.enabled = false;
            audioSource.Stop();
        }
    }
}