using UnityEngine;

namespace TunnelTone.Core
{
    public class Statistic
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void SystemInitializeOnLoad()
        {
            Settings.InitializeOnLoad();
            Score.InitializeOnLoad();
        }
    }
}