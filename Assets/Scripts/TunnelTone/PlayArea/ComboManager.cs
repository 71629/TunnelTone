using TMPro;
using TunnelTone.Events;
using UnityEngine;

namespace TunnelTone.PlayArea
{
    public class ComboManager : MonoBehaviour
    {
        private TextMeshProUGUI Combo => GetComponent<TextMeshProUGUI>();

        private static int CurrentCombo { get; set; }

        private void Start()
        {
            ChartEventReference.Instance.OnNoteHit.AddListener(UpdateCombo);
            ChartEventReference.Instance.OnNoteMiss.AddListener(delegate
            {
                CurrentCombo = 0;
                Combo.text = $"<size=20>combo</size>\n{CurrentCombo}";
            });
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
        }
    }
}