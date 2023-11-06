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
            StartCoroutine(fadeInFadeOut());
            time = 0;
        }
    }

    void changeSprite()
    {
            int num = UnityEngine.Random.Range(0, Sprites.Length);
            sprite = Sprites[num];
            image.sprite = sprite;
    }

    IEnumerator fadeInFadeOut()
    {
        Debug.Log("Function is called");
        
        //FadeIn loop
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            yield return image.color = new Color(1, 1, 1, i);
            Debug.Log("image.a = "+image.color.a);
        }
 
        //change Sprite
        yield return new WaitForSeconds(3);
        changeSprite();
        yield return new WaitForSeconds(3);
 
        //FadeOut loop
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            yield return image.color = new Color(1,1,1,i);
            Debug.Log("image.a = " + image.color.a);
        }
    }
}
