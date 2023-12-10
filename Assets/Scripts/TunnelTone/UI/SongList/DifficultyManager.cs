using System;
using System.Collections;
using TMPro;
using TunnelTone.Events;
using TunnelTone.GameSystem;
using TunnelTone.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI.SongList
{
    public class DifficultyManager : Singleton<DifficultyManager>
    {
        [Header("Difficulty Sliders")]
        [SerializeField] private Slider easy;
        [SerializeField] private Slider hard;
        [SerializeField] private Slider intensive;
        [SerializeField] private Slider insane;
        [SerializeField] private Slider main;
        
        [Header("Difficulty Text")]
        [SerializeField] private TextMeshProUGUI easyText;
        [SerializeField] private TextMeshProUGUI hardText;
        [SerializeField] private TextMeshProUGUI intensiveText;
        [SerializeField] private TextMeshProUGUI insaneText;
        [SerializeField] private TextMeshProUGUI mainText;
        
        [SerializeField] private Image mainImage;

        private Slider _currentSelected;

        public int currentlySelected
        {
            get
            {
                return _currentSelected.gameObject.name switch
                {
                    "Easy" => 0,
                    "Hard" => 1,
                    "Intensive" => 2,
                    "Insane" => 3,
                    _ => throw new ArgumentException()
                };
            }
        }

        private void Start()
        {
            SongListEventReference.Instance.OnSelectItem.AddListener(UpdateDifficulty);
            SongListEventReference.Instance.OnEnterSongList.AddListener(delegate
            {
                ChangeDifficulty(easy);
                mainImage.color = easy.fillRect.GetComponent<Image>().color;
            });
        }

        private void UpdateDifficulty(params object[] param)
        {
            var song = (SongListItem)param[0];

            StopAllCoroutines();
            StartCoroutine(UpdateDifficultySlider(easy, easyText, easy.value, song.source.difficulty[0]));
            StartCoroutine(UpdateDifficultySlider(hard, hardText, hard.value, song.source.difficulty[1]));
            StartCoroutine(UpdateDifficultySlider(intensive, intensiveText, intensive.value, song.source.difficulty[2]));
            StartCoroutine(UpdateDifficultySlider(insane, insaneText, insane.value, song.source.difficulty[3]));
        }

        private IEnumerator UpdateDifficultySlider(Slider slider, TextMeshProUGUI ind, float i, float f, float t = 0.21f, int iterations = 1)
        {
            yield return new WaitForEndOfFrame();
            
            if (t > 0.999f || Math.Abs(slider.value - f) < 0.005f)
            {
                slider.value = f;
                yield break;
            }

            slider.value = Mathf.Lerp(i, f, t);
            ind.text = Dictionaries.Instance.difficultyDictionary[Mathf.RoundToInt(slider.value)];

            if (_currentSelected && slider != main)
            {
                main.value = _currentSelected.value;
                mainText.text = Dictionaries.Instance.difficultyDictionary[Mathf.RoundToInt(main.value)];
            }
            
            StartCoroutine(UpdateDifficultySlider(slider, ind, i, f, t + 0.15f * Mathf.Pow(0.85f, iterations), iterations + 1));
        }

        private IEnumerator FadeSliderColor(Graphic image, Color i, Color f, float t = 0.21f, int iterations = 1)
        {
            yield return new WaitForEndOfFrame();
            
            if (t > 0.999f || image.color == f)
            {
                image.color = f;
                yield break;
            }
            
            image.color = Color.Lerp(i, f, t);
            
            StartCoroutine(FadeSliderColor(image, i, f, t + 0.15f * Mathf.Pow(0.85f, iterations), iterations + 1));
        }

        public void ChangeDifficulty(Slider target)
        {
            StopCoroutine(nameof(FadeSliderColor));
            StartCoroutine(UpdateDifficultySlider(main, mainText, main.value, target.value));
            _currentSelected = target;
        }

        public void FadeSliderColor(Image targetImage)
        {
            StartCoroutine(FadeSliderColor(mainImage, mainImage.color, targetImage.color));
        }
        
        public void OnDifficultyChange(int difficulty)
        {
            SongListEventReference.Instance.OnDifficultyChange.Trigger(difficulty);
        }
    }
}