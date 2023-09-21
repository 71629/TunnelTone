using TunnelTone.Singleton;

namespace TunnelTone.Events
{
    public class NoteEventReference : Singleton<NoteEventReference>
    {
        public NoteEvent OnNormalPosition = new NoteEvent();
        
        public NoteEvent OnTapHit = new NoteEvent();
    }
}