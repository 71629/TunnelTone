using TunnelTone.Elements;
using TunnelTone.Enumerators;
using TunnelTone.Events;
using TunnelTone.GameSystem;
using TunnelTone.ScriptableObjects;
using TunnelTone.UI.Reference;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.PlayArea
{
    public class ProgressBarManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        private int TotalSample => audioSource.clip.samples;
        private Slider Slider => GetComponent<Slider>();

        private void Start()
        {
            SystemEvent.OnChartLoad.AddListener(SetProgressBarColor);
        }

        private void SetProgressBarColor(object[] param)
        {
            var songData = (SongData)param[0];
            var difficulty = (int)param[1];
            
            Slider.fillRect.GetComponent<Image>().color = difficulty switch
            {
                0 => UIElementReference.Instance.easy,
                1 => UIElementReference.Instance.hard,
                2 => UIElementReference.Instance.intensive,
                3 => UIElementReference.Instance.insane,
                _ => Color.magenta
            };
        }

        private void Update()
        {
            if(NoteRenderer.isPlaying)
                Slider.value = audioSource.timeSamples / (float)TotalSample;
        }
    }
}