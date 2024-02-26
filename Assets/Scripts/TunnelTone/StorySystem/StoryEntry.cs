using TMPro;
using TunnelTone.Core;
using TunnelTone.Exceptions;
using TunnelTone.UI.SongList;
using UnityEngine;
using TunnelTone.UI.Reference;

namespace TunnelTone.StorySystem
{
    public class StoryEntry : MonoBehaviour
    {
        private Story story;

        private const float ExpandedWidth = 0;

        [Header("Linked Objects")]
        [SerializeField] private EmptyArea emptyArea;
        [SerializeField] private TMP_Text storyEntryTitleText;
        [SerializeField] private TMP_Text storyEntryDescriptionText;
        
        [Header("Components")]
        [SerializeField] private RectTransform rectTransform;

        public delegate void StoryExpandEvent(Story story);
        public static event StoryExpandEvent StoryExpand;
        
        public static void OnStoryExpand(Story story) => StoryExpand?.Invoke(story);
        private void Clear() => story = null;

        private void Start()
        {
            StoryExpand += Expand;
        }

        private void Expand(Story story)
        {
            this.story = story;

            storyEntryTitleText.text = story.storyTitle;
            storyEntryDescriptionText.text = story.storyDescription;
            emptyArea.Enable();
            LeanTween.value(gameObject, f =>
            {
                rectTransform.sizeDelta = new Vector2(f, rectTransform.sizeDelta.y);
            }, rectTransform.sizeDelta.x, ExpandedWidth, .5f);
        }

        public void Collapse()
        {
            LeanTween.value(gameObject, f =>
            {
                rectTransform.sizeDelta = new Vector2(f, rectTransform.sizeDelta.y);
            }, rectTransform.sizeDelta.x, 0, .5f);
            Clear();
        }

        public void Enter()
        {
            if (story == null)throw new StoryIsNullException();
            
            Shutter.Instance.CloseShutter(() =>
            {
                UIElementReference.Instance.storyInterface.enabled = true;
                UIElementReference.Instance.storyMap.enabled = false;
                UIElementReference.Instance.topView.enabled = false;
                StoryManager.LoadStory(story);
                Shutter.Instance.OpenShutter();
                Clear();
            });
        }

    }
}