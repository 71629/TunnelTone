using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace TunnelTone.PlayArea
{
    [CreateAssetMenu]
    public class InputReader : ScriptableObject
    {
        private const int MaxTouch = 10;
        
        [Header("Tap")]
        [SerializeField] private float maxTapDuration;
        [SerializeField] private float maxTapDrift;
        
        [Header("Swipe")]
        [SerializeField] private float maxSwipeDuration;
        [SerializeField] private float minSwipeDistance;
        [SerializeField] private float swipeStraightnessThreshold;
        
        public event TouchDownAction TouchDown;
        public event TouchMoveAction TouchMove;
        public event TouchUpAction TouchUp;
        
        public event GesturePressAction GesturePress;
        public event GestureTapAction GestureTap;
        public event GesturePotentialSwipeAction GesturePotentialSwipe;
        public event GestureSwipeAction GestureSwipe;
        
        public delegate void TouchDownAction(int pointerId, double time, Vector2 screenPosition);
        public delegate void TouchMoveAction(int pointerId, double time, Vector2 screenPosition);
        public delegate void TouchUpAction(int pointerId, double time, Vector2 screenPosition);
        
        public delegate void GesturePressAction(PressGesture press);
        public delegate void GestureTapAction(TapGesture tap);
        public delegate void GesturePotentialSwipeAction(SwipeGesture potentialSwipe);
        public delegate void GestureSwipeAction(SwipeGesture swipe);
        
        private Dictionary<int, InputGesture> touchGestures;
        
        private void OnEnable()
        {
            touchGestures ??= new Dictionary<int, InputGesture>(MaxTouch);
            for (var i = 0; i < MaxTouch; i++) touchGestures.Add(i, new InputGesture(i));
            
            EnhancedTouchSupport.Enable();
            EnhancedTouch.onFingerDown += OnFingerDown;
            EnhancedTouch.onFingerMove += OnFingerMove;
            EnhancedTouch.onFingerUp += OnFingerUp;
        }

        private void OnFingerDown(Finger finger)
        {
            TouchDown?.Invoke(finger.index, finger.currentTouch.time, finger.screenPosition);
            
            touchGestures[finger.index].Reset(finger.screenPosition, finger.currentTouch.time);
            GesturePress?.Invoke(new PressGesture(touchGestures[finger.index]));
        }
        
        private void OnFingerMove(Finger finger)
        {
            TouchMove?.Invoke(finger.index, finger.currentTouch.time, finger.screenPosition);
            
            var gesture = touchGestures[finger.index];
            gesture.Accumulate(finger.screenPosition, finger.currentTouch.time);
            
            if(IsValidSwipe(gesture)) GesturePotentialSwipe?.Invoke(new SwipeGesture(gesture));
        }
        
        private void OnFingerUp(Finger finger)
        {
            // Raw input
            TouchUp?.Invoke(finger.index, finger.currentTouch.time, finger.screenPosition);

            // Processed input
            var gesture = touchGestures[finger.index];
            gesture.Accumulate(finger.screenPosition, finger.currentTouch.time);
            
            if (IsValidSwipe(gesture)) GestureSwipe?.Invoke(new SwipeGesture(gesture));
            if (IsValidTap(gesture)) GestureTap?.Invoke(new TapGesture(gesture));
        }
        
        private bool IsValidTap(InputGesture gesture)
            => gesture.TravelDistance <= maxTapDrift && gesture.Duration <= maxTapDuration;

        private bool IsValidSwipe(InputGesture gesture)
            => gesture.TravelDistance >= minSwipeDistance && gesture.Duration <= maxSwipeDuration &&
               gesture.straightness >= swipeStraightnessThreshold;

        private void OnDisable()
        {
            EnhancedTouch.onFingerDown -= OnFingerDown;
            EnhancedTouch.onFingerMove -= OnFingerMove;
            EnhancedTouch.onFingerUp -= OnFingerUp;
            EnhancedTouchSupport.Disable();
        }
    }

    internal sealed class InputGesture
    {
        internal Vector2 lastPosition;
        private int samples;
        internal float straightness;
        
        public int PointerId { get; }
        public Vector2 StartPosition { get; private set; }
        public Vector2 EndPosition { get; private set; }
        public double StartTime { get; private set; }
        public double EndTime { get; private set; }
        public float TravelDistance { get; private set; }
        
        public double Duration => EndTime - StartTime;

        private Vector2 accumulateNormalized;
        
        internal InputGesture(int pointerId)
        {
            PointerId = pointerId;
        }

        internal void Reset(Vector2 startPosition, double startTime)
        {
            StartPosition = EndPosition = startPosition;
            StartTime = EndTime = startTime;

            samples = 1;
            straightness = 1f;
            TravelDistance = 0f;
            accumulateNormalized = Vector2.zero;
        }

        internal void Accumulate(Vector2 newPosition, double time)
        {
            var toDelta = newPosition - EndPosition;
            var distance = toDelta.magnitude;
            EndTime = time;
            
            if(Mathf.Approximately(distance, 0f)) return;

            toDelta /= distance;
            samples++;
            
            var fromDelta = (newPosition - StartPosition).normalized;
            lastPosition = EndPosition;
            EndPosition = newPosition;
            
            accumulateNormalized += toDelta;
            straightness = Vector2.Dot(fromDelta, accumulateNormalized / (samples - 1));
            TravelDistance += distance;
        }
    }

    public struct PressGesture
    {
        public readonly int pointerId;
        public readonly Vector2 position;
        public readonly double timestamp;

        internal PressGesture(InputGesture gesture)
        {
            pointerId = gesture.PointerId;
            position = gesture.StartPosition;
            timestamp = gesture.StartTime;
        }
    }

    public struct TapGesture
    {
        public readonly int pointerId;
        public readonly Vector2 startPosition, endPosition;
        public readonly float drift;
        public readonly double duration, timestamp;
        
        internal TapGesture(InputGesture inputGesture)
        {
            pointerId = inputGesture.PointerId;
            startPosition = inputGesture.StartPosition;
            endPosition = inputGesture.EndPosition;
            drift = inputGesture.TravelDistance;
            duration = inputGesture.Duration;
            timestamp = inputGesture.EndTime;
        }
    }
    
    public struct SwipeGesture
    {
        public readonly int pointerId;
        public readonly Vector2 startPosition, previousPosition, endPosition;
        public readonly Vector2 direction;
        public readonly float velocity, distance, straightness;
        public readonly double timestamp, duration;

        internal SwipeGesture(InputGesture gesture)
        {
            pointerId = gesture.PointerId;
            startPosition = gesture.StartPosition;
            previousPosition = gesture.lastPosition;
            endPosition = gesture.EndPosition;
            direction = (gesture.EndPosition - gesture.StartPosition).normalized;
            velocity = gesture.Duration > 0 ? (float)(gesture.TravelDistance / gesture.Duration) : 0f;
            distance = gesture.TravelDistance;
            straightness = gesture.straightness;
            timestamp = gesture.StartTime;
            duration = gesture.Duration;
        }
    }
}