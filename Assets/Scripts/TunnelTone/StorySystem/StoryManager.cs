using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using TunnelTone.Singleton;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TunnelTone.StorySystem
{
    public class StoryManager : Singleton<StoryManager>, IPointerUpHandler
    {
        private static readonly char[] DelayCharacters = {',', '.', '!', '?', ':', ';', '\n'};
        private const float BlackScreenTransitionTime = 1f;
        private const float BlackScreenTextTransitionTime = 1f;
        private static readonly WaitForSeconds RegularInterval = new(.02f);
        private static readonly WaitForSeconds PunctuationDelay = new(.1f);
        
        private static IEnumerator<StoryElement> timeline;
        private static StoryElement previous;
        private static bool isStoryLoaded;
        public static bool isTransitionComplete;
        
        public static event TransitionStartEvent TransitionStart;
        public static event TransitionCompleteEvent TransitionComplete;
        public delegate void TransitionStartEvent();
        public delegate void TransitionCompleteEvent();
        
        [Header("Interface Elements")]
        [SerializeField] private TMP_Text speaker;
        [SerializeField] private TMP_Text line;
        [SerializeField] private Image background;
        
        [SerializeField] private Image blackScreen;
        [SerializeField] private TMP_Text blackScreenText;
        
        [Header("Debug")]
        [SerializeField] private Story story;

        private void Start() => LoadStory(story);

        public static void LoadStory(Story story)
        {
            timeline = story.GetEnumerator();
            isStoryLoaded = true;
            TransitionStart += StartTransition;
            TransitionComplete += CompleteTransition;
        }

        public void Step()
        {
            if (!isTransitionComplete)
            {
                switch (timeline.Current)
                {
                    case Line l:
                        line.text = l.text;
                        break;
                    case Narrator n:
                        line.text = n.text;
                        break;
                    case BlackScreen:
                        return;
                }

                TransitionComplete?.Invoke();
                return;
            }

            PlayNext();
        }

        private void PlayNext()
        {
            TransitionStart?.Invoke();
            do
            {
                if (timeline.MoveNext())
                {
                    if (previous is BlackScreen && timeline.Current is not BlackScreen)
                    {
                        LeanTween.value(blackScreen.gameObject, c =>
                        {
                            blackScreen.color = c;
                        }, Color.black, Color.clear, BlackScreenTransitionTime);
                        LeanTween.value(blackScreenText.gameObject, c =>
                            {
                                blackScreenText.color = c;
                            }, Color.white, Color.clear, BlackScreenTextTransitionTime)
                            .setOnComplete(() =>
                            {
                                timeline.Current!.Play();
                                previous = timeline.Current;
                            });
                        continue;
                    }
                    timeline.Current!.Play();
                    previous = timeline.Current;
                    continue;
                }
                UnloadStory();
                Debug.Log("Story unloaded.");
            } while (timeline.Current is Background or Character);
        }
        
        private static void UnloadStory()
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
            speaker.transform.parent.gameObject.SetActive(true);
            StartCoroutine(DisplayLine(text.ToCharArray()));
        }

        public void SetNarrator(string text)
        {
            speaker.transform.parent.gameObject.SetActive(false);
            StartCoroutine(DisplayLine(text.ToCharArray()));
        }
        
        public void SetBackground(Sprite sprite)
        {
            background.sprite = sprite;
        }
        
        public void SetBlackScreen(string text)
        {
            isTransitionComplete = false;

            if (previous is not BlackScreen)
            {
                LeanTween.value(blackScreen.gameObject, c =>
                    {
                        blackScreen.color = c;
                    }, Color.clear, Color.black, BlackScreenTransitionTime)
                    .setOnComplete(() =>
                    {
                        blackScreenText.text = text;
                        LeanTween.value(blackScreenText.gameObject, c =>
                        {
                            blackScreenText.color = c;
                        }, Color.clear, Color.white, BlackScreenTextTransitionTime)
                            .setOnComplete(() => 
                            {
                                TransitionComplete?.Invoke();
                            });
                    });
            }
            else
            {
                LeanTween.value(blackScreenText.gameObject, c =>
                {
                    blackScreenText.color = c;
                }, Color.white, Color.clear, BlackScreenTextTransitionTime)
                .setOnComplete(() =>
                {
                    blackScreenText.text = text;
                    LeanTween.value(blackScreenText.gameObject, c =>
                    {
                        blackScreenText.color = c;
                    }, Color.clear, Color.white, BlackScreenTextTransitionTime)
                    .setOnComplete(() => 
                        {
                            TransitionComplete?.Invoke();
                        });
                });
            }
        }

        private IEnumerator DisplayLine(IEnumerable<char> text)
        {
            isTransitionComplete = false;
            StringBuilder sb = new();
            foreach (var t in text)
            {
                if(isTransitionComplete) yield break;
                sb.Append(t is '\r' ? ' ' : t);
                line.SetText(sb);
                yield return DelayCharacters.Contains(t) ? PunctuationDelay : RegularInterval;
            }
            TransitionComplete?.Invoke();
        }

        public void Skip()
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isStoryLoaded) Step();
        }

        private static void CompleteTransition()
        {
            isTransitionComplete = true;
        }

        private static void StartTransition()
        {
            isTransitionComplete = false;
        }
    }
}