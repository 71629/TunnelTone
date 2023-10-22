using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class RandomImage : MonoBehaviour
{
    public Sprite sprite;
    public Sprite[] Sprites;
    public SpriteRenderer spriteRenderer;
   
    

    void Start()
    {
        
    }

    private void Update()
    {
        changeSprite();
    }

    void changeSprite()
    {

        int num = UnityEngine.Random.Range(0, Sprites.Length);
        sprite = Sprites[num];
        spriteRenderer.sprite = sprite;

    }
}
