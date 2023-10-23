using TunnelTone.Singleton;

namespace TunnelTone.Events
{
    public class SongListEventReference : Singleton<SongListEventReference>
    {
        public readonly GameEvent OnEnterSongList = new GameEvent();
        
        public readonly GameEvent OnSelectItem = new GameEvent();
        public readonly GameEvent OnDifficultyChange = new GameEvent();
        public readonly GameEvent OnSongStart = new GameEvent();
        
        public readonly GameEvent OnExitSongList = new GameEvent();
    }
}