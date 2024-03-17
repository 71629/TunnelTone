using TMPro;
using TunnelTone.Charts;
using TunnelTone.Core;
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
        
        private SongData currentSong;
        
        private static readonly int LoadSong = Animator.StringToHash("LoadSong");
        private static readonly int FadeInfo = Animator.StringToHash("FadeInfo");

        private void Start()
        {
            jacket.enabled = false;
            
            SongListItem.SelectItem += UpdateInfo;
            SongListManager.SongStart += ShowInfo;
            JsonScanner.ChartLoadFinish += FadeSongInfo;
        }

        private void UpdateInfo(SongData songData)
        {
            currentSong = songData;
        }

        private void ShowInfo(ref MusicPlayDescription mpd)
        {
            title.text = currentSong.songTitle;
            artist.text = currentSong.artist;
            jacket.sprite = currentSong.jacket;
            charter.text = currentSong.charts[SongListDifficultyManager.Instance.CurrentlySelected].chartDesigner;
            
            DisplaySongInfo();
        }
        
        private void DisplaySongInfo() => animator.SetTrigger(LoadSong);
        private void FadeSongInfo() => animator.SetTrigger(FadeInfo);
    }
}