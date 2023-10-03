using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class Converter
    {
        public const int s = 0;
        public const int si = 1;
        public const int sisi = 1;
        public const int so = 2;
        public const int soso = 2;
        public const int siso = 3;
        public const int sosi = 4;
        public const int b = 5;

        public const int none = 10;

        public static Chart chart;

        public static void Main()
        {
            chart = new Chart
            {
                trails = new List<Trail>()
            };
            
            // Insert .aff (.lua)
            arc(891, 965, 0.00, 0.87, s, 0.40, 0.40, 0, none, true);
            arctap(915);
            
            // Convert
            var ret = JsonUtility.ToJson(chart);
            
            // Output the result to a new json file
            System.IO.File.WriteAllText(@"C:\Users\user\Desktop\test.json", ret);
        }

        private static void arc(int startTime, int endTime, double startX, double endX, int easingMode, double startY, double endY, int color, int fx, bool virtualArc)
        {
            var trail = new Trail
            {
                startTime = startTime,
                endTime = endTime,
                start = new Vector2((float)startX, (float)startY),
                end = new Vector2((float)endX, (float)endY),
                easing = easingMode,
                color = color,
                virtualTrail = virtualArc
            };

            chart.trails.Add(trail);
        }

        private static void arctap(int time)
        {
            var tap = new Tap
            {
                time = time
            };
            
            chart.trails.Last().taps.Add(tap);
        }
    }

    public class Trail
    {
        public Vector2 start, end;
        public int startTime, endTime;
        public int color;
        public int easing;
        public float easingRatio;
        public bool virtualTrail;

        public List<Tap> taps;
    }

    public class Tap
    {
        public int time;
    }

    public class Chart
    {
        public List<Trail> trails;
    }
}