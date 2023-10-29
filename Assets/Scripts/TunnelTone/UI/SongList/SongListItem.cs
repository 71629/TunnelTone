using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TunnelTone.Events;
using TunnelTone.GameSystem;
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

        public Song source;

        private Color color = new Color();
        
        private static readonly int IsSelected = Animator.StringToHash("isSelected");
        private SongListEventReference SongListEvent => SongListEventReference.Instance;
        private UIElementReference UIElement => UIElementReference.Instance;
        
        public void ItemSelected()
        {
            SongListEvent.OnSelectItem.Trigger(this);
            
            animator.SetBool(IsSelected, true);
            UIElement.songJacket.sprite = songJacket.sprite;
        }
        
        private void Start()
        {
            SongListEvent.OnSelectItem.AddListener(OnSelectItem);
            SongListEvent.OnDifficultyChange.AddListener(OnDifficultyChange);
            previewAudio = (AudioClip)Resources.Load($"Songs/{source.title}/{source.title}");
        }
        
        private void OnDifficultyChange(object[] param)
        {
            var index = (int)param[0];
            difficulty.text = $"{Dictionaries.Instance.difficultyDictionary[source.difficulty[index]]}";
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