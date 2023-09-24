using UnityEngine;
using TunnelTone.Elements;
using UnityEngine.Splines;
using UnityEngine.UI;

namespace TunnelTone.Charts
{
    public class LevelScanner : MonoBehaviour
    {
        /// <summary>
        /// This class is used to translate .aff files into TunnelTone charts (.ttc format in the future)
        /// </summary>
        [SerializeField] private NoteRenderer noteRenderer;
        public Sprite hitHint;
        
        private void Start()
        {
            int none = 0;

            arc(891, 965, 0.00, 0.87, Easing.s, 0.40, 0.40, 0, none, true);
            arctap(891);
            arc(891,965,1.00,0.13,Easing.s,0.40,0.40,0,none,true);
            arc(965, 1039, 0.87, 0.25, Easing.s, 0.40, 0.40, 0, none, true);
            arctap(965);
            arctap(1039);
            arc(965,1039,0.13,0.75,Easing.s,0.40,0.40,0,none,true);
            arc(1039, 1113, 0.25, 0.63, Easing.s, 0.40, 0.40, 0, none, true);
            arctap(1113);
            arc(1039,1113,0.75,0.38,Easing.s,0.40,0.40,0,none,true);
            arc(1113,1187,0.63,0.50,Easing.s,0.40,0.40,0,none,true);
            arc(1113, 1187, 0.38, 0.50, Easing.s, 0.40, 0.40, 0, none, true);
            arctap(1187);
            arc(1188,1336,0.50,1.00,Easing.sisi,0.40,0.20,0,none,true);
            arc(1188,1336,0.50,0.00,Easing.sisi,0.40,0.80,0,none,true);
            arc(1336,1485,1.00,1.00,Easing.s,0.20,0.20,0,none,true);
            arctap(1410);
            arc(1336,1485,0.00,0.00,Easing.s,0.80,0.80,0,none,true);
            arctap(1336);
            arctap(1485);
            arc(1485,1633,1.00,0.50,Easing.soso,0.20,0.40,0,none,true);
            arc(1485, 1633, 0.00, 0.50, Easing.soso, 0.80, 0.40, 0, none, true);
            arctap(1633);
            arc(1633,1781,0.50,0.00,Easing.sisi,0.40,0.20,1,none,true);
            arc(1633,1781,0.50,1.00,Easing.sisi,0.40,0.80,1,none,true);
            arc(1781,1930,1.00,1.00,Easing.s,0.80,0.80,1,none,true);
            arctap(1781);
            arctap(1930);
            arc(1781,1930,0.00,0.00,Easing.s,0.20,0.20,1,none,true);
            arctap(1855);
            arc(1930,2078,0.00,0.50,Easing.soso,0.20,0.40,1,none,true);
            arc(1930, 2078, 1.00, 0.50, Easing.soso, 0.80, 0.40, 1, none, true);
            arctap(2078);
            arc(2079,3267,0.50,1.00,Easing.sisi,0.40,0.40,0,none,true);
            arc(2079,3267,0.50,0.00,Easing.si,0.40,0.40,0,none,true);
            arc(3267,3341,0.00,0.87,Easing.s,0.40,0.40,1,none,true);
            arc(3267,3341,1.00,0.13,Easing.s,0.40,0.40,1,none,true);
            arctap(3267);
            arc(3341,3415,0.13,0.75,Easing.s,0.40,0.40,1,none,true);
            arctap(3341);
            arctap(3415);
            arc(3341,3415,0.87,0.25,Easing.s,0.40,0.40,1,none,true);
            arc(3415,3489,0.25,0.62,Easing.s,0.40,0.40,1,none,true);
            arc(3415, 3489, 0.75, 0.37, Easing.s, 0.40, 0.40, 1, none, true);
            arctap(3489);
            arc(3489,3563,0.37,0.50,Easing.s,0.40,0.40,1,none,true);
            arc(3489, 3563, 0.62, 0.50, Easing.s, 0.40, 0.40, 1, none, true);
            arctap(3563);
            arc(3564,3712,0.50,0.00,Easing.sisi,0.40,0.20,1,none,true);
            arc(3564,3712,0.50,1.00,Easing.sisi,0.40,0.80,1,none,true);
            arc(3712,3861,1.00,1.00,Easing.s,0.80,0.80,1,none,true);
            arctap(3712);
            arctap(3861);
            arc(3712,3861,0.00,0.00,Easing.s,0.20,0.20,1,none,true);
            arctap(3786);
            arc(3861,4009,0.00,0.50,Easing.soso,0.20,0.40,1,none,true);
            arc(3861, 4009, 1.00, 0.50, Easing.soso, 0.80, 0.40, 1, none, true);
            arctap(4009);
            arc(4009,4157,0.50,1.00,Easing.sisi,0.40,0.20,0,none,true);
            arc(4009,4157,0.50,0.00,Easing.sisi,0.40,0.80,0,none,true);
            arc(4157,4306,1.00,1.00,Easing.s,0.20,0.20,0,none,true);
            arctap(4231);
            arc(4157,4306,0.00,0.00,Easing.s,0.80,0.80,0,none,true);
            arctap(4157);
            arctap(4306);
            arc(4306, 4454, 0.00, 0.50, Easing.soso, 0.80, 0.40, 0, none, true);
            arctap(4454);
            arc(4306,4454,1.00,0.50,Easing.soso,0.20,0.40,0,none,true);
            arc(5643, 5717, 0.00, 0.87, Easing.s, 0.40, 0.40, 0, none, true);
            arctap(5643);
            arc(5643, 5717, 1.00, 0.13, Easing.s, 0.40, 0.40, 0, none, true);
            arctap(5717);
            arc(5717, 5791, 0.87, 0.25, Easing.s, 0.40, 0.40, 0, none, true);
            arctap(5791);
            arc(5717,5791,0.13,0.75,Easing.s,0.40,0.40,0,none,true);
            arc(5791,5865,0.25,0.63,Easing.s,0.40,0.40,0,none,true);
            arc(5791,5865,0.75,0.38,Easing.s,0.40,0.40,0,none,true);
            arc(5865,5939,0.63,0.50,Easing.s,0.40,0.40,0,none,true);
            arc(5865, 5939, 0.38, 0.50, Easing.s, 0.40, 0.40, 0, none, true);
            arctap(5866);
            arc(5940, 6089, 0.50, 0.00, Easing.sisi, 0.40, 0.80, 0, none, true);
            arctap(5940);
            arc(5940,6089,0.50,-0.10,Easing.sisi,0.40,0.20,0,none,true);
            arc(6089, 6237, -0.10, -0.10, Easing.s, 0.20, 0.20, 0, none, true);
            arctap(6163);
            arc(6089, 6237, 0.00, 0.00, Easing.s, 0.80, 0.80, 0, none, true);
            arctap(6089);
            arctap(6237);
            arc(6237,6386,-0.10,0.50,Easing.soso,0.20,0.40,0,none,true);
            arc(6237,6386,0.00,0.50,Easing.siso,0.80,0.40,0,none,true);
            arc(6386, 6535, 0.50, 1.00, Easing.sisi, 0.40, 0.80, 1, none, true);
            arctap(6386);
            arc(6386,6535,0.50,1.10,Easing.sisi,0.40,0.20,1,none,true);
            arc(6535, 6683, 1.00, 1.00, Easing.s, 0.80, 0.80, 1, none, true);
            arctap(6535);
            arctap(6683);
            arc(6535, 6683, 1.10, 1.10, Easing.s, 0.20, 0.20, 1, none, true);
            arctap(6609);
            arc(6683,6832,1.10,0.50,Easing.soso,0.20,0.40,1,none,true);
            arc(6683, 6832, 1.00, 0.50, Easing.siso, 0.80, 0.40, 1, none, true);
            arctap(6831);
            arc(8019, 8093, 1.00, 0.13, Easing.s, 0.40, 0.40, 1, none, true);
            arctap(8019);
            arc(8019, 8093, 0.00, 0.87, Easing.s, 0.40, 0.40, 1, none, true);
            arctap(8093);
            arc(8093, 8167, 0.13, 0.75, Easing.s, 0.40, 0.40, 1, none, true);
            arctap(8167);
            arc(8093,8167,0.87,0.25,Easing.s,0.40,0.40,1,none,true);
            arc(8167,8241,0.75,0.37,Easing.s,0.40,0.40,1,none,true);
            arc(8167,8241,0.25,0.62,Easing.s,0.40,0.40,1,none,true);
            arc(8241, 8315, 0.62, 0.50, Easing.s, 0.40, 0.40, 1, none, true);
            arctap(8242);
            arc(8241,8315,0.37,0.50,Easing.s,0.40,0.40,1,none,true);
            arc(8316, 8465, 0.50, 1.00, Easing.sisi, 0.40, 0.80, 1, none, true);
            arctap(8316);
            arc(8316,8465,0.50,1.10,Easing.sisi,0.40,0.20,1,none,true);
            arc(8465, 8613, 1.00, 1.00, Easing.s, 0.80, 0.80, 1, none, true);
            arctap(8465);
            arctap(8613);
            arc(8465, 8613, 1.10, 1.10, Easing.s, 0.20, 0.20, 1, none, true);
            arctap(8539);
            arc(8613,8762,1.10,0.50,Easing.soso,0.20,0.40,1,none,true);
            arc(8613,8762,1.00,0.50,Easing.siso,0.80,0.40,1,none,true);
            arc(8762, 8911, 0.50, 0.00, Easing.sisi, 0.40, 0.80, 0, none, true);
            arctap(8762);
            arc(8762,8911,0.50,-0.10,Easing.sisi,0.40,0.20,0,none,true);
            arc(8911, 9059, 0.00, 0.00, Easing.s, 0.80, 0.80, 0, none, true);
            arctap(8911);
            arctap(9059);
            arc(8911, 9059, -0.10, -0.10, Easing.s, 0.20, 0.20, 0, none, true);
            arctap(8985);
            arc(9059,9208,-0.10,0.50,Easing.soso,0.20,0.40,0,none,true);
            arc(9059, 9208, 0.00, 0.50, Easing.siso, 0.80, 0.40, 0, none, true);
            arctap(9207);
            arc(9207, 10396, 0.50, 0.50, Easing.s, 0.40, 0.40, 0, none, true);
            arctap(9504);
            arctap(9579);
            arctap(9653);
            arctap(9727);
            arctap(9801);
            arctap(9876);
            arctap(9950);
            arctap(10024);
            arctap(10099);
            arctap(10173);
            arctap(10247);
            arctap(10321);
            arctap(10396);
        }

        // ReSharper disable once IdentifierTypo
        // ReSharper disable once InconsistentNaming
        private void arctap(int time)
        {
            Spline spline = ChartDataStorage.TrailReference.GetComponent<SplineContainer>().Spline;
            Vector3 scale = new Vector3(0.6f, 0.6f, 0.6f);
            GameObject gb = new GameObject("Tap")
            {
                transform =
                {
                    parent = ChartDataStorage.TrailReference.transform,
                    localPosition = spline.EvaluatePosition((time * noteRenderer.chartSpeedModifier - spline.EvaluatePosition(0)).z / (spline.EvaluatePosition(1).z - spline.EvaluatePosition(0).z)),
                    rotation = Quaternion.Euler(0, 0, 45),
                    localScale = scale
                }
            };
            gb.AddComponent<Image>();
            Tap noteConfig = gb.AddComponent<Tap>();
            noteConfig.position = gb.transform.localPosition;
            noteConfig.time = time;
            
            ChartDataStorage.TapList.Add(gb);
        }

        // ReSharper disable once InconsistentNaming
        private void arc(int startTime, int endTime, double startX, double endX, Easing easingMode, double startY, double endY, int color, int fx, bool virtualArc)
        {
            var startPosition = new Vector2((float) startX - 0.5f, (float) startY - 0.4f);
            var endPosition = new Vector2((float) endX - 0.5f, (float) endY - 0.4f);
            EasingMode easing = easingMode switch
            {
                Easing.s => EasingMode.Straight,
                Easing.si => EasingMode.EaseIn,
                Easing.so => EasingMode.EaseOut,
                Easing.sosi => EasingMode.VerticalInHorizontalOut,
                Easing.siso => EasingMode.HorizontalInVerticalOut,
                Easing.sisi => EasingMode.EaseIn,
                Easing.soso => EasingMode.EaseOut,
                Easing.b => EasingMode.EaseInOut,
                _ => throw new System.ArgumentOutOfRangeException(nameof(easingMode), easingMode, null)
            };
            
            Direction direction = color switch
            {
                0 => Direction.Left,
                1 => Direction.Right,
                2 => throw new System.NotImplementedException(),
                _ => throw new System.ArgumentOutOfRangeException(nameof(color), color, null)
            };
            
            noteRenderer.BuildTrail(out var newGb, startTime, endTime, startPosition, endPosition, Direction.None, easing, 0.75f, true, false);
            
            ChartDataStorage.TrailList.Add(newGb);
        }
    }
}