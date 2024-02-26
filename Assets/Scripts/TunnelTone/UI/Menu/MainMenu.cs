using TunnelTone.UI.Reference;
using TunnelTone.UI.SongList;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
        private Canvas canvas;
        
        [SerializeField] private Image menuCharacter;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        public void ToSongSelect()
        {
            Shutter.Instance.ToSongList(() => canvas.enabled = false);
        }

        public void ToStoryMode()
        {
            Shutter.Instance.CloseShutter(() =>
            {
                UIElementReference.Instance.mainMenu.enabled = false;
                UIElementReference.Instance.storyMap.enabled = true;
                Shutter.Instance.OpenShutter();
            });
        }
    }
}