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
        private RaycastHit hit;
        private GameObject trackingTrail;
        [SerializeField] private SphereCollider trackingRange;
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] private SplineExtrude splineExtrude;
        private Spline Spline => splineContainer.Spline;
        private Vector3 Position => transform.position;
        private TouchControl trackingTouch;

        public Direction direction;

        private void Awake()
        {
            gameObject.layer = 12;
            direction = Direction.Any;
        }

        public GameObject Initialize(in TouchControl touch)
        {
            trackingTouch = touch;

            LeanTween.value(gameObject, f =>
                    {
                        
                        splineExtrude.Rebuild();
                    },
                    0, 15f, .5f)
                .setEase(LeanTweenType.easeOutSine);

            return gameObject;
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
            transform.position = UIElementReference.Instance.mainCamera.ScreenToWorldPoint((Vector3)trackingTouch.position.value + Vector3.forward * 100);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Long Note") && direction == Direction.Any)
            {
                GameObject o;
                direction = (o = other.gameObject).GetComponent<Trail>().Direction;
                trackingTrail = o;
            }
        }
    }
}