using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

// TODO: Refactor

namespace TunnelTone.Elements
{
    public class NoteRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject gameArea;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Material left, right, none;
        private Transform _transform;

        public float chartSpeedModifier;

        public float offsetTime;
        public const float StartDelay = 2500f;
        
        private float _dspSongStartTime, _dspSongEndTime;

        private void Start()
        {
            _transform = GetComponent<Transform>();
            _transform.localPosition = Vector3.zero;
            audioSource.PlayDelayed(StartDelay / 1000f);
            
            // Set up song start and end times
            _dspSongStartTime = (float)AudioSettings.dspTime + StartDelay / 1000f;
            _dspSongEndTime = (float)AudioSettings.dspTime + audioSource.clip.length * 1000;
            Debug.Log($"Start time: {_dspSongStartTime}\nEnd time: {_dspSongEndTime}");
        }

        private void Update()
        {
            _transform.localPosition = new Vector3(0, 0, chartSpeedModifier * (-1000 * ((float)AudioSettings.dspTime - _dspSongStartTime) + offsetTime + StartDelay));
        }

        public void BuildTrail(out GameObject gb, float startTime, float endTime, Vector2 startCoordinate, Vector2 endCoordinate, Direction direction, EasingMode easing, float easingRatio, bool newTrail, bool tapEmbedded)
        {
            #region Convert coordinates
            startCoordinate = new Vector2(startCoordinate.x * gameArea.GetComponent<RectTransform>().rect.width * 0.5f, startCoordinate.y * gameArea.GetComponent<RectTransform>().rect.height * 0.5f);
            endCoordinate = new Vector2(endCoordinate.x * gameArea.GetComponent<RectTransform>().rect.width * 0.5f, endCoordinate.y * gameArea.GetComponent<RectTransform>().rect.height * 0.5f);
            
            var startPosition = new Vector3(startCoordinate.x, startCoordinate.y, startTime * chartSpeedModifier);
            var endPosition = new Vector3(endCoordinate.x, endCoordinate.y, endTime * chartSpeedModifier);
            #endregion
            
            #region GameObject setup
            gb = new GameObject("Trail")
            {
                transform =
                {
                    parent = this.transform,
                    position = Vector3.zero,
                    rotation = Quaternion.identity,
                    localScale = Vector3.one
                }
            };
            #endregion
            
            #region Path setup
            var spline = gb.AddComponent<SplineContainer>().Spline;
            switch (easing)
            {
                case EasingMode.Straight:
                    spline.Insert(0, new BezierKnot(startPosition, 0, 0, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, 0, 0, quaternion.identity));
                    break;
                case EasingMode.EaseIn:
                    spline.Insert(0, new BezierKnot(startPosition, 0, 0, quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, 0, -Mathf.Pow(easingRatio, 2) * (endTime - startTime)), 0, quaternion.identity));
                    break;
                case EasingMode.EaseOut:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, 0, Mathf.Pow(easingRatio, 2) * (endTime - startTime)), quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, 0, 0, quaternion.identity));
                    break;
                case EasingMode.EaseInOut:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, 0, Mathf.Pow(easingRatio, 2) * (endTime - startTime)), quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, 0, -Mathf.Pow(easingRatio, 2) * (endTime - startTime)), 0, quaternion.identity));
                    break;
                case EasingMode.HorizontalInVerticalOut:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x, 0, Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x)), quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(0, -Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y, -Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y)), 0, quaternion.identity));
                    break;
                case EasingMode.VerticalInHorizontalOut:
                    spline.Insert(0, new BezierKnot(startPosition, 0, new Vector3(0, Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y, Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).y)), quaternion.identity));
                    spline.Insert(1, new BezierKnot(endPosition, new Vector3(-Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x, 0, -Mathf.Abs(Mathf.Pow(easingRatio, 2) * (endPosition - startPosition).x)), 0, quaternion.identity));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(easing), easing, null);
            }
            #endregion
            
            #region Create subsegments
            gb.AddComponent<MeshFilter>();
                        gb.AddComponent<MeshRenderer>().material = direction switch
                        {
                            Direction.Left => left,
                            Direction.Right => right,
                            Direction.None => none,
                            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                        };
                        gb.GetComponent<MeshFilter>().mesh = new Mesh();
            
            if (newTrail)
                BuildHead(ref gb, startCoordinate, startTime * chartSpeedModifier, direction);
            
            for(var i = 0f; i < 1; i += (10 / (spline.ElementAt(1).Position.z - spline.ElementAt(0).Position.z)))
            {
                BuildSubsegment(ref gb, (Vector3)spline.EvaluatePosition(i), spline.EvaluatePosition(i).z, direction);
            }
            BuildSubsegment(ref gb, (Vector3)spline.EvaluatePosition(1), spline.ElementAt(1).Position.z, direction);
            #endregion
        }

        private void BuildHead(ref GameObject gb, Vector2 coordinate, float time, Direction direction)
        {
            #region Set up local variables
            MeshFilter meshFilter = gb.GetComponent<MeshFilter>();
            List<Vector3> vertices;
            #endregion

            #region Set up mesh
                #region Determine head shape based on direction
            vertices = direction != Direction.None
                ? new List<Vector3>
                {
                    (Vector3)(coordinate) + new Vector3(0, 0, -15 + time),
                    (Vector3)(coordinate) + new Vector3(0, 25, 0 + time),
                    (Vector3)(coordinate) + new Vector3(25, 0, 0 + time),
                    (Vector3)(coordinate) + new Vector3(0, -25, 0 + time),
                    (Vector3)(coordinate) + new Vector3(-25, 0, 0 + time)
                }
                : new List<Vector3>()
                {
                    (Vector3)(coordinate) + new Vector3(0, 0, -8 + time),
                    (Vector3)(coordinate) + new Vector3(0, 8, 0 + time),
                    (Vector3)(coordinate) + new Vector3(8, 0, 0 + time),
                    (Vector3)(coordinate) + new Vector3(0, -8, 0 + time),
                    (Vector3)(coordinate) + new Vector3(-8, 0, 0 + time)
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
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f)
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

        private void BuildSubsegment(ref GameObject gb, Vector2 coordinate, float time, Direction direction)
        {
            Vector3 position = new Vector3(coordinate.x, coordinate.y, time);
            
            #region Concatinate new vertices
            Vector3[] vertices;
            vertices = direction != Direction.None
                ? new Vector3[]
                {
                    position + new Vector3(0, 25, 0),
                    position + new Vector3(25, 0, 0),
                    position + new Vector3(0, -25, 0),
                    position + new Vector3(-25, 0, 0)
                }
                : new Vector3[]
                {
                    position + new Vector3(0, 8, 0),
                    position + new Vector3(8, 0, 0),
                    position + new Vector3(0, -8, 0),
                    position + new Vector3(-8, 0, 0)
                };
            gb.GetComponent<MeshFilter>().mesh.vertices = gb.GetComponent<MeshFilter>().mesh.vertices.Concat(vertices).ToArray();
            #endregion

            #region Concatinate new triangles
            var i = gb.GetComponent<MeshFilter>().mesh.vertices.Length;
            var triangles = new int[]
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
            gb.GetComponent<MeshFilter>().mesh.triangles = gb.GetComponent<MeshFilter>().mesh.triangles.Concat(triangles).ToArray();
            #endregion
            
            #region Concatinate new UVs
            var uv = new List<Vector2>
            {
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f)
            };
            gb.GetComponent<MeshFilter>().mesh.uv = gb.GetComponent<MeshFilter>().mesh.uv.Concat(uv).ToArray();
            #endregion
        }
    }
}
