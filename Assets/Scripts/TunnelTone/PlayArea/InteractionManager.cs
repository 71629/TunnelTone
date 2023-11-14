using System.Linq;
using TunnelTone.Elements;
using TunnelTone.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace TunnelTone.PlayArea
{
    public class InteractionManager : Singleton<InteractionManager>
    {
        private Touchscreen Touchscreen => Touchscreen.current;
        private ReadOnlyArray<TouchControl> touches => Touchscreen.touches;
        private Camera MainCamera => Camera.main;

        [SerializeField] private Material leftTrailHit, rightTrailHit;

        private void Update()
        {
            if (Touchscreen is not { wasUpdatedThisFrame: true }) return;
            
            // Tap note interaction
            foreach (var touch in touches.Where(touch => touch.phase.value == TouchPhase.Began))
            {
                GameObject gb = new("Touch")
                {
                    transform = { parent = transform }
                };
                gb.AddComponent<Touch>().Initialize(in touch);
                var touchPosition =
                    MainCamera.ScreenToWorldPoint((Vector3)touch.startPosition.value + Vector3.forward * 100);
                var ray = new Ray(touchPosition + Vector3.back * 120, Vector3.forward);
                
                if (Physics.Raycast(ray, out var hit, 600, 1 << 10)) // evil bit hack
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                    hit.collider.GetComponent<Tap>().Hit();
                    continue;
                }
                Debug.DrawRay(ray.origin, ray.direction * 600, Color.red);
            }
            
            // Trail note detection
            foreach (var touch in touches.Where(touch => touch.phase.value is TouchPhase.Moved or TouchPhase.Stationary))
            {
                var touchPosition =
                    MainCamera.ScreenToWorldPoint((Vector3)touch.position.value + Vector3.forward * 100);
                var ray = new Ray(touchPosition + Vector3.back * 120, Vector3.forward);

                if (Physics.Raycast(ray, out var hit, 1000, 1 << 11))
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, new Color(0.63f, 0.46f, 0.85f));
                    continue;
                }
                Debug.DrawRay(ray.origin, ray.direction * 1000, new Color(0.64f, 0.41f, 0.29f));
            }
        }
    }
}