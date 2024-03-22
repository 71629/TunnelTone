using System;
using System.Collections;
using System.Collections.Generic;
using TunnelTone.Charts;
using TunnelTone.Core;
using TunnelTone.UI.Entry;
using TunnelTone.UI.Menu;
using TunnelTone.UI.Reference;
using TunnelTone.UI.SongList;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace TunnelTone.UI.Transition
{
    public class Transitioner : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool isNightMode;
#endif
        
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image cover;
        
        [Header("Covers")]
        [SerializeField] private Sprite daytimeCover;
        [SerializeField] private Sprite nighttimeCover;

        [Header("Transition Variants")] 
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private Graphic[] toMainMenu;
        [SerializeField] private GameObject songList;
        [SerializeField] private Graphic[] toSongList;
        [SerializeField] private GameObject musicPlay;
        [SerializeField] private Graphic[] toMusicPlay;

        [Header("Music Play Extras")] 
        [SerializeField] private Image jacket;

        private float transitionTime;

        private IEnumerator coroutine;
        
        private delegate void TransitionBreakHandler(IEnumerator coroutine);
        private static event TransitionBreakHandler TransitionBreak;
        
        private void Start()
        {
            GameEntry.ToMainMenu += c => StartTransition(toMainMenu, mainMenu, true, c);
            
            MainMenu.ToSongList += c => StartTransition(toSongList, songList, false, c);
            SongListManager.EnterSongList +=  () => EndTransition(toSongList, songList);

            SongListManager.SongStart += ToMusicPlay;
            JsonScanner.ChartLoadFinish += () => EndTransition(toMusicPlay, musicPlay);

            SetTheme(IsNightModeEnabled());
        }

        private void ToMusicPlay(ref MusicPlayDescription mpd)
        {
            jacket.enabled = true;
            jacket.sprite = mpd.jacket;
            jacket.color = Color.white;

            var jacketTransform = jacket.rectTransform;
            jacketTransform.localScale = Vector3.one;
            jacketTransform.position = UIElementReference.Instance.songJacket.transform.position;

            LeanTween.value(jacket.gameObject, v2 =>
            {
                jacketTransform.anchoredPosition = v2;
            }, jacketTransform.anchoredPosition, Vector2.zero, 0.5f).setEase(LeanTweenType.easeOutCubic);
            
            StartTransition(toMusicPlay, musicPlay, false, Callback);
            return;

            void Callback()
            {
                UIElementReference.Instance.musicPlay.enabled = true;
                UIElementReference.Instance.songList.enabled = false;
                UIElementReference.Instance.topView.enabled = false;
                JsonScanner.ChartLoadFinish += JsonScannerOnChartLoadFinish;
                return;

                void JsonScannerOnChartLoadFinish()
                {
                    JsonScanner.ChartLoadFinish -= JsonScannerOnChartLoadFinish;
                    LeanTween.value(jacket.gameObject, f =>
                    {
                        jacket.color = new Color(1, 1, 1,1 - f);
                        jacket.transform.localScale = Vector3.one * (1 + 0.2f * f);
                    }, 0, 1f, .5f);
                }
            }
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        private static bool IsNightModeEnabled()
        {
            using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            return unityPlayer
                .GetStatic<AndroidJavaObject>("currentActivity")
                .Call<AndroidJavaObject>("getSystemService", "uimode")
                .Call<int>("getNightMode") == 2;
        }
#else
        private bool IsNightModeEnabled()
        {
            return isNightMode;
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        private void SetTheme(bool isNight)
        {
            cover.sprite = isNight ? nighttimeCover : daytimeCover;
            SetWireframeColor(isNight ? Color.white : Color.black);
            return;

            void SetWireframeColor(Color color)
            {
                foreach (var image in toMainMenu)
                    image.color = new Color(1, 1, 1, image.color.a) * color;
                foreach (var image in toSongList)
                    image.color = new Color(1, 1, 1, image.color.a) * color;
                foreach (var image in toMusicPlay)
                    image.color = new Color(1, 1, 1, image.color.a) * color;
            }
        }
#else        
        private void SetTheme(bool isNight)
        {
            cover.sprite = isNight ? nighttimeCover : daytimeCover;
            SetWireframeColor(isNight ? Color.white : Color.black);
            return;

            void SetWireframeColor(Color color)
            {
                foreach (var image in toMainMenu)
                    image.color = new Color(1, 1, 1, image.color.a) * color;
                foreach (var image in toSongList)
                    image.color = new Color(1, 1, 1, image.color.a) * color;
                foreach (var image in toMusicPlay)
                    image.color = new Color(1, 1, 1, image.color.a) * color;
            }
        }
#endif        

        private void OnApplicationFocus(bool hasFocus)
        {
            SetTheme(IsNightModeEnabled());
        }

        private void OnApplicationPause(bool pauseStatus)
        {   
            SetTheme(IsNightModeEnabled());
        }

        private void StartTransition(
            IReadOnlyList<Graphic> target, 
            GameObject targetParent, 
            bool autoExit,
            Action callback = null)
        {
            coroutine = Flash(target);
            StartCoroutine(coroutine);
            
            canvas.enabled = true;
            targetParent.SetActive(true);
            
            // Bring up the cover
            LeanTween.cancel(cover.gameObject);
            LeanTween.value(cover.gameObject, f =>
            {
                cover.color = new Color(1, 1, 1, f);
            }, 0, 1, .5f);
            
            // Start Effect
            LeanTween.cancel(target[0].gameObject);
            LeanTween.value(target[0].gameObject, f => 
                { 
                    transitionTime = f;
                    foreach (var graphic in target)
                    {
                        graphic.enabled = Random.Range(0f, 1f) <= f;
                    }
                }, 0, 1, 1f)
                .setOnComplete(() =>
                {
                    callback?.Invoke();
                    TransitionBreak?.Invoke(coroutine);
                    TransitionBreak = null;
                    if (autoExit)
                        EndTransition(target, targetParent);
                });
        }
        
        private void EndTransition(IReadOnlyList<Graphic> target, GameObject targetParent)
        {
            coroutine = Flash(target);
            StartCoroutine(coroutine);
            
            LeanTween.cancel(cover.gameObject);
            LeanTween.value(cover.gameObject, f =>
            {
                cover.color = new Color(1, 1, 1, f);
            }, 1, 0, 1f);
            LeanTween.cancel(target[0].gameObject);
            LeanTween.value(target[0].gameObject, f =>
                {
                    foreach (var graphic in target)
                    {
                        graphic.enabled = Random.Range(0f, 1f) <= f;
                    }
                }, 1, 0, 1f)
                .setOnComplete(() =>
                {
                    TransitionBreak?.Invoke(coroutine);
                    TransitionBreak = null;
                    canvas.enabled = false;
                    targetParent.SetActive(false);
                });
        }

        private IEnumerator Flash(IReadOnlyList<Graphic> target)
        {
            TransitionBreak += StopCoroutine;
            
            while (true)
            {
                foreach (var graphic in target)
                {
                    graphic.enabled = Random.Range(0f, 1f) <= transitionTime;
                }

                for(var i = 0; i < 8; i++)
                    yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}