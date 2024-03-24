using TunnelTone.Core;
using TunnelTone.Elements;
using TunnelTone.UI.Reference;
using TunnelTone.UI.SongList;
using TunnelTone.UI.Transition;
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
            Transitioner.Instance.QuitMusicPlay(Callback);
            return;
            
            void Callback()
            {
                NoteRenderer.ResetContainer();
                AudioListener.pause = false;
                UIElementReference.Instance.musicPlay.enabled = false;
                instance.pauseMenuInterface.SetActive(false);
                MusicPlayDescription.instance.module.Quit();
            }
        }
    }
}