using TunnelTone.ScriptableObjects;
using TunnelTone.UI.PlayResult;
using UnityEngine;

namespace TunnelTone.Core
{
    public class MusicPlayDescription
    {
        public Sprite jacket;
        
        public MusicPlayMode playMode;
        public int difficulty;
        public Chart chart;
        public AudioClip music;
        public PlayResult playResult;
    }
}

namespace TunnelTone.Core
{
    public partial struct PlayResult
    {
        
    }
}