using System.Collections;
using TMPro;
using TunnelTone.Elements;
using TunnelTone.Events;
using TunnelTone.Singleton;
using UnityEngine;

namespace TunnelTone.PlayArea
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public int totalCombo;
        private const float MaxScore = 10000000;
        private TextMeshProUGUI Score => GetComponent<TextMeshProUGUI>();
        private static float CurrentScore { get; set; }
        private static float DisplayScore { get; set; }

        private void Start()
        {
            ChartEventReference.Instance.OnNoteHit.AddListener(UpdateScore);
        }

        public void UpdateScore(params object[] param)
        {
            #region Convert parameters

            var offset = (float)param[0];

            #endregion

            switch (offset)
            {
                case <= 25:
                    CurrentScore += MaxScore + 10000f / totalCombo;
                    StartCoroutine(UpdateDisplay((MaxScore + 10000) / totalCombo / 30f, 30));
                    break;
                case <= 50:
                    CurrentScore += MaxScore / totalCombo;
                    StartCoroutine(UpdateDisplay(MaxScore / totalCombo / 30f, 30));
                    break;
                case <= 100:
                    CurrentScore += 0.6f * (MaxScore / totalCombo);
                    StartCoroutine(UpdateDisplay(0.6f * (MaxScore / totalCombo / 30f), 30));
                    break;
                default:
                    CurrentScore += 0;
                    break;
            }
        }

        private IEnumerator UpdateDisplay(float delta, int remain)
        {
            yield return null;
            
            if (remain == 0) yield break;

            DisplayScore += delta;
            Score.text = $"{DisplayScore:00000000}";

            remain--;
            StartCoroutine(UpdateDisplay(delta, remain));
        }
    }
}