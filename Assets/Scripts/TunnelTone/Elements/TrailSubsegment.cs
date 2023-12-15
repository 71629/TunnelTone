using System;
using TunnelTone.UI.Reference;
using UnityEngine;
using UnityEngine.Splines;

// ReSharper disable SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault

namespace TunnelTone.Elements
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(SplineContainer))]
    public class TrailSubsegment : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private SplineContainer splineContainer;
        private Spline spline
        {
            set => splineContainer.Spline = value;
        }
        private Mesh mesh => meshFilter.mesh;
        [SerializeField] private MeshRenderer meshRenderer;
        private Material material
        {
            set => meshRenderer.material = value;
        }

        [Header("Values")]
        private Trail _parent;
        private Vector2 _startCoordinate;
        private Vector2 _endCoordinate;
        private float _startTime;
        private float _endTime;
        private Direction _direction;

        internal void Initialize(Trail segmentParent, Spline segmentSpline, Vector2 startCoordinate, Vector2 endCoordinate, float startTime, float endTime)
        {
            _parent = segmentParent;
            _startCoordinate = startCoordinate;
            _endCoordinate = endCoordinate;
            _startTime = startTime;
            _endTime = endTime;
            _direction = segmentParent.Direction;
            spline = segmentSpline;
            
            SetMesh();
        }

        private void SetMesh()
        {
            var startPosition = new Vector3(_startCoordinate.x, _startCoordinate.y, _startTime);
            var endPosition = new Vector3(_endCoordinate.x, _endCoordinate.y, _endTime);
            
            material = _direction switch
            {
                Direction.Left => UIElementReference.Instance.leftTrail,
                Direction.Right => UIElementReference.Instance.rightTrail,
                _ => throw new ArgumentException()
            };
            mesh.vertices = new[]
            {
                startPosition + new Vector3(0, 40, 0),
                startPosition + new Vector3(40, 0, 0),
                startPosition + new Vector3(0, -40, 0),
                startPosition + new Vector3(-40, 0, 0),
                
                endPosition + new Vector3(0, 40, 0),
                endPosition + new Vector3(40, 0, 0),
                endPosition + new Vector3(0, -40, 0),
                endPosition + new Vector3(-40, 0, 0)
            };
            mesh.triangles = new[]
            {
                0, 4, 5,
                0, 5, 1,
                1, 5, 6,
                1, 6, 2,
                2, 6, 7,
                2, 7, 3,
                3, 7, 4,
                3, 4, 0
            };
            mesh.uv = new[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(1, 0),
                
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            
            UpdateMesh();
        }

        private void Update()
        {
            if (NoteRenderer.currentTime * 1000 > _endTime + 120 && NoteRenderer.IsPlaying) Destroy(gameObject);
            if (_startTime < NoteRenderer.currentTime || !NoteRenderer.IsPlaying || !_parent.isTracking) return;
            for (var i = 0; i <= 3; i++)
            {
                mesh.vertices[i] = new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, 0);
            }
            UpdateMesh();
        }

        private void UpdateMesh()
        {
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
    }
}