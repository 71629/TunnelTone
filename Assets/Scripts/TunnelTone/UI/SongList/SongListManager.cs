using UnityEngine;
using Newtonsoft.Json;
using TunnelTone.Events;
using TunnelTone.GameSystem;
using TunnelTone.Singleton;

namespace TunnelTone.UI.SongList
{
    public class SongListManager : Singleton<SongListManager>
    {
        [SerializeField] private GameObject container;
        [SerializeField] private TextAsset songList;

        public Song CurrentlySelected;
        
        public Song[] Songs { get; set; }
        
        private void Start()
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
            }
        }

        private void OnSelectItem(params object[] param)
        {
            var item = (SongListItem)param[0];
            CurrentlySelected = item.source;
        }
    }
}