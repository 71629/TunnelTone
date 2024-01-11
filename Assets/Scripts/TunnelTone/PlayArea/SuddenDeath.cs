using TMPro;
using TunnelTone.Events;
using TunnelTone.Gauge;
using UnityEngine;

namespace TunnelTone.PlayArea
{
    public class SuddenDeath : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private RectTransform frame;
        [SerializeField] private RectTransform texture;
        [SerializeField] private TextMeshProUGUI suddenDeathText;

        private void Start()
        {
            IntegrityGauge.OnSuddenDeath.AddListener(OnSuddenDeath);
            SystemEvent.OnDisplayResult.AddListener(ResetUI);
        }

        private void OnSuddenDeath(object[] param)
        {
            frame.gameObject.SetActive(true);
            animator.enabled = true;
            animator.Rebind();
        }

        internal void ResetUI(object[] param)
        {
            frame.gameObject.SetActive(false);
        }
    }
}