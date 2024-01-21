using TMPro;
using TunnelTone.Events;
using TunnelTone.ScriptableObjects;
using TunnelTone.Singleton;
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
            SystemEvent.OnChartLoad.AddListener(o =>
            {
                var songData = (SongData) o[0];
                var difficulty = (int) o[1];

                title.text = songData.songTitle;
                artist.text = songData.artist;
                jacket.sprite = songData.jacket;

                // TODO: Implement charter
                charter.text = songData.charts[difficulty].chartDesigner;
                
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