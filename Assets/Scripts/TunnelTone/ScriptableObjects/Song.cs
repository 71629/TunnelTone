using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Song", menuName = "Song", order = 0)]
    public class Song : ScriptableObject
    {
        [Header("Basic Information")]
        public string SongTitle, Artist;
        public float BPM;
        public Image Jacket;
        public AudioClip Music;
        
        [Header("Chart Container")]
        public Chart[] Chart = new Chart[4];

        [Header("Preview Options")] 
        public bool Loop;
        public float From, To;
    }
}