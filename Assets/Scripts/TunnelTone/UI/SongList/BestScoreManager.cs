using TMPro;
using TunnelTone.Events;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI.SongList
{
    public class BestScoreManager : MonoBehaviour
    {
        private float scoreValue;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private Slider slider;
        
        private void Start()
        {
            SongListEvent.OnSelectItem.AddListener(UpdateScore);
            SongListEvent.OnDifficultyChange.AddListener(UpdateDifficulty);
        }
        
        private void UpdateDifficulty(params object[] param)
        {
            var finalScore = SongListManager.currentlySelected.GetScore(SongListDifficultyManager.Instance.CurrentlySelected);
            
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, f =>
                {
                    scoreValue = f;
                    slider.value = f;
                    score.text = $"{Mathf.FloorToInt(scoreValue):D8}";
                }, scoreValue,
                finalScore, .35f)
                .setOnComplete(() =>
                {
                    scoreValue = finalScore;
                    slider.value = finalScore;
                    score.text = $"{Mathf.FloorToInt(scoreValue):D8}";
                })
                .setEase(LeanTweenType.easeOutSine);
        }

        private void UpdateScore(params object[] param)
        {
            var song = (SongListItem)param[0];
            var songData = song.songData;
            
            var finalScore = songData.GetScore(SongListDifficultyManager.Instance.CurrentlySelected);
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, f =>
                {
                    scoreValue = f;
                    slider.value = f;
                    score.text = $"{Mathf.FloorToInt(scoreValue):D8}";
                }, scoreValue, finalScore, .35f)
                .setOnComplete(() =>
                {
                    scoreValue = finalScore;
                    slider.value = finalScore;
                    score.text = $"{Mathf.FloorToInt(scoreValue):D8}";
                })
                .setEase(LeanTweenType.easeOutSine);
        }
    }
}