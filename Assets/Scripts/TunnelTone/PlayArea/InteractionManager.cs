using System.Linq;
using TunnelTone.Elements;
using TunnelTone.Singleton;
using TunnelTone.UI.Reference;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace TunnelTone.PlayArea
{
    public class InteractionManager : Singleton<InteractionManager>
    {
        private static Touchscreen Touchscreen => Touchscreen.current;
        private static ReadOnlyArray<TouchControl> Touches => Touchscreen.touches;
        private static Camera MainCamera => UIElementReference.Instance.mainCamera;
        [SerializeField] private GameObject touchPrefab;

        [SerializeField] private Material leftTrailHit, rightTrailHit;

        private void Update()
        {
            if (Touchscreen is not { wasUpdatedThisFrame: true } || !NoteRenderer.isPlaying) return;
            
            // Tap note interaction
            foreach (var touch in Touches.Where(touch => touch.phase.value == TouchPhase.Began))
            {
                Instantiate(touchPrefab, transform).GetComponent<Touch>().Initialize(in touch).FindTap();
            }
        }
    }
}