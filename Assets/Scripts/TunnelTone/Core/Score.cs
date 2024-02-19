using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using TunnelTone.ScriptableObjects;
using TunnelTone.UI.PlayResult;
using TunnelTone.UI.SongList;
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

        internal Score SaveLocal() => SaveLocal(this);
        
        internal static Score LoadLocalScore(string query)
        {
            var path = $"{Application.persistentDataPath}/data/{query}/score.json";

            if (!File.Exists(path))
            {
                SaveLocal(new Score(query));
            }
            
            var ret = JsonConvert.DeserializeObject<Score>(Encoding.UTF8.GetString(File.ReadAllBytes(path)));
            ret.title = query;

            return ret;
        }

        internal static int GetTotalScore()
        {
            // Read all score.json in all subdirectories of data directory

            return SongListManager.Instance.songContainer.SelectMany(song => LoadLocalScore(song.songTitle).score).Sum();
        }
        
        // [MenuItem("TunnelTone/Storage/Clear Local Score")]
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