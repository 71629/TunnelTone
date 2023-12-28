using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace TunnelTone.Core
{
    internal static class NetworkManager
    {
        private const string ApiAddress = "https://hashtag071629.com/";
        
        internal static string GameVersion => Application.version;
        internal static string FirstLevelVersion => GameVersion.Split('.')[0];
        internal static string SecondLevelVersion => GameVersion.Split('.')[1];
        internal static string ThirdLevelVersion => GameVersion.Split('.')[2];
        internal static bool isOnline;

        internal static int uid;
        internal static string username;

        // Obsolete soon
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void AutoLogin()
        {
            WWWForm form = new();
            form.AddField("deviceIDPost", SystemInfo.deviceUniqueIdentifier);
            form.AddField("applicationVersionPost", "TunnelTone");

            using var req = UnityWebRequest.Post($"{ApiAddress}autologin", form);
            
            req.SendWebRequest();
            
            if (req.downloadHandler.text.Contains("LOGIN_SUCCESS"))
            {
                // output format LOGIN_SUCCESS_{uid:int}_{username:string}
                var data = req.downloadHandler.text.Remove(0, "LOGIN_SUCCESS_".Length);
                username = data.Split('_')[1];
                uid = int.Parse(data.Split('_')[0]);
            }
        }

        private static void AutoLoginJson()
        {
            var req = new UnityWebRequest();
            req.url = $"{ApiAddress}autologin";
            req.SetRequestHeader("Content-Type", "application/json");
            req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
            {
                application = "TunnelTone",
                version = GameVersion,
                deviceID = SystemInfo.deviceUniqueIdentifier
            })));
            req.downloadHandler = new DownloadHandlerBuffer();

            Task.Run(() =>
            {
                var response = SendRequest<AutoLoginResponse>(req);
                if (!response.isSuccessful) return;
                uid = response.uid;
                username = response.username;
            });
        }

        private static T SendRequest<T>(UnityWebRequest req) where T : ServerResponse
        {
            req.SendWebRequest();

            return JsonConvert.DeserializeObject<T>(System.Text.Encoding.UTF8.GetString(req.downloadHandler.data));
        }
    }
}