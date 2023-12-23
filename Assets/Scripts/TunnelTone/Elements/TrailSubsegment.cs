using System;
using TunnelTone.UI.Reference;
using Unity.XR.OpenVR;
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
        private Spline Spline
        {
            set => splineContainer.Spline = value;
        }
        private Mesh Mesh => meshFilter.mesh;
        [SerializeField] private MeshRenderer meshRenderer;
        private Material Material
        {
            set => meshRenderer.material = value;
        }

        [Header("Values")]
        private Trail parent;
        private Vector2 startCoordinate;
        private Vector2 endCoordinate;
        private float startTime;
        private float endTime;
        private Direction direction;

        internal TrailSubsegment Initialize(Trail segmentParent, Spline segmentSpline, Vector2 startCoordinate, Vector2 endCoordinate, float startTime, float endTime)
        {
            parent = segmentParent;
            this.startCoordinate = startCoordinate;
            this.endCoordinate = endCoordinate;
            this.startTime = startTime;
            this.endTime = endTime;
            direction = segmentParent.Direction;
            Spline = segmentSpline;
            
            SetMesh();

            return this;
        }

        private void SetMesh()
        {
            var startPosition = new Vector3(startCoordinate.x, startCoordinate.y, startTime);
            var endPosition = new Vector3(endCoordinate.x, endCoordinate.y, endTime);
            
            Material = direction switch
            {
                Direction.Left => UIElementReference.Instance.leftTrail,
                Direction.Right => UIElementReference.Instance.rightTrail,
                _ => throw new ArgumentException()
            };
            Mesh.vertices = new[]
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
            Mesh.triangles = new[]
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
            Mesh.uv = new[]
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
            if (NoteRenderer.currentTime * 1000 > endTime + 120 && NoteRenderer.IsPlaying) Destroy(gameObject);
            if (startTime > NoteRenderer.currentTime * 1000 || !NoteRenderer.IsPlaying || !parent.isTracking) return;
            for (var i = 0; i <= 3; i++)
            {
                Mesh.vertices[i] = new Vector3(Mesh.vertices[i].x, Mesh.vertices[i].y, 0);
            }
            UpdateMesh();
        }

        private void UpdateMesh()
        {
            Mesh.RecalculateBounds();
            Mesh.RecalculateNormals();
            Mesh.RecalculateTangents();
        }
    }
}