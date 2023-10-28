using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Splines;

namespace TunnelTone.Elements
{
    public class Trail : MonoBehaviour
    {
        private Trail _next;
        private bool _isHit;
        private TouchControl _trackingTouch;
        private Spline _path;
        
        private Sprite HitRing1 => Resources.Load<Sprite>("Sprites/HitRing1");
        private Sprite HitRing2 => Resources.Load<Sprite>("Sprites/HitRing2");
    }
}