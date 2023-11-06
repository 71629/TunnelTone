using System.Runtime.ConstrainedExecution;
using TunnelTone.Elements;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace TunnelTone.PlayArea
{
    public class Touch : MonoBehaviour
    {
        private RaycastHit _hit;
        private Collider _trackingTrail;
        private Vector3 position => transform.position;
        private TouchControl _trackingTouch;

        public void Initialize(in TouchControl touch)
        {
            _trackingTouch = touch;
        }
        
        private void Update()
        {
            #if DEBUG
            Debug.Log(_trackingTrail);
            #endif
            
            if(_trackingTouch.phase.value is TouchPhase.Ended) Destroy(gameObject);
            transform.position = _trackingTouch.position.ReadValue();

            Ray ray;
            
            if (_trackingTrail is not null)
            {
                var trackingColliderPosition = _trackingTrail.transform.position;
                ray = new Ray(position, Vector3.Normalize(trackingColliderPosition - position));
                Debug.DrawRay(position, trackingColliderPosition - position);
            }
            
            if (_trackingTrail is null)
            {
                ray = new Ray(position + Vector3.back * 650, Vector3.forward);
                
                if (Physics.Raycast(ray, out _hit, float.MaxValue, 1 << 11))
                {
                    Debug.DrawRay(ray.origin, ray.direction * _hit.distance, new Color(0.63f, 0.46f, 0.85f));
                    _trackingTrail = _hit.collider;
                }
            }
        }
    }
}