using TMPro;
using TunnelTone.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI.Reference
{
    public class UIElementReference : Singleton<UIElementReference>
    {
        [Header("System")]
        public Camera mainCamera;
        public AudioSource audioSource;
        public Animator audioAnimator;
        public Animator shutterAnimator;
        public Animator shutterInfoAnimator;

        [Header("ColorReference")] 
        public Color easy;
        public Color hard;
        public Color intensive;
        public Color insane;
        public Color left;
        public Color right;
        
        [Header("Song Select")]
        public Image songJacket;
        public TextMeshProUGUI title;
        public TextMeshProUGUI artist;
        public TextMeshProUGUI bpm;

        [Header("Canvas")] 
        public Canvas songList;
        public Canvas musicPlay;
        public Canvas shutter;
        public Canvas shutterInfo;
        public Canvas dialog;
        public Canvas mainMenu;
        public Canvas topView;

        [Header("UI Elements")] 
        public GameObject pause;

        [Header("Material")] 
        public Material leftTrail;
        public Material leftTrailHit;
        public Material rightTrail;
        public Material rightTrailHit;

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