using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TunnelTone.Charts;
using TunnelTone.Elements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace TunnelTone.PlayAreaManager
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayAreaManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        private void Update()
        {
            if(Touchscreen.current == null) return;
            
            // Debug.Log(Touchscreen.current.touches.Where(touch => touch.isInProgress).Aggregate("", (current, touch) => current + $"{touch.position.value}     "));
            foreach(var touch in Touchscreen.current.touches)
            {
                if (touch.isInProgress)
                {
                    Debug.DrawRay(mainCamera.ScreenToWorldPoint((Vector3)touch.position.value + new Vector3(0, 0, 100)), new Vector3(0, 0, 200), Color.green);
                }
            }
        }
    }
}