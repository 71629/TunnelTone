using System.Collections.Generic;
using TMPro;
using TunnelTone.Events;
using UnityEngine;

namespace TunnelTone.UI.Dialog
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private List<Object> dialogOptions;

        private void Start()
        {
            SystemEventReference.Instance.OnDisplayDialog.AddListener(DisplayDialog);
        }

        private void DisplayDialog(params object[] param)
        {
            title.text = (string)param[0];
            message.text = (string)param[1];
            animator.enabled = true;
        }
    }
}