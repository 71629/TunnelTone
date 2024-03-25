using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI.rhythmcircle
{ 
    public class newSpectrum : MonoBehaviour
    {
        public AudioSource audioSource;
        public int samples; // 64 - 8192
        private Vector3 scale;
        [SerializeField] public int numOfObjects;
        public GameObject objectPrefab;
        public float scaleX;
        public float scaleY;
        private float[] spectrum;
        private float middlepoint;
        private Vector3 startPos;
        GameObject _object;
        [SerializeField] float spacing;
        
        public float[] spectrumInputData
        {
            get { return spectrum; }
        }


        // Start is called before the first frame update
        void Start()
        {
            spectrum = new float[samples];
            startPos = new Vector3(0, middlepoint, 0);
            middlepoint = this.transform.position.y;
            if (audioSource != null)
            {
               Createbar(); 
            }
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
    
        public void Createbar()
        {
            for(int i = 0; i < numOfObjects; i++)
            {
                spectrum = audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
                scale = new Vector3(spectrum[i] * scaleX, scaleY, 5);
                
                //scale = new Vector3(scaleX, scaleY, 1);
                _object = Instantiate(objectPrefab, transform) as GameObject;
                _object.transform.position = new Vector3(startPos.x, startPos.y + (scaleY * i + spacing * i), startPos.z);
                _object.transform.localScale = scale;
            }
        }
        
        public void spectrumData()
        {
            spectrum = audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
                for(int i = 0; i < numOfObjects; i++)
                {
                    _object.transform.position = new (startPos.x + spectrum[i], startPos.y + (scaleY * i), startPos.z);
                    
                }
        }
        
        /// this method is not used 
        /*private void createBars(Vector3 pos, Vector3 scale)
        {
            if (audioSource != null)
            {
                for(int i = 0; i < numOfObjects; i++)
                {
                    GameObject _object;
                    _object = Instantiate(objectPrefab, transform) as GameObject;
                    _object.transform.position = new Vector3(pos.x, pos.y + (scaleY * i), pos.z);
                    _object.transform.localScale = scale;
                    Destroy(_object);
                }
            }
        }*/
    }
}

