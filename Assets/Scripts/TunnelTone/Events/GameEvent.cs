using System;

namespace TunnelTone.Events
{
    public class GameEvent
    {
        public delegate void GameEventHandler(params object[] param);
        public event GameEventHandler m_onGameEvent;
        
        public Delegate[] InvocationList => m_onGameEvent?.GetInvocationList();

        public void AddListener(GameEventHandler func) => m_onGameEvent += func;
        public void RemoveListener(GameEventHandler func) => m_onGameEvent -= func;

        public void RemoveAllListeners(object instance)
        {
            // Remove all listeners for instance
            if (InvocationList == null) return;
            foreach (var invocation in InvocationList)
            {
                if (invocation.Target == instance)
                {
                    m_onGameEvent -= (GameEventHandler) invocation;
                }
            }
        }

        public void RemoveAllListeners<T>()
        {
            // Remove all listeners for type T
            if (InvocationList == null) return;
            foreach (var invocation in InvocationList)
            {
                if (invocation.Target is T)
                {
                    m_onGameEvent -= (GameEventHandler) invocation;
                }
            }
        }

        public void RemoveAllListeners()
        {
            // Remove all listeners
            if (InvocationList == null) return;
            foreach (var invocation in InvocationList)
            {
                m_onGameEvent -= (GameEventHandler) invocation;
            }
        }
        
        public void Trigger(params object[] param) => m_onGameEvent?.Invoke(param);
    }
}