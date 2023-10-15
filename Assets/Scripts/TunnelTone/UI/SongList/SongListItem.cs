using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TunnelTone.UI.SongList
{
    public class SongListItem : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI artist;
        public TextMeshProUGUI difficulty;

        public Image difficultyBackground;
        public Image songJacket;
    }
}