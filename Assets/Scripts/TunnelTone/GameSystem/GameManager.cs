using TunnelTone.Enumerators;
using TunnelTone.Events;
using UnityEngine;

namespace TunnelTone.GameSystem
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 120;
        }

        private void Start()
        {
            SystemEventReference.Instance.OnChartLoadFinish.AddListener(delegate
            {
                GameStatusReference.Instance.GameStatus = GameStatus.MusicPlay;
            });
        }
    }
}