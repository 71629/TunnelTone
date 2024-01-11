using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TunnelTone.UI.Entry;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace TunnelTone.Core
{
    internal static class NetworkManager
    {
        internal static UnityEvent OnUserLogin = new();
        internal static UnityEvent OnUserLogout = new();
        
        internal static UnityEvent<int> OnSyncError = new();

        internal const string ApiAddress = "https://hashtag071629.com/tunneltone";
        
        internal static string GameVersion => Application.version;
        internal static NetworkStatus status;

        private static string apiKey;
        internal static int uid;
        internal static string username;
        
        internal static async void AutoLoginJson()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable) return;
            
            AutoLogin rawData = new()
            {
                index = "/autologin"
            };
            
            // var req = new UnityWebRequest($"http://localhost:80/dev/php/autoLogin.php", "POST");
            // var req = new UnityWebRequest($"{ApiAddress}/autologin", "POST");

            // var response = SendUnityRequest(rawData, "/autologin");

            var response = new AutoLogin();
            response = await SendRequest(rawData, "/autologin", () =>
            {
                Debug.Log($"{response.exitCode}: {response.message}");
                GameEntry.OnInitializeComplete.Invoke();
            });

            if (response.exitCode != 0)
            {
                status = NetworkStatus.SyncError;
                OnSyncError.Invoke(response.exitCode);
                return;
            }
            uid = response.uid;
            username = response.username;
            status = NetworkStatus.Online;
        }c
        
        private static async Task<T> SendHttpRequest<T>(T obj, string index) where T : TunnelTonePackage
        {
            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, $"{ApiAddress}{index}");
            req.Content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj)));
            
            var response = await client.SendAsync(req);
            var result = await response.Content.ReadAsByteArrayAsync();

            return JsonConvert.DeserializeObject<T>(System.Text.Encoding.UTF8.GetString(result));
        }

        internal static async Task<T> SendRequest<T>(T obj, string index, Action onCompleteCallback = null) where T : TunnelTonePackage
        {
            var ret = await SendHttpRequest(obj, obj.index);
            onCompleteCallback?.Invoke();
            return ret;
        }
    }

    internal enum NetworkStatus
    {
        Offline,
        Online,
        SyncError
    }
}