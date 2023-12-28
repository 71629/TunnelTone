using System;
using System.Collections;
using UnityEngine;
using TunnelTone.Elements;
using TunnelTone.Events;
using TunnelTone.ScriptableObjects;
using TunnelTone.Singleton;
using TunnelTone.UI.Reference;
using UnityEngine.UI;

namespace TunnelTone.UI.SongList
{
    public class SongListManager : Singleton<SongListManager>
    {
        [SerializeField] private GameObject container;
        [SerializeField] private TextAsset songList;
        [SerializeField] private SongData[] songContainer;
        [SerializeField] private GameObject currentBackground;
        
        internal static SongData currentlySelected;

        private UIElementReference UIElement => UIElementReference.Instance;
        private float oldSliderValue = 0.15f;
        
        private IEnumerator Start()
        {
            SongListEventReference.Instance.OnSelectItem.AddListener(OnSelectItem);

            if (songContainer is not null)
            {
                var containerRect = container.GetComponent<RectTransform>();
                containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, songContainer.Length * 160);
                
                foreach (var song in songContainer)
                {
                    if (song is null) continue;
                    Instantiate(Resources.Load<GameObject>("Prefabs/SongListItem/SongListItem"), container.transform)
                        .GetComponent<SongListItem>()
                        .SetData(song);
                }
            }

            currentlySelected = songContainer?[0];
            yield return null;
            SongListEventReference.Instance.OnEnterSongList.Trigger();
        }

        private void Update()
        {
            if (UIElement.startSlider.value >= 0.9f)
            {
                UIElement.startSlider.value = 0.15f;
                UIElement.startSlider.interactable = false;
                if (Resources.Load<TextAsset>($"Songs/{currentlySelected.songTitle}/{SongListDifficultyManager.Instance.CurrentlySelected}") is null)
                {
                    SystemEventReference.Instance.OnDisplayDialog.Trigger("Error", $"Chart not found.\npath: Songs/{currentlySelected.songTitle}/{SongListDifficultyManager.Instance.CurrentlySelected}.json",
                        new[] {"OK"}, new Action[] {
                            () =>
                            {
                                SystemEventReference.Instance.OnAbortDialog.Trigger();
                                UIElement.startSlider.interactable = true;
                            }}, Dialog.Dialog.Severity.Error);
                    return;
                }
                SongListEventReference.Instance.OnSongStart.Trigger(currentlySelected);
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
            SystemEventReference.Instance.OnChartLoad.Trigger(currentlySelected, SongListDifficultyManager.Instance.CurrentlySelected);
        }
        
        private IEnumerator TurnOffAndOn()
        {
            UIElement.startSlider.interactable = false;
            yield return new WaitForSeconds(0);
            UIElement.startSlider.interactable = true;
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