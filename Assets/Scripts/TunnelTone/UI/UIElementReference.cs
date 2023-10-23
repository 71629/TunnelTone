using System.Collections.Generic;
using TMPro;
using TunnelTone.Singleton;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TunnelTone.UI
{
    public class UIElementReference : Singleton<UIElementReference>
    {
        [Header("System")]
        public AudioSource audioSource;
        public Animator audioAnimator;

        [Header("ColorReference")] 
        public Color easy;
        public Color hard;
        public Color intensive;
        public Color insane;
        
        [Header("Song Select")]
        public Image songJacket;
        public TextMeshProUGUI title;
        public TextMeshProUGUI artist;
        public TextMeshProUGUI bpm;
    }
}