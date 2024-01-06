using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.UI.PlayResult
{
    public class Grade : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gradeText;
        [SerializeField] private Image shadow;

        private LTRect gradeTextRect;
        private LTRect shadowRect;

        private void Awake()
        {
            gradeText.gameObject.transform.localScale = 1.5f * Vector3.one;
            gradeText.color *= new Color(1, 1, 1, 0);
            shadow.color *= new Color(1, 1, 1, 0);
            
            gradeTextRect = new LTRect(gradeText.rectTransform.rect);
            shadowRect = new LTRect(shadow.rectTransform.rect);
        }
        
        internal void Display()
        {
            LeanTween.scale(gradeText.gameObject, Vector3.one, 0.5f)
                .setEase(LeanTweenType.easeOutCubic);
            LeanTween.value(gradeText.gameObject, f => { gradeText.alpha = f; }, 0, 1f, .5f)
                .setEase(LeanTweenType.easeOutCubic);
            LeanTween.value(shadow.gameObject, c => { shadow.color = c; }, shadow.color, shadow.color + Color.black, .5f)
                .setEase(LeanTweenType.easeOutCubic);
        }
    }
}