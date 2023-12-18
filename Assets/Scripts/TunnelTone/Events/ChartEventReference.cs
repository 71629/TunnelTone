using TunnelTone.Singleton;

namespace TunnelTone.Events
{
    public class ChartEventReference : Singleton<ChartEventReference>
    {
        public readonly GameEvent OnSongLoaded = new GameEvent();
        
        public readonly GameEvent OnSongBegin = new GameEvent();
        public readonly GameEvent OnSongAbort = new GameEvent();
        public readonly GameEvent OnSongEnd = new GameEvent();

        public readonly GameEvent OnPause = new GameEvent();
        public readonly GameEvent OnResume = new GameEvent();
        public readonly GameEvent OnRetry = new GameEvent();
        public readonly GameEvent OnQuit = new GameEvent();
        
        public readonly GameEvent OnNoteHit = new GameEvent();
        public readonly GameEvent OnNoteMiss = new GameEvent();
    }
}