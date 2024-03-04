using TunnelTone.Core;
using TunnelTone.UI.SongList;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.PlayArea
{
    public class PlayAreaDecoration : MonoBehaviour
    {
        [SerializeField] private Image jacket;

        private void Start()
        {
            SongListManager.SongStart += SetBackground;
        }

        private void SetBackground(ref MusicPlayDescription mpd)
        {
            // Throw IncompleteMusicPlayDescriptionException
            if (mpd.jacket is null) return;
            jacket.sprite = mpd.jacket;
        }
    }
}