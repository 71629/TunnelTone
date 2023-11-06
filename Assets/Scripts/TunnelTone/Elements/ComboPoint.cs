using System.Collections;
using UnityEngine;

namespace TunnelTone.Elements
{
    public class ComboPoint : MonoBehaviour
    {
        private float _time;

        private void Start()
        {
            StartCoroutine(DetermineCombo());
        }

        private IEnumerator DetermineCombo()
        {
            yield return new WaitUntil(() => (float)(AudioSettings.dspTime - NoteRenderer.dspSongStartTime) >= _time);
        }
    }
}