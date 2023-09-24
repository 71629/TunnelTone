using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TunnelTone.Charts
{
    public class ChartDataStorage : MonoBehaviour
    {
        public static List<GameObject> TapList = new();
        public static List<GameObject> TrailList = new();

        public static GameObject TrailReference => TrailList.Last();
    }
}