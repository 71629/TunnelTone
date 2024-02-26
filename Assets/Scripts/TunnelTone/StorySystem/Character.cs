using System;
using UnityEngine;

namespace TunnelTone.StorySystem
{
    [Serializable]
    [CreateAssetMenu(fileName = "Character", menuName = "Character", order = 0)]
    public class Character : StoryElement
    {
        public string name;
        public override void Play()
        {
            StoryManager.Instance.SetSpeakerName(name);
        }
    }
}