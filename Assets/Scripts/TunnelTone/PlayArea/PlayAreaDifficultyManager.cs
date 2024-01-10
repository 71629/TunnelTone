using TMPro;
using TunnelTone.Events;
using TunnelTone.ScriptableObjects;
using UnityEngine;
using TunnelTone.GameSystem;
using TunnelTone.UI.PlayResult;
using TunnelTone.UI.Reference;
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
            SystemEvent.OnChartLoad.AddListener(UpdateDisplay);
            SystemEvent.OnChartLoadFinish.AddListener(delegate
            {
                ResultScreen.playResult.difficulty = currentDifficulty;
                ResultScreen.playResult.level = currentLevel;
            });
        }

        private void UpdateDisplay(params object[] param)
        {
            var songData = (SongData)param[0];
            currentDifficulty = (int)param[1];
            currentLevel = songData.GetDifficulty(currentDifficulty);
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