using TunnelTone.Singleton;
using UnityEngine;

namespace TunnelTone.GameSystem
{
    public class SongManager : Singleton<SongManager>
    {
        public string songTitle;
        public float bpm;
        public int offset;
    }
}