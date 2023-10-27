using System.IO;
using UnityEngine;

namespace TunnelTone.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Chart", menuName = "Chart", order = 0)]
    public class Chart : ScriptableObject
    {
        [Range(1, 20)]public int Difficulty;
        public string ChartDesigner;
        
        public ChartObject[] objects;
        
        public void LoadFile(string path)
        {
            var rawText = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(rawText, this);
        }
        
        [System.Serializable]
        public class ChartObject
        {
            public float startTime;
            public float endTime;
            public float startPosX;
            public float startPosY;
            public float endPosX;
            public float endPosY;
            public string direction;
            public string easing;
            public string easingRatio;
            public string head;
            public string tapEmbed;
        }
    }
}