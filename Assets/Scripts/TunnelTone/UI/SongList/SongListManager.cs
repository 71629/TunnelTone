using System;
using System.Collections;
using System.Collections.Generic;
using TunnelTone.Core;
using TunnelTone.Elements;
using TunnelTone.Events;
using TunnelTone.ScriptableObjects;
using TunnelTone.Singleton;
using TunnelTone.UI.Menu;
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

        public delegate void StartSongEvent(MusicPlayDescription mpd);

        public static event StartSongEvent SongStart;

        public static void LoadSongList(MusicPlayMode mode)
        {
            if (Instance.songContainer is null) return;

            SongListItem.SelectItem += SetSong;
            
            Instance.musicPlayDescription = new MusicPlayDescription { playMode = mode };

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
            Instance.songListItems[0]?.ItemSelected();
        }

        private static void SetSong(SongData songData)
        {
            Instance.musicPlayDescription.songData = songData;
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
            SongListEvent.OnSongStart.Trigger(currentlySelected);
            NoteRenderer.Instance.currentBpm = currentlySelected.bpm;
            StartCoroutine(EnableCanvasDelayed());
        }

        IEnumerator EnableCanvasDelayed()
        {
            SystemEvent.OnChartLoad.Trigger(currentlySelected, SongListDifficultyManager.Instance.CurrentlySelected);
            SystemEvent.InvokeChartLoad(currentlySelected.charts[SongListDifficultyManager.Instance.CurrentlySelected], currentlySelected.music);
            yield return new WaitForSecondsRealtime(0.5f);
            UIElement.musicPlay.enabled = true;
            UIElement.songList.enabled = false;
            UIElement.topView.enabled = false;
        }

        private void OnSelectItem(params object[] param)
        {
            var item = (SongListItem)param[0];
            currentlySelected = item.songData;
            
            var newBackground = Instantiate(currentBackground, transform.parent).GetComponent<Image>();
            newBackground.gameObject.name = "Background";
            newBackground.transform.SetSiblingIndex(1);
            newBackground.sprite = item.songData.jacket;
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