using System;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
using TunnelTone.Core;
using TunnelTone.UI.SongList;
using UnityEngine;

namespace TunnelTone.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SongData", menuName = "Song data", order = 0)]
    public class SongData : ScriptableObject
    {
        [Header("Song information")]
        // Numeric
        [SerializeField] internal string songTitle;
        [SerializeField] internal string artist;
        [SerializeField] internal float bpm;
        // Media
        [Space(10)]
        [SerializeField] internal Sprite jacket;
        [SerializeField] internal AudioClip music;
        // Loop options
        [Space(10)]
        [SerializeField] internal float previewStart;
        [SerializeField] internal float previewDuration;
        
        [Space(10)]
        [SerializeField] private Chart[] charts = { null, null, null, null };

        private Score scoreData;

        private void OnEnable()
        {
            scoreData = Score.LoadLocalScore(songTitle);
        }

        internal int[] GetDifficulties() => new[] { charts[0].difficulty, charts[1].difficulty, charts[2].difficulty, charts[3].difficulty };
        
        internal int GetScore(int index)
        {
            return scoreData.score[index];
        }
        
        internal int GetDifficulty(int index)
        {
            return charts[index].difficulty;
        }
        
        internal TextAsset GetChart(int index)
        {
            if (charts[index] is null) Debug.LogWarning($"The chart of difficultyData {index} is null.\nThe chart will not render anything.");
            return charts[index].chart;
        }
        
        internal int SetScore(int difficulty, int score)
        {
            scoreData.score[difficulty] = score;
            return Score.SaveLocal(scoreData).score[difficulty];
        }
        
        internal string SerializeDataPackage()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        
        internal SongData DeserializeDataPackage(string data)
        {
            return JsonConvert.DeserializeObject<SongData>(data);
        }
    }
}