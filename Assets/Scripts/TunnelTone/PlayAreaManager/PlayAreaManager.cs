using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TunnelTone.Charts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace TunnelTone.PlayAreaManager
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayAreaManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput touchInput;

        #region Tap properties
        private InputAction Tap0 => touchInput.actions.FindAction("Tap0");
        private InputAction Tap1 => touchInput.actions.FindAction("Tap1");
        private InputAction Tap2 => touchInput.actions.FindAction("Tap2");
        private InputAction Tap3 => touchInput.actions.FindAction("Tap3");
        private InputAction Tap4 => touchInput.actions.FindAction("Tap4");
        private InputAction Tap5 => touchInput.actions.FindAction("Tap5");
        private InputAction Tap6 => touchInput.actions.FindAction("Tap6");
        private InputAction Tap7 => touchInput.actions.FindAction("Tap7");
        private InputAction Tap8 => touchInput.actions.FindAction("Tap8");
        private InputAction Tap9 => touchInput.actions.FindAction("Tap9");
        #endregion
        
        private List<InputAction> _taps;

        private void Start()
        {
            _taps = new List<InputAction> { Tap0, Tap1, Tap2, Tap3, Tap4, Tap5, Tap6, Tap7, Tap8, Tap9 };

            foreach (var tap in _taps)
            {
                tap.performed += OnTap;
            }
        }

        private static void OnTap(InputAction.CallbackContext ctx)
        {
            Debug.Log(ctx.ReadValue<Vector2>());
        }
    }
}