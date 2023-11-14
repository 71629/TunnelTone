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
            SystemEventReference.Instance.OnChartLoad.AddListener(delegate
            {
                title.text = SongListManager.Instance.CurrentlySelected.title;
                artist.text = SongListManager.Instance.CurrentlySelected.artist;
                jacket.sprite = UIElementReference.Instance.songJacket.sprite;

                // TODO: Implement charter
                charter.text = "Charter: 71629";
                
                DisplaySongInfo();
            });
            SystemEventReference.Instance.OnChartLoadFinish.AddListener(delegate
            {
                FadeSongInfo();
            });
        }
        private void DisplaySongInfo() => animator.SetTrigger(LoadSong);
        private void FadeSongInfo() => animator.SetTrigger(FadeInfo);
    }
}