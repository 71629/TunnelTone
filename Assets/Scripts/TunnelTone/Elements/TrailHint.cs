using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TunnelTone.Elements
{
    public class TrailHint : MonoBehaviour
    {
        [SerializeField] private Transform backTransform;
        [SerializeField] private Transform frontTransform;
        [SerializeField] private Image back;
        [SerializeField] private Image front;

        private Color backColor;
        private Color frontColor;

        internal UnityEvent OnParentHit = new UnityEvent();
        internal UnityEvent OnParentIdle = new UnityEvent();
        internal UnityEvent OnWrongHand = new UnityEvent();
        internal UnityEvent OnParentDestroy = new UnityEvent();

        internal void Enable(Direction direction)
        {
            OnParentDestroy.AddListener(OnParentDestroyed);
            
            back.enabled = true;
            front.enabled = true;

            switch (direction)
            {
                case Direction.Left:
                    backColor = new Color(.15f, .9f, 1, .35f);
                    frontColor = new Color(.15f, .9f, 1, 1);
                    break;
                case Direction.Right:
                    backColor = new Color(1, .13f, .6f, .35f);
                    frontColor = new Color(1, .13f, .6f, 1);
                    break;
                default:
                    throw new Exception();
            }

            LeanTween.value(gameObject, f =>
            {
                var c = 1.5f - f * 0.5f;
                backTransform.localScale = new Vector3(f, f, 1);
                frontTransform.localScale = new Vector3(c, c, 1);

                var mask = new Color(1, 1, 1, f);

                back.color = backColor * mask;
                front.color = frontColor * mask;
            }, 0, 1, .55f);
        }

        private void OnParentDestroyed()
        {
            Destroy(gameObject);
        }
    }
}