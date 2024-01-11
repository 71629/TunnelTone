using TunnelTone.Singleton;

namespace TunnelTone.Events
{
    public class SongListEvent : Singleton<SongListEvent>
    {
        public static readonly GameEvent OnEnterSongList = new GameEvent();
        
        public static readonly GameEvent OnSelectItem = new GameEvent();
        public static readonly GameEvent OnDifficultyChange = new GameEvent();
        public static readonly GameEvent OnSongStart = new GameEvent();
        
        public static readonly GameEvent OnExitSongList = new GameEvent();
    }
}