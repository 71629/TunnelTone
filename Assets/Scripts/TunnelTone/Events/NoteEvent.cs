namespace TunnelTone.Events
{
    public class NoteEvent
    {
        public delegate void NoteEventHandler(params object[] param);
        public event NoteEventHandler m_onNoteEvent;

        public void AddListener(NoteEventHandler func) => m_onNoteEvent += func;
        public void RemoveListener(NoteEventHandler func) => m_onNoteEvent -= func;
        
        public void Trigger(params object[] param) => m_onNoteEvent?.Invoke(param);
    }
}