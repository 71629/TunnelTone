using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TunnelTone.Events;
using UnityEditor.iOS;
using UnityEngine.Serialization;

namespace TunnelTone.UI.SongList
{
    public class SongListItem : MonoBehaviour
    {
        public Animator animator;
        public Button button;
        
        public TextMeshProUGUI title;
        public TextMeshProUGUI artist;
        public TextMeshProUGUI difficulty;

        public Image difficultyBackground;
        public Image songJacket;

        public float previewStart;
        public float previewDuration;

        public SongListManager.Song source;
        
        private static readonly int IsSelected = Animator.StringToHash("isSelected");
        private static readonly int FadeOut = Animator.StringToHash("FadeOut");

        private SongListEventReference SongListEvent => SongListEventReference.Instance;
        private UIElementReference UIElement => UIElementReference.Instance;
        private AudioSource AudioSource => UIElement.audioSource;
        private Animator AudioAnimator => UIElement.audioAnimator;
        
        public void ItemSelected()
        {
            SongListManager.Instance.CurrentlySelected = source;
            SongListEventReference.Instance.OnSelectItem.Trigger(this);
            
            animator.SetBool(IsSelected, true);
            UIElementReference.Instance.songJacket.sprite = Resources.Load<Sprite>($"Songs/{title.text}/Jacket");
            
            // Start previewing song at previewStart
            AudioSource.clip = Resources.Load<AudioClip>($"Songs/{title.text}/{title.text}");
            ResetPreviewTime();
            AudioSource.Play();
        }

        private void ResetPreviewTime()
        {
            AudioSource.time = previewStart * 0.001f;
            Invoke(nameof(RestartPreview), previewDuration * 0.001f);
        }
        private void RestartPreview()
        { 
            AudioAnimator.SetTrigger(FadeOut);
            Invoke(nameof(ResetPreviewTime), 1);
        }
            
        
        private void Start()
        {
            SongListEvent.OnSelectItem.AddListener(OnSelectItem);
            SongListEvent.OnDifficultyChange.AddListener(OnDifficultyChange);
        }

        private void OnDifficultyChange(object[] param)
        {
            var index = (int)param[0];
            difficulty.text = $"{DifficultyManager.Instance.difficultyDictionary[source.difficulty[index]]}";
            difficultyBackground.color = index switch
            {
                0 => UIElement.easy,
                1 => UIElement.hard,
                2 => UIElement.intensive,
                3 => UIElement.insane,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void OnSelectItem(object[] param)
        {
            CancelInvoke();
            animator.SetBool(IsSelected, false);
            button.interactable = SongListManager.Instance.CurrentlySelected != source;
        }
    }
}