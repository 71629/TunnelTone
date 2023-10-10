﻿using System.Linq;
using TunnelTone.Elements;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace TunnelTone.PlayAreaManager
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayAreaManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private NoteRenderer noteRenderer;

        private void Awake()
        {
            Application.targetFrameRate = 120;
        }
        
        private void Update()
        {
            if(Touchscreen.current == null) return;
            
            // Debug.Log(Touchscreen.current.touches.Where(touch => touch.isInProgress).Aggregate("", (current, touch) => current + $"{touch.position.value}     "));
            foreach (var touch in Touchscreen.current.touches.Where(touch => touch.phase.value == TouchPhase.Began))
            {
                var touchPosition = mainCamera.ScreenToWorldPoint((Vector3)touch.startPosition.value + Vector3.forward * 100);
                var ray = new Ray(touchPosition + Vector3.back * 120, Vector3.forward);
                
                if (Physics.Raycast(ray, out var hit, 600))
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                    if (hit.collider.CompareTag("Note"))
                    {
                        Hit(hit);
                    }
                    continue;
                }
                Debug.DrawRay(ray.origin, ray.direction * 600, Color.red);
            }
        }

        private void Hit(RaycastHit hit)
        {
            var note = hit.collider.GetComponent<Tap>();
            note.Hit();
        }
    }
}
