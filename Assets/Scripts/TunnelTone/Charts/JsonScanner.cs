using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TunnelTone.Elements;
using Unity.Mathematics;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using UnityEngine.UI;

// ReSharper disable InconsistentNaming
namespace TunnelTone.Charts
{
    public class JsonScanner : MonoBehaviour
    {
        [SerializeField] private TextAsset chartFile;
        [SerializeField] private NoteRenderer _noteRenderer;

        private void Start()
        {
            // Deserialize chartFile to Chart object
            var chart = new Chart();
            
            chart = JsonConvert.DeserializeObject<Chart>(chartFile.text);
            
            foreach(var trail in chart.trails)
            {
                GameObject gb;
                _noteRenderer.BuildTrail(out gb, trail.startTime, trail.endTime,
                    new Vector2((float)trail.startX, (float)trail.startY),
                    new Vector2((float)trail.endX, (float)trail.endY), directionDictionary[trail.color],
                    easingDictionary[trail.easing], 0.6f, true, false);
                ChartDataStorage.TrailList.Add(gb);
                foreach(var tap in trail.taps)
                {
                    var spline = ChartDataStorage.TrailReference.GetComponent<SplineContainer>().Spline;
                    var scale = new Vector3(0.6f, 0.6f, 0.6f);
                    var tgb = new GameObject("Tap")
                    {
                        transform =
                        {
                            parent = ChartDataStorage.TrailReference.transform,
                            localPosition = spline.EvaluatePosition((tap.time * _noteRenderer.chartSpeedModifier - spline.EvaluatePosition(0)).z / (spline.EvaluatePosition(1).z - spline.EvaluatePosition(0).z)),
                            rotation = Quaternion.Euler(0, 0, 45),
                            localScale = scale
                        }
                    };
                    tgb.AddComponent<Image>();
                    var noteConfig = tgb.AddComponent<Tap>();
                    noteConfig.position = tgb.transform.localPosition;
                    noteConfig.time = tap.time;
            
                    ChartDataStorage.TapList.Add(tgb);
                }
            }
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