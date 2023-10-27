using System.Collections;
using TMPro;
using TunnelTone.Events;
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
            SongListEventReference.Instance.OnSelectItem.AddListener(UpdateInfo);
        }

        private void UpdateInfo(params object[] param)
        {
            var songListItem = (SongListItem)param[0];

            StartCoroutine(IEUpdateInfo(songListItem));
        }

        IEnumerator IEUpdateInfo(SongListItem songListItem)
        {
            yield return null;
            
            title.text = songListItem.source.title;
            artist.text = songListItem.source.artist;
            bpm.text = $"{songListItem.source.bpm:BPM: 0}";
        }
    }
}