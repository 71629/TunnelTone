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
arc(1685,2435,1.31,1.0,si,0.32,0.84,0,none,true);arctap(1685);arctap(2435);
arc(3214,6407,1.15,0.9,si,0.58,0.83,0,none,true);arctap(3214);arctap(4807);arctap(5607);arctap(6407);
arc(7966,8766,1.01,1.28,b,0.76,0.57,0,none,true);arctap(7966);arctap(8766);
arc(9555,11113,0.63,0.83,sosi,0.9,0.9,0,none,true);arctap(9555);arctap(11113);
arc(11902,14078,1.15,1.23,so,0.87,0.47,0,none,true);arctap(11902);arctap(12494);arctap(13481);arctap(14078);
arc(14867,15261,0.15,0.89,si,0.26,0.46,0,none,true);arctap(14867);arctap(15261);
arc(15656,18019,1.36,0.1,soso,0.43,0.4,0,none,true);arctap(15656);arctap(16840);arctap(17235);arctap(18019);
arc(18411,20176,1.29,0.24,sisi,0.53,0.29,0,none,true);arctap(18411);arctap(18803);arctap(19783);arctap(20176);
arc(20568,21573,0.39,0.83,soso,0.81,0.61,0,none,true);arctap(20568);arctap(21573);
arc(22379,22777,0.24,0.86,so,0.87,0.34,0,none,true);arctap(22379);arctap(22777);
arc(23367,24935,1.34,0.94,sosi,0.3,0.69,0,none,true);arctap(23367);arctap(23759);arctap(24935);
arc(25327,26504,1.18,1.34,b,0.8,0.74,0,none,true);arctap(25327);arctap(26504);
arc(26912,27898,0.99,0.83,sisi,0.39,0.48,0,none,true);arctap(26912);arctap(27898);
arc(28688,29083,0.43,0.77,sisi,0.93,0.46,0,none,true);arctap(28688);arctap(29083);
arc(29675,30454,1.29,1.42,sisi,0.88,0.79,0,none,true);arctap(29675);arctap(30060);arctap(30454);
arc(31244,32033,0.78,1.03,s,0.17,0.55,0,none,true);arctap(31244);arctap(32033);
arc(33217,33806,0.34,1.14,so,0.41,0.1,0,none,true);arctap(33217);arctap(33806);
arc(34395,35974,0.88,0.58,so,0.1,0.64,0,none,true);arctap(34395);arctap(34790);arctap(35974);
arc(36368,36961,0.45,1.18,sosi,0.64,0.25,0,none,true);arctap(36368);arctap(36961);
arc(37553,37947,1.07,0.91,s,0.3,0.76,0,none,true);arctap(37553);arctap(37947);
arc(39132,39526,0.47,0.44,s,0.62,0.53,0,none,true);arctap(39132);arctap(39526);
arc(40118,40513,1.19,0.46,sosi,0.11,0.93,0,none,true);arctap(40118);arctap(40513);
arc(40908,42487,1.19,0.54,so,0.52,0.71,0,none,true);arctap(40908);arctap(41895);arctap(42487);
arc(42873,43454,1.32,0.42,s,0.61,0.58,0,none,true);arctap(42873);arctap(43454);
arc(44046,45017,0.49,1.26,si,0.93,0.65,0,none,true);arctap(44046);arctap(44436);arctap(45017);
arc(45412,46616,0.77,0.73,sosi,0.79,0.57,0,none,true);arctap(45412);arctap(45806);arctap(46616);
arc(47208,48614,1.39,1.08,s,0.75,0.84,0,none,true);arctap(47208);arctap(48018);arctap(48614);
arc(49009,49792,0.4,0.76,si,0.23,0.46,0,none,true);arctap(49009);arctap(49792);
arc(50384,51765,0.31,1.32,si,0.86,0.73,0,none,true);arctap(50384);arctap(50778);arctap(51765);
arc(52159,52949,1.04,0.47,sisi,0.64,0.68,0,none,true);arctap(52159);arctap(52949);
arc(53541,56699,1.31,0.96,s,0.6,0.29,0,none,true);arctap(53541);arctap(54331);arctap(54923);arctap(55317);arctap(55712);arctap(56106);arctap(56699);
arc(57291,58475,1.22,0.42,soso,0.73,0.58,0,none,true);arctap(57291);arctap(58081);arctap(58475);
arc(59265,61249,1.25,0.31,si,0.57,0.29,0,none,true);arctap(59265);arctap(59857);arctap(60646);arctap(61249);
arc(61643,62432,0.53,1.36,soso,0.13,0.45,0,none,true);arctap(61643);arctap(62432);
arc(63024,64406,0.06,0.85,soso,0.57,0.9,0,none,true);arctap(63024);arctap(63419);arctap(64406);
arc(64800,66182,0.28,0.95,s,0.58,0.9,0,none,true);arctap(64800);arctap(65590);arctap(66182);
arc(66971,69921,0.77,0.52,soso,0.41,0.48,0,none,true);arctap(66971);arctap(67556);arctap(67943);arctap(68337);arctap(68732);arctap(69336);arctap(69921);
arc(70694,71496,0.93,0.57,sisi,0.51,0.71,0,none,true);arctap(70694);arctap(71083);arctap(71496);
arc(71896,73477,0.31,0.89,si,0.58,0.38,0,none,true);arctap(71896);arctap(72492);arctap(73082);arctap(73477);
arc(74069,75254,1.27,0.81,soso,0.64,0.46,0,none,true);arctap(74069);arctap(74464);arctap(74859);arctap(75254);
arc(75649,76241,1.36,0.62,so,0.55,0.4,0,none,true);arctap(75649);arctap(76241);
arc(76630,78007,0.56,1.27,soso,0.78,0.88,0,none,true);arctap(76630);arctap(77207);arctap(78007);
arc(78407,78807,0.73,1.39,si,0.39,0.47,0,none,true);arctap(78407);arctap(78807);
arc(79990,81175,1.44,0.36,sisi,0.22,0.79,0,none,true);arctap(79990);arctap(80385);arctap(81175);
arc(81569,82951,1.43,1.18,so,0.81,0.8,0,none,true);arctap(81569);arctap(81964);arctap(82951);
arc(83543,84536,1.36,1.05,soso,0.22,0.25,0,none,true);arctap(83543);arctap(84536);
arc(85105,85302,0.24,0.76,s,0.6,0.88,0,none,true);arctap(85105);arctap(85302);

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