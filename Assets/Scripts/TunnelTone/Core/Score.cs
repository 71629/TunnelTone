using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace TunnelTone.Core
{
    [System.Serializable]
    public class Score : Statistic
    {
        internal string title;
        public int[] score;

        public Score(string title, int[] data = null)
        {
            this.title = title;
            score = data ?? new[] { 0, 0, 0, 0 };
        }
        
        protected internal static void InitializeOnLoad()
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/data");
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
        
        [MenuItem("TunnelTone/Storage/Clear Local Settings")]
        private static void ClearLocalSettings()
        {
            File.Delete($"{Application.persistentDataPath}/player/settings.json");
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
}