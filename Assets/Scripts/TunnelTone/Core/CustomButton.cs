using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TunnelTone.Core
{
    public class CustomButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        public UnityEvent onPointerDown;
        
        public void OnPointerUp(PointerEventData eventData)
        {
            StopAllCoroutines();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StartCoroutine(DasEffect());
        }

        private IEnumerator DasEffect()
        {
            onPointerDown.Invoke();
            yield return new WaitForSecondsRealtime(.5f);
            while (true)
            {
                onPointerDown.Invoke();
                yield return null;
            }
        }
    }
}