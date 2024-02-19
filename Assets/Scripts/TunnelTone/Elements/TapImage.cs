using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapImage : MonoBehaviour
{
    [SerializeField] private Image tapImage;
    [SerializeField] private Transform changeImange;

    private void Start()
    {
        tapImage = GetComponent<Image>();
    }

    private void Update()
    {
        var images = changeImange.GetComponentsInChildren<Image>();

        foreach (var image in images)
        {
            image.sprite = tapImage.sprite;
            image.color = tapImage.color;
        }
    }
}