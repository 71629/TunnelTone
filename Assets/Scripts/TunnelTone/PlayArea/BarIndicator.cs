using TunnelTone.Charts;
using TunnelTone.Core;
using TunnelTone.Elements;
using UnityEngine;

namespace TunnelTone.PlayArea
{
    public class BarIndicator : PlayAreaElements
    {
        private float zPosition;
        private void Start()
        {
            transform.localPosition = new Vector3(0, 0, time.TranslateTiming() * NoteRenderer.Instance.chartSpeedModifier);
        }
        private void Update()
        {
            if(NoteRenderer.isPlaying && NoteRenderer.CurrentTime > time)
            {
                Destroy(gameObject);
            }
        }
    }
}