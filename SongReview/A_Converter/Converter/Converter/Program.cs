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
arc(1685,3214,0.73,1.03,sosi,0.73,0.71,0,none,true);arctap(1685);arctap(2435);arctap(3214);
arc(4807,5607,1.07,1.17,b,0.71,0.62,0,none,true);arctap(4807);arctap(5607);
arc(6407,7966,0.38,0.86,sisi,0.78,0.88,0,none,true);arctap(6407);arctap(7966);
arc(8766,11113,0.59,0.91,b,0.52,0.81,0,none,true);arctap(8766);arctap(9555);arctap(11113);
arc(11902,12494,0.37,1.12,s,0.53,0.67,0,none,true);arctap(11902);arctap(12494);
arc(13481,14867,1.28,0.28,b,0.54,0.92,0,none,true);arctap(13481);arctap(14078);arctap(14867);
arc(15261,15656,1.25,0.59,sisi,0.83,0.18,0,none,true);arctap(15261);arctap(15656);
arc(16840,18803,1.1,0.6,sisi,0.65,0.92,0,none,true);arctap(16840);arctap(17235);arctap(18019);arctap(18411);arctap(18803);
arc(19783,20568,0.87,0.49,sisi,0.21,0.77,0,none,true);arctap(19783);arctap(20176);arctap(20568);
arc(21573,22379,1.38,0.77,sisi,0.72,0.93,0,none,true);arctap(21573);arctap(22379);
arc(22777,23367,0.68,1.09,sosi,0.36,0.68,0,none,true);arctap(22777);arctap(23367);
arc(23759,26912,1.15,1.43,s,0.24,0.32,0,none,true);arctap(23759);arctap(24935);arctap(25327);arctap(26504);arctap(26912);
arc(27898,29083,0.37,0.92,s,0.63,0.79,0,none,true);arctap(27898);arctap(28688);arctap(29083);
arc(29675,30060,0.65,0.89,b,0.42,0.77,0,none,true);arctap(29675);arctap(30060);
arc(30454,33806,0.91,0.59,sisi,0.86,0.25,0,none,true);arctap(30454);arctap(31244);arctap(32033);arctap(33217);arctap(33806);
arc(34395,34790,0.53,0.55,sisi,0.51,0.24,0,none,true);arctap(34395);arctap(34790);
arc(35974,36961,1.19,0.85,sosi,0.89,0.43,0,none,true);arctap(35974);arctap(36368);arctap(36961);
arc(37553,37947,0.15,0.88,si,0.49,0.9,0,none,true);arctap(37553);arctap(37947);
arc(39132,39526,1.32,0.98,s,0.76,0.63,0,none,true);arctap(39132);arctap(39526);
arc(40118,40513,0.95,0.22,sisi,0.18,0.92,0,none,true);arctap(40118);arctap(40513);
arc(40908,41895,1.3,1.14,sisi,0.78,0.53,0,none,true);arctap(40908);arctap(41895);
arc(42487,43454,1.35,0.18,b,0.24,0.38,0,none,true);arctap(42487);arctap(42873);arctap(43454);
arc(44046,45806,0.93,0.56,s,0.69,0.9,0,none,true);arctap(44046);arctap(44436);arctap(45017);arctap(45412);arctap(45806);
arc(46616,48018,0.15,1.26,s,0.83,0.4,0,none,true);arctap(46616);arctap(47208);arctap(48018);
arc(48614,49792,1.4,0.3,soso,0.29,0.45,0,none,true);arctap(48614);arctap(49009);arctap(49792);
arc(50384,52949,0.71,1.11,s,0.32,0.61,0,none,true);arctap(50384);arctap(50778);arctap(51765);arctap(52159);arctap(52949);
arc(53541,54331,1.37,0.98,b,0.32,0.06,0,none,true);arctap(53541);arctap(54331);
arc(54923,55317,0.38,0.96,s,0.63,0.24,0,none,true);arctap(54923);arctap(55317);
arc(55712,56106,0.34,0.75,soso,0.69,0.92,0,none,true);arctap(55712);arctap(56106);
arc(56699,57291,1.43,1.01,s,0.83,0.63,0,none,true);arctap(56699);arctap(57291);
arc(58081,58475,1.37,0.47,s,0.81,0.91,0,none,true);arctap(58081);arctap(58475);
arc(59265,59857,1.16,0.74,s,0.61,0.51,0,none,true);arctap(59265);arctap(59857);
arc(60646,62432,0.61,0.86,s,0.65,0.86,0,none,true);arctap(60646);arctap(61249);arctap(61643);arctap(62432);
arc(63024,63419,0.74,0.09,s,0.43,0.66,0,none,true);arctap(63024);arctap(63419);
arc(64406,66182,1.02,1.06,s,0.87,0.15,0,none,true);arctap(64406);arctap(64800);arctap(65590);arctap(66182);
arc(66971,67556,0.61,0.55,b,0.93,0.41,0,none,true);arctap(66971);arctap(67556);
arc(67943,68732,1.05,0.62,sosi,0.77,0.23,0,none,true);arctap(67943);arctap(68337);arctap(68732);
arc(69336,75254,1.22,0.56,sosi,0.69,0.6,0,none,true);arctap(69336);arctap(69921);arctap(70694);arctap(71083);arctap(71496);arctap(71896);arctap(72492);arctap(73082);arctap(73477);arctap(74069);arctap(74464);arctap(74859);arctap(75254);
arc(75649,76630,0.93,1.01,s,0.4,0.61,0,none,true);arctap(75649);arctap(76241);arctap(76630);
arc(77207,78007,0.49,1.05,s,0.35,0.72,0,none,true);arctap(77207);arctap(78007);
arc(78407,79990,0.45,0.83,sosi,0.38,0.91,0,none,true);arctap(78407);arctap(78807);arctap(79990);
arc(80385,81175,0.97,0.79,s,0.61,0.88,0,none,true);arctap(80385);arctap(81175);
arc(81569,82951,0.89,1.41,sisi,0.52,0.59,0,none,true);arctap(81569);arctap(81964);arctap(82951);
arc(83543,84536,0.57,0.37,sosi,0.42,0.5,0,none,true);arctap(83543);arctap(84536);
arc(85105,85302,1.33,0.33,b,0.83,0.77,0,none,true);arctap(85105);arctap(85302);

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
