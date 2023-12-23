using System;
using System.Linq;
using TunnelTone.Elements;
using TunnelTone.UI.Reference;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Splines;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace TunnelTone.PlayArea
{
    public class Touch : MonoBehaviour
    {
        private static Camera MainCamera => UIElementReference.Instance.mainCamera;
        
        private RaycastHit hit;
        internal GameObject trackingTrail;
        [SerializeField] private SphereCollider trackingRange;
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] private SplineExtrude splineExtrude;
        private Spline Spline => splineContainer.Spline;
        private Vector3 Position => transform.position;
        private TouchControl trackingTouch;

        internal Direction direction;

        private void Awake()
        {
            gameObject.layer = 12;
            direction = Direction.Any;
        }

        internal Touch Initialize(in TouchControl touch)
        {
            trackingTouch = touch;

            LeanTween.value(gameObject, f =>
                    {
                        Spline.SetKnot(1, new BezierKnot(new float3(0, 0, f)));
                        splineExtrude.Rebuild();
                    },
                    0, 250f, .3f)
                .setEase(LeanTweenType.easeOutSine);

            return this;
        }

        internal Touch FindTap()
        {
            Ray ray = new(transform.position + Vector3.back * 1000, Vector3.forward);
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
                Debug.DrawRay(ray.origin, Vector3.forward * hit.distance, Color.green);
                return this;
            }
            Debug.DrawRay(ray.origin, Vector3.forward * 1200, Color.red);
            return this;
        }

        private void Update()
        {
            if (trackingTouch.phase.value == TouchPhase.Ended)
            {
                if(trackingTrail is not null) {
                    trackingTrail.GetComponent<Trail>().isTracking = false;
                    trackingTrail.GetComponent<Trail>().trackingTouch = null;
                }
                Destroy(gameObject);
            }
            
            // Update object position
            transform.position = MainCamera.ScreenToWorldPoint((Vector3)trackingTouch.position.value + Vector3.forward * 100);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ( other.gameObject == trackingTrail || (other.gameObject.layer == LayerMask.NameToLayer("Long Note") && direction == Direction.Any))
            {
                var o = other.gameObject;
                direction = o.GetComponent<Trail>().Direction;
                trackingTrail = o;
                
                LeanTween.cancel(gameObject);
                LeanTween.value(gameObject,
                    f =>
                    {
                        Spline.SetKnot(1, new BezierKnot(new float3(0, 0, f)));
                        splineExtrude.Rebuild();
                    },
                    Spline.Knots.ToArray()[1].Position.z, 175f, .3f)
                .setEase(LeanTweenType.easeOutSine);
                
                LeanTween.value(gameObject,
                    f =>
                    {
                        splineExtrude.Radius = f;
                    },
                    splineExtrude.Radius, 4f, .3f)
                .setEase(LeanTweenType.easeOutSine);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == trackingTrail)
            {
                LeanTween.cancel(gameObject);
                LeanTween.value(gameObject,
                    f =>
                    {
                        Spline.SetKnot(1, new BezierKnot(new float3(0, 0, f)));
                        splineExtrude.Rebuild();
                    },
                    Spline.Knots.ToArray()[1].Position.z, 250f, .3f)
                .setEase(LeanTweenType.easeOutSine);

                LeanTween.value(gameObject,
                    f =>
                    {
                        splineExtrude.Radius = f;
                    },
                    splineExtrude.Radius, .3f, .3f)
                .setEase(LeanTweenType.easeOutSine);
            }
        }

        private void OnDestroy()
        {
            LeanTween.cancel(gameObject);
            if (trackingTrail is null) return;
            var trail = trackingTrail.GetComponent<Trail>();
            trail.isTracking = false;
            trail.trackingTouch = null;
        }
    }
}