using System;
using System.Collections;
using TMPro;
using TunnelTone.Charts;
using TunnelTone.Core;
using TunnelTone.Elements;
using TunnelTone.Events;
using TunnelTone.GameSystem;
using TunnelTone.UI.Reference;
using TunnelTone.UI.SongList;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TunnelTone.UI.PlayResult
{
    public class ResultScreen : MonoBehaviour
    {
        public static readonly GameEvent OnPlayResultCreated = new();
        
        [Header("This Object")]
        [SerializeField] internal Canvas canvas;
        
        [Header("Song Info")]
        [SerializeField] internal TextMeshProUGUI info;

        [Header("UI Elements")] 
        [SerializeField] internal Image jacket;
        [Space(10)]
        [SerializeField] internal Image texture;
        [Space(10)] 
        [SerializeField] internal Slider gauge;
        [SerializeField] internal TextMeshProUGUI gaugeModeText;
        [Space(10)]
        [SerializeField] internal Transform scoreTransform;

        [Space(10)]
        [SerializeField] internal Button retry;
        [SerializeField] internal Button back;

        [Space(10)]
        [SerializeField] internal Image difficulty;
        [SerializeField] internal TextMeshProUGUI difficultyText;
        [SerializeField] internal Image difficultyShadow;
        [SerializeField] internal TextMeshProUGUI clearType;

        [Header("Statistics")] 
        [SerializeField] internal TextMeshProUGUI score;
        [SerializeField] internal TextMeshProUGUI bestScore;
        [SerializeField] internal TextMeshProUGUI delta;
        [Space(10)] 
        [SerializeField] internal Image perfectDisplay;
        [SerializeField] internal TextMeshProUGUI perfectText;
        [SerializeField] internal TextMeshProUGUI perfectCount;
        [Space(10)]
        [SerializeField] internal Image greatDisplay;
        [SerializeField] internal TextMeshProUGUI greatText;
        [SerializeField] internal TextMeshProUGUI greatCount;
        [Space(10)]
        [SerializeField] internal Image missDisplay;
        [SerializeField] internal TextMeshProUGUI missText;
        [SerializeField] internal TextMeshProUGUI missCount;
        [Space(10)]
        [SerializeField] internal Image maxComboDisplay;
        [SerializeField] internal TextMeshProUGUI maxComboText;
        [SerializeField] internal TextMeshProUGUI maxComboCount;

        [Header("Prefabs")] 
        // ReSharper disable InconsistentNaming
        [SerializeField] internal Object rankEX;
        [SerializeField] internal Object rankAAA;
        // ReSharper restore InconsistentNaming
        [SerializeField] internal Object rankAA;
        [SerializeField] internal Object rankA;
        [SerializeField] internal Object rankB;
        [SerializeField] internal Object rankC;
        [SerializeField] internal Object rankD;

        internal static Core.PlayResult playResult;

        private void Start()
        {
            SystemEvent.OnDisplayResult.AddListener(delegate
            {
                DisplayResult();
            });
            SystemEvent.OnChartLoad.AddListener(o =>
            {
                playResult = new Core.PlayResult();
                OnPlayResultCreated.Trigger(o);
            });
        }
        
        private void DisplayResult()
        {
            canvas.enabled = true;
            
            // Prepare UI
            info.text = $"{playResult.title}\n<size=42>{playResult.artist}";
            jacket.sprite = playResult.jacket;
            bestScore.text = $"{Mathf.CeilToInt(playResult.bestScore):D8}";
            var gradeObject = playResult.score switch
            {
                >= 9800000 => Instantiate(rankEX, scoreTransform) as GameObject,
                >= 9650000 => Instantiate(rankAAA, scoreTransform) as GameObject,
                >= 9500000 => Instantiate(rankAA, scoreTransform) as GameObject,
                >= 9200000 => Instantiate(rankA, scoreTransform) as GameObject,
                >= 8600000 => Instantiate(rankB, scoreTransform) as GameObject,
                >= 7800000 => Instantiate(rankC, scoreTransform) as GameObject,
                _ => Instantiate(rankD, scoreTransform) as GameObject
            };
            difficultyText.text = Dictionaries.Instance.levelDictionary[playResult.level];
            var color = playResult.difficulty switch
            {
                0 => UIElementReference.Instance.easy,
                1 => UIElementReference.Instance.hard,
                2 => UIElementReference.Instance.intensive,
                3 => UIElementReference.Instance.insane,
                _ => throw new ArgumentOutOfRangeException()
            };
            difficulty.color = color;
            difficultyShadow.color = color;
            clearType.text = playResult.clearType switch
            {
                ClearType.EasyClear => "Track Complete",
                ClearType.NormalClear => "Track Complete",
                ClearType.HardClear => "Integrity Passed",
                ClearType.FullCombo => "Full Combo",
                ClearType.FlawlessPlus => "Flawless+",
                ClearType.FlawlessPlusPlus => "Flawless+ +",
                ClearType.Failed => "Track Failed",
                _ => throw new ArgumentOutOfRangeException()
            };
            texture.color = playResult.clearType == ClearType.Failed ? new Color(0.5f, 0f, 0f) : new Color(1f, 1f, 1f, 0.2f);
            gaugeModeText.text = playResult.gaugeMode switch
            {
                GaugeMode.Completion => "Completion",
                GaugeMode.Integrity => "Integrity",
                GaugeMode.FourShot => "Four Shot",
                GaugeMode.OneShot => "One Shot",
                _ => throw new ArgumentOutOfRangeException()
            };
            gauge.minValue = playResult.gaugeMin;
            gauge.maxValue = playResult.gaugeMax;
            gauge.value = playResult.gaugeRemain;
            
            // Animations
            LeanTween.value(perfectCount.gameObject, f => { perfectCount.text = $"{Mathf.RoundToInt(f)}"; }, 0, playResult.perfect, .5f)
                .setEase(LeanTweenType.easeOutCubic)
                .setOnStart(() =>
                {
                    if (playResult.perfect != 0) return;
                    LeanTween.value(perfectDisplay.gameObject, c => { perfectDisplay.color = c; }, perfectDisplay.color, perfectDisplay.color * new Color(1, 1, 1, .5f), .5f)
                        .setEase(LeanTweenType.easeOutCubic);
                    LeanTween.value(perfectText.gameObject, c => { perfectText.color = c; }, perfectText.color, perfectText.color * new Color(1, 1, 1, .5f), .5f)
                        .setEase(LeanTweenType.easeOutCubic);
                });
            LeanTween.value(greatCount.gameObject, f => { greatCount.text = $"{Mathf.RoundToInt(f)}"; }, 0, playResult.great, .5f)
                .setEase(LeanTweenType.easeOutCubic)
                .setOnStart(() =>
                {
                    if (playResult.great != 0) return;
                    LeanTween.value(greatDisplay.gameObject, c => { greatDisplay.color = c; }, greatDisplay.color, greatDisplay.color * new Color(1, 1, 1, .5f), .5f)
                        .setEase(LeanTweenType.easeOutCubic);
                    LeanTween.value(greatText.gameObject, c => { greatText.color = c; }, greatText.color, greatText.color * new Color(1, 1, 1, .5f), .5f)
                        .setEase(LeanTweenType.easeOutCubic);
                });
            LeanTween.value(missCount.gameObject, f => { missCount.text = $"{Mathf.RoundToInt(f)}"; }, 0, playResult.miss, .5f)
                .setEase(LeanTweenType.easeOutCubic)
                .setOnStart(() =>
                {
                    if (playResult.miss != 0) return;
                    LeanTween.value(missDisplay.gameObject, c => { missDisplay.color = c; }, missDisplay.color, missDisplay.color * new Color(1, 1, 1, .5f), .5f)
                        .setEase(LeanTweenType.easeOutCubic);
                    LeanTween.value(missText.gameObject, c => { missText.color = c; }, missText.color, missText.color * new Color(1, 1, 1, .5f), .5f)
                        .setEase(LeanTweenType.easeOutCubic);
                });
            LeanTween.value(maxComboCount.gameObject, f => { maxComboCount.text = $"{Mathf.RoundToInt(f)}"; }, 0, playResult.maxCombo, .5f)
                .setEase(LeanTweenType.easeOutCubic)
                .setOnStart(() =>
                {
                    if (playResult.maxCombo != 0) return;
                    LeanTween.value(maxComboDisplay.gameObject, c => { maxComboDisplay.color = c; }, maxComboDisplay.color, maxComboDisplay.color * new Color(1, 1, 1, .5f), .5f)
                        .setEase(LeanTweenType.easeOutCubic);
                    LeanTween.value(maxComboText.gameObject, c => { maxComboText.color = c; }, maxComboText.color, maxComboText.color * new Color(1, 1, 1, .5f), .5f)
                        .setEase(LeanTweenType.easeOutCubic);
                });
            
            LeanTween.value(score.gameObject, f =>
                {
                    score.text = $"{Mathf.CeilToInt(f):D8}";
                    if (f > playResult.bestScore)
                        delta.text = $"+{Mathf.CeilToInt(f - playResult.bestScore):D8}";
                    else
                        delta.text = $"{Mathf.CeilToInt(f - playResult.bestScore):D8}";
                }, 0, playResult.score, 1.5f)
                .setEase(LeanTweenType.easeOutCubic)
                .setOnComplete(() =>
                {
                    gradeObject!.GetComponent<Grade>().Display();
                    score.text = $"{playResult.score:D8}";
                    delta.text = $"{playResult.score - playResult.bestScore:D8}";
                    
                    var backRect = back.GetComponent<RectTransform>();
                    var retryRect = retry.GetComponent<RectTransform>();
                    LeanTween.value(back.gameObject, v => { backRect.anchoredPosition = v; }, new Vector2(-125, 50), new Vector2(125, 50), .7f);
                    LeanTween.value(retry.gameObject, v => { retryRect.anchoredPosition = v; }, new Vector2(125, 50), new Vector2(-125, 50), .7f);
                });
            StartCoroutine(UploadScore());
        }

        private static IEnumerator UploadScore()
        {
            yield return NetworkManager.SendRequest(new Package
            {
                songTitle = playResult.title,
                difficulty = playResult.difficulty,
                score = playResult.score,
                index = "/uploadscore"
            }, "/uploadscore");
        }

        public void ToSongList()
        {
            Shutter.Instance.ToSongList(() => {
                canvas.enabled = false;
            });
        }

        public void Retry()
        {
            Shutter.Instance.CloseShutter(() =>
            {
                canvas.enabled = false;
                UIElementReference.Instance.musicPlay.enabled = true;
                NoteRenderer.Instance.Retry();
            });
        }
        
        public class Package : TunnelTonePackage
        {
            public string songTitle;
            public int difficulty;
            public int score;
        }
    }

    public enum ClearType
    {
        EasyClear = 0,
        NormalClear = 1,
        HardClear = 2,
        FullCombo = 3,
        FlawlessPlus = 4,
        FlawlessPlusPlus = 5,
        Failed = 6
    }
}