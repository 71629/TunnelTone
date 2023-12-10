using System;
using TunnelTone.Elements;
using TunnelTone.UI.Reference;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace TunnelTone.PlayArea
{
    public class Touch : MonoBehaviour
    {
        // TODO: Make this a prefab too
        
        private RaycastHit _hit;
        private GameObject _trackingTrail;
        private SphereCollider _collider;
        private Rigidbody _rigidbody;
        private Vector3 position => transform.position;
        public TouchControl TrackingTouch;

        public Direction direction;

        private void Awake()
        {
            gameObject.layer = 12;
            direction = Direction.Any;
        }

        public void Initialize(in TouchControl touch)
        {
            TrackingTouch = touch;
            _collider = gameObject.AddComponent<SphereCollider>();
            _collider.radius = 1;
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        private void Update()
        {
            if (TrackingTouch.phase.value == TouchPhase.Ended)
            {
                if(_trackingTrail is not null) {
                    _trackingTrail.GetComponent<Trail>().isTracking = false;
                    _trackingTrail.GetComponent<Trail>().trackingTouch = null;
                }
                Destroy(gameObject);
            }
            transform.position = UIElementReference.Instance.mainCamera.ScreenToWorldPoint((Vector3)TrackingTouch.position.value + Vector3.forward * 100);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Long Note") && direction == Direction.Any)
            {
                GameObject o;
                direction = (o = other.gameObject).GetComponent<Trail>().Direction;
                _trackingTrail = o;
            }
        }
    }
}