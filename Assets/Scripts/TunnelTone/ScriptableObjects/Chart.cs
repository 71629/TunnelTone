using UnityEngine;
using TextAsset = UnityEngine.TextCore.Text.TextAsset;

namespace TunnelTone
{
    [CreateAssetMenu(fileName = "Chart", menuName = "Chart", order = 0)]
    public class Chart : ScriptableObject
    {
        [Range(1, 20)]public int Difficulty;
        public TextAsset ChartFile;
    }
}