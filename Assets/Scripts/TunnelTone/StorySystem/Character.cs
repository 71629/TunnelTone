using System;
using System.Collections.Generic;
using TunnelTone.Editor;
using UnityEditor;
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
            
        }
    }
}