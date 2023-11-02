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
arc(1685,3214,1.22,0.54,so,0.94,0.08,0,none,true);arctap(1685);arctap(2435);arctap(3214);
arc(4807,7966,0.63,0.33,so,0.12,0.74,0,none,true);arctap(4807);arctap(5607);arctap(6407);arctap(7966);
arc(8766,9555,0.15,0.42,si,0.8,0.68,0,none,true);arctap(8766);arctap(9555);
arc(11113,12494,0.05,0.49,s,0.88,0.43,0,none,true);arctap(11113);arctap(11902);arctap(12494);
arc(13481,14867,1.03,1.0,so,0.61,0.67,0,none,true);arctap(13481);arctap(14078);arctap(14867);
arc(15261,15656,1.18,0.57,s,0.39,0.76,0,none,true);arctap(15261);arctap(15656);
arc(16840,19783,0.44,0.48,so,0.02,0.5,0,none,true);arctap(16840);arctap(17235);arctap(18019);arctap(18411);arctap(18803);arctap(19783);
arc(20176,20568,0.51,0.22,soso,0.9,0.33,0,none,true);arctap(20176);arctap(20568);
arc(21573,22379,0.79,0.11,b,0.44,0.88,0,none,true);arctap(21573);arctap(22379);
arc(22777,23759,0.97,0.89,so,0.58,0.52,0,none,true);arctap(22777);arctap(23367);arctap(23759);
arc(24935,27898,0.69,0.93,soso,0.58,0.69,0,none,true);arctap(24935);arctap(25327);arctap(26504);arctap(26912);arctap(27898);
arc(28688,29083,0.38,1.43,s,0.18,0.85,0,none,true);arctap(28688);arctap(29083);
arc(29675,31244,1.3,0.77,si,0.59,0.34,0,none,true);arctap(29675);arctap(30060);arctap(30454);arctap(31244);
arc(32033,33806,0.64,1.33,soso,0.25,0.61,0,none,true);arctap(32033);arctap(33217);arctap(33806);
arc(34395,34790,1.27,1.39,s,0.85,0.7,0,none,true);arctap(34395);arctap(34790);
arc(35974,36961,0.69,0.87,sosi,0.68,0.91,0,none,true);arctap(35974);arctap(36368);arctap(36961);
arc(37553,37947,0.8,0.87,sosi,0.89,0.66,0,none,true);arctap(37553);arctap(37947);
arc(39132,41895,1.08,0.53,soso,0.81,0.67,0,none,true);arctap(39132);arctap(39526);arctap(40118);arctap(40513);arctap(40908);arctap(41895);
arc(42487,42873,0.31,0.57,s,0.63,0.51,0,none,true);arctap(42487);arctap(42873);
arc(43454,45017,1.11,0.52,sisi,0.92,0.4,0,none,true);arctap(43454);arctap(44046);arctap(44436);arctap(45017);
arc(45412,45806,0.88,1.15,b,0.85,0.87,0,none,true);arctap(45412);arctap(45806);
arc(46616,47208,1.27,0.39,sosi,0.67,0.49,0,none,true);arctap(46616);arctap(47208);
arc(48018,49792,1.07,1.31,b,0.79,0.94,0,none,true);arctap(48018);arctap(48614);arctap(49009);arctap(49792);
arc(50384,50778,0.7,1.35,s,0.65,0.45,0,none,true);arctap(50384);arctap(50778);
arc(51765,52949,0.72,1.19,sisi,0.66,0.4,0,none,true);arctap(51765);arctap(52159);arctap(52949);
arc(53541,55317,0.41,0.92,so,0.57,0.44,0,none,true);arctap(53541);arctap(54331);arctap(54923);arctap(55317);
arc(55712,56106,0.7,1.38,sosi,0.38,0.91,0,none,true);arctap(55712);arctap(56106);
arc(56699,57291,0.78,1.37,soso,0.21,0.52,0,none,true);arctap(56699);arctap(57291);
arc(58081,58475,0.56,0.27,so,0.6,0.13,0,none,true);arctap(58081);arctap(58475);
arc(59265,59857,1.23,0.55,s,0.66,0.43,0,none,true);arctap(59265);arctap(59857);
arc(60646,61249,0.75,0.45,soso,0.7,0.4,0,none,true);arctap(60646);arctap(61249);
arc(61643,63024,0.35,0.99,b,0.39,0.91,0,none,true);arctap(61643);arctap(62432);arctap(63024);
arc(63419,64800,1.03,0.55,sisi,0.42,0.61,0,none,true);arctap(63419);arctap(64406);arctap(64800);
arc(65590,66971,0.84,0.65,b,0.5,0.92,0,none,true);arctap(65590);arctap(66182);arctap(66971);
arc(67556,67943,1.27,0.9,b,0.6,0.39,0,none,true);arctap(67556);arctap(67943);
arc(68337,68732,1.27,1.07,sisi,0.63,0.6,0,none,true);arctap(68337);arctap(68732);
arc(69336,71896,0.9,0.71,b,0.56,0.61,0,none,true);arctap(69336);arctap(69921);arctap(70694);arctap(71083);arctap(71496);arctap(71896);
arc(72492,73082,1.22,0.22,si,0.88,0.23,0,none,true);arctap(72492);arctap(73082);
arc(73477,74464,0.21,0.63,so,0.6,0.84,0,none,true);arctap(73477);arctap(74069);arctap(74464);
arc(74859,75649,1.0,1.2,b,0.64,0.44,0,none,true);arctap(74859);arctap(75254);arctap(75649);
arc(76241,81175,0.68,0.87,s,0.38,0.89,0,none,true);arctap(76241);arctap(76630);arctap(77207);arctap(78007);arctap(78407);arctap(78807);arctap(79990);arctap(80385);arctap(81175);
arc(81569,81964,1.08,1.08,si,0.9,0.48,0,none,true);arctap(81569);arctap(81964);

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
