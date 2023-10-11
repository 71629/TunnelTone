using TunnelTone.Singleton;

namespace TunnelTone.Events
{
    public class NoteEventReference : Singleton<NoteEventReference>
    {
        public readonly NoteEvent OnSongBegin = new NoteEvent();
        
        public readonly NoteEvent OnNoteHit = new NoteEvent();
        public readonly NoteEvent OnNoteMiss = new NoteEvent();
    }
}