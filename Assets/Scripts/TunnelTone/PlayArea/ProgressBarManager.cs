using TunnelTone.Elements;
using TunnelTone.UI.Reference;
using TunnelTone.UI.SongList;
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
            SongListDifficultyManager.DifficultyChange += SetProgressBarColor;
        }

        private void SetProgressBarColor(int i)
        {
            Slider.fillRect.GetComponent<Image>().color = i switch
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
