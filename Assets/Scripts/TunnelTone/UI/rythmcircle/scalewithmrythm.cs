using UnityEngine;
using System.Collections;
using TunnelTone.GameSystem;


namespace TunnelTone.UI.rythmcircle
{
    [RequireComponent(typeof(AudioSource))]
    public class scalewithmrythm : MonoBehaviour
    {
        GameObject _circle;
        public AudioSource audioClip;
        public float[] _testbar;
        public LineRenderer line;
        private int count = 32;
        
        // Start is called before the first frame update
        void Start()
        {
            _testbar = new float[64];
            _circle = this.gameObject;
            line.positionCount = count;
            line.startWidth = 0.02f;
            line.endWidth = 0.02f;
        }
    
        // Update is called once per frame
        void Update()
        {
            //_source = AudioManager.Instance.audioSource;
            //_source.clip = audioClip.clip;
            GetAudioSpectrumData();
            for (int i = 0; i < count; i++)
            {
                var v = _testbar[i];
                line.SetPosition(i, new Vector3((i - (count / 2) * 0.2f), v * 0.2f, -5));
            }
        }

        void GetAudioSpectrumData()
        {
            audioClip.GetSpectrumData(_testbar, 0 ,FFTWindow.Blackman);
        }
    }
}

