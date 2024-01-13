using TunnelTone.Events;
using TunnelTone.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.PlayArea
{
    public class PlayAreaDecoration : MonoBehaviour
    {
        [SerializeField] private Image jacket;

        private void Start()
        {
            SystemEvent.OnChartLoad.AddListener(o =>
            {
                var songData = (SongData)o[0];
                
                if (songData.jacket is not null)
                {
                    jacket.sprite = songData.jacket;
                }
            });
        }
    }
}