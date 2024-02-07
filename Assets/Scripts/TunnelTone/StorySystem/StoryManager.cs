using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.StorySystem
{
    public class StoryManager : MonoBehaviour
    {
        [SerializeField] private Image textBox;
        [SerializeField] private TMP_Text speaker;
        [SerializeField] private TMP_Text line;
        [SerializeField] private Image background;
        
        private IEnumerator<StoryElement> timeline;
        private bool isStoryLoaded;
        private bool isTransitionComplete;

        public void LoadStory(Story story)
        {
            timeline = story.GetEnumerator();
            isStoryLoaded = true;
        }

        public void Step()
        {
            if (!isTransitionComplete)
            {
                if (timeline.Current is Line l) { line.text = l.text; }
                if (timeline.Current is Narrator n) { line.text = n.text; }
                
                isTransitionComplete = true;
            }
            do
            {
                if (timeline.MoveNext())
                {
                    timeline.Current.Play();
                    continue;
                }
                UnloadStory();
            } while (timeline.Current is Background or Character);
        }
        
        public void UnloadStory()
        {
            timeline.Dispose();
            isStoryLoaded = false;
        }
        
        public void SetSpeakerName(string name)
        {
            speaker.text = name;
        }
        
        public void SetLine(string text)
        {
            speaker.gameObject.SetActive(true);
            StartCoroutine(DisplayLine(text.ToCharArray()));
        }

        public void SetNarrator(string text)
        {
            speaker.gameObject.SetActive(false);
            StartCoroutine(DisplayLine(text.ToCharArray()));
        }
        
        public void SetBackground(Sprite sprite)
        {
            background.sprite = sprite;
        }

        private IEnumerator DisplayLine(IEnumerable<char> text)
        {
            isTransitionComplete = false;
            StringBuilder sb = new();
            foreach (var t in text)
            {
                if(isTransitionComplete) yield break;
                sb.Append(t);
                line.text = sb.ToString();
                yield return new WaitForSeconds(0.05f);
            }
            isTransitionComplete = true;
        }
    }
}