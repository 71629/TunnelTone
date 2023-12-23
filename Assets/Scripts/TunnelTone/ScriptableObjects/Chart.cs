using UnityEngine;

namespace TunnelTone.ScriptableObjects
{
    [System.Serializable]
    public class Chart
    {
        [SerializeField][Range(1, 14)] internal int difficulty;
        [SerializeField] internal string chartDesigner = "[Redacted]";
        
        [SerializeField] internal TextAsset chart;
        [SerializeField] private int score;
    }
}