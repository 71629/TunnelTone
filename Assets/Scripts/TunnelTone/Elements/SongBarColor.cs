using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SongBarColor : MonoBehaviour
{
    [SerializeField] Button pause, retry, resume, quit, start;
    [SerializeField] bool isPlaying; 

    private Image SongBar, Difficulty, LengthsBar;
    private GameObject GSongBar;

    private AudioSource audioSource;
    private float dspTimeEnd, currentTime;

    private Vector2 fillVector2;

    private bool isPlayingSetOnce;
    
    // Update is called once per frame
    void Start()
    {
        GSongBar = GameObject.Find("Music Play/UI/Progress/Fill Area/Fill");
        fillVector2 = GSongBar.transform.position;

        SongBar = GameObject.Find("Music Play/UI/Progress/Fill Area/Fill").GetComponent<Image>();
        Difficulty = GameObject.Find("Music Play/UI/Difficulty").GetComponent<Image>();
        LengthsBar = GameObject.Find("Music Play/UI/Progress/Fill Area/Fill/Image").GetComponent<Image>();

        audioSource = GameObject.Find("Basic/Main Camera/Music Player").GetComponent<AudioSource>();

        start.onClick.AddListener(() => startOnClick());
        pause.onClick.AddListener(() => LeanTween.pause(GSongBar));
        resume.onClick.AddListener(() => LeanTween.pause(GSongBar));
        retry.onClick.AddListener(() => retryOnClick());
        quit.onClick.AddListener(() => quitOnClick());

        LengthsBar.rectTransform.sizeDelta = new Vector2(Screen.width, LengthsBar.rectTransform.sizeDelta.y);
        LengthsBar.transform.position = new Vector2((-86.5f * 2), LengthsBar.transform.position.y);
        isPlaying = false;
        isPlayingSetOnce = false;
        dspTimeEnd = 0f;
    }
    void Update()
    {
        if(isPlaying)
        {
            currentTime = audioSource.time;
            dspTimeEnd = audioSource.clip.length;
            if (SongBar.color != Difficulty.color)
            {
                SongBar.color = Color.white;
                LengthsBar.color = Difficulty.color;
            }
        }
        if (isPlayingSetOnce&& (Vector2)GSongBar.transform.position == fillVector2)
        {
            LeanTween.moveX(GSongBar, 85.5f, dspTimeEnd);
            isPlayingSetOnce = false;
        }
    }
    private void retryOnClick()
    {
        LeanTween.cancel(GSongBar);
        GSongBar.transform.position = new Vector2(-86.15f, 47.60f);
        LeanTween.move(GSongBar, fillVector2, 0.1f);
        GSongBar.transform.position = new Vector2(0, GSongBar.transform.position.z);
        isPlayingSetOnce = true;
    }
    private void quitOnClick()
    {
        isPlaying = false;
        LeanTween.cancel(GSongBar);
        GSongBar.transform.position = new Vector2(-86.15f, 47.60f);
        LeanTween.move(GSongBar, fillVector2, 0.1f);
        GSongBar.transform.position = new Vector2(0, GSongBar.transform.position.z);
    }
    private void startOnClick()
    {
        isPlayingSetOnce = true;
        isPlaying = true;
        LeanTween.move(GSongBar, fillVector2, 0.1f);
    }
}
