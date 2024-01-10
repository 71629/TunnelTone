using TMPro;
using TunnelTone.Core;
using TunnelTone.UI.SongList;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TunnelTone.UI.Entry
{
    public class GameEntry : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        
        [SerializeField] private Button start;
        [SerializeField] private TextMeshProUGUI version;
        [SerializeField] private TextMeshProUGUI status;
        
        [TextArea(5, 10)][SerializeField] private string bootText;
        [TextArea(5, 10)][SerializeField] private string loadingText;
        
        internal static UnityEvent OnInitializeComplete = new();

        private void Start()
        {
            version.text = $"version {Application.version}";
            status.text = "Logging in...";
            OnInitializeComplete.AddListener(delegate
            {
                status.text = "Touch to start";
                start.interactable = true;
                OnInitializeComplete.RemoveAllListeners();
            });
            NetworkManager.AutoLoginJson();
        }
        
        public void StartGame()
        {
            start.interactable = false;

            Shutter.Instance.ToSongList(() => canvas.enabled = false);
        }
     }
}