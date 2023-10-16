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
arc(1685,2435,0.84,0.56,sisi,0.79,0.8,0,none,true);arctap(1685);arctap(2435);
arc(3214,5607,0.41,0.59,b,0.3,0.4,0,none,true);arctap(3214);arctap(4807);arctap(5607);
arc(6407,9555,0.3,1.05,b,0.68,0.91,0,none,true);arctap(6407);arctap(7966);arctap(8766);arctap(9555);
arc(11113,15261,0.63,0.55,si,0.89,0.37,0,none,true);arctap(11113);arctap(11902);arctap(12494);arctap(13481);arctap(14078);arctap(14867);arctap(15261);
arc(15656,16840,1.08,0.72,s,0.49,0.47,0,none,true);arctap(15656);arctap(16840);
arc(17235,18019,1.34,0.34,soso,0.64,0.62,0,none,true);arctap(17235);arctap(18019);
arc(18411,18803,1.32,1.1,b,0.85,0.6,0,none,true);arctap(18411);arctap(18803);
arc(19783,22379,1.08,0.99,b,0.67,0.04,0,none,true);arctap(19783);arctap(20176);arctap(20568);arctap(21573);arctap(22379);
arc(22777,26504,0.36,1.42,si,0.76,0.45,0,none,true);arctap(22777);arctap(23367);arctap(23759);arctap(24935);arctap(25327);arctap(26504);
arc(26912,29675,0.9,0.06,so,0.84,0.28,0,none,true);arctap(26912);arctap(27898);arctap(28688);arctap(29083);arctap(29675);
arc(30060,30454,0.65,0.06,so,0.59,0.49,0,none,true);arctap(30060);arctap(30454);
arc(31244,33217,0.89,1.29,soso,0.53,0.38,0,none,true);arctap(31244);arctap(32033);arctap(33217);
arc(33806,34395,0.35,0.3,soso,0.76,0.92,0,none,true);arctap(33806);arctap(34395);
arc(34790,35974,0.92,0.02,s,0.61,0.68,0,none,true);arctap(34790);arctap(35974);
arc(36368,36961,0.42,0.26,s,0.91,0.89,0,none,true);arctap(36368);arctap(36961);
arc(37553,37947,1.07,0.72,so,0.37,0.76,0,none,true);arctap(37553);arctap(37947);
arc(39132,40513,1.26,0.13,si,0.23,0.56,0,none,true);arctap(39132);arctap(39526);arctap(40118);arctap(40513);
arc(40908,41895,1.07,0.57,s,0.67,0.82,0,none,true);arctap(40908);arctap(41895);
arc(42487,43454,1.12,0.29,s,0.66,0.7,0,none,true);arctap(42487);arctap(42873);arctap(43454);
arc(44046,44436,0.36,0.77,sisi,0.55,0.69,0,none,true);arctap(44046);arctap(44436);
arc(45017,45412,0.59,1.0,soso,0.54,0.71,0,none,true);arctap(45017);arctap(45412);
arc(45806,46616,0.68,0.61,so,0.92,0.06,0,none,true);arctap(45806);arctap(46616);
arc(47208,48018,0.64,0.92,s,0.55,0.83,0,none,true);arctap(47208);arctap(48018);
arc(48614,50384,0.36,0.32,b,0.3,0.59,0,none,true);arctap(48614);arctap(49009);arctap(49792);arctap(50384);
arc(50778,53541,1.24,0.52,so,0.27,0.28,0,none,true);arctap(50778);arctap(51765);arctap(52159);arctap(52949);arctap(53541);
arc(54331,55317,1.34,1.06,si,0.43,0.78,0,none,true);arctap(54331);arctap(54923);arctap(55317);
arc(55712,56699,0.52,0.27,sisi,0.79,0.5,0,none,true);arctap(55712);arctap(56106);arctap(56699);
arc(57291,58081,0.8,0.28,s,0.9,0.54,0,none,true);arctap(57291);arctap(58081);
arc(58475,59265,0.81,1.18,s,0.05,0.65,0,none,true);arctap(58475);arctap(59265);
arc(59857,61643,0.91,1.06,sosi,0.39,0.32,0,none,true);arctap(59857);arctap(60646);arctap(61249);arctap(61643);
arc(62432,63419,1.18,0.33,sosi,0.61,0.63,0,none,true);arctap(62432);arctap(63024);arctap(63419);
arc(64406,65590,1.35,0.64,soso,0.81,0.18,0,none,true);arctap(64406);arctap(64800);arctap(65590);
arc(66182,66971,0.41,1.07,s,0.52,0.69,0,none,true);arctap(66182);arctap(66971);
arc(67556,68337,0.34,0.58,sosi,0.78,0.83,0,none,true);arctap(67556);arctap(67943);arctap(68337);
arc(68732,70694,0.5,1.01,soso,0.52,0.78,0,none,true);arctap(68732);arctap(69336);arctap(69921);arctap(70694);
arc(71083,71496,0.6,0.17,b,0.62,0.25,0,none,true);arctap(71083);arctap(71496);
arc(71896,73477,0.69,1.24,s,0.59,0.54,0,none,true);arctap(71896);arctap(72492);arctap(73082);arctap(73477);
arc(74069,74859,1.3,1.39,so,0.55,0.14,0,none,true);arctap(74069);arctap(74464);arctap(74859);
arc(75254,75649,1.02,0.52,b,0.43,0.37,0,none,true);arctap(75254);arctap(75649);
arc(76241,80385,1.1,0.89,s,0.5,0.61,0,none,true);arctap(76241);arctap(76630);arctap(77207);arctap(78007);arctap(78407);arctap(78807);arctap(79990);arctap(80385);
arc(81175,81569,0.65,0.55,si,0.23,0.82,0,none,true);arctap(81175);arctap(81569);
arc(81964,83543,0.56,1.12,so,0.67,0.62,0,none,true);arctap(81964);arctap(82951);arctap(83543);
arc(84536,85105,0.47,1.04,b,0.18,0.81,0,none,true);arctap(84536);arctap(85105);

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
