using TMPro;
using TunnelTone.Events;
using UnityEngine;

namespace TunnelTone.UI
{
    public class ComboManager : MonoBehaviour
    {
        private TextMeshProUGUI Combo => GetComponent<TextMeshProUGUI>();

        private static int CurrentCombo { get; set; }

        private void Start()
        {
            NoteEventReference.Instance.OnNoteHit.AddListener(UpdateCombo);
        }
        
        public void UpdateCombo(params object[] param)
        {
            #region Convert parameters

            var offset = (float)param[0];

            #endregion

            // Update ComboCount
            CurrentCombo += offset switch
            {
                <= 100 => 1,
                _ => -CurrentCombo
            };
            
            // Update Combo text
            Combo.text = $"<size=20>combo</size>\n{CurrentCombo}";
        }
    }
}