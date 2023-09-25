using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TextAsset = UnityEngine.TextCore.Text.TextAsset;

namespace TunnelTone
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