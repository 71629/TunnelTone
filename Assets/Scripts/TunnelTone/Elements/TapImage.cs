using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapImage : MonoBehaviour
{
    [SerializeField] private Image tapImage;
    [SerializeField] private GameObject tapPref;

    private void Start()
    {
        //tapImage = GetComponent<Image>();
    }

    private void Update()
    {
        tapPref.GetComponent<Image>().sprite = tapImage.sprite;
    }
}