using System;
using System.Collections.Generic;
using TunnelTone.Core;
using TunnelTone.Elements;
using TunnelTone.Events;
using TunnelTone.ScriptableObjects;
using TunnelTone.Singleton;
using TunnelTone.UI.Reference;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI.SongList
{
    public class SongListManager : Singleton<SongListManager>
    {
        public SongData[] songContainer;
        
        [SerializeField] private GameObject container;
        [SerializeField] private GameObject currentBackground;
        
        internal static SongData currentlySelected;

        private List<SongListItem> songListItems = new();
        private MusicPlayDescription musicPlayDescription;

        private UIElementReference UIElement => UIElementReference.Instance;

        public delegate void StartSongEvent(ref MusicPlayDescription mpd);
        public delegate void EnterSongListEvent();
        public static event StartSongEvent SongStart;
        public static event StartSongEvent MusicPlayInitialize;
        public static event EnterSongListEvent EnterSongList;

        public static void LoadSongList(MusicPlayMode mode)
        {
            if (Instance.songContainer is null) return;

            SongListItem.SelectItem += SetSong;
            SongListItem.SelectItem += Instance.OnSelectItem;
            SongListDifficultyManager.DifficultyChange += UpdateChartObject;
            
            Instance.musicPlayDescription = new MusicPlayDescription
            {
                playMode = mode,
                playResult = new Core.PlayResult()
            };

            var containerRect = Instance.container.GetComponent<RectTransform>();
            containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, Instance.songContainer.Length * 160);
                
            foreach (var song in Instance.songContainer)
            {
                if (song is null) continue;

                var item = Instantiate(Resources.Load<GameObject>("Prefabs/SongListItem/SongListItem"),
                        Instance.container.transform)
                    .GetComponent<SongListItem>()
                    .SetData(song);
                Instance.songListItems.Add(item);
            }
            EnterSongList?.Invoke();
            Instance.songListItems[0]?.ItemSelected();
        }

        private static void UpdateChartObject(int difficulty)
        {
            Instance.musicPlayDescription.chart = currentlySelected.charts[difficulty];
        }

        private static void SetSong(SongData songData)
        {
            Instance.musicPlayDescription.music = songData.music;
            Instance.musicPlayDescription.chart = songData.charts[SongListDifficultyManager.Instance.CurrentlySelected];
        }

        public void StartSong()
        {
            if (Resources.Load<TextAsset>($"Songs/{currentlySelected.songTitle}/{SongListDifficultyManager.Instance.CurrentlySelected}") is null)
            {
                SystemEvent.OnDisplayDialog.Trigger("Error", $"Chart not found.\npath: Songs/{currentlySelected.songTitle}/{SongListDifficultyManager.Instance.CurrentlySelected}.json",
                    new[] {"OK"}, new Action[] {
                        () =>
                        {
                            SystemEvent.OnAbortDialog.Trigger();
                        }}, Dialog.Dialog.Severity.Error);
                return;
            }
            
            SongStart?.Invoke(ref musicPlayDescription);
            LoadBestScore(ref musicPlayDescription);
            MusicPlayInitialize?.Invoke(ref musicPlayDescription);
            
            NoteRenderer.Instance.currentBpm = currentlySelected.bpm;
            Shutter.Seal(() =>
            {
                UIElement.musicPlay.enabled = true;
                UIElement.songList.enabled = false;
                UIElement.topView.enabled = false;
            });
        }

        private static void LoadBestScore(ref MusicPlayDescription mpd)
        {
            mpd.playResult.title = currentlySelected.songTitle;
            mpd.playResult.artist = currentlySelected.artist;
            mpd.playResult.bestScore = currentlySelected.GetScore(mpd.difficulty);
            mpd.jacket = currentlySelected.jacket;
            mpd.playResult.level = currentlySelected.GetDifficulty(mpd.difficulty);
        }

        private void OnSelectItem(SongData songData)
        {
            currentlySelected = songData;
            
            var newBackground = Instantiate(currentBackground, transform.parent).GetComponent<Image>();
            newBackground.gameObject.name = "Background";
            newBackground.transform.SetSiblingIndex(1);
            newBackground.sprite = songData.jacket;
            LeanTween.value(newBackground.gameObject, f =>
                {
                    newBackground.color = new Color(1, 1, 1, f);
                }, 0f, 1f, .25f)
                .setOnComplete(() =>
                {
                    Destroy(currentBackground);
                    currentBackground = newBackground.gameObject;
                });
        }
    }
}