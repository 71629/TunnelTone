using System;
using TunnelTone.Editor;
using UnityEngine;

namespace TunnelTone.StorySystem
{
    [Serializable]
    public class Line : StoryElement
    {
        [SerializeField] public TextMode size = TextMode.Normal;
        [TextArea(4, 4)][SerializeField] public string text;
        
        public override void Play()
        {
            
        }
    }
    
    [Serializable]
    public enum TextMode
    {
        Big,
        Normal,
        Small
    }
}