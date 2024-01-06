using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using TunnelTone.Events;
using TunnelTone.ScriptableObjects;
using TunnelTone.UI.PlayResult;
using UnityEditor;
using UnityEngine;

namespace TunnelTone.Core
{
    [System.Serializable]
    public class Score : Statistic
    {
        public string title;
        public int[] score;

        public Score(string title, int[] data = null)
        {
            this.title = title;
            score = data ?? new[] { 0, 0, 0, 0 };
        }
        
        protected internal static void InitializeOnLoad()
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/data");
            ResultScreen.OnPlayResultCreated.AddListener(SetResultScreenBestScore);
            SystemEvent.OnDisplayResult.AddListener(delegate
            {
                // TODO: Import statistics to result screen
            });
        }

        internal static void SetResultScreenBestScore(params object[] param)
        {
            var songData = (SongData)param[0];
            var difficulty = (int)param[1];
                
            var bestScore = LoadLocalScore(songData.songTitle).score[difficulty];
                
            ResultScreen.playResult.bestScore = bestScore;
        }
        
        internal static Score SaveLocal(Score data)
        {
            var path = $"{Application.persistentDataPath}/data/{data.title}";
            Directory.CreateDirectory(path);
            File.WriteAllBytes($"{path}/score.json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
            return LoadLocalScore(data.title);
        }
        
        internal static Score LoadLocalScore(string query)
        {
            var path = $"{Application.persistentDataPath}/data/{query}/score.json";

            if (!File.Exists(path))
            {
                SaveLocal(new Score(query));
            }
            
            return JsonConvert.DeserializeObject<Score>(Encoding.UTF8.GetString(File.ReadAllBytes(path)));
        }

        internal void UploadScore()
        {
            
        }
        
        [MenuItem("TunnelTone/Storage/Clear Local Score")]
        private static void ClearLocalScore()
        {
            foreach (var directory in Directory.GetDirectories($"{Application.persistentDataPath}/data"))
            {
                File.Delete($"{directory}/score.json");
            }
        }
    }
    
    [StructLayout(LayoutKind.Auto)]
    public partial struct PlayResult
    {
        public int bestScore;  // Assigned
    }
}