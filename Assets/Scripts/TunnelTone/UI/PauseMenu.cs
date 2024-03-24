using UnityEngine;

namespace TunnelTone.UI
{
    public class PauseMenu : MonoBehaviour
    {
        private static PauseMenu instance;

        [SerializeField] private GameObject pauseMenuInterface;
        
        public delegate void PauseHandler();
        public static event PauseHandler GamePause;
        public static event PauseHandler GameResume;
        public static event PauseHandler GameRetry;
        public static event PauseHandler GameQuit;
        
        private void Awake()
        {
            instance = this;
        }

        public static void Pause()
        {
            instance.pauseMenuInterface.SetActive(true);
            GamePause?.Invoke();
        }

        public void Resume()
        {
            GameResume?.Invoke();
            pauseMenuInterface.SetActive(false);
        }
        
        public void Retry()
        {
            GameRetry?.Invoke();
        }

        public void Quit()
        {
            GameQuit?.Invoke();
        }
    }
}