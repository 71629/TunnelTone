using TunnelTone.ScriptableObjects;
using TunnelTone.Singleton;

namespace TunnelTone.GameSystem
{
    public class SongManager : Singleton<SongManager>
    {
        internal static SongData nowPlaying;
    }
}