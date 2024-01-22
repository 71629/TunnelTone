using TMPro;
using TunnelTone.Core;
using TunnelTone.UI.SongList;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TunnelTone.UI.Entry
{
    public class GameEntry : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        
        [SerializeField] private Button start;
        [SerializeField] private TextMeshProUGUI version;
        [SerializeField] private TextMeshProUGUI status;
        
        [TextArea(5, 10)][SerializeField] private string bootText;
        [TextArea(5, 10)][SerializeField] private string loadingText;

        [SerializeField] private RectTransform notice;
        [SerializeField] private TextMeshProUGUI noticeText;

        [SerializeField] private Color color;
        [SerializeField] private float time;
        [SerializeField] private bool  upDown;
        [SerializeField] float scaleTime;

        internal static UnityEvent OnInitializeComplete = new();

        private void Start()
        {
            time = 0;
            status.color = Color.white;
            upDown = false;
            scaleTime = 2; //Speed of change color
            version.text = $"version {Application.version}";
            status.text = "Logging in...";
            OnInitializeComplete.AddListener(delegate
            {
                status.text = "— Touch to start —";
                start.interactable = true;
                OnInitializeComplete.RemoveAllListeners();
            });
            NetworkManager.OnStatusChanged.AddListener(delegate
            {
                if (NetworkManager.status is NetworkStatus.Online)
                {
                    DisplayNotice($"Logged in as {NetworkManager.username}");
                }
            });
            NetworkManager.AutoLoginJson();
            LeanTween.value(status.gameObject, f => { status.color = new(1, 1, 1, f); }, 1, .35f, .9f)
                .setLoopPingPong();
        }

        //private void Update()
        //{
        //    if (!status.text.Equals("Initializing..."))
        //    {
        //        color.r = Mathf.PingPong(Time.time / scaleTime, 1) +0.3f;
        //        color.g = Mathf.PingPong(Time.time / scaleTime, 1) + 0.3f;
        //        color.b = Mathf.PingPong(Time.time / scaleTime, 1) + 0.3f;
        //        color.a = Mathf.PingPong(Time.time / scaleTime, 1) + 0.3f;
        //    }
        //}


        public void StartGame()
        {
            start.interactable = false;

            Shutter.Instance.ToSongList(() => canvas.enabled = false);
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