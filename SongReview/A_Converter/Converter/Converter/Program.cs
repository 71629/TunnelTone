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
arc(1505,2232,1.08,1.04,b,0.26,0.24,0,none,true);arctap(1505);arctap(2232);
arc(2959,4414,0.9,0.57,sisi,0.76,0.25,0,none,true);arctap(2959);arctap(4414);
arc(5141,6595,0.53,0.63,sosi,0.53,0.38,0,none,true);arctap(5141);arctap(5868);arctap(6595);
arc(7323,14777,0.91,1.08,sisi,0.78,0.77,0,none,true);arctap(7323);arctap(8050);arctap(8777);arctap(10232);arctap(14777);
arc(15323,15686,0.95,0.78,s,0.82,0.34,0,none,true);arctap(15323);arctap(15686);
arc(16050,16777,0.3,0.55,s,0.38,0.65,0,none,true);arctap(16050);arctap(16414);arctap(16777);
arc(16959,17868,1.2,0.62,s,0.87,0.36,0,none,true);arctap(16959);arctap(17141);arctap(17505);arctap(17868);
arc(18232,18414,1.12,1.03,s,0.47,0.42,0,none,true);arctap(18232);arctap(18414);
arc(18595,18959,0.89,0.66,si,0.69,0.35,0,none,true);arctap(18595);arctap(18959);
arc(19686,21141,1.44,0.27,b,0.27,0.52,0,none,true);arctap(19686);arctap(20414);arctap(20595);arctap(21141);
arc(21868,24777,0.78,1.03,sisi,0.21,0.67,0,none,true);arctap(21868);arctap(22232);arctap(22595);arctap(23141);arctap(23323);arctap(23868);arctap(24050);arctap(24777);
arc(25323,25686,1.25,0.73,so,0.55,0.79,0,none,true);arctap(25323);arctap(25505);arctap(25686);
arc(25868,26595,0.95,0.42,si,0.38,0.8,0,none,true);arctap(25868);arctap(26049);arctap(26232);arctap(26595);
arc(26959,29505,0.36,0.47,soso,0.55,0.91,0,none,true);arctap(26959);arctap(29141);arctap(29505);
arc(29868,33505,1.39,1.08,si,0.78,0.59,0,none,true);arctap(29868);arctap(30414);arctap(31505);arctap(32050);arctap(33505);
arc(34050,35323,0.3,0.89,sosi,0.76,0.27,0,none,true);arctap(34050);arctap(34595);arctap(35323);
arc(35686,37323,0.79,1.32,so,0.66,0.54,0,none,true);arctap(35686);arctap(36232);arctap(37323);
arc(37505,38595,0.86,0.92,s,0.13,0.56,0,none,true);arctap(37505);arctap(37868);arctap(38595);
arc(39141,40777,1.18,1.26,so,0.7,0.74,0,none,true);arctap(39141);arctap(39686);arctap(40777);
arc(41141,42050,1.1,0.14,s,0.81,0.87,0,none,true);arctap(41141);arctap(41505);arctap(41686);arctap(42050);
arc(42232,44050,0.48,1.23,soso,0.58,0.94,0,none,true);arctap(42232);arctap(42595);arctap(42959);arctap(43323);arctap(43686);arctap(44050);
arc(44414,45868,1.35,0.44,so,0.59,0.73,0,none,true);arctap(44414);arctap(44595);arctap(44959);arctap(45141);arctap(45505);arctap(45868);
arc(46595,47323,1.25,1.0,si,0.77,0.61,0,none,true);arctap(46595);arctap(47323);
arc(48050,48777,1.37,1.39,sosi,0.74,0.32,0,none,true);arctap(48050);arctap(48414);arctap(48777);
arc(49505,50232,0.25,0.36,s,0.4,0.5,0,none,true);arctap(49505);arctap(49868);arctap(50232);
arc(50777,50959,0.68,0.6,soso,0.94,0.4,0,none,true);arctap(50777);arctap(50959);
arc(52414,54232,0.81,1.19,si,0.53,0.94,0,none,true);arctap(52414);arctap(53868);arctap(54232);
arc(54595,55323,0.66,1.05,soso,0.16,0.66,0,none,true);arctap(54595);arctap(54959);arctap(55323);
arc(55686,56777,1.26,0.93,sosi,0.7,0.31,0,none,true);arctap(55686);arctap(56050);arctap(56777);
arc(58232,60232,0.5,0.17,sisi,0.77,0.74,0,none,true);arctap(58232);arctap(59686);arctap(60232);
arc(60414,61868,0.59,0.32,so,0.61,0.67,0,none,true);arctap(60414);arctap(60959);arctap(61141);arctap(61686);arctap(61868);
arc(62232,62959,0.61,0.5,sisi,0.71,0.6,0,none,true);arctap(62232);arctap(62595);arctap(62777);arctap(62959);
arc(63323,63505,0.45,1.16,b,0.45,0.67,0,none,true);arctap(63323);arctap(63505);
arc(63687,64050,1.31,0.77,b,0.26,0.37,0,none,true);arctap(63687);arctap(64050);
arc(64595,65141,0.29,1.21,si,0.37,0.75,0,none,true);arctap(64595);arctap(65141);
arc(65505,65869,1.22,0.64,s,0.56,0.86,0,none,true);arctap(65505);arctap(65687);arctap(65869);
arc(66233,66415,1.3,0.7,soso,0.21,0.53,0,none,true);arctap(66233);arctap(66415);
arc(66595,66960,1.31,0.34,so,0.7,0.76,0,none,true);arctap(66595);arctap(66960);
arc(67505,68050,0.25,1.15,b,0.15,0.69,0,none,true);arctap(67505);arctap(68050);
arc(68414,71323,0.21,0.16,si,0.78,0.48,0,none,true);arctap(68414);arctap(68777);arctap(69141);arctap(69504);arctap(69868);arctap(70414);arctap(70959);arctap(71323);
arc(71505,71686,1.25,1.2,s,0.19,0.64,0,none,true);arctap(71505);arctap(71686);
arc(72050,72232,0.54,1.03,so,0.44,0.75,0,none,true);arctap(72050);arctap(72232);
arc(72414,72777,0.58,0.53,b,0.77,0.52,0,none,true);arctap(72414);arctap(72777);
arc(73505,73868,1.41,0.55,b,0.61,0.23,0,none,true);arctap(73505);arctap(73868);
arc(74232,74595,0.91,1.13,soso,0.45,0.94,0,none,true);arctap(74232);arctap(74414);arctap(74595);
arc(74959,75141,0.73,0.79,s,0.89,0.47,0,none,true);arctap(74959);arctap(75141);
arc(75322,75686,1.35,1.38,s,0.64,0.28,0,none,true);arctap(75322);arctap(75686);
arc(76232,78231,0.55,0.99,si,0.08,0.75,0,none,true);arctap(76232);arctap(76777);arctap(77141);arctap(77323);arctap(77504);arctap(77868);arctap(78050);arctap(78231);
arc(78595,78959,0.64,0.08,so,0.56,0.61,0,none,true);arctap(78595);arctap(78959);
arc(79595,80050,1.13,0.24,s,0.61,0.9,0,none,true);arctap(79595);arctap(80050);
arc(80777,81505,0.85,0.51,sosi,0.91,0.31,0,none,true);arctap(80777);arctap(81505);
arc(82595,83141,0.78,0.56,si,0.66,0.7,0,none,true);arctap(82595);arctap(83141);
arc(83323,85141,0.54,0.27,b,0.93,0.17,0,none,true);arctap(83323);arctap(83686);arctap(83868);arctap(84232);arctap(84414);arctap(85141);
arc(85868,86595,1.33,0.73,s,0.78,0.83,0,none,true);arctap(85868);arctap(86595);
arc(87323,88414,1.26,1.16,so,0.8,0.17,0,none,true);arctap(87323);arctap(88050);arctap(88414);
arc(88777,94595,0.63,1.23,b,0.92,0.58,0,none,true);arctap(88777);arctap(88823);arctap(94595);
arc(95323,96778,0.52,1.06,soso,0.85,0.19,0,none,true);arctap(95323);arctap(96050);arctap(96778);
arc(97505,98232,0.1,0.52,sosi,0.46,0.73,0,none,true);arctap(97505);arctap(98232);
arc(98777,101141,1.27,0.77,b,0.84,0.64,0,none,true);arctap(98777);arctap(98868);arctap(100414);arctap(100777);arctap(101141);
arc(101323,101505,1.34,1.37,si,0.52,0.89,0,none,true);arctap(101323);arctap(101505);
arc(101868,102232,1.05,0.13,sisi,0.91,0.26,0,none,true);arctap(101868);arctap(102232);
arc(102595,104050,1.09,0.17,si,0.16,0.6,0,none,true);arctap(102595);arctap(102777);arctap(102959);arctap(103323);arctap(104050);
arc(104777,106232,1.32,0.73,sisi,0.93,0.93,0,none,true);arctap(104777);arctap(104959);arctap(105505);arctap(106232);
arc(106595,106959,0.28,0.15,sisi,0.26,0.49,0,none,true);arctap(106595);arctap(106959);
arc(107322,107686,1.39,1.39,so,0.13,0.57,0,none,true);arctap(107322);arctap(107686);
arc(107868,108414,0.71,1.39,si,0.76,0.7,0,none,true);arctap(107868);arctap(108414);
arc(109141,109687,1.09,0.72,s,0.62,0.24,0,none,true);arctap(109141);arctap(109687);
arc(109869,110232,0.97,1.42,soso,0.41,0.74,0,none,true);arctap(109869);arctap(110050);arctap(110232);
arc(110413,110596,1.27,0.48,s,0.58,0.9,0,none,true);arctap(110413);arctap(110596);
arc(110959,111323,0.94,1.15,b,0.58,0.73,0,none,true);arctap(110959);arctap(111323);
arc(112414,112777,1.11,1.12,sisi,0.7,0.86,0,none,true);arctap(112414);arctap(112777);
arc(113141,113868,0.31,0.87,s,0.46,0.75,0,none,true);arctap(113141);arctap(113868);
arc(114595,115323,1.37,0.26,b,0.88,0.33,0,none,true);arctap(114595);arctap(115323);
arc(115686,116414,0.44,1.1,b,0.39,0.42,0,none,true);arctap(115686);arctap(116050);arctap(116414);
arc(116777,117141,0.96,0.61,si,0.84,0.53,0,none,true);arctap(116777);arctap(117141);
arc(117868,118595,0.53,0.84,sosi,0.81,0.28,0,none,true);arctap(117868);arctap(118232);arctap(118595);
arc(118959,121141,0.72,1.03,sosi,0.5,0.71,0,none,true);arctap(118959);arctap(119686);arctap(120414);arctap(121141);
arc(121868,123686,0.71,0.87,sosi,0.69,0.63,0,none,true);arctap(121868);arctap(123686);
arc(125141,126595,1.29,0.58,s,0.59,0.94,0,none,true);arctap(125141);arctap(126595);
arc(128050,129505,0.68,1.36,si,0.51,0.35,0,none,true);arctap(128050);arctap(129505);
arc(130959,132414,0.43,0.44,si,0.84,0.76,0,none,true);arctap(130959);arctap(132414);
arc(133141,133868,1.44,0.21,s,0.88,0.75,0,none,true);arctap(133141);arctap(133868);
arc(134959,135323,1.13,1.17,so,0.09,0.63,0,none,true);arctap(134959);arctap(135323);
arc(135687,136051,1.1,0.48,si,0.46,0.56,0,none,true);arctap(135687);arctap(136051);
arc(136595,136777,0.83,0.84,sisi,0.5,0.5,0,none,true);arctap(136595);arctap(136777);
arc(137141,137869,0.46,0.54,soso,0.38,0.36,0,none,true);arctap(137141);arctap(137505);arctap(137869);
arc(138232,138595,0.11,0.77,sosi,0.22,0.79,0,none,true);arctap(138232);arctap(138595);
arc(138959,139505,1.41,0.36,si,0.9,0.42,0,none,true);arctap(138959);arctap(139505);
arc(139686,140414,0.27,0.59,sisi,0.49,0.23,0,none,true);arctap(139686);arctap(140050);arctap(140414);
arc(140778,142414,0.8,0.63,s,0.36,0.77,0,none,true);arctap(140778);arctap(142414);
arc(142595,144232,0.97,0.59,sisi,0.57,0.32,0,none,true);arctap(142595);arctap(144232);
arc(144595,145141,1.11,0.59,si,0.94,0.52,0,none,true);arctap(144595);arctap(144959);arctap(145141);
arc(145505,146959,0.32,0.83,s,0.34,0.52,0,none,true);arctap(145505);arctap(146959);
arc(147686,176050,1.21,0.75,s,0.63,0.62,0,none,true);arctap(147686);arctap(148050);arctap(153868);arctap(158595);arctap(161505);arctap(170232);arctap(170277);arctap(176050);
arc(176414,177141,0.39,1.07,so,0.71,0.13,0,none,true);arctap(176414);arctap(176777);arctap(177141);
arc(177505,178777,0.72,0.54,so,0.57,0.58,0,none,true);arctap(177505);arctap(178777);
arc(178959,179323,0.83,0.89,sisi,0.52,0.58,0,none,true);arctap(178959);arctap(179323);
arc(179686,180414,0.57,0.5,soso,0.63,0.79,0,none,true);arctap(179686);arctap(180050);arctap(180414);
arc(181868,183323,0.76,0.62,si,0.47,0.69,0,none,true);arctap(181868);arctap(182232);arctap(182595);arctap(182959);arctap(183323);
arc(184777,185141,0.52,0.41,soso,0.68,0.89,0,none,true);arctap(184777);arctap(185141);
arc(185868,188050,0.29,1.07,b,0.48,0.81,0,none,true);arctap(185868);arctap(186414);arctap(186959);arctap(187686);arctap(188050);
arc(188414,188595,0.77,0.77,sosi,0.24,0.56,0,none,true);arctap(188414);arctap(188595);
arc(188777,188958,1.28,0.4,sosi,0.93,0.17,0,none,true);arctap(188777);arctap(188958);
arc(189141,190777,0.56,1.16,sosi,0.75,0.54,0,none,true);arctap(189141);arctap(189505);arctap(189868);arctap(190777);
arc(190959,191323,1.35,1.37,s,0.47,0.68,0,none,true);arctap(190959);arctap(191323);
arc(191505,191868,0.77,1.31,s,0.75,0.05,0,none,true);arctap(191505);arctap(191868);
arc(192050,194959,0.31,0.83,sosi,0.65,0.63,0,none,true);arctap(192050);arctap(194959);
arc(197868,197914,0.98,0.32,s,0.88,0.72,0,none,true);arctap(197868);arctap(197914);
arc(203686,204414,0.28,1.04,sisi,0.77,0.82,0,none,true);arctap(203686);arctap(204414);
arc(205141,207323,1.15,0.88,so,0.66,0.32,0,none,true);arctap(205141);arctap(205869);arctap(206595);arctap(207323);
arc(208050,209505,0.45,0.53,so,0.32,0.85,0,none,true);arctap(208050);arctap(208777);arctap(209505);
arc(210959,213868,0.46,0.85,sosi,0.81,0.82,0,none,true);arctap(210959);arctap(212414);arctap(213141);arctap(213868);
arc(215323,215687,0.26,0.93,sosi,0.56,0.49,0,none,true);arctap(215323);arctap(215505);arctap(215687);
arc(216050,216232,0.88,0.95,si,0.64,0.92,0,none,true);arctap(216050);arctap(216232);
arc(216414,216778,0.82,1.17,b,0.13,0.49,0,none,true);arctap(216414);arctap(216778);
arc(217323,217869,1.44,1.3,sosi,0.23,0.65,0,none,true);arctap(217323);arctap(217869);
arc(218233,218959,0.99,0.96,soso,0.74,0.69,0,none,true);arctap(218233);arctap(218415);arctap(218595);arctap(218959);
arc(219141,219686,1.14,1.01,s,0.37,0.81,0,none,true);arctap(219141);arctap(219323);arctap(219686);
arc(220233,220778,0.58,0.83,sisi,0.32,0.48,0,none,true);arctap(220233);arctap(220778);
arc(221141,223686,0.67,0.79,b,0.48,0.6,0,none,true);arctap(221141);arctap(221505);arctap(221867);arctap(222232);arctap(222596);arctap(223142);arctap(223686);
arc(224050,224232,1.22,0.62,b,0.27,0.56,0,none,true);arctap(224050);arctap(224232);
arc(224414,224777,1.23,1.0,si,0.39,0.88,0,none,true);arctap(224414);arctap(224777);
arc(224959,227141,1.43,0.98,s,0.73,0.42,0,none,true);arctap(224959);arctap(225141);arctap(225504);arctap(226232);arctap(226595);arctap(226959);arctap(227141);
arc(227322,227686,0.65,0.36,so,0.79,0.48,0,none,true);arctap(227322);arctap(227686);
arc(227868,228049,0.59,0.7,s,0.21,0.72,0,none,true);arctap(227868);arctap(228049);
arc(228414,228960,1.24,0.56,so,0.59,0.74,0,none,true);arctap(228414);arctap(228960);
arc(229505,230232,1.15,0.86,si,0.31,0.32,0,none,true);arctap(229505);arctap(229869);arctap(230051);arctap(230232);
arc(230595,230958,0.44,1.08,si,0.39,0.8,0,none,true);arctap(230595);arctap(230777);arctap(230958);
arc(231323,232778,1.4,1.11,soso,0.35,0.48,0,none,true);arctap(231323);arctap(231687);arctap(232323);arctap(232778);
arc(233505,236051,1.43,1.09,b,0.55,0.66,0,none,true);arctap(233505);arctap(234232);arctap(235323);arctap(235869);arctap(236051);
arc(236414,237142,1.18,1.24,b,0.67,0.12,0,none,true);arctap(236414);arctap(236596);arctap(236960);arctap(237142);
arc(237869,239323,0.55,1.43,s,0.65,0.23,0,none,true);arctap(237869);arctap(238596);arctap(239323);
arc(240051,240777,0.87,0.99,s,0.34,0.73,0,none,true);arctap(240051);arctap(240777);
arc(241141,242595,1.06,1.05,s,0.79,0.21,0,none,true);arctap(241141);arctap(241505);arctap(241868);arctap(242414);arctap(242595);
arc(243141,243323,0.46,1.14,si,0.89,0.83,0,none,true);arctap(243141);arctap(243323);
arc(243686,244414,1.2,1.41,s,0.3,0.33,0,none,true);arctap(243686);arctap(244414);
arc(245141,248777,0.5,1.37,soso,0.88,0.33,0,none,true);arctap(245141);arctap(245869);arctap(248777);
arc(253322,253868,1.07,0.37,b,0.53,0.42,0,none,true);arctap(253322);arctap(253868);
arc(254231,254595,0.86,0.75,s,0.84,0.69,0,none,true);arctap(254231);arctap(254595);
arc(254958,255322,0.47,1.24,so,0.28,0.12,0,none,true);arctap(254958);arctap(255322);
arc(255504,256958,0.49,0.63,s,0.4,0.58,0,none,true);arctap(255504);arctap(255686);arctap(256049);arctap(256413);arctap(256776);arctap(256958);
arc(257140,261140,0.35,0.57,s,0.5,0.64,0,none,true);arctap(257140);arctap(257504);arctap(258231);arctap(258958);arctap(259140);arctap(259686);arctap(260413);arctap(260776);arctap(261140);
arc(261505,262595,0.93,0.53,so,0.57,0.67,0,none,true);arctap(261505);arctap(261867);arctap(262049);arctap(262595);
arc(263322,263868,1.29,0.55,s,0.67,0.09,0,none,true);arctap(263322);arctap(263868);
arc(264050,264777,0.63,0.64,s,0.72,0.92,0,none,true);arctap(264050);arctap(264231);arctap(264413);arctap(264594);arctap(264777);

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
