using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI
{
    public class ProgressBarManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        private int TotalSample => audioSource.clip.samples;
        private Slider Slider => GetComponent<Slider>();

        private void Update()
        {
            Slider.value = audioSource.timeSamples / (float)TotalSample;
        }
    }
}