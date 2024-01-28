using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SongBarColor : MonoBehaviour
{
    public Image SongBar;
    public Image Difficulty;
    public Image LengthsBar;
    //public Gradient gradient;
    // Update is called once per frame
    void Start()
    {
        LengthsBar.rectTransform.sizeDelta = new Vector2(Screen.width, LengthsBar.rectTransform.sizeDelta.y);
        LengthsBar.transform.position = new Vector2((-86.5f * 2), LengthsBar.transform.position.y);
    }
    void Update()
    {
        // Blend color from red at 0% to blue at 100%
        //var colors = new GradientColorKey[2];
        //colors[0] = new GradientColorKey(Color.white, 0.0f);
        //colors[1] = new GradientColorKey(Difficulty.color, 1.0f);
        //colors[0].color.a = 255;
        // Blend alpha from opaque at 0% to transparent at 100%
        //var alphas = new GradientAlphaKey[2];
        //alphas[0] = new GradientAlphaKey(1.0f, 1.0f);
        //alphas[1] = new GradientAlphaKey(1.0f, 1.0f);
        if (SongBar.color != Difficulty.color)
        {
            //gradient.SetKeys(colors ,alphas);
            //SongBar.material.Set(gradient.Evaluate(1));
            SongBar.color = Color.white;
            LengthsBar.color = Difficulty.color;
        }
    }
}
