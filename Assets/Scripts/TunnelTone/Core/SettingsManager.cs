using TMPro;
using TunnelTone.Events;
using UnityEngine;
using UnityEngine.UI;
using static TunnelTone.Core.Category;
using static TunnelTone.Core.DSPBufferSize;
using static TunnelTone.Core.FrameRate;

namespace TunnelTone.Core
{
    public class SettingsManager : MonoBehaviour
    {
        private Settings currentSettings;
        [SerializeField] private Canvas settings;

        [Header("UI")] 
        [SerializeField] private Scrollbar categoryScrollbar;
        
        [Header("Settings Button")]
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button revert;
        [SerializeField] private Button apply;
        
        [Header("Categories")] 
        [SerializeField] private TextMeshProUGUI gameplay;
        [SerializeField] private TextMeshProUGUI video;
        [SerializeField] private TextMeshProUGUI audio;
        [SerializeField] private TextMeshProUGUI other;
        
        [Header("Setting Tiles")]
        [SerializeField] private GameObject gameplayTile;
        [SerializeField] private GameObject videoTile;
        [SerializeField] private GameObject audioTile;
        [SerializeField] private GameObject otherTile;
        private Category currentCategory;

        [Header("Gameplay Settings")] 
        [SerializeField] private TextMeshProUGUI noteSpeedText;
        [SerializeField] private Button increaseSpeed;
        [SerializeField] private Button decreaseSpeed;
        [Space(10)] 
        [SerializeField] private TextMeshProUGUI vibrationIntensityText;
        [SerializeField] private Button increaseVibration;
        [SerializeField] private Button decreaseVibration;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI topDisplayModeText;
        [SerializeField] private Button nextTopDisplayMode;
        [SerializeField] private Button previousTopDisplayMode;
        [Header("Background Effect (Mask) setting")]
        [SerializeField] private Button disableDBGE;
        [SerializeField] private Button enableDBGE;
        [SerializeField] private TextMeshProUGUI disableDBGEText;
        [SerializeField] private TextMeshProUGUI enableDBGEText;
        [SerializeField] private GameObject BGMask;
        [SerializeField] private Image bgLine;
        private bool applys;
        [SerializeField] private Image[] tapImage;
        [SerializeField] private GameObject tapPref;
        [SerializeField] private Image DisplayImage;
        [SerializeField] private Button nextTapButton;
        [SerializeField] private Button backTapButton;

        [Header("Display Settings")] 
        [SerializeField] private TextMeshProUGUI fps60;
        [SerializeField] private TextMeshProUGUI fps75;
        [SerializeField] private TextMeshProUGUI fps90;
        [SerializeField] private TextMeshProUGUI fps120;
        [SerializeField] private TextMeshProUGUI fps144;
        [SerializeField] private TextMeshProUGUI fpsInfinity;
        
        [Header("Audio Settings")]
        [SerializeField] private TextMeshProUGUI calibrationText;
        [SerializeField] private Button increaseCalibration;
        [SerializeField] private Button decreaseCalibration;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI musicVolumeText;
        [SerializeField] private Button increaseMusicVolume;
        [SerializeField] private Button decreaseMusicVolume;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI soundEffectVolumeText;
        [SerializeField] private Button increaseSoundEffectVolume;
        [SerializeField] private Button decreaseSoundEffectVolume;
        [Space(10)] 
        [SerializeField] private TextMeshProUGUI dsp128;
        [SerializeField] private TextMeshProUGUI dsp256;
        [SerializeField] private TextMeshProUGUI dsp512;
        [SerializeField] private TextMeshProUGUI dsp1024;
        [SerializeField] private TextMeshProUGUI dsp2048;
        [SerializeField] private TextMeshProUGUI dsp4096;

        private void Start()
        {
            SystemEvent.OnEnterSettings.AddListener(delegate { FetchSettings(); });
            currentSettings = Settings.instance;
        }
        public void tapImageChange(bool click)
        {
            int i = 0;
            for (; i < tapImage.Length; i++)
            {
                if(tapImage[i].sprite == DisplayImage.sprite)
                {
                    break;
                }
            }
            if (click)
            {
                if(i<tapImage.Length-1)
                {
                    DisplayImage.sprite = tapImage[i+1].sprite;
                }
                else
                {
                    DisplayImage.sprite = tapImage[0].sprite;
                }
            }
            else
            {
                if (i  > 0)
                {
                    DisplayImage.sprite = tapImage[i - 1].sprite;
                }
                else
                {
                    DisplayImage.sprite = tapImage[tapImage.Length-1].sprite;
                }
            }
            Debug.Log(i);
            
        }
        private void FetchSettings()
        {
            noteSpeedText.text = $"{currentSettings.chartSpeed:0.0}";
            vibrationIntensityText.text = $"{currentSettings.vibrationIntensity:0.0}";
            topDisplayModeText.text = $"{(TopDisplayMode)currentSettings.gaugeMode}";
            calibrationText.text = $"{currentSettings.universalOffset:0.0}";
            musicVolumeText.text = $"{currentSettings.musicVolume:0.0}";
            soundEffectVolumeText.text = $"{currentSettings.effectVolume:0.0}";
            ResetDspTextColor();
            ResetFpsTextColor();
        }

        private void ResetDspTextColor()
        {
            Color gray = new(1, 1, 1, .5f);
            dsp128.color = currentSettings.dspBufferSize == DSP128 ? Color.white : gray;
            dsp256.color = currentSettings.dspBufferSize == DSP256 ? Color.white : gray;
            dsp512.color = currentSettings.dspBufferSize == DSP512 ? Color.white : gray;
            dsp1024.color = currentSettings.dspBufferSize == DSP1024 ? Color.white : gray;
            dsp2048.color = currentSettings.dspBufferSize == DSP2048 ? Color.white : gray;
            dsp4096.color = currentSettings.dspBufferSize == DSP4096 ? Color.white : gray;
        }
        
        private void ResetFpsTextColor()
        {
            Color gray = new(1, 1, 1, .5f);
            fps60.color = currentSettings.targetFrameRate == FPS60 ? Color.white : gray;
            fps75.color = currentSettings.targetFrameRate == FPS75 ? Color.white : gray;
            fps90.color = currentSettings.targetFrameRate == FPS90 ? Color.white : gray;
            fps120.color = currentSettings.targetFrameRate == FPS120 ? Color.white : gray;
            fps144.color = currentSettings.targetFrameRate == FPS144 ? Color.white : gray;
            fpsInfinity.color = currentSettings.targetFrameRate == FPSInfinity ? Color.white : gray;
        }
        
        private void ResetBGMask()
        {
            Color gray = new(1, 1, 1, .5f);
            fps60.color = currentSettings.targetFrameRate == FPS60 ? Color.white : gray;
            fps75.color = currentSettings.targetFrameRate == FPS75 ? Color.white : gray;
            fps90.color = currentSettings.targetFrameRate == FPS90 ? Color.white : gray;
            fps120.color = currentSettings.targetFrameRate == FPS120 ? Color.white : gray;
            fps144.color = currentSettings.targetFrameRate == FPS144 ? Color.white : gray;
            fpsInfinity.color = currentSettings.targetFrameRate == FPSInfinity ? Color.white : gray;
        }
        // Gameplay
        public void AdjustNoteSpeed(float value)
        {
            currentSettings.chartSpeed = Mathf.Clamp(currentSettings.chartSpeed + value, 1f, 6.5f);
            noteSpeedText.text = $"{currentSettings.chartSpeed:0.0}";
        }
        
        public void AdjustVibrationIntensity(float value)
        {
            currentSettings.vibrationIntensity = Mathf.Clamp(currentSettings.vibrationIntensity + value, 0f, 10f);
            vibrationIntensityText.text = $"{currentSettings.vibrationIntensity:0.0}";
        }
        
        public void AdjustTopDisplayMode(int value)
        {
            currentSettings.gaugeMode += value;
            topDisplayModeText.text = $"{(TopDisplayMode)currentSettings.gaugeMode}";
        }
        
        // Video
        public void AdjustFrameRate(int frameRate)
        {
            currentSettings.targetFrameRate = (FrameRate)frameRate;
            ResetFpsTextColor();
        }
        
        // Audio
        public void AdjustCalibration(int value)
        {
            currentSettings.universalOffset = Mathf.Clamp(currentSettings.universalOffset + value, -2000f, 2000f);
            calibrationText.text = $"{currentSettings.universalOffset:0}";
        }
        
        public void AdjustMusicVolume(float value)
        {
            currentSettings.musicVolume = Mathf.Clamp(currentSettings.musicVolume + value, 0f, 10f);
            musicVolumeText.text = $"{currentSettings.musicVolume:0.0}";
        }
        
        public void AdjustSoundEffectVolume(float value)
        {
            currentSettings.effectVolume = Mathf.Clamp(currentSettings.effectVolume + value, 0f, 10f);
            soundEffectVolumeText.text = $"{currentSettings.effectVolume:0.0}";
        }
        
        public void AdjustDspBufferSize(int value)
        {
            currentSettings.dspBufferSize = (DSPBufferSize)value;
            ResetDspTextColor();
        }
        
        // Other

        public void ApplySettings()
        {
            Settings.ApplySettings(currentSettings);
            BGMaskSetting(applys);
            tapPref.GetComponent<Image>().sprite = DisplayImage.sprite;
        }

        public void RevertSettings()
        {
            currentSettings = Settings.LoadSettings();
        }
        
        public void BGMaskSetting(bool enable)
        {
            BGMask.SetActive(disableDBGEText.color == enableDBGEText.color ? true : enable);
            bgLine.color = disableDBGEText.color == enableDBGEText.color || enable == true ? new(1, 1, 1, .2f) : new(1, 1, 1, .7f);
        }
        public void BGMaskSettingButton(bool click)
        {
            applys = click;
            disableDBGEText.color = click ? new(1, 1, 1, .5f) : Color.white;
            enableDBGEText.color = click ? Color.white : new(1, 1, 1, .5f);
        }

        // UI
        public void SwitchCategory(int value)
        {
            if (value == (int)currentCategory) return;
            currentCategory = (Category)value;
            
            gameplayTile.SetActive(value == (int)Gameplay);
            videoTile.SetActive(value == (int)Video);
            audioTile.SetActive(value == (int)Audio);
            otherTile.SetActive(value == (int)Other);
            
            LeanTween.cancel(categoryScrollbar.gameObject);
            LeanTween.value(categoryScrollbar.gameObject, f => { categoryScrollbar.value = f; },
                    categoryScrollbar.value, value / 3f, .45f)
                .setEase(LeanTweenType.easeOutCubic);
        }
    }
    
    // ReSharper disable InconsistentNaming
    internal enum Category
    {
        Gameplay = 0,
        Video = 1,
        Audio = 2,
        Other = 3
    }

    internal enum TopDisplayMode
    {
        Combo = 0,
        EXPlusThreshold = 1,
        EXThreshold = 2,
        Gauge = 3,
    }

    public enum DSPBufferSize
    {
        DSP128 = 128,
        DSP256 = 256,
        DSP512 = 512,
        DSP1024 = 1024,
        DSP2048 = 2048,
        DSP4096 = 4096
    }
    
    public enum FrameRate
    {
        FPS60 = 60,
        FPS75 = 75,
        FPS90 = 90,
        FPS120 = 120,
        FPS144 = 144,
        FPSInfinity = int.MaxValue
    }
    // ReSharper restore InconsistentNaming
}