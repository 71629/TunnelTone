﻿using System.Collections;
using TunnelTone.Events;
using TunnelTone.Singleton;
using TunnelTone.UI.SongList;
using UnityEngine;

namespace TunnelTone.GameSystem
{
    public class AudioManager : Singleton<AudioManager>
    {
        private SongListItem _current;
        
        [Header("This GameObject")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Animator animator;
        
        private static readonly int FadeOut = Animator.StringToHash("FadeOut");
        private static readonly int IsPreviewing = Animator.StringToHash("isPreviewing");
        private static readonly int EnterSong = Animator.StringToHash("EnterSong");
        private SongListEventReference songListEvent => SongListEventReference.Instance;
        private void Start()
        {
            songListEvent.OnSelectItem.AddListener(Preview);
            songListEvent.OnSongStart.AddListener(StopPreview);
        }

        private void Preview(params object[] param)
        {
            _current = (SongListItem)param[0];
            StopAllCoroutines();
            StartCoroutine(PlayPreview());
        }

        private IEnumerator PlayPreview()
        {
            audioSource.clip = _current.previewAudio;
            audioSource.Play();

            while(true)
            {
                audioSource.time = _current.previewStart * 0.001f;
                yield return new WaitForSecondsRealtime(_current.previewDuration * 0.001f);
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