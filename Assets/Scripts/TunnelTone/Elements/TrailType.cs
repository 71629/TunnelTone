﻿using System;
using System.Collections.Generic;
using System.Linq;
using TunnelTone.UI.Reference;
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
        internal abstract void Initialize(Trail t);
        internal abstract List<Vector3> GetVertices(Vector3 position, float time);
    }
    
    internal class RealTrail : TrailType
    {
        internal override void Initialize(Trail t)
        {
            t.meshRenderer.material = t.Direction switch
            {
                Direction.Left => UIElementReference.Instance.leftTrail,
                Direction.Right => UIElementReference.Instance.rightTrail,
                _ => throw new ArgumentOutOfRangeException(nameof(t.Direction), t.Direction,
                    $"Given direction is not valid for Trail type: {t.Direction}")
            };
            t.trailHint = Object.Instantiate(t.trailHintObject, GameObject.Find("Hit Zone").transform).GetComponent<TrailHint>();
            t.trailHint.transform.localPosition = new Vector3(t.spline.EvaluatePosition(0).x, t.spline.EvaluatePosition(0).y, 0);
            t.StartCoroutine(t.TrailHint());
            
            var bpm = NoteRenderer.Instance.currentBpm;
            t.BuildHead(t.spline.EvaluatePosition(0), t.startTime * NoteRenderer.Instance.chartSpeedModifier, false);

            var density = 60 / (bpm * 2) * NoteRenderer.Instance.chartSpeedModifier;
            for (var i = 0f; i < 1; i += density * 1000 / (t.spline.ElementAt(1).Position.z - t.spline.ElementAt(0).Position.z))
            {
                t.BuildCombo(out var gb, (Vector3)t.spline.EvaluatePosition(i),
                    (t.spline.EvaluatePosition(i).z - NoteRenderer.Instance.universalOffset) / NoteRenderer.Instance.chartSpeedModifier);
            }
            // Build subsegments
            float tail = 0;
            for(var i = 0f; i < 1; i += 1000 / (t.spline.ElementAt(1).Position.z - t.spline.ElementAt(0).Position.z))
            {
                Object.Instantiate(t.trailSubsegmentPrefab, t.transform).GetComponent<TrailSubsegment>().Initialize(t, t.spline, 
                    (Vector3)t.spline.EvaluatePosition(tail), 
                    (Vector3)t.spline.EvaluatePosition(i), 
                    Mathf.Lerp(t.startTime, t.endTime, Mathf.InverseLerp(t.spline.EvaluatePosition(0).z, t.spline.EvaluatePosition(1).z, t.spline.EvaluatePosition(tail).z)), 
                    Mathf.Lerp(t.startTime, t.endTime, Mathf.InverseLerp(t.spline.EvaluatePosition(0).z, t.spline.EvaluatePosition(1).z, t.spline.EvaluatePosition(i).z))
                );
                tail = i;
            }
            Object.Instantiate(t.trailSubsegmentPrefab, t.transform).GetComponent<TrailSubsegment>().Initialize(t, t.spline,
                (Vector3)t.spline.EvaluatePosition(tail), 
                (Vector3)t.spline.EvaluatePosition(1), 
                t.spline.EvaluatePosition(tail).z, 
                t.spline.EvaluatePosition(1).z
            );
        }

        internal override List<Vector3> GetVertices(Vector3 position, float time)
        {
            return new List<Vector3>
            {
                position + new Vector3(0, 0, -25 + time),
                position + new Vector3(0, 40, 0 + time),
                position + new Vector3(40, 0, 0 + time),
                position + new Vector3(0, -40, 0 + time),
                position + new Vector3(-40, 0, 0 + time)
            };
        }
    }
    
    internal class VirtualTrail : TrailType
    {
        internal override void Initialize(Trail t)
        {
            t.gameObject.layer = 20;
            t.meshRenderer.material = NoteRenderer.Instance.none;
            
            var j = 0;
            t.lineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, .5f));
            for (var i = 0f; i < 1; i += 80 / (t.spline.ElementAt(1).Position.z - t.spline.ElementAt(0).Position.z))
            {
                t.lineRenderer.SetPosition(j, t.spline.EvaluatePosition(i));
                t.lineRenderer.positionCount++;
                j++;
            }
            t.lineRenderer.SetPosition(j, t.spline.EvaluatePosition(1));
            t.lineRenderer.positionCount--;
            t.lineRenderer.material = t.virtualTrailMaterial;
        }

        internal override List<Vector3> GetVertices(Vector3 position, float time)
        {
            return new List<Vector3>
                {
                    position + new Vector3(0, 0, -8 + time),
                    position + new Vector3(0, 8, 0 + time),
                    position + new Vector3(8, 0, 0 + time),
                    position + new Vector3(0, -8, 0 + time),
                    position + new Vector3(-8, 0, 0 + time)
                };
        }
    }
}