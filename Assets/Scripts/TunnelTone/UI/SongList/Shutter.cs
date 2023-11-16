using TunnelTone.Events;
using TunnelTone.Singleton;
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
                CloseShutter();
            });
            SystemEventReference.Instance.OnChartLoadFinish.AddListener(delegate
            {
                OpenShutter();
            });
            ChartEventReference.Instance.OnRetry.AddListener(delegate
            {
                CloseShutter();
            });
        }
        
        private void CloseShutter() => shutterAnimator.SetTrigger(Close);
        private void OpenShutter() => shutterAnimator.SetTrigger(Open);
        
    }
}