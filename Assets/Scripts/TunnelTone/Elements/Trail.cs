using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelTone.PlayArea;
using TunnelTone.UI.Reference;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using Object = UnityEngine.Object;

namespace TunnelTone.Elements
{
    [RequireComponent(typeof(SphereCollider))]
    public partial class Trail : MonoBehaviour
    {
        // Basic trail properties
        internal float startTime, endTime;
        internal Vector2 startCoordinate, endCoordinate;
        private bool virtualTrail;
        internal Direction Direction { get; private set; }
        internal TrailType trailContext;
        
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
        private List<GameObject> comboPoint;
        internal GameObject trackingTouch;
        private List<TrailSubsegment> subsegments;
        internal TrailHint trailHint;
        [SerializeField] internal GameObject trailHintObject;
        
        // Objects
        [SerializeField] internal GameObject trailSubsegmentPrefab;
        [SerializeField] internal Material virtualTrailMaterial;
        [SerializeField] private Sprite outerRing;
        [SerializeField] private Sprite innerRing;
        
        internal TrailState state;
        internal UnityEvent OnStateChanged = new();

        private void Start()
        {
            OnStateChanged.AddListener(() => Debug.Log(state));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Touch")) return;
            if ((other.gameObject.GetComponent<PlayArea.Touch>().direction == Direction.Any && allowTrack) || other.gameObject.GetComponent<PlayArea.Touch>().direction == Direction)
            {
                allowTrack = true;
                isTracking = true;
                trackingTouch = other.gameObject;
                state = new Tracking();
                OnStateChanged.Invoke();
                return;
            }
            isTracking = false;
            allowTrack = false;
            state = new WrongHand();
            OnStateChanged.Invoke();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Touch"))
            {
                isTracking = false;
                state = new Idle();
                OnStateChanged.Invoke();
            }
        }
        
        public Trail Initialize(float startTime, float endTime, Vector2 startCoordinate, Vector2 endCoordinate, Direction direction, EasingMode easing, float easingRatio, bool newTrail, bool virtualTrail)
        {
            meshFilter.mesh = new Mesh();
            
            Direction = direction;
            allowTrack = true;
            
            this.startTime = startTime;
            this.endTime = endTime;
            this.startCoordinate = startCoordinate;
            this.endCoordinate = endCoordinate;
            trailContext = virtualTrail ? new VirtualTrail(this) : new RealTrail(this);
            
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

            trailContext.Initialize();
            trailContext.ConfigureCollider();

            // if (!virtualTrail)
            // {
            //     meshRenderer.material = direction switch
            //     {
            //         Direction.Left => UIElementReference.Instance.leftTrail,
            //         Direction.Right => UIElementReference.Instance.rightTrail,
            //         _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, $"Given direction is not valid for Trail type: {direction}")
            //     };
            //     trailHint = Instantiate(trailHintObject, GameObject.Find("Hit Zone").transform).GetComponent<TrailHint>();
            //     trailHint.transform.localPosition = new Vector3(startPosition.x, startPosition.y, 0);
            //     StartCoroutine(TrailHint());
            // }
            // else
            // {
            //     gameObject.layer = 20;
            //     meshRenderer.material = NoteRenderer.Instance.none;
            // }
            //
            // // Build critical combo points based on bpm
            // if (!virtualTrail)
            // {
            //     var bpm = NoteRenderer.Instance.currentBpm;
            //     if (newTrail)
            //         BuildHead(spline.EvaluatePosition(0), startTime * NoteRenderer.Instance.chartSpeedModifier, false);
            //
            //     var density = 60 / (bpm * 2) * NoteRenderer.Instance.chartSpeedModifier;
            //     for (var i = 0f; i < 1; i += density * 1000 / (spline.ElementAt(1).Position.z - spline.ElementAt(0).Position.z))
            //     {
            //         BuildCombo(out var gb, (Vector3)spline.EvaluatePosition(i),
            //             (spline.EvaluatePosition(i).z - NoteRenderer.Instance.universalOffset) / NoteRenderer.Instance.chartSpeedModifier);
            //     }
            //     // Build subsegments
            //     float tail = 0;
            //     for(var i = 0f; i < 1; i += 1000 / (spline.ElementAt(1).Position.z - spline.ElementAt(0).Position.z))
            //     {
            //         Instantiate(trailSubsegmentPrefab, transform).GetComponent<TrailSubsegment>().Initialize(this, spline, 
            //             (Vector3)spline.EvaluatePosition(tail), 
            //             (Vector3)spline.EvaluatePosition(i), 
            //             Mathf.Lerp(startTime, endTime, Mathf.InverseLerp(spline.EvaluatePosition(0).z, spline.EvaluatePosition(1).z, spline.EvaluatePosition(tail).z)), 
            //             Mathf.Lerp(startTime, endTime, Mathf.InverseLerp(spline.EvaluatePosition(0).z, spline.EvaluatePosition(1).z, spline.EvaluatePosition(i).z))
            //         );
            //         tail = i;
            //     }
            //     Instantiate(trailSubsegmentPrefab, transform).GetComponent<TrailSubsegment>().Initialize(this, spline,
            //         (Vector3)spline.EvaluatePosition(tail), 
            //         (Vector3)spline.EvaluatePosition(1), 
            //         spline.EvaluatePosition(tail).z, 
            //         spline.EvaluatePosition(1).z
            //     );
            // }
            // else
            // {
            //     var j = 0;
            //     lineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, .5f));
            //     for (var i = 0f; i < 1; i += 80 / (spline.ElementAt(1).Position.z - spline.ElementAt(0).Position.z))
            //     {
            //         lineRenderer.SetPosition(j, spline.EvaluatePosition(i));
            //         lineRenderer.positionCount++;
            //         j++;
            //     }
            //     lineRenderer.SetPosition(j, spline.EvaluatePosition(1));
            //     lineRenderer.positionCount--;
            //     lineRenderer.material = virtualTrailMaterial;
            // }

            return this;
        }

        internal void BuildCombo(out GameObject gb, Vector2 coordinate, float time)
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
            ScoreManager.totalCombo++;
        }

        internal void BuildHead(Vector3 position, float time, bool virtualTrail)
        {

            var vertices = trailContext.GetVertices(position, time);

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

        internal IEnumerator TrailHint()
        {
            yield return new WaitUntil(() => NoteRenderer.CurrentTime * 1000 >= startTime - 550 && NoteRenderer.isPlaying);
            trailHint.Enable(Direction);
        }

        private void Update()
        {
            if(NoteRenderer.CurrentTime * 1000 > endTime + 120 && NoteRenderer.isPlaying)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (trailContext is RealTrail) trailHint.OnParentDestroy.Invoke();
            if (trackingTouch is null) return;
            var touch = trackingTouch.GetComponent<PlayArea.Touch>();
            touch.trackingTrail = null;
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