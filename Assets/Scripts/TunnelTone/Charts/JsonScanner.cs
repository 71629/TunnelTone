using System;
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
using Debug = UnityEngine.Debug;

// ReSharper disable InconsistentNaming
namespace TunnelTone.Charts
{
    public class JsonScanner : MonoBehaviour
    {
        private static NoteRenderer NoteRenderer => NoteRenderer.Instance;
        private Chart chartCache;
        [SerializeField] private GameObject trailPrefab;
        [SerializeField] private GameObject tapPrefab;
        [SerializeField] private GameObject barIndicatorPrefab;

        public delegate void ChartLoadHandler();
        public static event ChartLoadHandler ChartLoadFinish;

        private void Start()
        {
            SongListManager.MusicPlayInitialize += ScanChart;
            NoteRenderer.Retry += () => StartCoroutine(Retry());
            
            ChartEventReference.Instance.OnQuit.AddListener(delegate { chartCache = new Chart(); });
        }

        private void ScanChart()
        {
            var mpd = MusicPlayDescription.instance;
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
                
                NoteRenderer.TrailList.Add(component);
                foreach(var tap in trail.taps)
                {
                    var spline = NoteRenderer.TrailReference.GetComponent<SplineContainer>().Spline;

                    var tapNote = Instantiate(tapPrefab, NoteRenderer.TrailReference.transform).GetComponent<Tap>();
                    GameObject tgb;
                    (tgb = tapNote.gameObject).transform.localPosition = spline.EvaluatePosition(
                        (tap.time * NoteRenderer.chartSpeedModifier - spline.EvaluatePosition(0)).z /
                        (spline.EvaluatePosition(1).z - spline.EvaluatePosition(0).z));
                    tapNote.position = tgb.transform.localPosition;
                    tapNote.time = tap.time;
                    
                    NoteRenderer.TapList.Add(tgb);
                    ScoreManager.totalCombo++;
                }

                if (timer.ElapsedMilliseconds < Time.deltaTime * 1000)
                {
                    yield return null;
                    timer.Reset();
                }
            }
            
            // Spawn bar indicators according to BPM and timing sheet
            for 
            (
                var i = 0f;
                i < MusicPlayDescription.instance.music.length * 1000;
                i += 240000 / MusicPlayDescription.instance.songData.bpm
            ){
                var barIndicator = Instantiate(barIndicatorPrefab, transform);
                barIndicator.GetComponent<BarIndicator>().time = i;
            }
            
            var audioTask = LoadAudioData(MusicPlayDescription.instance.music, destroyCancellationToken);
            yield return new WaitUntil(() => audioTask.IsCompleted);
            if (!audioTask.Result) yield break;

            yield return new WaitForSecondsRealtime(0.5f);
            ChartLoadFinish?.Invoke();
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