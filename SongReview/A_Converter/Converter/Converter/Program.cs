using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DefaultNamespace
{
    public class Converter
    {
        public const int s = 0;
        public const int si = 1;
        public const int sisi = 1;
        public const int so = 2;
        public const int soso = 2;
        public const int sosi = 4;
        public const int b = 5;

        public const int none = 10;

        public static List<Trail> trails = new();
        public static Chart chart = new()
        {
            trails = Array.Empty<Trail>()
        };

        public static void Main()
        {
            // Insert .aff (.lua)
arc(3310,3471,1.14,0.46,si,0.51,0.64,0,none,true);arctap(3310);arctap(3471);
arc(3632,3793,0.70,1.29,sisi,0.88,0.52,0,none,true);arctap(3632);arctap(3793);
arc(4116,4439,0.57,0.29,so,0.86,0.49,0,none,true);arctap(4116);arctap(4439);
arc(4761,5568,1.18,1.20,sosi,0.30,0.89,0,none,true);arctap(4761);arctap(4922);arctap(5084);arctap(5245);arctap(5568);
arc(5890,6051,0.47,1.36,si,0.70,0.49,0,none,true);arctap(5890);arctap(6051);
arc(6213,8471,0.73,0.65,si,0.85,0.38,0,none,true);arctap(6213);arctap(6374);arctap(6697);arctap(7019);arctap(8471);
arc(8632,9600,1.37,1.05,s,0.53,0.51,0,none,true);arctap(8632);arctap(8793);arctap(8955);arctap(9277);arctap(9600);
arc(9922,10084,1.13,0.73,s,0.27,0.74,0,none,true);arctap(9922);arctap(10084);
arc(10245,10406,1.39,1.11,si,0.55,0.32,0,none,true);arctap(10245);arctap(10406);
arc(10729,11535,0.28,1.40,si,0.63,0.72,0,none,true);arctap(10729);arctap(10890);arctap(11213);arctap(11374);arctap(11535);
arc(11858,12503,0.24,0.82,soso,0.70,0.88,0,none,true);arctap(11858);arctap(12019);arctap(12180);arctap(12503);
arc(12664,13309,0.34,0.31,sosi,0.47,0.83,0,none,true);arctap(12664);arctap(13309);
arc(13309,13632,0.89,0.43,si,0.93,0.16,0,none,true);arctap(13309);arctap(13632);
arc(13632,14439,1.04,0.94,s,0.84,0.66,0,none,true);arctap(13632);arctap(13793);arctap(13955);arctap(14116);arctap(14439);
arc(14761,15084,0.44,0.39,sisi,0.38,0.73,0,none,true);arctap(14761);arctap(15084);
arc(15245,15406,1.03,0.62,s,0.47,0.54,0,none,true);arctap(15245);arctap(15406);
arc(15568,16535,0.76,0.93,sisi,0.48,0.89,0,none,true);arctap(15568);arctap(15890);arctap(16213);arctap(16374);arctap(16535);
arc(16697,17019,0.40,0.37,sisi,0.35,0.75,0,none,true);arctap(16697);arctap(17019);
arc(17342,17826,0.36,0.85,si,0.37,0.64,0,none,true);arctap(17342);arctap(17826);
arc(18148,18471,0.43,0.37,si,0.54,0.93,0,none,true);arctap(18148);arctap(18471);
arc(18793,18955,1.39,0.43,b,0.35,0.87,0,none,true);arctap(18793);arctap(18955);
arc(19116,19600,0.47,1.18,s,0.33,0.61,0,none,true);arctap(19116);arctap(19277);arctap(19600);
arc(19922,20568,0.79,0.48,sisi,0.71,0.22,0,none,true);arctap(19922);arctap(20245);arctap(20406);arctap(20568);
arc(20729,21535,1.38,0.75,so,0.28,0.13,0,none,true);arctap(20729);arctap(21051);arctap(21213);arctap(21535);
arc(21697,22342,0.53,1.28,s,0.31,0.54,0,none,true);arctap(21697);arctap(21858);arctap(22180);arctap(22342);
arc(22503,22826,1.27,1.10,si,0.40,0.27,0,none,true);arctap(22503);arctap(22826);
arc(22987,23309,0.68,1.42,sisi,0.56,0.34,0,none,true);arctap(22987);arctap(23309);
arc(23471,23632,0.91,1.06,b,0.72,0.55,0,none,true);arctap(23471);arctap(23632);
arc(23632,23955,0.42,1.04,soso,0.63,0.13,0,none,true);arctap(23632);arctap(23955);
arc(23955,24116,0.80,0.77,si,0.63,0.68,0,none,true);arctap(23955);arctap(24116);
arc(24277,25245,0.42,1.40,sisi,0.85,0.90,0,none,true);arctap(24277);arctap(24439);arctap(24600);arctap(24922);arctap(25084);arctap(25245);
arc(25406,25568,0.92,0.65,si,0.64,0.53,0,none,true);arctap(25406);arctap(25568);
arc(25729,26374,1.15,1.07,si,0.82,0.78,0,none,true);arctap(25729);arctap(26051);arctap(26213);arctap(26374);
arc(26374,26858,0.73,1.03,so,0.25,0.57,0,none,true);arctap(26374);arctap(26858);
arc(27180,27503,1.19,0.97,so,0.54,0.81,0,none,true);arctap(27180);arctap(27342);arctap(27503);
arc(27664,28471,1.36,1.44,so,0.59,0.62,0,none,true);arctap(27664);arctap(27987);arctap(28471);
arc(28793,29116,0.84,1.12,soso,0.55,0.44,0,none,true);arctap(28793);arctap(29116);
arc(29277,29600,0.67,0.81,so,0.45,0.85,0,none,true);arctap(29277);arctap(29439);arctap(29600);
arc(29761,30084,0.65,1.32,sosi,0.51,0.37,0,none,true);arctap(29761);arctap(30084);
arc(30245,30406,0.96,0.62,sosi,0.84,0.25,0,none,true);arctap(30245);arctap(30406);
arc(30568,30890,1.31,0.97,b,0.75,0.35,0,none,true);arctap(30568);arctap(30729);arctap(30890);
arc(31213,31535,0.49,1.34,sisi,0.55,0.84,0,none,true);arctap(31213);arctap(31374);arctap(31535);
arc(32019,32342,1.44,1.30,sisi,0.89,0.05,0,none,true);arctap(32019);arctap(32342);
arc(32503,33148,0.77,1.27,si,0.82,0.56,0,none,true);arctap(32503);arctap(32664);arctap(32826);arctap(33148);
arc(33309,34277,0.35,1.34,s,0.35,0.90,0,none,true);arctap(33309);arctap(33632);arctap(34277);
arc(34277,34922,0.35,1.22,soso,0.37,0.43,0,none,true);arctap(34277);arctap(34600);arctap(34922);
arc(35245,36535,0.60,0.79,si,0.42,0.28,0,none,true);arctap(35245);arctap(35568);arctap(35890);arctap(36213);arctap(36535);
arc(36858,37180,1.29,0.36,sisi,0.35,0.84,0,none,true);arctap(36858);arctap(37180);
arc(37342,37664,1.44,0.72,sosi,0.35,0.55,0,none,true);arctap(37342);arctap(37664);
arc(37826,39439,0.72,0.42,b,0.88,0.94,0,none,true);arctap(37826);arctap(37987);arctap(39439);
arc(39922,39923,0.93,0.93,sosi,0.56,0.56,0,none,true);arctap(39922);
arc(40406,40407,0.71,0.71,b,0.33,0.33,0,none,true);arctap(40406);
arc(40890,41374,0.13,0.82,soso,0.31,0.72,0,none,true);arctap(40890);arctap(41374);
arc(41697,41700,1.28,1.28,b,0.84,0.84,0,none,true);arctap(41697);
arc(41939,42342,1.40,1.31,sosi,0.32,0.84,0,none,true);arctap(41939);arctap(42180);arctap(42342);
arc(42342,42664,0.93,0.90,s,0.49,0.52,0,none,true);arctap(42342);arctap(42664);
arc(42664,43148,0.34,1.29,s,0.59,0.87,0,none,true);arctap(42664);arctap(42906);arctap(43148);
arc(43309,43310,1.22,1.22,so,0.16,0.16,0,none,true);arctap(43309);
arc(43955,44600,1.40,0.02,b,0.89,0.55,0,none,true);arctap(43955);arctap(44600);
arc(44600,45245,1.08,1.04,so,0.53,0.61,0,none,true);arctap(44600);arctap(44922);arctap(45245);
arc(45568,46213,0.73,1.06,so,0.38,0.86,0,none,true);arctap(45568);arctap(45890);arctap(46213);
arc(46535,47987,1.22,1.05,so,0.72,0.40,0,none,true);arctap(46535);arctap(46858);arctap(47180);arctap(47503);arctap(47826);arctap(47987);
arc(48148,48471,0.62,0.75,b,0.07,0.29,0,none,true);arctap(48148);arctap(48309);arctap(48471);
arc(48471,48793,0.28,1.24,sosi,0.71,0.51,0,none,true);arctap(48471);arctap(48793);
arc(49116,49277,1.01,1.06,sosi,0.44,0.93,0,none,true);arctap(49116);arctap(49277);
arc(49439,49600,0.56,1.18,sisi,0.84,0.81,0,none,true);arctap(49439);arctap(49600);
arc(49761,50084,0.44,0.65,soso,0.32,0.69,0,none,true);arctap(49761);arctap(50084);
arc(50406,51374,0.44,1.25,sosi,0.60,0.82,0,none,true);arctap(50406);arctap(50729);arctap(51051);arctap(51374);
arc(51697,52019,0.62,1.34,b,0.38,0.75,0,none,true);arctap(51697);arctap(52019);
arc(52342,52503,1.00,0.12,s,0.73,0.38,0,none,true);arctap(52342);arctap(52503);
arc(52664,52987,0.95,0.78,sosi,0.27,0.63,0,none,true);arctap(52664);arctap(52987);
arc(53148,53309,1.03,1.38,soso,0.06,0.60,0,none,true);arctap(53148);arctap(53309);
arc(53632,53874,0.91,0.32,sisi,0.36,0.39,0,none,true);arctap(53632);arctap(53874);
arc(54116,54600,1.23,1.32,s,0.38,0.33,0,none,true);arctap(54116);arctap(54439);arctap(54600);
arc(54922,54923,0.78,0.78,soso,0.44,0.44,0,none,true);arctap(54922);
arc(55245,55890,1.18,1.43,so,0.58,0.34,0,none,true);arctap(55245);arctap(55568);arctap(55890);
arc(56213,56535,1.38,0.98,sisi,0.77,0.52,0,none,true);arctap(56213);arctap(56535);
arc(56858,58148,0.64,1.34,soso,0.92,0.26,0,none,true);arctap(56858);arctap(57180);arctap(57503);arctap(57826);arctap(58148);
arc(58309,58471,1.37,0.69,b,0.70,0.74,0,none,true);arctap(58309);arctap(58471);
arc(58632,59116,0.12,1.16,sisi,0.64,0.34,0,none,true);arctap(58632);arctap(58793);arctap(59116);
arc(59439,59600,0.64,1.30,soso,0.34,0.80,0,none,true);arctap(59439);arctap(59600);
arc(59761,59922,1.09,0.96,so,0.34,0.10,0,none,true);arctap(59761);arctap(59922);
arc(60084,60085,0.19,0.19,si,0.48,0.48,0,none,true);arctap(60084);
arc(60406,60729,1.05,0.78,soso,0.47,0.29,0,none,true);arctap(60406);arctap(60729);
arc(61051,62664,0.88,1.21,s,0.76,0.87,0,none,true);arctap(61051);arctap(61374);arctap(61697);arctap(62019);arctap(62342);arctap(62664);
arc(62987,63309,0.73,1.29,sisi,0.78,0.64,0,none,true);arctap(62987);arctap(63309);
arc(63632,63955,0.84,1.29,s,0.32,0.14,0,none,true);arctap(63632);arctap(63955);
arc(64277,64600,1.14,0.27,sisi,0.65,0.43,0,none,true);arctap(64277);arctap(64600);
arc(64761,64922,0.97,0.82,sisi,0.20,0.85,0,none,true);arctap(64761);arctap(64922);
arc(65084,65245,0.80,0.94,so,0.21,0.83,0,none,true);arctap(65084);arctap(65245);
arc(65245,67180,0.52,0.99,sosi,0.42,0.71,0,none,true);arctap(65245);arctap(65568);arctap(65890);arctap(66213);arctap(66535);arctap(66858);arctap(67180);
arc(67503,68471,0.49,1.18,si,0.35,0.42,0,none,true);arctap(67503);arctap(67826);arctap(68148);arctap(68471);
arc(68793,69116,0.48,0.73,so,0.89,0.53,0,none,true);arctap(68793);arctap(68955);arctap(69116);
arc(69439,71051,0.81,1.28,sosi,0.46,0.65,0,none,true);arctap(69439);arctap(69761);arctap(70084);arctap(70406);arctap(70568);arctap(71051);
arc(71213,71858,1.38,0.21,sosi,0.77,0.74,0,none,true);arctap(71213);arctap(71858);
arc(72100,72342,1.14,0.44,b,0.21,0.80,0,none,true);arctap(72100);arctap(72342);
arc(72342,72987,1.20,0.78,si,0.61,0.59,0,none,true);arctap(72342);arctap(72664);arctap(72987);
arc(73148,73632,1.21,0.38,sosi,0.58,0.45,0,none,true);arctap(73148);arctap(73632);
arc(73632,73793,0.60,0.78,b,0.93,0.93,0,none,true);arctap(73632);arctap(73793);
arc(74439,74440,0.48,0.48,soso,0.89,0.89,0,none,true);arctap(74439);
arc(74680,74922,0.94,0.00,so,0.74,0.44,0,none,true);arctap(74680);arctap(74922);
arc(75568,75569,0.56,0.56,s,0.67,0.67,0,none,true);arctap(75568);

            // Convert
            chart.trails = trails.ToArray();
            var ret = JsonSerializer.Serialize(chart, new JsonSerializerOptions{WriteIndented = true});

            // Output the result to a new json file
            File.WriteAllText(@"D:\Prismatic.json", ret);

            //Open explorer
            Process.Start("explorer.exe", @"D:\");
        }

        private static void arc(int startTime, int endTime, double startX, double endX, int easingMode, double startY, double endY, int color, int fx, bool virtualArc)
        {
            var trail = new Trail
            {
                startTime = startTime,
                endTime = endTime,
                startX = startX,
                startY = startY,
                endX = endX,
                endY = endY,
                easing = easingMode,
                easingRatio = 0.65f,
                color = color,
                virtualTrail = virtualArc,
                taps = new List<Tap>()
            };

            trails.Add(trail);
        }

        private static void arctap(int time)
        {
            var tap = new Tap
            {
                time = time
            };

            trails.Last().taps.Add(tap);
        }
    }

    public class Chart
    {
        public Trail[] trails { get; set; }
    }

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
    }

    public class Tap
    {
        public int time { get; set; }
    }
}
