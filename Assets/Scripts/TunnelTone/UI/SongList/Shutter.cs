using System;
using System.Collections;
using TunnelTone.Elements;
using TunnelTone.Events;
using TunnelTone.Singleton;
using TunnelTone.UI.PlayResult;
using TunnelTone.UI.Reference;
using UnityEngine;

namespace TunnelTone.UI.SongList
{
    public class Shutter : Singleton<Shutter>
    {
        private Animator shutterAnimator => UIElementReference.Instance.shutterAnimator;
        
        private static readonly int Close = Animator.StringToHash("Close");
        private static readonly int Open = Animator.StringToHash("Open");

        private void Start()
        {
            SongListEventReference.Instance.OnSongStart.AddListener(delegate
            {
                ChartEventReference.Instance.OnSongEnd.AddListener(OnSongEnd);
                CloseShutter();
            });
            SystemEvent.OnChartLoadFinish.AddListener(delegate
            {
                OpenShutter();
            });
            ChartEventReference.Instance.OnRetry.AddListener(delegate
            {
                CloseShutter();
            });
            ChartEventReference.Instance.OnQuit.AddListener(delegate
            {
                ChartEventReference.Instance.OnSongEnd.RemoveListener(OnSongEnd);
                ToSongList();
            });
        }

        private void ToSongList()
        {
            CloseShutter(() =>
            {
                UIElementReference.Instance.songList.enabled = true;
                UIElementReference.Instance.topView.enabled = true;
                UIElementReference.Instance.musicPlay.enabled = false;
                OpenShutter();
                AudioListener.pause = false;
                SongListEventReference.Instance.OnEnterSongList.Trigger();
                NoteRenderer.OnDestroyChart.Trigger();
            });
        }

        private void OnSongEnd(params object[] param)
        {
            StartCoroutine(ToResultScreen());
        }

        private IEnumerator ToResultScreen()
        {
            yield return new WaitForSecondsRealtime(5f);
            
            CloseShutter(() =>
            {
                UIElementReference.Instance.musicPlay.enabled = false;
                SystemEvent.OnDisplayResult.Trigger();
                NoteRenderer.OnDestroyChart.Trigger();
                OpenShutter();
            });
        }
        
        private void CloseShutter(Action onCompleteCallback = null)
        {
            shutterAnimator.SetTrigger(Close);
            StartCoroutine(CallbackAfterAnimation(onCompleteCallback));
        }
        private void OpenShutter() => shutterAnimator.SetTrigger(Open);
        
        private static IEnumerator CallbackAfterAnimation(Action callback)
        {
            yield return new WaitForSecondsRealtime(1f);
            callback?.Invoke();
        }
    }
}