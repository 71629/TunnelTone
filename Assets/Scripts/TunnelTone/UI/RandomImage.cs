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
                    fadeInFadeOut();
                    image.sprite = sprite;
    }

    IEnumerator fadeInFadeOut()
    {
        //FadeIn loop
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            image.color = new Color(1, 1, 1, i);
            yield return null;
        }
 
        //hold 1 sec
        yield return new WaitForSeconds(1);
 
        //FadeOut loop
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            image.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }
}
