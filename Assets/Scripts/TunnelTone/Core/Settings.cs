using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TunnelTone.Events;
using TunnelTone.GameSystem;
using TunnelTone.Gauge;
using UnityEditor;
using UnityEngine;
using static TunnelTone.Core.DSPBufferSize;
using static TunnelTone.Core.FrameRate;
using static TunnelTone.Core.GaugeMode;

namespace TunnelTone.Core
{
    public class Settings : Statistic
    {
        internal static Settings instance;
        
        // local settings (most likely device specific / device preferences)
        public float universalOffset;
        public float chartSpeed;
        public bool isHitSoundEnabled;
        public float vibrationIntensity;
        public FrameRate targetFrameRate;
        public DSPBufferSize dspBufferSize;
        public float musicVolume;
        public float effectVolume;
        
        // cloud settings (most likely user preferences / user specific)
        public GaugeMode gaugeMode;
        
        // Hidden settings (Changes automatically and cannot be manually edited)
        public string recentPlay;

        internal static Gauge.Gauge Gauge
        {
            get
            {
              return instance.gaugeMode switch
              {
                  Completion => new CompletionGauge(),
                  Integrity => new IntegrityGauge(),
                  OneShot => throw new NotImplementedException(),
                  FourShot => throw new NotImplementedException(),
                  _ => throw new ArgumentOutOfRangeException()
              };
            }
        }

        protected internal static void InitializeOnLoad()
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/player");
            instance = LoadSettings();
            ApplySettings();
            
            Application.quitting += SaveOnQuit;
        }

        private static void SaveOnQuit()
        {
            SaveSettings();
        }
        
        internal static Settings LoadSettings()
        {
            var path = $"{Application.persistentDataPath}/player/settings.json";

            try
            {
                var data = File.ReadAllBytes(path);
                instance = JsonConvert.DeserializeObject<Settings>(Encoding.UTF8.GetString(data));

                if (instance.targetFrameRate is not (FPS60 or FPS75 or FPS90 or FPS120 or FPS144 or FPSInfinity))
                {
                    Debug.LogError($@"<b>Invalid Statistic for Settings.targetFrameRate</b>
                                                 Expected: FPS60, FPS75, FPS90, FPS120, FPS144, FPSInfinity
                                                 Actual: {instance.targetFrameRate}
                                                 Resetting to default: FPS60");
                    instance.targetFrameRate = FPS60;
                }
                if (instance.universalOffset is > 2000f or < -2000f)
                {
                    Debug.LogError($@"<b>Invalid Statistic for Settings.universalOffset</b>
                                                 Expected: -2000f < universalOffset < 2000f
                                                 Actual: {instance.universalOffset}
                                                 Resetting to default: 0");
                    instance.universalOffset = 0f;
                }
                if (instance.chartSpeed is > 6.5f or < 0f)
                {
                    Debug.LogError($@"<b>Invalid Statistic for Settings.chartSpeed</b>
                                                 Expected: 0.1f < chartSpeed < 10f
                                                 Actual: {instance.chartSpeed}
                                                 Resetting to default: 1");
                    instance.chartSpeed = 1f;
                }
                if (instance.vibrationIntensity is > 10f or < 0f)
                {
                    Debug.LogError($@"<b>Invalid Statistic for Settings.vibrationIntensity</b>
                                                 Expected: 0f < vibrationIntensity < 10f
                                                 Actual: {instance.vibrationIntensity}
                                                 Resetting to default: 5");
                    instance.vibrationIntensity = 5f;
                }
                if (instance.dspBufferSize is not (DSP128 or DSP256 or DSP512 or DSP1024 or DSP1024 or DSP2048 or DSP4096))
                {
                    Debug.LogError($@"<b>Invalid Statistic for Settings.dspBufferSize</b>
                                                 Expected: DSPBufferSize128, DSPBufferSize256, DSPBufferSize512, DSPBufferSize1024, DSPBufferSize2048, DSPBufferSize4096
                                                 Actual: {instance.dspBufferSize}
                                                 Resetting to default: DSPBufferSize128");
                    instance.dspBufferSize = DSP128;
                }
                if (instance.gaugeMode is not (Completion or Integrity or OneShot or FourShot))
                {
                    Debug.LogError($@"<b>Invalid Statistic for Settings.gaugeMode</b>
                                                 Expected: GaugeMode.Completion, GaugeMode.Integrity, GaugeMode.OneShot, GaugeMode.FourShot
                                                 Actual: {instance.gaugeMode}
                                                 Resetting to default: GaugeMode.Completion");
                    instance.gaugeMode = Completion;
                }
                if (instance.musicVolume is > 1f or < 0f)
                {
                    Debug.LogError($@"<b>Invalid Statistic for Settings.musicVolume</b>
                                                 Expected: 0f < musicVolume < 1f
                                                 Actual: {instance.musicVolume}
                                                 Resetting to default: 1");
                    instance.musicVolume = 1f;
                }
                if (instance.effectVolume is > 1f or < 0f)
                {
                    Debug.LogError($@"<b>Invalid Statistic for Settings.effectVolume</b>
                                                 Expected: 0f < effectVolume < 1f
                                                 Actual: {instance.effectVolume}
                                                 Resetting to default: 1");
                    instance.effectVolume = 1f;
                }

                return instance;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Setting not found, restoring to default.");
                return instance = new Settings
                {
                    universalOffset = 0f,
                    chartSpeed = 1f,
                    isHitSoundEnabled = true,
                    vibrationIntensity = 5f,
                    targetFrameRate = (FrameRate)60,
                    dspBufferSize = (DSPBufferSize)256,
                    gaugeMode = 0,
                    musicVolume = 1f,
                    effectVolume = 1f
                };
            }
        }

        internal static Settings SaveSettings()
        {
            var path = $"{Application.persistentDataPath}/player/settings.json";
            try
            {
                File.WriteAllBytes(path, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(instance, Formatting.Indented)));
                return LoadSettings();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save settings to {path}. Error: {ex.Message}");
                return null;
            }
        }
        
        internal static void ApplySettings(Settings settings)
        {
            instance = settings;
            ApplySettings();
            SystemEvent.OnSettingsChanged.Trigger();
        }

        internal static void ApplySettings()
        {
            // Save settings to device
            SaveSettings();
            LoadSettings();
            
            // Apply settings to game
            if(Application.targetFrameRate != (int)instance.targetFrameRate) Application.targetFrameRate = (int)instance.targetFrameRate;
            if (AudioSettings.GetConfiguration().dspBufferSize != (int)instance.dspBufferSize)
            {
                var config = AudioSettings.GetConfiguration(); 
                config.dspBufferSize = (int)instance.dspBufferSize;
                AudioSettings.Reset(config);
                SystemEvent.OnAudioSystemReset.Trigger(config);
            }
            GaugeManager.gauge = instance.gaugeMode switch
            {
                Completion => new CompletionGauge(),
                Integrity => new IntegrityGauge(),
                OneShot => throw new NotImplementedException(),
                FourShot => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        // [MenuItem("TunnelTone/Storage/Clear Local Settings")]
        private static void ClearLocalSettings()
        {
            if (Application.isPlaying)
            { 
                Debug.LogWarning("Unable to perform this operation while the game is running.");
                return;
            }
            File.Delete($"{Application.persistentDataPath}/player/settings.json");
        }
    }

    public enum GaugeMode
    {
        Completion = 0,
        Integrity = 1,
        OneShot = 2,
        FourShot = 3
    }
}