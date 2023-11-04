using TMPro;
using TunnelTone.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI.Reference
{
    public class UIElementReference : Singleton<UIElementReference>
    {
        [Header("System")]
        public AudioSource audioSource;
        public Animator audioAnimator;
        public Animator shutterAnimator;
        public Animator shutterInfoAnimator;

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
        public Slider startSlider;

        [Header("Canvas")] 
        public Canvas songList;
        public Canvas musicPlay;
        public Canvas shutter;
        public Canvas shutterInfo;
        public Canvas dialog;
        public Canvas mainMenu;

        public void ToSongSelect()
        {
            
        }
        
        public void ToMusicPlay()
        {
            
        }

        public void ToMainMenu()
        {
            
        }
    }
}