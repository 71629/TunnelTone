using TunnelTone.Core;
using TunnelTone.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.Elements
{
    public class HitEffect : MonoBehaviour, IOnPauseHandler
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Image image;
        private Sprite sprite
        {
            set => image.sprite = value;
        }

        public void Initialize(Sprite sprite)
        {
            this.sprite = sprite;
            transform.localScale = Vector3.one * 1.25f;
            Destroy(gameObject, 0.34f);
        }

        private void OnEnable()
        {
            PauseMenu.GamePause += OnPause;
            PauseMenu.GameResume += OnResume;
            PauseMenu.GameRetry += OnRetry;
            PauseMenu.GameQuit += OnQuit;
        }
        
        private void OnDisable()
        {
            PauseMenu.GamePause -= OnPause;
            PauseMenu.GameResume -= OnResume;
            PauseMenu.GameRetry -= OnRetry;
            PauseMenu.GameQuit -= OnQuit;
        }

        public void OnPause()
        {
            animator.speed = 0;
        }

        public void OnResume()
        {
            animator.speed = 1;
        }

        public void OnRetry()
        {
            animator.speed = 0;
        }

        public void OnQuit()
        {
            animator.speed = 0;
        }
    }
}