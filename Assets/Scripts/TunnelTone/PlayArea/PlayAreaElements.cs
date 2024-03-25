using TunnelTone.Core;
using TunnelTone.UI;
using UnityEngine;

namespace TunnelTone.PlayArea
{
    public class PlayAreaElements : MonoBehaviour, IOnPauseHandler
    {
        public float time;

        private void OnEnable()
        {
            PauseMenu.GamePause += OnPause;
            PauseMenu.GameResume += OnResume;
            PauseMenu.GameRetry += OnRetry;
            PauseMenu.GameQuit += OnQuit;
        }
        
        private void OnDisable()
        {
            PauseMenu.GamePause -= OnPause;
            PauseMenu.GameResume -= OnResume;
            PauseMenu.GameRetry -= OnRetry;
            PauseMenu.GameQuit -= OnQuit;
        }

        public virtual void OnPause(){}
        public virtual void OnResume(){}
        public virtual void OnRetry(){}
        public virtual void OnQuit(){}
    }
}