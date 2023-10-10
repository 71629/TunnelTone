using TunnelTone.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI
{
    public class ProgressBarManager : MonoBehaviour
    {
        [SerializeField] AudioSource m_audioSource;
        private int TotalSample => m_audioSource.clip.samples;
        private Slider Slider => GetComponent<Slider>();

        private void Awake()
        {
            
        }

        private void Update()
        {
            Slider.value = m_audioSource.timeSamples / (float)TotalSample;
        }
    }
}