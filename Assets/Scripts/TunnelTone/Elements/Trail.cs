using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TunnelTone.Charts;
using TunnelTone.PlayArea;
using TunnelTone.UI.Reference;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

namespace TunnelTone.Elements
{
    [RequireComponent(typeof(SphereCollider))]
    public partial class Trail : PlayAreaElements
    {
        private static readonly int[] MeshTriangleSequence = { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 1 };
        private static readonly Vector2 DefaultUV = Vector2.one / 2;
        private static readonly Vector2[] MeshUVSequence = { DefaultUV, DefaultUV, DefaultUV, DefaultUV, DefaultUV };
        private static readonly float TwistModifier = Mathf.Pow(0.4f, 3);
        
        // Basic trail properties
        internal float startTime
        {
            get => time;
            private set => time = value;
        }

        internal float endTime;
        internal Vector2 startCoordinate, endCoordinate;
        private bool virtualTrail;
        internal Direction Direction { get; private set; }
        public TrailType trailContext;
        
        // Trail References
        internal Trail next;
        internal bool skipSpawnAnimation = false;
        
        // Status Reference
        private bool isHit;
        internal bool isTracking;
        internal bool allowTrack;
        
        // Coupled Components
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] internal SphereCollider col;
        [SerializeField] internal LineRenderer lineRenderer;
        [SerializeField] internal MeshRenderer meshRenderer;
        [SerializeField] private MeshFilter meshFilter;
        
        internal Spline spline;
        internal int? trackingPointerId;
        
        private List<GameObject> comboPoint;
        private List<TrailSubsegment> subsegments;
        
        internal TrailHint trailHint;
        [SerializeField] internal GameObject trailHintObject;
        
        // Objects
        [SerializeField] internal GameObject trailSubsegmentPrefab;
        [SerializeField] internal Material virtualTrailMaterial;
        [SerializeField] private Sprite outerRing;
        [SerializeField] private Sprite innerRing;
        
        internal TrailState state;
        internal readonly UnityEvent onStateChanged = new();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent<PlayArea.Touch>(out var touch)) return;
            if (trackingPointerId is not null && touch.pointerId != trackingPointerId) return;
            
            if (touch.direction == Direction.Any || touch.direction == Direction)
            {
                InteractionManager.Instance.inputReader.TouchUp += OnTouchUp;
                allowTrack = true;
                isTracking = true;
                // trackingTouch = other.gameObject;
                touch.direction = Direction;
                state = new Tracking();
                onStateChanged.Invoke();
                return;
            }
            isTracking = false;
            allowTrack = false;
            state = new WrongHand();
            onStateChanged.Invoke();
        }

        private void OnTouchUp(int pointerId, double time, Vector2 screenPosition)
        {
            if (pointerId != trackingPointerId) return;
            
            InteractionManager.Instance.inputReader.TouchUp -= OnTouchUp;
            trackingPointerId = null;
            isTracking = false;
            state = new Idle();
            onStateChanged.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<PlayArea.Touch>(out var touch) || touch.pointerId != trackingPointerId) return;
            
            isTracking = false;
            state = new Idle();
            onStateChanged.Invoke();
        }
        
        public Trail Initialize(
            float startTime, 
            float endTime, 
            Vector2 startCoordinate, 
            Vector2 endCoordinate, 
            Direction direction, 
            EasingMode easing, 
            float easingRatio, 
            bool newTrail, 
            bool virtualTrail)
        {
            meshFilter.mesh = new Mesh();
            
            Direction = direction;
            allowTrack = true;
            
            this.startTime = startTime;
            this.endTime = endTime;
            this.startCoordinate = startCoordinate;
            this.endCoordinate = endCoordinate;

            var gameAreaRect = NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect;
            startCoordinate = new Vector2(startCoordinate.x * gameAreaRect.width * 0.5f, startCoordinate.y * gameAreaRect.height * 0.5f);
            endCoordinate = new Vector2(endCoordinate.x * gameAreaRect.width * 0.5f, endCoordinate.y * gameAreaRect.height * 0.5f);
            
            var startPosition = new Vector3(startCoordinate.x, startCoordinate.y, startTime * NoteRenderer.Instance.chartSpeedModifier);
            var endPosition = new Vector3(endCoordinate.x, endCoordinate.y, endTime * NoteRenderer.Instance.chartSpeedModifier);

            spline = splineContainer.Spline;
            
            // Create curve with respect previewDuration easing and easing ratio
            var chartSpeedModifier = NoteRenderer.Instance.chartSpeedModifier;
            var easingModifier = Mathf.Pow(easingRatio, 2) * (endTime - startTime);
            switch (easing)
            {
                case EasingMode.Straight:
                    spline.Insert(0, new BezierKnot(startPosition, 0, 0, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, 0, 0, quaternion.identity));
                    break;
                case EasingMode.EaseIn:
                    spline.Insert(0, new BezierKnot(startPosition, 0, 0, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, 0, -easingModifier) * chartSpeedModifier, 0, quaternion.identity));
                    break;
                case EasingMode.EaseOut:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, 0, easingModifier) * chartSpeedModifier, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, 0, 0, quaternion.identity));
                    break;
                case EasingMode.Bezier:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, 0, easingModifier) * chartSpeedModifier, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, 0, -easingModifier) * chartSpeedModifier, 0, quaternion.identity));
                    break;
                case EasingMode.HorizontalInVerticalOut:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(TwistModifier * (endPosition - startPosition).x, 0, Mathf.Abs(TwistModifier * (endPosition - startPosition).x)) * chartSpeedModifier, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, -TwistModifier * (endPosition - startPosition).y, -Mathf.Abs(TwistModifier * (endPosition - startPosition).y)) * chartSpeedModifier, 0, quaternion.identity));
                    break;
                case EasingMode.VerticalInHorizontalOut:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, TwistModifier * (endPosition - startPosition).y, Mathf.Abs(TwistModifier * (endPosition - startPosition).y)) * chartSpeedModifier, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(-TwistModifier * (endPosition - startPosition).x, 0, -Mathf.Abs(TwistModifier * (endPosition - startPosition).x)) * chartSpeedModifier, 0, quaternion.identity));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(easing), easing, null);
            }

            trailContext = virtualTrail ? new VirtualTrail(this) : new RealTrail(this);
            trailContext.Initialize();
            trailContext.ConfigureCollider();

            return this;
        }

        internal void BuildHead(Vector3 position, float time, bool virtualTrail)
        {
            meshFilter.mesh = new Mesh
            {
                vertices = trailContext.GetVertices(position, time),
                triangles = MeshTriangleSequence,
                uv = MeshUVSequence
            };
        }

        internal IEnumerator TrailHint()
        {
            yield return new WaitUntil(() => NoteRenderer.CurrentTime * 1000 >= startTime - 550 && NoteRenderer.isPlaying);
            trailHint.Enable(Direction, skipSpawnAnimation);
            
            if (!skipSpawnAnimation) yield break;
            
            yield return new WaitUntil(() => NoteRenderer.CurrentTime * 1000 >= startTime && NoteRenderer.isPlaying);
            trailHint.EnableImage();
        }

        private void Start()
        {
            if(trailContext is RealTrail)
                JsonScanner.ChartLoadFinish += ConfigureSkip;
            return;
            
            void ConfigureSkip()
            {
                JsonScanner.ChartLoadFinish -= ConfigureSkip;
                foreach (var c in NoteRenderer.TrailList.Where(ComparePositionTiming))
                {
                    c.next = this;
                    skipSpawnAnimation = true;
                    break;
                }
                return;
                
                bool ComparePositionTiming(Trail t)
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    return startTime == t.endTime &&
                           startCoordinate == t.endCoordinate &&
                           trailContext is RealTrail &&
                           t.trailContext is RealTrail;
                }
            }
        }

        private void Update()
        {
            if(NoteRenderer.CurrentTime * 1000 > endTime + 120 && NoteRenderer.isPlaying)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (trailContext is RealTrail) trailHint.OnParentDestroy.Invoke();
        }
    }

    internal abstract class TrailState
    {
        internal abstract void UpdateMaterial(MeshRenderer mr, GameObject gb, Direction dir);
    }

    internal class Tracking : TrailState
    {
        internal override void UpdateMaterial(MeshRenderer mr, GameObject gb, Direction dir)
        {
            mr.material = dir switch
            {
                Direction.Left => UIElementReference.Instance.leftTrailHit,
                Direction.Right => UIElementReference.Instance.rightTrailHit,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir,
                    $"Given direction is not valid for Trail type: {dir}")
            };
        }
    }
    
    internal class Idle : TrailState
    {
        internal override void UpdateMaterial(MeshRenderer mr, GameObject gb, Direction dir)
        {
            mr.material = dir switch
            {
                Direction.Left => UIElementReference.Instance.leftTrail,
                Direction.Right => UIElementReference.Instance.rightTrail,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir,
                    $"Given direction is not valid for Trail type: {dir}")
            };
        }
    }

    internal class WrongHand : TrailState
    {
        internal override void UpdateMaterial(MeshRenderer mr, GameObject gb, Direction dir)
        {
            var o = mr.material.color;

            LeanTween.value(gb, f =>
                {
                    mr.material.color = Color.Lerp(o, new Color(1, .1f, .1f, .75f), f);
                }, 1, 0, .6f);
        }
    }
}