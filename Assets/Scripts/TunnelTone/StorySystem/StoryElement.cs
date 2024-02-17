using System;
using UnityEngine;

namespace TunnelTone.StorySystem
{
    [Serializable]
    public abstract class StoryElement : ScriptableObject
    {
        public bool skippible;
        public abstract void Play();
    }
}