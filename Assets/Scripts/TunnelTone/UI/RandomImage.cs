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
    public int startTimeRate = 0;
   
    

    IEnumerator Start()
    {
        image = GetComponent<Image>();
        yield return new WaitForSecondsRealtime(startTimeRate);
        StartCoroutine(FadeInFadeOut());
    }

    private void Update()
    {
       
    }

    private void FixedUpdate()
    {
        
    }

    void changeSprite()
    {
            int num = UnityEngine.Random.Range(0, Sprites.Length);
            sprite = Sprites[num];
            image.sprite = sprite;
    }

    IEnumerator FadeInFadeOut()
    {
        
        while (true)
        {
            yield return new WaitForSecondsRealtime(timerate);
                for (float i = 1; i >= 0; i -= Time.deltaTime)
                {
                    yield return image.color = new Color((float)0.2196079, (float)0.2196079, (float)0.2196079, i);
                    Debug.Log("image.a = " + image.color.a);
                }

                yield return new WaitForSecondsRealtime(3);
                changeSprite();
                yield return new WaitForSecondsRealtime(3);
                
                for (float i = 0; i <= 1; i += Time.deltaTime)
                {
                    yield return image.color = new Color((float)0.2196079, (float)0.2196079, (float)0.2196079, i);
                    Debug.Log("image.a = " + image.color.a);
                }
            
        }
    }
    }
    
