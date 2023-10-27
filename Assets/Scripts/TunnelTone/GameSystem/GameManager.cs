using UnityEngine;

namespace TunnelTone.GameSystem
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 120;
        }
    }
}