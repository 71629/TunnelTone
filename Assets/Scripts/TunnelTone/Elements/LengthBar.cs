using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LengthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Image LengthsBar;
    void Start()
    {
        LengthsBar.rectTransform.sizeDelta = new Vector2(Screen.width, LengthsBar.rectTransform.sizeDelta.y);
        LengthsBar.transform.position = new Vector2((-86*2), LengthsBar.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
