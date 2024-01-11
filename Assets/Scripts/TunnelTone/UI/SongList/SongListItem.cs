using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TunnelTone.Events;
using TunnelTone.GameSystem;
using TunnelTone.ScriptableObjects;
using TunnelTone.UI.Reference;

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
        public AudioClip previewAudio;

        public float previewStart;
        public float previewDuration;

        public AudioClip source;
        public SongData songData;
        
        private static readonly int IsSelected = Animator.StringToHash("isSelected");
        private static SongListEvent SongListEvent => SongListEvent.Instance;
        private static UIElementReference UIElement => UIElementReference.Instance;
        
        internal void ItemSelected()
        {
            SongListEvent.OnSelectItem.Trigger(this);
            
            animator.SetBool(IsSelected, true);
            UIElement.songJacket.sprite = songData.jacket;
        }
        
        private void Start()
        {
            SongListEvent.OnEnterSongList.AddListener(delegate
            {
                ItemSelected();
            });
            SongListEvent.OnSelectItem.AddListener(OnSelectItem);
            SongListEvent.OnDifficultyChange.AddListener(OnDifficultyChange);
            previewAudio = songData.music;
        }
        
        private void OnDifficultyChange(object[] param)
        {
            var index = (int)param[0];
            difficulty.text = $"{Dictionaries.Instance.levelDictionary[songData.GetDifficulties()[index]]}";
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
            button.interactable = SongListManager.currentlySelected != songData;
        }
        
        internal SongListItem SetData(SongData data)
        {
            songData = data;
            
            title.text = data.songTitle;
            artist.text = data.artist;
            difficulty.text = $"{data.GetDifficulty(3)}";
            songJacket.sprite = data.jacket;
            previewStart = data.previewStart;
            previewDuration = data.previewDuration;
            source = data.music;
            
            return this;
        }
    }
}