using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TunnelTone.Elements;
using TunnelTone.Events;
using UnityEngine.Splines;
using UnityEngine.UI;

// ReSharper disable InconsistentNaming
namespace TunnelTone.Charts
{
    public class JsonScanner : MonoBehaviour
    {
        private static NoteRenderer NoteRenderer => NoteRenderer.Instance;

        private void Start()
        {
            SystemEventReference.Instance.OnChartLoad.AddListener(Scan);
        }

        private void Scan(params object[] param)
        {
            var chartFile = (TextAsset)param[0];
            StartCoroutine(CreateElement(JsonConvert.DeserializeObject<Chart>(chartFile.text)));
        }

        IEnumerator CreateElement(Chart chart)
        {
            foreach(var trail in chart.trails)
            {
                yield return new WaitForSeconds(0);
                var gb = new GameObject("Trail")
                {
                    transform =
                    {
                        parent = transform,
                        position = Vector3.zero,
                        rotation = Quaternion.identity,
                        localScale = Vector3.one
                    },
                    layer = 11
                };
                gb.AddComponent<Trail>().Initialize(trail.startTime, trail.endTime,
                    new Vector2((float)trail.startX - 0.5f, (float)trail.startY - 0.4f),
                    new Vector2((float)trail.endX - 0.5f, (float)trail.endY - 0.4f), directionDictionary[trail.color],
                    easingDictionary[trail.easing], trail.easingRatio, true, trail.virtualTrail);
                
                NoteRenderer.TrailList.Add(gb);
                foreach(var tap in trail.taps)
                {
                    var spline = NoteRenderer.TrailReference.GetComponent<SplineContainer>().Spline;
                    var scale = new Vector3(0.6f, 0.6f, 0.6f);
                    var tgb = new GameObject("Tap")
                    {
                        transform =
                        {
                            parent = NoteRenderer.TrailReference.transform,
                            localPosition = spline.EvaluatePosition((tap.time * NoteRenderer.chartSpeedModifier - spline.EvaluatePosition(0)).z / (spline.EvaluatePosition(1).z - spline.EvaluatePosition(0).z)),
                            rotation = Quaternion.Euler(0, 0, 45),
                            localScale = scale
                        }
                    };
                    tgb.AddComponent<Image>();
                    var noteConfig = tgb.AddComponent<Tap>();
                    noteConfig.position = tgb.transform.localPosition;
                    noteConfig.time = tap.time;
            
                    NoteRenderer.TapList.Add(tgb);
                }
            }
            
            SystemEventReference.Instance.OnChartLoadFinish.Trigger();
            Invoke(nameof(StartSong), 1f);
        }

        private void StartSong()
        {
            NoteRenderer.StartSong();
        }

        private Dictionary<int, EasingMode> easingDictionary = new()
        {
            {0, EasingMode.Straight},
            {1, EasingMode.EaseIn},
            {2, EasingMode.EaseOut},
            {3, EasingMode.HorizontalInVerticalOut},
            {4, EasingMode.VerticalInHorizontalOut},
            {5, EasingMode.Bezier}
        };

        private Dictionary<int, Direction> directionDictionary = new()
        {
            { 0, Direction.Left },
            { 1, Direction.Right }
        };
        
        // Local class for deserialization
        public class Chart
        {
            public List<Trail> trails { get; set; }
            
            public class Trail
            {
                public double startX { get; set; }        
                public double startY { get; set; }

                public double endX { get; set; }
                public double endY { get; set; }
                public int startTime { get; set; }
                public int endTime { get; set; }
                public int color { get; set; }
                public int easing { get; set; }
                public float easingRatio { get; set; }
                public bool virtualTrail { get; set; }

                public List<Tap> taps { get; set; }
                public class Tap
                {
                    public int time { get; set; }
                }
            }
        }
    }
}