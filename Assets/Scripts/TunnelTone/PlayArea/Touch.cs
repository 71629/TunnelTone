using System.Linq;
using TunnelTone.Elements;
using TunnelTone.UI.Reference;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Splines;
using TMPro;

namespace TunnelTone.PlayArea
{
    public class Touch : MonoBehaviour
    {
        private static bool effectEnabled = false;
        
        private static Camera MainCamera => UIElementReference.Instance.mainCamera;
        
        private RaycastHit hit;
        
        [SerializeField] private SphereCollider trackingRange;
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] private SplineExtrude splineExtrude;
        [SerializeField] private TextMeshPro Score;
        [SerializeField] private RectTransform rectTransform;

        public Vector2 AnchoredPosition
        {
            get => rectTransform.anchoredPosition;
            set => rectTransform.anchoredPosition = value;
        }

        public int pointerId;

        private Spline Spline => splineContainer.Spline;
        private Vector3 Position => transform.position;
        private TouchControl trackingTouch;

        internal Direction direction;

        private void Awake()
        {
            gameObject.layer = 12;
            direction = Direction.Any;
        }

        private void OnEnable()
        {
            Ray ray = new( transform.position + Vector3.back * 1000, Vector3.forward);
            if (Physics.Raycast(ray, out var hit, 1200, 1 << 10))
            {
                if (hit.collider.GetComponent<Tap>().Hit() <= 100)
                {
                    LeanTween.cancel(gameObject);
                    LeanTween.value(gameObject, f =>
                            {
                                splineExtrude.Radius = f;
                            }, 
                            4, .3f, .3f)
                        .setEase(LeanTweenType.easeOutSine);
                }
                Debug.DrawRay(ray.origin, Vector3.forward * hit.distance, Color.green, 1f);
            }
            Debug.DrawRay(ray.origin, Vector3.forward * 1200, Color.red, 1f);
        }

        private void OnDisable()
        {
            direction = Direction.Any;
        }
    }
}