using TunnelTone.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;

namespace TunnelTone.PlayArea
{
    public class InteractionManager : Singleton<InteractionManager>
    {
        private static Touchscreen Touchscreen => Touchscreen.current;
        private static ReadOnlyArray<TouchControl> Touches => Touchscreen.touches;

        private static Touch[] touches;

        [SerializeField] private RectTransform interactionRectRoot;

        [SerializeField] private Camera canvasCamera;
        [SerializeField] private GameObject touchPrefab;
        [SerializeField] public InputReader inputReader;

        public void Start()
        {
            touches = new Touch[10];
            for (var i = 0; i < touches.Length; i++)
            {
                var instance = Instantiate(touchPrefab, interactionRectRoot);
                instance.gameObject.SetActive(false);
                touches[i] = instance.GetComponent<Touch>();
                touches[i].pointerId = i;
            }
        }
        
        public void Enable()
        {
            inputReader.TouchDown += OnTouchDown;
            inputReader.TouchMove += OnTouchMove;
            inputReader.TouchUp += OnTouchUp;
        }

        private static void OnTouchUp(int pointerId, double _, Vector2 screenPosition)
        {
            touches[pointerId].gameObject.SetActive(false);
        }

        private void OnTouchMove(int pointerId, double _, Vector2 screenPosition)
        { 
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(interactionRectRoot, screenPosition, canvasCamera, out var canvasPosition)) return;
            
            touches[pointerId].AnchoredPosition = canvasPosition;
        }
        
        private void OnTouchDown(int pointerId, double _, Vector2 screenPosition)
        {
            if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(interactionRectRoot, screenPosition, canvasCamera, out var canvasPosition)) return;
            
            var touch = touches[pointerId];
            touch.AnchoredPosition = canvasPosition;
            touch.gameObject.SetActive(true);
        }

        public void Disable()
        {
            inputReader.TouchDown -= OnTouchDown;
            inputReader.TouchMove -= OnTouchMove;
            inputReader.TouchUp -= OnTouchUp;   
        }


        // private void Update()
        // {
        //     if (Touchscreen is not { wasUpdatedThisFrame: true } || !NoteRenderer.isPlaying) return;
        //     
        //     // Tap note interaction
        //     foreach (var touch in Touches.Where(touch => touch.phase.value == TouchPhase.Began))
        //     {
        //         Instantiate(touchPrefab, transform).GetComponent<Touch>().Initialize(in touch).FindTap();
        //     }
        // }
    }
}