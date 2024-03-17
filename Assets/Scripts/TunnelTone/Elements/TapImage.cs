using UnityEngine;
using UnityEngine.UI;
namespace TunnelTone.Elements
{

    public class TapImage : MonoBehaviour
    {
        [SerializeField] private Image tapImage;
        [SerializeField] private GameObject tapPref;

        private void Start()
        {
            tapPref.GetComponent<Image>().sprite = tapImage.sprite;
        }
    }
}