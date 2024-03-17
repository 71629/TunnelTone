using System;
using System.Collections;
using System.Linq;
using TunnelTone.PlayArea;
using TunnelTone.UI.Reference;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Object = UnityEngine.Object;

namespace TunnelTone.Elements
{
    /// <summary>
    /// State machine for trail
    /// </summary>
    internal abstract class TrailType
    {
        protected readonly Trail t;

        protected readonly float headTailZAxisDelta;
        protected readonly float3 headPosition;
        protected readonly float3 tailPosition;
        protected readonly float headZPosition;
        protected readonly float tailZPosition;

        protected float trailWidth;

        internal TrailType(Trail t)
        {
            this.t = t;
            headTailZAxisDelta = t.spline.ElementAt(1).Position.z - t.spline.ElementAt(0).Position.z;
            headPosition = t.spline.EvaluatePosition(0);
            tailPosition = t.spline.EvaluatePosition(1);
            headZPosition = headPosition.z;
            tailZPosition = tailPosition.z;
        }
        
        internal abstract void Initialize();
        internal abstract Vector3[] GetVertices(Vector3 position, float time);
        internal abstract void ConfigureCollider();
    }
    
    internal class RealTrail : TrailType
    {
        internal RealTrail(Trail t) : base(t)
        {
            trailWidth = 40;
        }

        internal override void Initialize()
        {
            t.meshRenderer.material = t.Direction switch
            {
                Direction.Left => UIElementReference.Instance.leftTrail,
                Direction.Right => UIElementReference.Instance.rightTrail,
                _ => throw new ArgumentOutOfRangeException(nameof(t.Direction), t.Direction,
                    $"Given direction is not valid for Trail type: {t.Direction}")
            };
            t.trailHint = Object.Instantiate(t.trailHintObject, GameObject.Find("Hit Zone").transform).GetComponent<TrailHint>();
            t.trailHint.transform.localPosition = new Vector3(headPosition.x, headPosition.y, 0);
            t.StartCoroutine(t.TrailHint());
            
            var bpm = NoteRenderer.Instance.currentBpm;
            t.BuildHead(headPosition, t.startTime * NoteRenderer.Instance.chartSpeedModifier, false);

            var density = 60 / (bpm * 2) * NoteRenderer.Instance.chartSpeedModifier;
            for (var i = 0f; i < 1; i += density * 1000 / headTailZAxisDelta)
            {
                BuildCombo((Vector3)t.spline.EvaluatePosition(i),
                    (t.spline.EvaluatePosition(i).z - NoteRenderer.Instance.universalOffset) / NoteRenderer.Instance.chartSpeedModifier);
            }
            // Build subsegments
            float tail = 0;
            for(var i = 0f; i < 1; i += 350 / headTailZAxisDelta)
            {
                BuildSubsegment(tail, i);
                tail = i;
            }
            BuildSubsegment(tail, 1);
        }

        private void BuildSubsegment(float tail, float i)
        {
            Object.Instantiate(t.trailSubsegmentPrefab, t.transform).GetComponent<TrailSubsegment>().Initialize(t, t.spline, 
                (Vector3)t.spline.EvaluatePosition(tail), 
                (Vector3)t.spline.EvaluatePosition(i), 
                Mathf.Lerp(t.startTime, t.endTime, Mathf.InverseLerp(headZPosition, tailZPosition, t.spline.EvaluatePosition(tail).z)), 
                Mathf.Lerp(t.startTime, t.endTime, Mathf.InverseLerp(headZPosition, tailZPosition, t.spline.EvaluatePosition(i).z))
            );
        }

        internal override Vector3[] GetVertices(Vector3 position, float time)
        {
            return new[]
            {
                position + new Vector3(0, 0,  time - 25),
                position + new Vector3(0, trailWidth, time),
                position + new Vector3(trailWidth, 0, time),
                position + new Vector3(0, -trailWidth, time),
                position + new Vector3(-trailWidth, 0, time)
            };
        }

        internal override void ConfigureCollider()
        {
            t.StartCoroutine(t.UpdateCollider());
        }
        
        private void BuildCombo(Vector2 coordinate, float time)
        {
            // TODO: Replace with prefab
            var gb = new GameObject("Combo")
            {
                transform =
                {
                    parent = t.transform,
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
    }
    
    internal class VirtualTrail : TrailType
    {
        internal VirtualTrail(Trail t) : base(t)
        {
            trailWidth = 8;
        }

        internal override void Initialize()
        {
            t.gameObject.layer = 20;
            t.meshRenderer.material = NoteRenderer.Instance.none;
            
            var j = 0;
            t.lineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, .5f));
            for (var i = 0f; i < 1; i += 80 / headTailZAxisDelta)
            {
                t.lineRenderer.SetPosition(j, t.spline.EvaluatePosition(i));
                t.lineRenderer.positionCount++;
                j++;
            }
            t.lineRenderer.SetPosition(j, tailPosition);
            t.lineRenderer.positionCount--;
            t.lineRenderer.material = t.virtualTrailMaterial;
        }

        internal override Vector3[] GetVertices(Vector3 position, float time)
        {
            return new[]
                {
                    position + new Vector3(0, 0, time - 8),
                    position + new Vector3(0, trailWidth, time),
                    position + new Vector3(trailWidth, 0, time),
                    position + new Vector3(0, -trailWidth, time),
                    position + new Vector3(-trailWidth, 0, time)
                };
        }

        internal override void ConfigureCollider()
        {
            t.col.enabled = false;
        }
    }

    public partial class Trail
    {
        // Mask for z axis
        private static readonly float3 AxisMaskZ = new(1, 1, 0);
        
        internal IEnumerator UpdateCollider()
        {
            while (spline is not null)
            {
                var t = Mathf.InverseLerp(startTime, endTime, NoteRenderer.CurrentTime * 1000 + NoteRenderer.Instance.universalOffset);
                col.center = spline.EvaluatePosition(Mathf.Clamp01(t));
                
                trailHint.transform.localPosition = spline.EvaluatePosition(Mathf.Clamp01(t)) * AxisMaskZ;

                yield return null;
            }
        }
    }
}