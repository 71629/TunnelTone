using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TunnelTone.PlayArea;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace TunnelTone.Elements
{
    [RequireComponent(typeof(SphereCollider))]
    public class Trail : MonoBehaviour
    {
        // Basic trail properties
        private float startTime, endTime;
        private bool virtualTrail;
        internal Direction Direction { get; private set; }
        
        // Trail References
        private Trail next;
        
        // Status Reference
        private bool isHit;
        internal bool isTracking;
        
        // Coupled Components
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] private SphereCollider col;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshFilter meshFilter;
        private Spline spline;
        private List<GameObject> comboPoint;
        internal GameObject trackingTouch;
        private List<TrailSubsegment> subsegments;
        
        // Objects
        [SerializeField] private GameObject trailSubsegmentPrefab;
        
        private Sprite HitRing1 => Resources.Load<Sprite>("Sprites/HitRing1");
        private Sprite HitRing2 => Resources.Load<Sprite>("Sprites/HitRing2");

        private void Start()
        {
            StartCoroutine(UpdateCollider());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Touch") && other.gameObject.GetComponent<PlayArea.Touch>().direction == Direction.Any || other.gameObject.GetComponent<PlayArea.Touch>().direction == Direction)
            {
                isTracking = true;
                trackingTouch = other.gameObject;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Touch"))
            {
                isTracking = false;
            }
        }

        private IEnumerator UpdateCollider()
        {
            if (virtualTrail)
            {
                col.enabled = false;
                yield break;
            }
            while (spline is not null)
            {
                var t = Mathf.InverseLerp(startTime, endTime, NoteRenderer.currentTime * 1000);
                yield return col.center = spline.EvaluatePosition(Mathf.Clamp01(t));
            }
        }
        
        public void Initialize(float startTime, float endTime, Vector2 startCoordinate, Vector2 endCoordinate, Direction direction, EasingMode easing, float easingRatio, bool newTrail, bool virtualTrail)
        {
            Direction = direction;
            
            this.startTime = startTime;
            this.endTime = endTime;
            this.virtualTrail = virtualTrail;
            
            startCoordinate = new Vector2(startCoordinate.x * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.width * 0.5f, startCoordinate.y * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.height * 0.5f);
            endCoordinate = new Vector2(endCoordinate.x * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.width * 0.5f, endCoordinate.y * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.height * 0.5f);
            
            var startPosition = new Vector3(startCoordinate.x, startCoordinate.y, startTime * NoteRenderer.Instance.chartSpeedModifier);
            var endPosition = new Vector3(endCoordinate.x, endCoordinate.y, endTime * NoteRenderer.Instance.chartSpeedModifier);

            spline = splineContainer.Spline;
            
            // Create curve with respect previewDuration easing and easing ratio
            switch (easing)
            {
                case EasingMode.Straight:
                    spline.Insert(0, new BezierKnot(startPosition, 0, 0, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, 0, 0, quaternion.identity));
                    break;
                case EasingMode.EaseIn:
                    spline.Insert(0, new BezierKnot(startPosition, 0, 0, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, 0, -Mathf.Pow(easingRatio, 2) * (endTime - startTime)) * NoteRenderer.Instance.chartSpeedModifier, 0, quaternion.identity));
                    break;
                case EasingMode.EaseOut:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, 0, Mathf.Pow(easingRatio, 2) * (endTime - startTime)) * NoteRenderer.Instance.chartSpeedModifier, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, 0, 0, quaternion.identity));
                    break;
                case EasingMode.Bezier:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, 0, Mathf.Pow(easingRatio, 2) * (endTime - startTime)) * NoteRenderer.Instance.chartSpeedModifier, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, 0, -Mathf.Pow(easingRatio, 2) * (endTime - startTime)) * NoteRenderer.Instance.chartSpeedModifier, 0, quaternion.identity));
                    break;
                case EasingMode.HorizontalInVerticalOut:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x * NoteRenderer.Instance.chartSpeedModifier, 0, Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x)) * NoteRenderer.Instance.chartSpeedModifier, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, -Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y * NoteRenderer.Instance.chartSpeedModifier, -Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y)) * NoteRenderer.Instance.chartSpeedModifier, 0, quaternion.identity));
                    break;
                case EasingMode.VerticalInHorizontalOut:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y * NoteRenderer.Instance.chartSpeedModifier, Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y)) * NoteRenderer.Instance.chartSpeedModifier, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(-Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x * NoteRenderer.Instance.chartSpeedModifier, 0, -Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x)) * NoteRenderer.Instance.chartSpeedModifier, 0, quaternion.identity));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(easing), easing, null);
            }

            if (!virtualTrail)
            {
                meshRenderer.material = direction switch
                {
                    Direction.Left => NoteRenderer.Instance.left,
                    Direction.Right => NoteRenderer.Instance.right,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, $"Given direction is not valid for Trail type: {direction}")
                };
            }
            else
            {
                meshRenderer.material = NoteRenderer.Instance.none;
            }
            
            meshFilter.mesh = new Mesh();
            
            // Build critical combo points based on bpm
            if (!virtualTrail)
            {
                var bpm = NoteRenderer.Instance.currentBpm;
                if (newTrail)
                    BuildHead(startCoordinate, startTime * NoteRenderer.Instance.chartSpeedModifier, false);
                for (var i = 0f; i < 1; i += bpm * 8 / (spline.ElementAt(1).Position.z - spline.ElementAt(0).Position.z))
                {
                    BuildCombo(out var gb, (Vector3)spline.EvaluatePosition(i), spline.EvaluatePosition(i).z / NoteRenderer.Instance.chartSpeedModifier);
                }
                // Build subsegments
                float tail = 0;
                for(var i = 0f; i < 1; i += 1000 / (spline.ElementAt(1).Position.z - spline.ElementAt(0).Position.z))
                {
                    Instantiate(trailSubsegmentPrefab, transform).GetComponent<TrailSubsegment>().Initialize(this, spline, 
                        (Vector3)spline.EvaluatePosition(tail), 
                        (Vector3)spline.EvaluatePosition(i), 
                        Mathf.Lerp(startTime, endTime, Mathf.InverseLerp(spline.EvaluatePosition(0).z, spline.EvaluatePosition(1).z, spline.EvaluatePosition(tail).z)), 
                        Mathf.Lerp(startTime, endTime, Mathf.InverseLerp(spline.EvaluatePosition(0).z, spline.EvaluatePosition(1).z, spline.EvaluatePosition(i).z))
                    );
                    tail = i;
                }
                Instantiate(trailSubsegmentPrefab, transform).GetComponent<TrailSubsegment>().Initialize(this, spline,
                    (Vector3)spline.EvaluatePosition(tail), 
                    (Vector3)spline.EvaluatePosition(1), 
                    spline.EvaluatePosition(tail).z, 
                    spline.EvaluatePosition(1).z
                );
            }
            else
            {
                var j = 0;
                lineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, .5f));
                for (var i = 0f; i < 1; i += 80 / (spline.ElementAt(1).Position.z - spline.ElementAt(0).Position.z))
                {
                    lineRenderer.SetPosition(j, spline.EvaluatePosition(i));
                    lineRenderer.positionCount++;
                    j++;
                }
                lineRenderer.SetPosition(j, spline.EvaluatePosition(1));
                lineRenderer.positionCount--;
            }
        }

        private void BuildCombo(out GameObject gb, Vector2 coordinate, float time)
        {
            gb = new GameObject("Combo")
            {
                transform =
                {
                    parent = transform,
                    localPosition = new Vector3
                    {
                        x = coordinate.x,
                        y = coordinate.y,
                        z = time
                    },
                    rotation = Quaternion.identity,
                    localScale = Vector3.one
                }
            };
            gb.AddComponent<ComboPoint>().time = time;
            ScoreManager.Instance.totalCombo++;
        }
        
        private void BuildHead(Vector2 coordinate, float time, bool virtualTrail)
        {

            var vertices = !virtualTrail
                ? new List<Vector3>
                {
                    (Vector3)coordinate + new Vector3(0, 0, -25 + time),
                    (Vector3)coordinate + new Vector3(0, 40, 0 + time),
                    (Vector3)coordinate + new Vector3(40, 0, 0 + time),
                    (Vector3)coordinate + new Vector3(0, -40, 0 + time),
                    (Vector3)coordinate + new Vector3(-40, 0, 0 + time)
                }
                : new List<Vector3>
                {
                    (Vector3)coordinate + new Vector3(0, 0, -8 + time),
                    (Vector3)coordinate + new Vector3(0, 8, 0 + time),
                    (Vector3)coordinate + new Vector3(8, 0, 0 + time),
                    (Vector3)coordinate + new Vector3(0, -8, 0 + time),
                    (Vector3)coordinate + new Vector3(-8, 0, 0 + time)
                };

            var triangles = new List<int>
            {
                0, 1, 2, 
                0, 2, 3, 
                0, 3, 4,
                0, 4, 1
            };

            var uv = new List<Vector2>
            {
                new(0.5f, 0.5f),
                new(0.5f, 0.5f),
                new(0.5f, 0.5f),
                new(0.5f, 0.5f),
                new(0.5f, 0.5f)
            };

            var mesh1 = new Mesh
            {
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray(),
                uv = uv.ToArray()
            };

            meshFilter.mesh = mesh1;
        }

        private void Update()
        {
            if(NoteRenderer.currentTime * 1000 > endTime + 120 && NoteRenderer.IsPlaying)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (trackingTouch is null || virtualTrail) return;
            var touch = trackingTouch.GetComponent<PlayArea.Touch>();
            touch.trackingTrail = null;
            touch.direction = Direction.Any;
        }
    }
}