using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Splines;
using TouchPhase = UnityEngine.TouchPhase;

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
        private Spline spline;

        private List<GameObject> comboPoint;

        public bool isTracking;

        private SphereCollider _col;
        
        private Sprite HitRing1 => Resources.Load<Sprite>("Sprites/HitRing1");
        private Sprite HitRing2 => Resources.Load<Sprite>("Sprites/HitRing2");

        private void Start()
        {
            _col = gameObject.GetComponent<SphereCollider>();
            _col.radius = 160;
            StartCoroutine(UpdateCollider());
        }

        private IEnumerator UpdateCollider()
        {
            if (_virtualTrail)
            {
                _col.enabled = false;
                yield break;
            }
            while (spline is not null)
            {
                var t = Mathf.InverseLerp(_startTime, _endTime, NoteRenderer.currentTime * 1000);
                yield return _col.center = spline.EvaluatePosition(Mathf.Clamp01(t));
            }
        }
        
        public void Initialize(float startTime, float endTime, Vector2 startCoordinate, Vector2 endCoordinate, Direction direction, EasingMode easing, float easingRatio, bool newTrail, bool virtualTrail)
        {
            _startTime = startTime;
            _endTime = endTime;
            _virtualTrail = virtualTrail;
            
            #region Convert coordinates
            startCoordinate = new Vector2(startCoordinate.x * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.width * 0.5f, startCoordinate.y * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.height * 0.5f);
            endCoordinate = new Vector2(endCoordinate.x * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.width * 0.5f, endCoordinate.y * NoteRenderer.Instance.gameArea.GetComponent<RectTransform>().rect.height * 0.5f);
            
            var startPosition = new Vector3(startCoordinate.x, startCoordinate.y, startTime * NoteRenderer.Instance.chartSpeedModifier);
            var endPosition = new Vector3(endCoordinate.x, endCoordinate.y, endTime * NoteRenderer.Instance.chartSpeedModifier);
            #endregion
            
            #region GameObject setup

            
            gameObject.AddComponent<Trail>();
            #endregion
            
            #region Path setup
            spline = gameObject.AddComponent<SplineContainer>().Spline;
            
            // Create curve with respect to easing and easing ratio
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
            #endregion
            
            #region Create subsegments
            gameObject.AddComponent<MeshFilter>();
            if (!virtualTrail)
            {
                gameObject.AddComponent<MeshRenderer>().material = direction switch
                {
                    Direction.Left => NoteRenderer.Instance.left,
                    Direction.Right => NoteRenderer.Instance.right,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                };
            }
            else
            {
                gameObject.AddComponent<MeshRenderer>().material = NoteRenderer.Instance.none;
            }
            
            gameObject.GetComponent<MeshFilter>().mesh = new Mesh();
            
            if (newTrail)
                BuildHead(startCoordinate, startTime * NoteRenderer.Instance.chartSpeedModifier, direction, virtualTrail);
            
            // Build subsegments
            for(var i = 0f; i < 1; i += (200 / (spline.ElementAt(1).Position.z - spline.ElementAt(0).Position.z)))
            {
                BuildSubsegment((Vector3)spline.EvaluatePosition(i), spline.EvaluatePosition(i).z, direction, virtualTrail);
            }
            BuildSubsegment((Vector3)spline.EvaluatePosition(1), spline.ElementAt(1).Position.z, direction, virtualTrail);
            
            // Build critical combo points based on bpm
            if (!virtualTrail)
            {
                var bpm = NoteRenderer.Instance.currentBpm;
                for (var i = 0f; i < 1; i += bpm / (spline.ElementAt(1).Position.z - spline.ElementAt(0).Position.z))
                {
                    BuildCombo(out var gb, (Vector3)spline.EvaluatePosition(i), spline.EvaluatePosition(i).z);
                }
            }
            
            #endregion
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
        
        private void BuildHead(Vector2 coordinate, float time, Direction direction, bool virtualTrail)
        {
            #region Set up local variables
            var meshFilter = gameObject.GetComponent<MeshFilter>();
            #endregion

            #region Set up mesh
                #region Determine head shape based on trail type
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
            #endregion

                #region Set up triangles
            var triangles = new List<int>
            {
                0, 1, 2, 
                0, 2, 3, 
                0, 3, 4,
                0, 4, 1
            };
            #endregion

                #region Set up UVs
            var uv = new List<Vector2>
            {
                new(0.5f, 0.5f),
                new(0.5f, 0.5f),
                new(0.5f, 0.5f),
                new(0.5f, 0.5f),
                new(0.5f, 0.5f)
            };
            #endregion
            
                #region Apply attributes to mesh
            var mesh1 = new Mesh
            {
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray(),
                uv = uv.ToArray()
            };
            #endregion
            #endregion
            
            meshFilter.mesh = mesh1;
        }
        
        private void BuildSubsegment(Vector2 coordinate, float time, Direction direction, bool virtualTrail)
        {
            Vector3 position = new Vector3(coordinate.x, coordinate.y, time);
            
            #region Concatinate new vertices

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
            gameObject.GetComponent<MeshFilter>().mesh.vertices = gameObject.GetComponent<MeshFilter>().mesh.vertices.Concat(vertices).ToArray();
            #endregion

            #region Concatinate new triangles
            var i = gameObject.GetComponent<MeshFilter>().mesh.vertices.Length;
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
            gameObject.GetComponent<MeshFilter>().mesh.triangles = gameObject.GetComponent<MeshFilter>().mesh.triangles.Concat(triangles).ToArray();
            #endregion
            
            #region Concatinate new UVs
            var uv = new List<Vector2>
            {
                new(1f, 1f),
                new(1f, -1f),
                new(-1f, -1f),
                new(-1f, 1f)
            };
            gameObject.GetComponent<MeshFilter>().mesh.uv = gameObject.GetComponent<MeshFilter>().mesh.uv.Concat(uv).ToArray();
            #endregion
        }
    }
}