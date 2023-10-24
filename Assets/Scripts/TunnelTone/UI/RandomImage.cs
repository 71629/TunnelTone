using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class RandomImage : MonoBehaviour
{
    public Image image;
    public Sprite sprite;
    public Sprite[] Sprites;
    public SpriteRenderer spriteRenderer;
    public float time;
    public float timerate = 10;
   
    

    void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= timerate)
        {
            changeSprite();
            time = 0;
        }
    }

    void changeSprite()
    {
            int num = UnityEngine.Random.Range(0, Sprites.Length);
                    sprite = Sprites[num];
                    image.sprite = sprite;
    }
}
