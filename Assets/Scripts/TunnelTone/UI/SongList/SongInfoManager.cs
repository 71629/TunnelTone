using System.Collections;
using TMPro;
using TunnelTone.Events;
using TunnelTone.ScriptableObjects;
using TunnelTone.UI.Reference;
using UnityEngine;

namespace TunnelTone.UI.SongList
{
    public class SongInfoManager : MonoBehaviour
    {
        private TextMeshProUGUI title => UIElementReference.Instance.title.GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI artist => UIElementReference.Instance.artist.GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI bpm => UIElementReference.Instance.bpm.GetComponent<TextMeshProUGUI>();
        
        private void Start()
        {
            SongListItem.SelectItem += UpdateInfo;
        }

        private void UpdateInfo(SongData songData)
        {
            title.text = songData.songTitle;
            artist.text = songData.artist;
            bpm.text = $"{songData.bpm:BPM: 0}";
        }
    }
}