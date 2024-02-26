using System;
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
            Debug.Log($"Playing: {text}");
            StoryManager.Instance.SetLine(text);
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