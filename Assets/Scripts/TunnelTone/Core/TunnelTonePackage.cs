using UnityEngine;

namespace TunnelTone.Core
{
    public class TunnelTonePackage
    {
        public string identifier = SystemInfo.deviceUniqueIdentifier;
        public int uid = NetworkManager.uid;
        public string version = NetworkManager.GameVersion;
        public int exitCode;
        public string message;
        internal string index;
    }
}