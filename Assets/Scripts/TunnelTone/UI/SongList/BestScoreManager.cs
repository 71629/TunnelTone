using TMPro;
using TunnelTone.Events;
using TunnelTone.ScriptableObjects;
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
            SongListItem.SelectItem += UpdateScore;
            SongListDifficultyManager.DifficultyChange += UpdateDifficulty;
        }
        
        private void UpdateDifficulty(int difficulty)
        {
            var finalScore = SongListManager.currentlySelected.GetScore(difficulty);
            
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

        private void UpdateScore(SongData songData)
        {
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