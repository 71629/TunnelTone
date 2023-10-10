using System.Collections;
using TMPro;
using TunnelTone.Elements;
using TunnelTone.Events;
using UnityEngine;

namespace TunnelTone.UI
{
    public class ScoreManager : MonoBehaviour
    {
        private float TotalCombo => NoteRenderer.TapList.Count;
        private const int MaxScore = 10000000;
        private TextMeshProUGUI Score => GetComponent<TextMeshProUGUI>();
        private static float CurrentScore { get; set; }
        private static float DisplayScore { get; set; }

        private void Start()
        {
            NoteEventReference.Instance.OnNoteHit.AddListener(UpdateScore);
        }

        public void UpdateScore(params object[] param)
        {
            #region Convert parameters

            var offset = (float)param[0];

            #endregion

            switch (offset)
            {
                case <= 25:
                    CurrentScore += MaxScore + 10000 / TotalCombo;
                    StartCoroutine(UpdateDisplay((MaxScore + 10000) / TotalCombo / 30, 30));
                    break;
                case <= 50:
                    CurrentScore += MaxScore / TotalCombo;
                    StartCoroutine(UpdateDisplay(MaxScore / TotalCombo / 30, 30));
                    break;
                case <= 100:
                    CurrentScore += 0.6f * (MaxScore / TotalCombo);
                    StartCoroutine(UpdateDisplay(0.6f * (MaxScore / TotalCombo / 30), 30));
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
            Score.text = $"{(int)DisplayScore:00000000}";

            remain--;
            StartCoroutine(UpdateDisplay(delta, remain));
        }
    }
}