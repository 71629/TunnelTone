using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SongBarColor : MonoBehaviour
{
    public Image SongBar;
    public Image Difficulty;
    public Image LengthsBar;
    // Update is called once per frame
    void Start()
    {
        LengthsBar.rectTransform.sizeDelta = new Vector2(Screen.width, LengthsBar.rectTransform.sizeDelta.y);
        LengthsBar.transform.position = new Vector2((-86.5f * 2), LengthsBar.transform.position.y);
    }
    void Update()
    {
        if(SongBar.color != Difficulty.color)
        {
            SongBar.color = Difficulty.color;
            LengthsBar.color = Difficulty.color;
        }
    }
}
