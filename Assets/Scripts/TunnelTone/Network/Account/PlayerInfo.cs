using TMPro;
using TunnelTone.Core;
using UnityEngine;
using UnityEngine.Events;

namespace TunnelTone.Network.Account
{
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI username;
        [SerializeField] private TextMeshProUGUI totalScore;
        [SerializeField] private TextMeshProUGUI recentPlay;
        [SerializeField] private TextMeshProUGUI playerRating;

        internal static readonly UnityEvent OnDisplayPlayerInfo = new();

        private void Start()
        {
            OnDisplayPlayerInfo.AddListener(SetData);
        }

        private void SetData()
        {
            username.text = NetworkManager.username;
            totalScore.text = $"<size=24>Total score: </size>\n{Score.GetTotalScore()}";
            recentPlay.text = $"<size=24>Recent play</size>\n{Settings.instance.recentPlay}";
        }
    }
}