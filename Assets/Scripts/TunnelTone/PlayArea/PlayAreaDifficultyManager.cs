using TMPro;
using TunnelTone.Events;
using TunnelTone.ScriptableObjects;
using UnityEngine;
using TunnelTone.Core;
using TunnelTone.GameSystem;
using TunnelTone.UI.Reference;
using TunnelTone.UI.SongList;
using UnityEngine.UI;

namespace TunnelTone.PlayArea
{
    public class PlayAreaDifficultyManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI difficultyDisplay;
        [SerializeField] private Image difficultyColorDisplay;

        private void Start()
        {
            SystemEventReference.Instance.OnChartLoad.AddListener(UpdateDisplay);
        }

        private void UpdateDisplay(params object[] param)
        {
            var songData = (SongData)param[0];
            var difficulty = (int)param[1];
            var level = Dictionaries.Instance.levelDictionary[songData.GetDifficulty(difficulty)];
            
            difficultyDisplay.text = $"{level}<size=30>/ {Dictionaries.Instance.difficultyDictionary[difficulty]}</size>";
            difficultyColorDisplay.color = difficulty switch
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