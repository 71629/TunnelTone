﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using TunnelTone.Core;
using TunnelTone.UI.SongList;

namespace TunnelTone.UI.Entry
{
    public class GameEntry : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        [SerializeField] private Button start;
        [SerializeField] private TextMeshProUGUI version;
        [SerializeField] private TextMeshProUGUI status;

        [TextArea(5, 10)] [SerializeField] private string bootText;
        [TextArea(5, 10)] [SerializeField] private string loadingText;

        [SerializeField] private RectTransform notice;
        [SerializeField] private TextMeshProUGUI noticeText;

        internal static UnityEvent OnInitializeComplete = new();

        private string[] address = { "www.google.com", "www.hashtag071629.com", "one.one.one.one", "4.2.2.2", "208.67.222.222", "www.opendns.com" };
        //4.2.2.2 = Verizon Public DNS
        private IEnumerator PingCheck(System.Action<bool> onResult)
        {
            bool result = false;

            foreach (var ipAddress in address) //check each address is pingable
            {
                var request = UnityWebRequest.Get(ipAddress);
                request.timeout = 5;
                yield return request.SendWebRequest();
                //Debug.Log(ipAddress); //Check now address
                if (!request.isNetworkError && !request.isHttpError) //If no error then go online else offline
                {
                    result = true;
                    //Debug.Log(result); //Check status (true)
                    break;
                }
                //Debug.Log(result); //Check status (false)
            }
            onResult?.Invoke(result);
        }
        private IEnumerator Start()
        {
            version.text = $"version {Application.version}";
            status.text = "Logging in...";

            bool online = false;
            yield return PingCheck(result => online = result);

            OnInitializeComplete.AddListener(delegate
            {
                status.text = "Touch to start";
                start.interactable = true;
                OnInitializeComplete.RemoveAllListeners();
            });
            if (!online)
            {
                DisplayNotice($"Offline Mode");
                status.text = "Touch to start";
                start.interactable = true;
                OnInitializeComplete.RemoveAllListeners();
            }
            else
            {
                NetworkManager.OnStatusChanged.AddListener(delegate
                {
                    DisplayNotice($"Logged in as {NetworkManager.username}");
                });
            }
            NetworkManager.AutoLoginJson();
            LeanTween.value(status.gameObject, f => { status.color = new(1, 1, 1, f); }, 1, .35f, .9f)
    .setLoopPingPong(); //Tap to start pingpong display
        }
        public void StartGame()
        {
            start.interactable = false;

            Shutter.Instance.ToMainMenu(() => canvas.enabled = false);
        }
        private void DisplayNotice(string text)
        {
            noticeText.text = text;
            var anchoredPosition = notice.anchoredPosition;
            LeanTween.value(notice.gameObject, v =>
            {
                notice.anchoredPosition = v;
            }, anchoredPosition, anchoredPosition - new Vector2(noticeText.preferredWidth + 100, 0), .5f)
                .setEase(LeanTweenType.easeOutCubic)
                .setOnComplete(() =>
                {
                    LeanTween.value(notice.gameObject, v =>
                    {
                        notice.anchoredPosition = v;
                    }, notice.anchoredPosition, anchoredPosition, .5f)
                        .setEase(LeanTweenType.easeInCubic)
                        .setDelay(2.5f);
                });
        }
    }
}