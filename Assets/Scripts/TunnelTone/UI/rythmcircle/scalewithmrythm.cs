using UnityEngine;
using System.Collections;
using TunnelTone.GameSystem;


namespace TunnelTone.UI.rythmcircle
{
    [RequireComponent(typeof(AudioSource))]
    public class scalewithmrythm : MonoBehaviour
    {
        GameObject _circle;
        //public AudioSource _source;
        public AudioSource audioClip;
        public float[] _testbar;
        //public float scalesize;
        
        // Start is called before the first frame update
        void Start()
        {
            _testbar = new float[64];
            _circle = this.gameObject;
            //_source = this.GetComponent<AudioSource>();
        }
    
        // Update is called once per frame
        void Update()
        {
            //_source = AudioManager.Instance.audioSource;
            //_source.clip = audioClip.clip;
            GetAudioSpectrumData();
        }

        void GetAudioSpectrumData()
        {
            audioClip.GetSpectrumData(_testbar, 0 ,FFTWindow.Blackman);
        }
    }
}

