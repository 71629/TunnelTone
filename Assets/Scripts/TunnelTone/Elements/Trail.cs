#define USE_LINE_RENDERER
// #define USE_MESH

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Splines;

namespace TunnelTone.Elements
{
    [RequireComponent(typeof(SphereCollider))]
    public class Trail : MonoBehaviour
    {
        private float _startTime, _endTime;
        private bool _virtualTrail;
        
        private Trail _next;
        private bool _isHit;
        private TouchControl _trackingTouch;
        private Spline _spline;

        private List<GameObject> _comboPoint;

        public bool isTracking;

        private SphereCollider _col;
        private SplineContainer _splineContainer;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private LineRenderer LineRenderer => GetComponent<LineRenderer>();
        
        private Sprite HitRing1 => Resources.Load<Sprite>("Sprites/HitRing1");
        private Sprite HitRing2 => Resources.Load<Sprite>("Sprites/HitRing2");

        private void Start()
        {
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _splineContainer = gameObject.AddComponent<SplineContainer>();
            _col = gameObject.GetComponent<SphereCollider>();
            _col.radius = 160;
            StartCoroutine(UpdateCollider());
            LineRenderer.useWorldSpace = false;
        }

        private IEnumerator UpdateCollider()
        {
            if (_virtualTrail)
            {
                _col.enabled = false;
                yield break;
            }
            while (_spline is not null)
            {
                var t = Mathf.InverseLerp(_startTime, _endTime, NoteRenderer.currentTime * 1000);
                yield return _col.center = _spline.EvaluatePosition(Mathf.Clamp01(t));
            }
        }
        
        public void Initialize(float startTime, float endTime, Vector2 startCoordinate, Vector2 endCoordinate, Direction direction, EasingMode easing, float easingRatio, bool newTrail, bool virtualTrail)
        {
            _startTime = startTime;
            _endTime = endTime;
            _virtualTrail = virtualTrail;

            startCoordinate = new Vector2(startCoordinate.x * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.width * 0.5f, startCoordinate.y * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.height * 0.5f);
            endCoordinate = new Vector2(endCoordinate.x * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.width * 0.5f, endCoordinate.y * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.height * 0.5f);
            
            var startPosition = new Vector3(startCoordinate.x, startCoordinate.y, startTime * NoteRenderer.Instance.chartSpeedModifier);
            var endPosition = new Vector3(endCoordinate.x, endCoordinate.y, endTime * NoteRenderer.Instance.chartSpeedModifier);

            _spline = _splineContainer.Spline;
            
            // Create curve with respect to easing and easing ratio
            switch (easing)
            {
                case EasingMode.Straight:
                    _spline.Insert(0, new BezierKnot(startPosition, 0, 0, quaternion.identity));
                    _spline.Insert(1, new BezierKnot(endPosition, 0, 0, quaternion.identity));
                    break;
                case EasingMode.EaseIn:
                    _spline.Insert(0, new BezierKnot(startPosition, 0, 0, quaternion.identity));
                    _spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, 0, -Mathf.Pow(easingRatio, 2) * (endTime - startTime)) * NoteRenderer.Instance.chartSpeedModifier, 0, quaternion.identity));
                    break;
                case EasingMode.EaseOut:
                    _spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, 0, Mathf.Pow(easingRatio, 2) * (endTime - startTime)) * NoteRenderer.Instance.chartSpeedModifier, quaternion.identity));
                    _spline.Insert(1, new BezierKnot(endPosition, 0, 0, quaternion.identity));
                    break;
                case EasingMode.Bezier:
                    _spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, 0, Mathf.Pow(easingRatio, 2) * (endTime - startTime)) * NoteRenderer.Instance.chartSpeedModifier, quaternion.identity));
                    _spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, 0, -Mathf.Pow(easingRatio, 2) * (endTime - startTime)) * NoteRenderer.Instance.chartSpeedModifier, 0, quaternion.identity));
                    break;
                case EasingMode.HorizontalInVerticalOut:
                    _spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x * NoteRenderer.Instance.chartSpeedModifier, 0, Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x)) * NoteRenderer.Instance.chartSpeedModifier, quaternion.identity));
                    _spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, -Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y * NoteRenderer.Instance.chartSpeedModifier, -Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y)) * NoteRenderer.Instance.chartSpeedModifier, 0, quaternion.identity));
                    break;
                case EasingMode.VerticalInHorizontalOut:
                    _spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y * NoteRenderer.Instance.chartSpeedModifier, Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y)) * NoteRenderer.Instance.chartSpeedModifier, quaternion.identity));
                    _spline.Insert(1, new BezierKnot(endPosition, new Vector3(-Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x * NoteRenderer.Instance.chartSpeedModifier, 0, -Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x)) * NoteRenderer.Instance.chartSpeedModifier, 0, quaternion.identity));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(easing), easing, null);
            }

            if (!virtualTrail)
            {
                _meshRenderer.material = direction switch
                {
                    Direction.Left => NoteRenderer.Instance.left,
                    Direction.Right => NoteRenderer.Instance.right,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                };
            }
            else
            {
                _meshRenderer.material = NoteRenderer.Instance.none;
            }
            
            _meshFilter.mesh = new Mesh();
            
#if USE_MESH // Keeping the code in case if needed
            if (newTrail)
                BuildHead(startCoordinate, startTime * NoteRenderer.Instance.chartSpeedModifier, direction, virtualTrail);
            
            // Build subsegments
            for(var i = 0f; i < 1; i += 200 / (spline.ElementAt(1).Position.z - spline.ElementAt(0).Position.z))
            {
                BuildSubsegment((Vector3)spline.EvaluatePosition(i), spline.EvaluatePosition(i).z, direction, virtualTrail);
            }
            BuildSubsegment((Vector3)spline.EvaluatePosition(1), spline.ElementAt(1).Position.z, direction, virtualTrail);
#endif

#if USE_LINE_RENDERER
            // Build critical combo points based on bpm
            if (!virtualTrail)
            {
                var bpm = NoteRenderer.Instance.currentBpm;
                if (newTrail)
                    BuildHead(startCoordinate, startTime * NoteRenderer.Instance.chartSpeedModifier, false);
                for (var i = 0f; i < 1; i += bpm / (_spline.ElementAt(1).Position.z - _spline.ElementAt(0).Position.z))
                {
                    BuildCombo(out var gb, (Vector3)_spline.EvaluatePosition(i), _spline.EvaluatePosition(i).z);
                }
                // Build subsegments
                for(var i = 0f; i < 1; i += 200 / (_spline.ElementAt(1).Position.z - _spline.ElementAt(0).Position.z))
                {
                    BuildSubsegment((Vector3)_spline.EvaluatePosition(i), _spline.EvaluatePosition(i).z, false);
                }
                BuildSubsegment((Vector3)_spline.EvaluatePosition(1), _spline.ElementAt(1).Position.z, false);
            }
            else
            {
                var j = 0;
                LineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, .5f));
                for (var i = 0f; i < 1; i += 200 / (_spline.ElementAt(1).Position.z - _spline.ElementAt(0).Position.z))
                {
                    LineRenderer.SetPosition(j, _spline.EvaluatePosition(i));
                    LineRenderer.positionCount++;
                    j++;
                }
                LineRenderer.SetPosition(j, _spline.EvaluatePosition(1));
                LineRenderer.positionCount--;
            }
#endif
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
            gb.AddComponent<SphereCollider>().radius = 0;
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
                : new List<Vector3>()
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

            _meshFilter.mesh = mesh1;
        }

        private void BuildSubsegment(Vector2 coordinate, float time, bool virtualTrail)
        {
            var position = new Vector3(coordinate.x, coordinate.y, time);
            var mesh = _meshFilter.mesh;

            // Concatenate new vertices
            var vertices = !virtualTrail
                ? new[]
                {
                    position + new Vector3(0, 40, 0),
                    position + new Vector3(40, 0, 0),
                    position + new Vector3(0, -40, 0),
                    position + new Vector3(-40, 0, 0)
                }
                : new[]
                {
                    position + new Vector3(0, 8, 0),
                    position + new Vector3(8, 0, 0),
                    position + new Vector3(0, -8, 0),
                    position + new Vector3(-8, 0, 0)
                };
            mesh.vertices = _meshFilter.mesh.vertices.Concat(vertices).ToArray();

            // Concatenate new triangles
            var i = mesh.vertices.Length;
            var triangles = new[]
            {
                i - 8, i - 4, i - 3,
                i - 8, i - 3, i - 7,
                i - 7, i - 3, i - 2,
                i - 7, i - 2, i - 6,
                i - 6, i - 2, i - 1,
                i - 6, i - 1, i - 5,
                i - 5, i - 1, i - 4,
                i - 5, i - 4, i - 8
            };

            // Concatenate new UVs
            var uv = new List<Vector2>
            {
                new Vector2(1f, 1f),
                new Vector2(1f, 0),
                new Vector2(0, 0),
                new Vector2(0, 1f)
            };

            // Repeat the UVs for each vertex in the mesh
            var totalVertices = mesh.vertexCount + vertices.Length;
            uv = Enumerable.Repeat(uv, totalVertices / uv.Count).SelectMany(x => x).ToList();

            // Set the vertices, triangles, and UVs in the correct order
            mesh.triangles = _meshFilter.mesh.triangles.Concat(triangles).ToArray();
            mesh.uv = uv.ToArray();

            // Recalculate bounds and normals
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
    }
}