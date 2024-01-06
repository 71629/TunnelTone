using TMPro;
using TunnelTone.Events;
using TunnelTone.Singleton;
using TunnelTone.UI.Reference;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI.SongList
{
    public class ShutterInfo : Singleton<ShutterInfo>
    {
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI title, artist, charter;
        [SerializeField] private Image jacket;
        
        private static readonly int LoadSong = Animator.StringToHash("LoadSong");
        private static readonly int FadeInfo = Animator.StringToHash("FadeInfo");

        private void Start()
        {
            SystemEvent.OnChartLoad.AddListener(delegate
            {
                title.text = SongListManager.currentlySelected.songTitle;
                artist.text = SongListManager.currentlySelected.artist;
                jacket.sprite = UIElementReference.Instance.songJacket.sprite;

                // TODO: Implement charter
                charter.text = "Charter: 71629";
                
                DisplaySongInfo();
            });
            SystemEvent.OnChartLoadFinish.AddListener(delegate
            {
                FadeSongInfo();
            });
        }
        private void DisplaySongInfo() => animator.SetTrigger(LoadSong);
        private void FadeSongInfo() => animator.SetTrigger(FadeInfo);
    }
}