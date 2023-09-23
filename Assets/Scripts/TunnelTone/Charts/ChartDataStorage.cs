using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TunnelTone.Charts
{
    public class ChartDataStorage : MonoBehaviour
    {
        public static List<GameObject> TapList = new List<GameObject>();
        public static List<GameObject> TrailList = new List<GameObject>();

        public static GameObject TrailReference => TrailList.Last();
    }
}