using System;
using UnityEngine;

namespace TunnelTone.StorySystem
{
    [Serializable]
    public class Narrator : StoryElement
    {
        [TextArea(4, 4)][SerializeField] public string text;
        
        public override void Play()
        {
            
        }
    }
}