using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using TunnelTone.Events;
using TunnelTone.Singleton;
using TunnelTone.Core;
using TunnelTone.UI.PlayResult;

namespace TunnelTone.PlayArea
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public static int totalCombo;
        private const decimal MaxScore = 10000000;
        private TextMeshProUGUI Score => GetComponent<TextMeshProUGUI>();
        private static decimal CurrentScore { get; set; }
        private static decimal DisplayScore { get; set; }
        private const byte LerpIteration = 30;

        private void Start()
        {
            ChartEventReference.Instance.OnNoteHit.AddListener(UpdateScore);
            SystemEvent.OnChartLoadFinish.AddListener(delegate
            {
                CurrentScore = 0;
                DisplayScore = 0;
                Score.text = $"{DisplayScore:00000000}";
            });
            ChartEventReference.Instance.OnSongEnd.AddListener(delegate
            {
                ResultScreen.playResult.score = (int)CurrentScore;
            });
            ChartEventReference.Instance.OnNoteMiss.AddListener(delegate
            {
                ResultScreen.playResult.miss++;
            });
        }

        public void UpdateScore(params object[] param)
        {
            #region Convert parameters

            var offset = (float)param[0];

            #endregion

            switch (offset)
            {
                case <= 25:
                    ResultScreen.playResult.perfect++;
                    CurrentScore += (MaxScore + 10000m) / totalCombo;
                    StartCoroutine(UpdateDisplay((MaxScore + 10000) / totalCombo / LerpIteration, LerpIteration));
                    break;
                case <= 50:
                    ResultScreen.playResult.perfect++;
                    CurrentScore += MaxScore / totalCombo;
                    StartCoroutine(UpdateDisplay(MaxScore / totalCombo / LerpIteration, LerpIteration));
                    break;
                case <= 100:
                    ResultScreen.playResult.great++;
                    CurrentScore += 0.6m * (MaxScore / totalCombo);
                    StartCoroutine(UpdateDisplay(0.6m * (MaxScore / totalCombo / LerpIteration), LerpIteration));
                    break;
                default:
                    CurrentScore += 0;
                    break;
            }
        }

        private IEnumerator UpdateDisplay(decimal delta, byte remain)
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

namespace TunnelTone.Core
{
    public partial struct PlayResult
    {
        public int score; // Assigned
        
        public int perfect; //Assigned
        public int great; //Assigned
        public int miss; //Assigned
    }
}