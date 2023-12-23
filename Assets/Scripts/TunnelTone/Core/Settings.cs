using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace TunnelTone.Core
{
    public class Settings : Statistic
    {
        internal static Settings instance;
        
        // local settings
        public float universalOffset;
        public float chartSpeed;
        public bool isHitSoundEnabled;
        public bool isVibrationEnabled;
        
        protected internal static void InitializeOnLoad()
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/player");
            instance = LoadSettings();
        }
        
        internal static Settings LoadSettings()
        {
            var path = $"{Application.persistentDataPath}/player/settings.json";
            
            try
            {
                // ! Problematic line: Possible stuck / infinite loop
                var data = File.ReadAllBytes(path);
                return JsonConvert.DeserializeObject<Settings>(Encoding.UTF8.GetString(data));
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load settings from {path}. Error: {ex.Message}");
                
                Debug.LogWarning("Setting not found, restoring to default.");
                instance = new Settings
                {
                    universalOffset = 0f,
                    chartSpeed = 1f,
                    isHitSoundEnabled = true,
                    isVibrationEnabled = false
                };
                return instance;
            }
        }

        internal static Settings SaveSettings()
        {
            var path = $"{Application.persistentDataPath}/player/settings.json";
            try
            {
                File.WriteAllBytes(path, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(instance)));
                return LoadSettings();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save settings to {path}. Error: {ex.Message}");
                return null;
            }
        }
    }
}