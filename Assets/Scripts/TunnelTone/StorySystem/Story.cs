using System;
using System.Collections.Generic;
using UnityEngine;

namespace TunnelTone.StorySystem
{
    /// <summary>
    /// Contains building blocks for a story.
    /// </summary>
    [CreateAssetMenu(fileName = "Story", menuName = "Story", order = 0)]
    [Serializable]
    public class Story : ScriptableObject
    {
        public string storyTitle;
        public string storyDescription;
        [HideInInspector] public List<StoryElement> timeline;
        
        public IEnumerator<StoryElement> GetEnumerator()
        {
            return timeline.GetEnumerator();
        }
    }
}