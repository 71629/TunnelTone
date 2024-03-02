using Newtonsoft.Json;
using TunnelTone.Core;
using TunnelTone.Events;
using TunnelTone.UI.PlayResult;
using TunnelTone.UI.SongList;
using UnityEditor;
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
        private static SongData selected;

        private AnimationCurve defaultTimingData => new()
        {
            keys = new[]
            {
                new Keyframe(0, 0),
                new Keyframe(music.length * 1000, music.length * 1000)
            }
        };
        
        internal TextAsset this[int index]
        {
            get
            {
                if (charts[index] is null)
                    Debug.LogWarning(
                        $"The chart of difficultyData {index} is null.\nThe chart will not render anything.");
                return charts[index].chart;
            }
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeOnLoad()
        {
            SongListItem.SelectItem += SongListItemOnSelectCallback;
            SystemEvent.OnChartLoadFinish.AddListener(o =>
            {
                ResultScreen.playResult.title = selected.songTitle;
                ResultScreen.playResult.artist = selected.artist;
                ResultScreen.playResult.jacket = selected.jacket;
            });
        }

        private static void SongListItemOnSelectCallback(SongData songData)
        {
            selected = songData;
        }

#if UNITY_EDITOR
        [ContextMenu("Generate timing data")]
        public void GenerateTimingData()
        {
            foreach (var chart in charts)
            {
                if(chart is null) continue;
                if (chart.timingSheet.length > 1) continue;
                chart.timingSheet = defaultTimingData;
                AnimationUtility.SetKeyLeftTangentMode(chart.timingSheet, 0, AnimationUtility.TangentMode.Linear);
                AnimationUtility.SetKeyRightTangentMode(chart.timingSheet, 0, AnimationUtility.TangentMode.Linear);
                AnimationUtility.SetKeyLeftTangentMode(chart.timingSheet, 1, AnimationUtility.TangentMode.Linear);
                AnimationUtility.SetKeyRightTangentMode(chart.timingSheet, 1, AnimationUtility.TangentMode.Linear);
            }
        }
#endif

        private void OnEnable()
        {
            RefreshScoreData();
            SongListManager.EnterSongList += RefreshScoreData;
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