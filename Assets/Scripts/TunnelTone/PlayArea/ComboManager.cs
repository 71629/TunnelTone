﻿using TMPro;
using TunnelTone.Charts;
using TunnelTone.Events;
using TunnelTone.UI.PlayResult;
using UnityEngine;

namespace TunnelTone.PlayArea
{
    public class ComboManager : MonoBehaviour
    {
        private TextMeshProUGUI Combo => GetComponent<TextMeshProUGUI>();

        private static short CurrentCombo { get; set; }
        private static short MaxCombo { get; set; }

        private void Start()
        {
            JsonScanner.ChartLoadFinish += ResetCombo;
            ChartEventReference.Instance.OnNoteHit.AddListener(UpdateCombo);
            ChartEventReference.Instance.OnNoteMiss.AddListener(delegate
            {
                CurrentCombo = 0;
                Combo.text = $"<size=20>combo</size>\n{CurrentCombo}";
            });
            ChartEventReference.Instance.OnSongEnd.AddListener(delegate
            {
                ResultScreen.playResult.maxCombo = MaxCombo;
            });
        }

        private void ResetCombo()
        {
            CurrentCombo = 0;
            Combo.text = $"<size=20>combo</size>\n{CurrentCombo}";
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
            Combo.text = $"<size=20>combo</size>\n{CurrentCombo}";
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