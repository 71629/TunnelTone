using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TunnelTone.Core
{
    [RequireComponent(typeof(Image))]
    public class EmptyArea : MonoBehaviour, IPointerDownHandler
    {
        private Image image;
        public UnityEvent pointerDownEvent = new();

        private void Awake()
        {
            image = GetComponent<Image>();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDownEvent?.Invoke();
        }

        public void Enable()
        {
            image.enabled = true;
        }
    }
}