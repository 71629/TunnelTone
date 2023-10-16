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
arc(170,1552,1.31,1.24,so,0.52,0.51,0,none,true);arctap(170);arctap(368);arctap(565);arctap(762);arctap(1552);
arc(2145,3329,0.91,1.21,b,0.76,0.33,0,none,true);arctap(2145);arctap(3329);
arc(3527,3724,1.30,1.22,si,0.62,0.64,0,none,true);arctap(3527);arctap(3724);
arc(3922,6884,0.54,0.00,s,0.69,0.40,0,none,true);arctap(3922);arctap(4712);arctap(4909);arctap(5107);arctap(5304);arctap(6489);arctap(6686);arctap(6884);
arc(7081,7871,1.43,1.40,soso,0.58,0.80,0,none,true);arctap(7081);arctap(7871);
arc(9648,10043,0.23,1.19,soso,0.83,0.17,0,none,true);arctap(9648);arctap(9846);arctap(10043);
arc(10438,10833,0.14,1.39,b,0.49,0.84,0,none,true);arctap(10438);arctap(10635);arctap(10833);
arc(11228,19324,1.08,1.02,sosi,0.90,0.30,0,none,true);arctap(11228);arctap(11623);arctap(12018);arctap(13005);arctap(13202);arctap(19126);arctap(19324);
arc(19521,19916,1.33,0.71,si,0.40,0.86,0,none,true);arctap(19521);arctap(19719);arctap(19916);
arc(20114,21101,0.69,0.57,soso,0.80,0.16,0,none,true);arctap(20114);arctap(20706);arctap(20903);arctap(21101);
arc(21298,22286,1.02,0.33,b,0.44,0.28,0,none,true);arctap(21298);arctap(21496);arctap(21693);arctap(22286);
arc(22680,22878,0.62,0.39,s,0.86,0.07,0,none,true);arctap(22680);arctap(22878);
arc(23075,23273,0.75,0.99,s,0.49,0.03,0,none,true);arctap(23075);arctap(23273);
arc(23470,23668,0.51,1.20,sosi,0.27,0.26,0,none,true);arctap(23470);arctap(23668);
arc(23865,24063,0.14,1.24,sisi,0.94,0.17,0,none,true);arctap(23865);arctap(24063);
arc(24260,24458,0.86,1.21,soso,0.12,0.04,0,none,true);arctap(24260);arctap(24458);
arc(24853,26235,1.16,0.69,s,0.79,0.38,0,none,true);arctap(24853);arctap(25050);arctap(25445);arctap(25642);arctap(25840);arctap(26037);arctap(26235);
arc(26432,27025,0.96,0.41,sisi,0.24,0.93,0,none,true);arctap(26432);arctap(27025);
arc(27222,27420,1.00,1.29,b,0.41,0.59,0,none,true);arctap(27222);arctap(27420);
arc(27617,27814,1.05,1.20,so,0.65,0.50,0,none,true);arctap(27617);arctap(27814);
arc(28012,28999,1.27,0.72,soso,0.35,0.62,0,none,true);arctap(28012);arctap(28604);arctap(28999);
arc(29197,30974,0.82,0.11,b,0.33,0.32,0,none,true);arctap(29197);arctap(29394);arctap(29592);arctap(29986);arctap(30184);arctap(30381);arctap(30974);
arc(35515,38675,0.61,1.00,s,0.41,0.77,0,none,true);arctap(35515);arctap(36700);arctap(36898);arctap(37095);arctap(38082);arctap(38280);arctap(38477);arctap(38675);
arc(39070,39267,0.51,1.09,sosi,0.74,0.39,0,none,true);arctap(39070);arctap(39267);
arc(39662,39859,0.69,0.57,b,0.94,0.19,0,none,true);arctap(39662);arctap(39859);
arc(40057,40452,0.86,1.00,so,0.81,0.86,0,none,true);arctap(40057);arctap(40452);
arc(40649,40847,1.35,0.60,sosi,0.91,0.94,0,none,true);arctap(40649);arctap(40847);
arc(41439,41637,0.67,0.47,sosi,0.45,0.64,0,none,true);arctap(41439);arctap(41637);
arc(41735,43583,0.99,1.37,so,0.89,0.36,0,none,true);arctap(41735);arctap(43583);
arc(43780,44176,1.36,0.50,s,0.17,0.89,0,none,true);arctap(43780);arctap(43978);arctap(44176);
arc(44373,44768,1.16,0.73,soso,0.49,0.93,0,none,true);arctap(44373);arctap(44571);arctap(44768);
arc(44966,45954,1.03,0.56,si,0.65,0.09,0,none,true);arctap(44966);arctap(45756);arctap(45954);
arc(46151,46349,0.45,0.43,b,0.18,0.62,0,none,true);arctap(46151);arctap(46349);
arc(47535,47930,0.83,0.71,soso,0.37,0.61,0,none,true);arctap(47535);arctap(47732);arctap(47930);
arc(48127,48918,0.19,0.57,s,0.29,0.57,0,none,true);arctap(48127);arctap(48918);
arc(49313,49510,1.08,1.02,sosi,0.69,0.36,0,none,true);arctap(49313);arctap(49510);
arc(50696,50894,1.41,1.31,si,0.49,0.43,0,none,true);arctap(50696);arctap(50894);
arc(51091,52277,0.63,1.19,si,0.90,0.72,0,none,true);arctap(51091);arctap(51289);arctap(52079);arctap(52277);
arc(52474,52672,0.51,1.09,sisi,0.91,0.71,0,none,true);arctap(52474);arctap(52672);
arc(53881,54276,1.23,0.18,soso,0.82,0.53,0,none,true);arctap(53881);arctap(54276);
arc(53881,54276,-0.33,-0.43,s,0.00,-0.10,0,none,false);
arc(54671,55065,0.58,0.76,so,0.48,0.53,0,none,true);arctap(54671);arctap(55065);
arc(54671,55065,-0.30,0.08,so,0.00,0.30,0,none,false);
arc(55438,55636,0.29,0.81,soso,0.31,0.81,0,none,true);arctap(55438);arctap(55636);
arc(55833,57414,0.69,1.19,soso,0.91,0.63,0,none,true);arctap(55833);arctap(56229);arctap(57019);arctap(57216);arctap(57414);
arc(57612,58402,1.34,1.32,s,0.36,0.88,0,none,true);arctap(57612);arctap(58402);
arc(58600,58797,0.41,0.49,b,0.41,0.41,0,none,true);arctap(58600);arctap(58797);
arc(58995,60180,0.98,0.78,sosi,0.30,0.90,0,none,true);arctap(58995);arctap(60180);
arc(60378,60575,0.34,0.98,b,0.37,0.84,0,none,true);arctap(60378);arctap(60575);
arc(60773,61563,1.23,0.52,b,0.93,0.57,0,none,true);arctap(60773);arctap(61563);
arc(61959,62354,0.87,0.52,soso,0.94,0.30,0,none,true);arctap(61959);arctap(62354);
arc(62551,63539,0.53,0.41,sosi,0.74,0.79,0,none,true);arctap(62551);arctap(63342);arctap(63539);
arc(63737,63935,0.36,0.32,si,0.60,0.37,0,none,true);arctap(63737);arctap(63935);
arc(64527,64725,1.28,0.14,sisi,0.71,0.50,0,none,true);arctap(64527);arctap(64725);
arc(65120,66503,0.59,0.40,s,0.66,0.19,0,none,true);arctap(65120);arctap(66503);
arc(66898,68084,0.56,0.24,s,0.11,0.51,0,none,true);arctap(66898);arctap(67294);arctap(67689);arctap(68084);
arc(68281,68479,1.17,0.63,sisi,0.76,0.62,0,none,true);arctap(68281);arctap(68479);
arc(68874,69665,1.28,0.82,sosi,0.76,0.66,0,none,true);arctap(68874);arctap(69665);
arc(70060,70455,1.18,0.83,s,0.57,0.81,0,none,true);arctap(70060);arctap(70455);
arc(70850,71443,1.29,1.28,so,0.93,0.75,0,none,true);arctap(70850);arctap(71245);arctap(71443);
arc(71641,72036,1.30,0.32,s,0.76,0.68,0,none,true);arctap(71641);arctap(72036);
arc(72826,73616,1.12,1.36,soso,0.73,0.52,0,none,true);arctap(72826);arctap(73419);arctap(73616);
arc(74209,74407,0.40,0.26,sisi,0.90,0.53,0,none,true);arctap(74209);arctap(74407);
arc(74604,75394,1.39,0.90,si,0.70,0.90,0,none,true);arctap(74604);arctap(74802);arctap(75197);arctap(75394);
arc(76381,77368,-0.15,-0.20,s,0.30,0.20,0,none,false);
arc(77368,78848,1.30,1.22,s,0.10,0.20,0,none,false);
arc(78848,80328,-0.30,-0.20,s,0.20,0.30,0,none,false);
arc(80328,81907,1.27,1.15,s,0.20,0.40,0,none,false);
arc(82006,84473,0.50,0.50,s,0.10,0.90,0,none,false);

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
