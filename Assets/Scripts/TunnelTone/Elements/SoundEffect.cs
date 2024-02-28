using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace TunnelTone.Elements
{
    public class SoundEffect : MonoBehaviour
    {
        private AudioSource soundEffect => GetComponent<AudioSource>();
        public Tap tap;
        private float offset;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            offset = tap.Hit();
            PlaySoundEffect(offset);
        }
        
        void PlaySoundEffect(float hitOffset)
        {
            if(hitOffset >= 120) return;
            else soundEffect.Play();
        }
    }
}

