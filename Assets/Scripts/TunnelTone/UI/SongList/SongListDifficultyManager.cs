﻿using System;
using TMPro;
using TunnelTone.Events;
using TunnelTone.GameSystem;
using TunnelTone.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI.SongList
{
    public class SongListDifficultyManager : Singleton<SongListDifficultyManager>
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

        private Slider currentSelected;

        internal int CurrentlySelected 
        {
            get
            {
                return currentSelected.gameObject.name switch
                {
                    "Easy" => 0,
                    "Hard" => 1,
                    "Intensive" => 2,
                    "Insane" => 3,
                    _ => throw new ArgumentException()
                };
            }
            set
            {
                currentSelected = value switch
                {
                    0 => easy,
                    1 => hard,
                    2 => intensive,
                    3 => insane,
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

            LeanTween.cancel(easy.gameObject);
            LeanTween.value(easy.gameObject, f => { UpdateSliderValue(easy, easyText, f); }, easy.value, song.songData.GetDifficulties()[0], .35f).setEase(LeanTweenType.easeOutCubic);
            LeanTween.cancel(hard.gameObject);
            LeanTween.value(hard.gameObject, f => { UpdateSliderValue(hard, hardText, f); }, hard.value, song.songData.GetDifficulties()[1], .35f).setEase(LeanTweenType.easeOutCubic);
            LeanTween.cancel(intensive.gameObject);
            LeanTween.value(intensive.gameObject, f => { UpdateSliderValue(intensive, intensiveText, f); }, intensive.value, song.songData.GetDifficulties()[2], .35f).setEase(LeanTweenType.easeOutCubic);
            LeanTween.cancel(insane.gameObject);
            LeanTween.value(insane.gameObject, f => { UpdateSliderValue(insane, insaneText, f); }, insane.value, song.songData.GetDifficulties()[3], .35f).setEase(LeanTweenType.easeOutCubic);
        }

        private void UpdateSliderValue(Slider slider, TMP_Text ind, float f)
        {
            slider.value = f;
            ind.text = Dictionaries.Instance.levelDictionary[Mathf.RoundToInt(slider.value)];
            if (!currentSelected || slider == main) return;
            main.value = currentSelected.value;
            mainText.text = Dictionaries.Instance.levelDictionary[Mathf.RoundToInt(main.value)];
        }

        public void ChangeDifficulty(Slider target)
        {
            LeanTween.value(mainImage.gameObject, f => { UpdateSliderValue(main, mainText, f); }, main.value, target.value, .35f).setEase(LeanTweenType.easeOutCubic);
            currentSelected = target;
        }

        public void FadeSliderColor(Image targetImage)
        {
            if (targetImage == insane.fillRect.GetComponent<Image>())
            {
                LeanTween.value(main.gameObject, color => { UpdateSliderColor(mainImage, color); }, mainImage.color, new Color32(190, 65, 207, 255), .35f).setEase(LeanTweenType.easeOutCubic);
                return;
            }
            LeanTween.value(main.gameObject, color => { UpdateSliderColor(mainImage, color); }, mainImage.color, targetImage.color, .35f).setEase(LeanTweenType.easeOutCubic);
        }

        private static void UpdateSliderColor(Graphic target, Color color)
        {
            target.color = color;
        }
        
        public void OnDifficultyChange(int difficulty)
        {
            SongListEventReference.Instance.OnDifficultyChange.Trigger(difficulty);
        }
        
        public void QuickChangeDifficulty()
        {
            CurrentlySelected = (CurrentlySelected + 1) % 4;
            SongListEventReference.Instance.OnDifficultyChange.Trigger(CurrentlySelected);
            ChangeDifficulty(currentSelected);
            FadeSliderColor(currentSelected.fillRect.GetComponent<Image>());
        }
    }
}