using TunnelTone.ScriptableObjects;
using TunnelTone.UI.PlayResult;
using UnityEngine;

namespace TunnelTone.Core
{
    public class MusicPlayDescription
    {
        public MusicPlayMode playMode;
        public SongData songData;
        public Chart chart;
        public AudioClip music;
        public PlayResult playResult;
    }
}

namespace TunnelTone.Core
{
    public partial struct PlayResult
    {
        public PlayResult(SongData songData, Gauge.Gauge gauge, int difficulty)
        {
            title = songData.songTitle;
            artist = songData.artist;
            this.difficulty = difficulty;
            perfect = 0;
            great = 0;
            miss = 0;
            maxCombo = 0;
            score = 0;
            bestScore = songData.GetScore(difficulty);
            jacket = songData.jacket;
            level = songData.GetDifficulty(difficulty);

            gaugeMode = gauge.gaugeMode;
            gaugeMin = 0;
            gaugeRemain = 0;
            gaugeMax = 100;
            clearType = ClearType.EasyClear;
        }
    }
}