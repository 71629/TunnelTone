using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.StorySystem
{
    public class StoryMenu : MonoBehaviour
    {
        [SerializeField] private RectTransform menu;
        [SerializeField] private Image autoButton;
        private bool isMenuOpen;
        private bool isAutoPlay;
        
        private const float MenuOpenSize = 900;
        private const float MenuCloseSize = 150;
        private const float MenuToggleTime = .5f;

        private void Start()
        {
            StoryManager.TransitionStart += CancelInvoke;
        }
        
        public void ToggleMenu()
        {
            if (isMenuOpen)
            {
                LeanTween.value(gameObject, f =>
                    {
                        menu.sizeDelta = new Vector2(f, menu.sizeDelta.y);
                    }, MenuOpenSize, MenuCloseSize, MenuToggleTime)
                    .setEase(LeanTweenType.easeOutCubic);
            }
            else
            {
                LeanTween.value(gameObject, f =>
                    {
                        menu.sizeDelta = new Vector2(f, menu.sizeDelta.y);
                    }, MenuCloseSize, MenuOpenSize, MenuToggleTime)
                    .setEase(LeanTweenType.easeOutCubic);
            }
        }

        public void ToggleAutoPlay()
        {
            isAutoPlay ^= true;
            
            autoButton.color = isAutoPlay ? Color.cyan : Color.white;
            
            if (isAutoPlay)
            {
                if (StoryManager.isTransitionComplete) Step();
                StoryManager.TransitionComplete += AutoPlay;
            }
            else StoryManager.TransitionComplete -= AutoPlay;
        }

        private void AutoPlay()
        {
            Invoke(nameof(Step), 1.5f);
        }

        private void Step() => StoryManager.Instance.Step();
    }
}