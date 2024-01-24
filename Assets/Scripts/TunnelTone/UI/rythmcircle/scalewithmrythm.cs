using UnityEngine;
using System.Collections;
using TunnelTone.GameSystem;


namespace TunnelTone.UI.rythmcircle
{
    [RequireComponent(typeof(AudioSource))]
    public class scalewithmrythm : MonoBehaviour
    {
        GameObject _circle;
        public AudioSource _source;
        public AudioSource audioClip;
        public float[] _testbar;
        //public float scalesize;
        
        // Start is called before the first frame update
        void Start()
        {
            _testbar = new float[64];
            _circle = this.gameObject;
        }
    
        // Update is called once per frame
        void Update()
        {
            //_source = AudioManager.Instance.audioSource;
            _source.clip = audioClip.clip;
            Debug.Log(_source);
            GetAudioSpectrumData();
            for (int i = 0; i < _testbar.Length; i++)
            {
                Debug.Log(_testbar[i]);
            }
        }

        void GetAudioSpectrumData()
        {
            _source.GetSpectrumData(_testbar, 0 ,FFTWindow.Blackman);
            Debug.Log(_source);
        }
    }
}

