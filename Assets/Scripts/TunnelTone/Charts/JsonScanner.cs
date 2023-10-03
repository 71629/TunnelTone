using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace TunnelTone.Charts
{
    public class JsonScanner : MonoBehaviour
    {
        [SerializeField] private TextAsset chart;

        private void Start()
        {
            // Deserialize chart to Chart object
            var ret = new Chart();
            ret = JsonConvert.DeserializeObject<Chart>(chart.text);
        }
        public class Chart
        {
            List<Trail> trails { get; set; }
            
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