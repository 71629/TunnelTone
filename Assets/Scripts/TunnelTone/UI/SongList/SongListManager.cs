using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;

namespace TunnelTone.UI.SongList
{
    public class SongListManager : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private TextAsset songList;
        private Song[] Songs { get; set; }
        
        private void Start()
        {
            if (songList == null) return;
            
            // Deserialize songList to List<Song> object
            Songs = JsonConvert.DeserializeObject<Song[]>(songList.text);
            
            // Determine container height based on song count
            var containerRect = container.GetComponent<RectTransform>();
            containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, Songs.Length * 160);
            
            // Instantiate song list items
            foreach (var song in Songs)
            {
                var songListItem = Instantiate(Resources.Load<GameObject>("Prefabs/SongListItem"), container.transform);
                var listItem = songListItem.GetComponent<SongListItem>();
                listItem.title.text = song.title;
                listItem.artist.text = song.artist;
                listItem.difficulty.text = $"{song.difficulty[3]}";
                listItem.songJacket.sprite = Resources.Load<Sprite>($"Songs/{song.title}/Jacket") ?? null;
            }
        }
        
        private class Song
        {
            public string title { get; set; }
            public string artist { get; set; }
            public float bpm { get; set; }
            public int[] difficulty { get; set; }
        }
    }
}