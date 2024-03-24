using TMPro;
using TunnelTone.Charts;
using TunnelTone.Events;
using TunnelTone.UI.PlayResult;
using UnityEngine;

namespace TunnelTone.PlayArea
{
    public class ComboManager : MonoBehaviour
    {
        private const string ComboTextPrefix = "<size=20>combo</size>\n";
        private TextMeshProUGUI combo;

        private static short CurrentCombo { get; set; }
        private static short MaxCombo { get; set; }

        private void Awake()
        {
            combo = GetComponent<TextMeshProUGUI>();
        }
        
        private void Start()
        {
            JsonScanner.ChartLoadFinish += ResetCombo;
            ChartEventReference.Instance.OnNoteHit.AddListener(UpdateCombo);
            ChartEventReference.Instance.OnNoteMiss.AddListener(delegate
            {
                CurrentCombo = 0;
                combo.text = $"<size=20>combo</size>\n{CurrentCombo}";
            });
            ChartEventReference.Instance.OnSongEnd.AddListener(delegate
            {
                ResultScreen.playResult.maxCombo = MaxCombo;
            });
        }

        private void ResetCombo()
        {
            CurrentCombo = 0;
            combo.text = $"{ComboTextPrefix}{CurrentCombo}";
        }
        
        public void UpdateCombo(params object[] param)
        {
            #region Convert parameters

            var offset = (float)param[0];

            #endregion

            // Update ComboCount
            switch (offset)
            {
                case <120:
                    CurrentCombo += 1;
                    break;
                default:
                    CurrentCombo = 0;
                    break;
            }
            // Update Combo text
            combo.text = $"<size=20>combo</size>\n{CurrentCombo}";
            MaxCombo = CurrentCombo > MaxCombo ? CurrentCombo : MaxCombo;
        }
    }
}

namespace TunnelTone.Core
{
    public partial struct PlayResult
    {
        public int maxCombo;
    }
}