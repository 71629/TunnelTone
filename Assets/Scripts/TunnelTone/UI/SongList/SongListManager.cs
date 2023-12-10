using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using TunnelTone.Elements;
using TunnelTone.Events;
using TunnelTone.GameSystem;
using TunnelTone.Singleton;
using TunnelTone.UI.Reference;

namespace TunnelTone.UI.SongList
{
    public class SongListManager : Singleton<SongListManager>
    {
        [SerializeField] private GameObject container;
        [SerializeField] private TextAsset songList;

        public Song currentlySelected;
        
        public Song[] Songs { get; set; }

        private UIElementReference UIElement => UIElementReference.Instance;
        private float oldSliderValue = 0.15f;
        
        private IEnumerator Start()
        {
            SongListEventReference.Instance.OnSelectItem.AddListener(OnSelectItem);
            
            if (songList != null)
            {
                // Deserialize songList to List<Song> object
                Songs = JsonConvert.DeserializeObject<Song[]>(songList.text);

                // Determine container height based on song count
                var containerRect = container.GetComponent<RectTransform>();
                containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, Songs.Length * 160);

                // Instantiate song list items
                foreach (var song in Songs)
                {
                    var songListItem = Instantiate(Resources.Load<GameObject>("Prefabs/SongListItem/SongListItem"),
                        container.transform);
                    var listItem = songListItem.GetComponent<SongListItem>();
                    listItem.title.text = song.title;
                    listItem.artist.text = song.artist;
                    listItem.difficulty.text = $"{song.difficulty[3]}";
                    listItem.songJacket.sprite = Resources.Load<Sprite>($"Songs/{song.title}/Jacket") ?? null;
                    listItem.previewStart = song.previewStart;
                    listItem.previewDuration = song.previewDuration;
                    listItem.source = song;
                }
                
                currentlySelected = Songs[0];
                yield return null;
                SongListEventReference.Instance.OnEnterSongList.Trigger();
            }
        }

        private void Update()
        {
            if (UIElement.startSlider.value >= 0.9f)
            {
                UIElement.startSlider.value = 0.15f;
                UIElement.startSlider.interactable = false;
                if (Resources.Load<TextAsset>($"Songs/{currentlySelected.title}/{DifficultyManager.Instance.currentlySelected}") is null)
                {
                    SystemEventReference.Instance.OnDisplayDialog.Trigger("Error", $"Chart not found.\npath: Songs/{currentlySelected.title}/{DifficultyManager.Instance.currentlySelected}.json",
                        new[] {"OK"}, new Action[] {
                            () =>
                            {
                                SystemEventReference.Instance.OnAbortDialog.Trigger();
                                UIElement.startSlider.interactable = true;
                            }}, Dialog.Dialog.Severity.Error);
                    return;
                }
                SongListEventReference.Instance.OnSongStart.Trigger();
                NoteRenderer.Instance.currentBpm = currentlySelected.bpm;
                StartCoroutine(EnableCanvasDelayed());
            }
        }

        IEnumerator EnableCanvasDelayed()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            UIElement.musicPlay.enabled = true;
            UIElement.songList.enabled = false;
            UIElement.topView.enabled = false;
            SystemEventReference.Instance.OnChartLoad.Trigger(Resources.Load<TextAsset>($"Songs/{currentlySelected.title}/{DifficultyManager.Instance.currentlySelected}"));
        }
        
        IEnumerator TurnOffAndOn()
        {
            UIElement.startSlider.interactable = false;
            yield return new WaitForSeconds(0);
            UIElement.startSlider.interactable = true;
        }

        private void OnSelectItem(params object[] param)
        {
            var item = (SongListItem)param[0];
            currentlySelected = item.source;
        }
    }
}