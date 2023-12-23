using System.IO;
using UnityEngine;

namespace TunnelTone.Core
{
    internal static class ErrorHandler
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeOnLoad()
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/log/dump");
        }
    }
}