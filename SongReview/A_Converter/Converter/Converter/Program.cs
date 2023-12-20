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
arc(1685,2435,1.17,0.66,sisi,0.87,0.76,0,none,true);arctap(1685);arctap(2435);
arc(3214,4807,0.9,0.61,s,0.05,0.48,0,none,true);arctap(3214);arctap(4807);
arc(5607,6407,1.16,0.64,s,0.57,0.3,0,none,true);arctap(5607);arctap(6407);
arc(7966,9555,1.13,1.05,soso,0.42,0.47,0,none,true);arctap(7966);arctap(8766);arctap(9555);
arc(11113,13481,0.9,0.92,s,0.6,0.94,0,none,true);arctap(11113);arctap(11902);arctap(12494);arctap(13481);
arc(14078,16840,0.94,1.31,sisi,0.64,0.66,0,none,true);arctap(14078);arctap(14867);arctap(15261);arctap(15656);arctap(16840);
arc(17235,18019,1.33,0.33,so,0.29,0.39,0,none,true);arctap(17235);arctap(18019);
arc(18411,20176,0.74,0.25,so,0.75,0.69,0,none,true);arctap(18411);arctap(18803);arctap(19783);arctap(20176);
arc(20568,21573,0.44,0.6,si,0.41,0.61,0,none,true);arctap(20568);arctap(21573);
arc(22379,23367,0.29,0.22,si,0.89,0.54,0,none,true);arctap(22379);arctap(22777);arctap(23367);
arc(23759,24935,0.8,0.56,soso,0.64,0.58,0,none,true);arctap(23759);arctap(24935);
arc(25327,26504,0.99,0.84,so,0.29,0.81,0,none,true);arctap(25327);arctap(26504);
arc(26912,28688,0.43,0.51,s,0.51,0.48,0,none,true);arctap(26912);arctap(27898);arctap(28688);
arc(29083,29675,1.31,0.42,sisi,0.59,0.9,0,none,true);arctap(29083);arctap(29675);
arc(30060,33217,0.89,0.79,sosi,0.59,0.75,0,none,true);arctap(30060);arctap(30454);arctap(31244);arctap(32033);arctap(33217);
arc(33806,34395,0.69,0.94,si,0.6,0.76,0,none,true);arctap(33806);arctap(34395);
arc(34790,36961,0.91,0.94,b,0.3,0.77,0,none,true);arctap(34790);arctap(35974);arctap(36368);arctap(36961);
arc(37553,37947,0.66,1.26,s,0.21,0.84,0,none,true);arctap(37553);arctap(37947);
arc(39132,40908,0.05,1.32,sosi,0.27,0.57,0,none,true);arctap(39132);arctap(39526);arctap(40118);arctap(40513);arctap(40908);
arc(41895,42873,1.41,1.35,si,0.94,0.48,0,none,true);arctap(41895);arctap(42487);arctap(42873);
arc(43454,44046,1.12,1.08,s,0.74,0.27,0,none,true);arctap(43454);arctap(44046);
arc(44436,45017,0.37,0.58,so,0.48,0.86,0,none,true);arctap(44436);arctap(45017);
arc(45412,47208,0.44,0.4,s,0.31,0.78,0,none,true);arctap(45412);arctap(45806);arctap(46616);arctap(47208);
arc(48018,48614,1.12,1.28,soso,0.76,0.8,0,none,true);arctap(48018);arctap(48614);
arc(49009,50384,1.09,1.37,si,0.27,0.53,0,none,true);arctap(49009);arctap(49792);arctap(50384);
arc(50778,52159,1.03,0.53,b,0.8,0.57,0,none,true);arctap(50778);arctap(51765);arctap(52159);
arc(52949,53541,0.79,1.31,sisi,0.3,0.7,0,none,true);arctap(52949);arctap(53541);
arc(54331,54923,0.81,0.73,b,0.35,0.79,0,none,true);arctap(54331);arctap(54923);
arc(55317,55712,0.76,0.96,so,0.25,0.69,0,none,true);arctap(55317);arctap(55712);
arc(56106,56699,0.76,0.67,sisi,0.52,0.72,0,none,true);arctap(56106);arctap(56699);
arc(57291,59265,0.69,0.72,soso,0.5,0.57,0,none,true);arctap(57291);arctap(58081);arctap(58475);arctap(59265);
arc(59857,62432,0.85,1.18,si,0.6,0.55,0,none,true);arctap(59857);arctap(60646);arctap(61249);arctap(61643);arctap(62432);
arc(63024,65590,1.34,1.09,so,0.49,0.79,0,none,true);arctap(63024);arctap(63419);arctap(64406);arctap(64800);arctap(65590);
arc(66182,67943,1.1,0.32,so,0.53,0.69,0,none,true);arctap(66182);arctap(66971);arctap(67556);arctap(67943);
arc(68337,68732,1.23,1.17,sisi,0.52,0.43,0,none,true);arctap(68337);arctap(68732);
arc(69336,69921,0.64,0.58,b,0.43,0.66,0,none,true);arctap(69336);arctap(69921);
arc(70694,71496,0.07,1.19,b,0.41,0.9,0,none,true);arctap(70694);arctap(71083);arctap(71496);
arc(71896,74859,0.44,0.25,si,0.77,0.64,0,none,true);arctap(71896);arctap(72492);arctap(73082);arctap(73477);arctap(74069);arctap(74464);arctap(74859);
arc(75254,75649,0.39,0.68,sisi,0.47,0.67,0,none,true);arctap(75254);arctap(75649);
arc(76241,76630,0.49,0.38,so,0.51,0.77,0,none,true);arctap(76241);arctap(76630);
arc(77207,78007,0.93,1.4,s,0.8,0.4,0,none,true);arctap(77207);arctap(78007);
arc(78407,78807,1.31,0.41,so,0.75,0.92,0,none,true);arctap(78407);arctap(78807);
arc(79990,80385,1.11,1.02,so,0.48,0.79,0,none,true);arctap(79990);arctap(80385);
arc(81175,81569,0.13,0.59,soso,0.72,0.54,0,none,true);arctap(81175);arctap(81569);
arc(81964,83543,0.88,0.43,so,0.79,0.91,0,none,true);arctap(81964);arctap(82951);arctap(83543);
arc(84536,85105,0.66,0.33,sisi,0.32,0.62,0,none,true);arctap(84536);arctap(85105);

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
