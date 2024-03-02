using System;
using UnityEngine;

namespace TunnelTone.StorySystem
{
    [Serializable]
    public class Background : StoryElement
    {
        [SerializeField] public Sprite image;
        public override void Play()
        {
            StoryManager.Instance.SetBackground(image);
        }
    }
}