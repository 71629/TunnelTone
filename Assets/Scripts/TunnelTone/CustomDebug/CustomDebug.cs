using TMPro;
using TunnelTone.Elements;
using UnityEngine;

namespace TunnelTone.CustomDebug
{
    public class CustomDebug : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeDisplay;
        
        private void Update()
        {
            timeDisplay.text =
                $"Time Display\nStart: {NoteRenderer.dspSongStartTime}\nEnd: {NoteRenderer.dspSongEndTime}\nCurrent/Elapsed: {1000 * (AudioSettings.dspTime - NoteRenderer.dspSongStartTime)}\n";
        }
    }
}