using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class RandomImage : MonoBehaviour
{
    public Image image, image2;
    public Sprite sprite;
    public Sprite[] Sprites;
    public SpriteRenderer spriteRenderer;
    public float time;
    public float timerate = 10;
   
    

    void Start()
    {
        image = GetComponent<Image>();
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
            image2.sprite = sprite;
    }

    IEnumerator FadeInFadeOut()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(timerate);

            int loop = 0;

            if (loop == 0)
            {
                for (float i = 1; i >= 0; i -= Time.deltaTime)
                {
                    yield return image.color = new Color((float)0.2196079, (float)0.2196079, (float)0.2196079, i);
                    Debug.Log("image.a = " + image.color.a);
                }
                changeSprite();
                for (float i = 0; i <= 1; i += Time.deltaTime)
                {
                    yield return image2.color = new Color((float)0.2196079, (float)0.2196079, (float)0.2196079, i);
                    Debug.Log("image.a = " + image.color.a);
                }
                changeSprite();
                loop += 1;
            }
            else
            {
                for (float i = 1; i >= 0; i -= Time.deltaTime)
                {
                    yield return image2.color = new Color((float)0.2196079, (float)0.2196079, (float)0.2196079, i);
                    Debug.Log("image.a = " + image.color.a);
                }
                changeSprite();
                for (float i = 0; i <= 1; i += Time.deltaTime)
                {
                    yield return image.color = new Color((float)0.2196079, (float)0.2196079, (float)0.2196079, i);
                    Debug.Log("image.a = " + image.color.a);
                }
                changeSprite();
                loop -= 1;
            }
        }
    }
    }
