using UnityEngine;

namespace TunnelTone.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                Init();
            }
            else
            {
                Destroy(this);
            }
        }

        protected virtual void Init()
        {
        }
    }
}