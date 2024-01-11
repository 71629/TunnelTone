using Newtonsoft.Json;
using TunnelTone.Core;
using TunnelTone.Events;
using TunnelTone.UI.PlayResult;
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
        [SerializeField]
        internal Chart[] charts = { null, null, null, null };

        private Score scoreData;
        private static SongData currentlyPlaying;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeOnLoad()
        {
            SystemEvent.OnChartLoad.AddListener(o =>
            {
                currentlyPlaying = (SongData)o[0];
            });
            SystemEvent.OnChartLoadFinish.AddListener(o =>
            {
                ResultScreen.playResult.title = currentlyPlaying.songTitle;
                ResultScreen.playResult.artist = currentlyPlaying.artist;
                ResultScreen.playResult.jacket = currentlyPlaying.jacket;
            });
        }

        private void OnEnable()
        {
            RefreshScoreData();
            SongListEvent.OnEnterSongList.AddListener(delegate
            {
                RefreshScoreData();
            });
        }
        
        private void RefreshScoreData()
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

namespace TunnelTone.Core
{
    public partial struct PlayResult
    {
        public string title; // Assigned
        public string artist; // Assigned
        public Sprite jacket;
    }
}