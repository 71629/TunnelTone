using TMPro;
using TunnelTone.Charts;
using TunnelTone.ScriptableObjects;
using TunnelTone.GameSystem;
using TunnelTone.UI.PlayResult;
using TunnelTone.UI.Reference;
using TunnelTone.UI.SongList;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.PlayArea
{
    public class PlayAreaDifficultyManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI difficultyDisplay;
        [SerializeField] private Image difficultyColorDisplay;
        
        private static int currentDifficulty;
        private static int currentLevel;

        private void Start()
        {
            SongListItem.SelectItem += UpdateDisplay;
            SongListDifficultyManager.DifficultyChange += UpdateDisplay;
            JsonScanner.ChartLoadFinish += SetPlayResultDifficulty;
        }

        private void SetPlayResultDifficulty()
        {
            ResultScreen.playResult.difficulty = currentDifficulty;
            ResultScreen.playResult.level = currentLevel;
        }

        private void UpdateDisplay(int difficulty)
        {
            currentDifficulty = difficulty;
            UpdateDisplayText();
        }

        private void UpdateDisplay(SongData songData)
        {
            currentLevel = songData.GetDifficulty(currentDifficulty);
            UpdateDisplayText();
        }

        private void UpdateDisplayText()
        {
            var level = Dictionaries.Instance.levelDictionary[currentLevel];
            difficultyDisplay.text = $"{level}<size=30>/ {Dictionaries.Instance.difficultyDictionary[currentDifficulty]}</size>";
            difficultyColorDisplay.color = currentDifficulty switch
            {
                0 => UIElementReference.Instance.easy,
                1 => UIElementReference.Instance.hard,
                2 => UIElementReference.Instance.intensive,
                3 => UIElementReference.Instance.insane,
                _ => Color.magenta
            };
        }
    }
}

namespace TunnelTone.Core
{
    public partial struct PlayResult
    {
        public int difficulty; // Assigned
        public int level; // Assigned
    }
}