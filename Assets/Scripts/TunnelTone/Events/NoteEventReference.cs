using TunnelTone.Singleton;

namespace TunnelTone.Events
{
    public class NoteEventReference : Singleton<NoteEventReference>
    {
        public readonly GameEvent OnSongLoaded = new GameEvent();
        
        public readonly GameEvent OnSongBegin = new GameEvent();
        public readonly GameEvent OnSongAbort = new GameEvent();
        
        public readonly GameEvent OnNoteHit = new GameEvent();
        public readonly GameEvent OnNoteMiss = new GameEvent();
    }
}