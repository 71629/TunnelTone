namespace TunnelTone.Events
{
    public class GameEvent
    {
        public delegate void GameEventHandler(params object[] param);
        public event GameEventHandler m_onGameEvent;

        public void AddListener(GameEventHandler func) => m_onGameEvent += func;
        public void RemoveListener(GameEventHandler func) => m_onGameEvent -= func;
        
        public void Trigger(params object[] param) => m_onGameEvent?.Invoke(param);
    }
}