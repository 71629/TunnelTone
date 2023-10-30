using TunnelTone.Enumerators;
using TunnelTone.GameSystem;
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
            if(GameStatusReference.Instance.GameStatus is GameStatus.MusicPlay)
                Slider.value = audioSource.timeSamples / (float)TotalSample;
        }
    }
}