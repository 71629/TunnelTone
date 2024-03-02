using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TunnelTone.Core;
using TunnelTone.Elements;
using TunnelTone.Events;
using TunnelTone.PlayArea;
using TunnelTone.UI.SongList;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

// ReSharper disable InconsistentNaming
namespace TunnelTone.Charts
{
    public class JsonScanner : MonoBehaviour
    {
        private static NoteRenderer NoteRenderer => NoteRenderer.Instance;
        private Chart chartCache;
        [SerializeField] private GameObject trailPrefab;

        private void Start()
        {
            SongListManager.MusicPlayInitialize += ScanChart;
            
            ChartEventReference.Instance.OnRetry.AddListener(delegate
            {
                StartCoroutine(Retry());
            });
            ChartEventReference.Instance.OnQuit.AddListener(delegate { chartCache = new Chart(); });
        }

        private void ScanChart(ref MusicPlayDescription mpd)
        {
            var chart = JsonConvert.DeserializeObject<Chart>(mpd.chart.chart.text);
            chartCache = chart;
            TimingManager.timingSheet = mpd.chart.timingSheet;
            
            NoteRenderer.ResetContainer();
            StartCoroutine(CreateElement(chart));
        }
        
        private static async Task<bool> LoadAudioData(AudioClip audioClip, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!audioClip.LoadAudioData())
                return false;

            while (audioClip.loadState == AudioDataLoadState.Loading)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }

            return true;
        }

        private IEnumerator Retry()
        {
            yield return new WaitForSecondsRealtime(.5f); 
            NoteRenderer.ResetContainer();
            StartCoroutine(CreateElement(chartCache));
        }

        private IEnumerator CreateElement(Chart chart, AudioClip audioClip = null)
        {
            yield return new WaitForSecondsRealtime(1f);
            var timer = new Stopwatch();
            
            NoteRenderer.TrailList.Clear();
            ScoreManager.totalCombo = 0;
            foreach(var trail in chart.trails)
            {
                timer.Start();
                
                var gb = Instantiate(trailPrefab, transform);
                gb.layer = 11;
                gb.transform.parent = transform;
                gb.transform.localPosition = Vector3.zero;
                gb.transform.rotation = Quaternion.identity;
                gb.transform.localScale = Vector3.one;
                
                var component = gb.GetComponent<Trail>().Initialize(trail.startTime, trail.endTime,
                    new Vector2((float)trail.startX - 0.5f, (float)trail.startY - 0.4f),
                    new Vector2((float)trail.endX - 0.5f, (float)trail.endY - 0.4f), 
                    directionDictionary[trail.color],
                    easingDictionary[trail.easing], 
                    trail.easingRatio, 
                    true, 
                    trail.virtualTrail);

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                foreach (var c in NoteRenderer.TrailList.Select(q => q.GetComponent<Trail>()).Where(c => component.startTime == c.endTime && component.startCoordinate == c.endCoordinate))
                {
                    component.next = c;
                    c.skipSpawnAnimation = false;
                }
                
                NoteRenderer.TrailList.Add(gb);
                foreach(var tap in trail.taps)
                {
                    var spline = NoteRenderer.TrailReference.GetComponent<SplineContainer>().Spline;
                    var scale = new Vector3(0.6f, 0.6f, 0.6f);
                    var tgb = new GameObject("Tap")
                    {
                        transform =
                        {
                            parent = NoteRenderer.TrailReference.transform,
                            localPosition = spline.EvaluatePosition((tap.time * NoteRenderer.chartSpeedModifier - spline.EvaluatePosition(0)).z / (spline.EvaluatePosition(1).z - spline.EvaluatePosition(0).z)),
                            rotation = Quaternion.Euler(0, 0, 45),
                            localScale = scale
                        }
                    };
                    tgb.AddComponent<Image>();
                    var noteConfig = tgb.AddComponent<Tap>();
                    noteConfig.position = tgb.transform.localPosition;
                    noteConfig.time = tap.time;
            
                    NoteRenderer.TapList.Add(tgb);
                    ScoreManager.totalCombo++;
                }

                if (timer.ElapsedMilliseconds < Time.deltaTime * 1000)
                {
                    yield return null;
                    timer.Reset();
                }
            }

            if (audioClip is not null)
            {
                var audioTask = LoadAudioData(audioClip, destroyCancellationToken);
                yield return new WaitUntil(() => audioTask.IsCompleted);
                if (!audioTask.Result)
                {
                    Debug.Log("Audio clip data failed to load.");
                    yield break;
                }
            }

            yield return new WaitForSecondsRealtime(0.5f);
            SystemEvent.OnChartLoadFinish.Trigger();
            Invoke(nameof(StartSong), 1f);
        }

        private void StartSong()
        {
            NoteRenderer.StartSong();
        }

        private readonly Dictionary<int, EasingMode> easingDictionary = new()
        {
            {0, EasingMode.Straight},
            {1, EasingMode.EaseIn},
            {2, EasingMode.EaseOut},
            {3, EasingMode.HorizontalInVerticalOut},
            {4, EasingMode.VerticalInHorizontalOut},
            {5, EasingMode.Bezier}
        };

        private readonly Dictionary<int, Direction> directionDictionary = new()
        {
            { 0, Direction.Left },
            { 1, Direction.Right }
        };
        

        // ReSharper disable ClassNeverInstantiated.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public class Chart
        {
            // ReSharper disable once CollectionNeverUpdated.Global
            public List<Trail> trails { get; set; }
            
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

                // ReSharper disable once CollectionNeverUpdated.Global
                public List<Tap> taps { get; set; }
                public class Tap
                {
                    public int time { get; set; }
                }
            }
        }
    }
}