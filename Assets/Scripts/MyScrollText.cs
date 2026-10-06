using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MyScrollText : MonoBehaviour
{
    float scrollStep = 0;
    float tempScore;
    Text scoreText;
    float _initialScore;
    float _difference;
   private AudioSource clickSound;
    public AudioClip incrementSound, FinishedSound;
    public static MyScrollText instant_scroll;
    public bool WinCounting = false;
    private void Start()
    {
        clickSound = GetComponent<AudioSource>();
        instant_scroll = this;
    }
    public void ScrollTo(Text text, float initialScore, float finalScore, float duration, float initialDelay)
    {
        tempScore = 0;
        float frequency = .08f;

        _initialScore = initialScore;
        scoreText = text;
        float functionCallPerSecond = 1.0f / frequency;
        float difference = finalScore - initialScore;
        scrollStep = difference / (functionCallPerSecond * duration);
        _difference = difference;
        clickSound.clip = incrementSound;
        InvokeRepeating("_ScrollText", initialDelay, frequency);
    }

    private void Update()
    {
        if (WinCounting)
        {
            if (Input.GetMouseButtonUp(0))
            {
                tempScore = _difference;
            }
        }
    }

    void _ScrollText()
    {
        WinCounting = true;

        if (!clickSound.isPlaying)
            clickSound.Play();
        tempScore += scrollStep;

        if (_difference > 0)
        {
         
            if (tempScore >= _difference)
            {
                clickSound.clip = FinishedSound;
                clickSound.Play();
                
                tempScore = _difference;
                CancelInvoke("_ScrollText");
                iTween.PunchScale(scoreText.gameObject, new Vector3(.2f, .2f, .2f), 1f);
                WinCounting = false;
              //  Destroy(GetComponent<ScrollTextScript>());
            }
        }
        else
        {
            if (tempScore <= _difference)
            {
                clickSound.clip = FinishedSound;
                clickSound.Play();
                tempScore = _difference;
                CancelInvoke("_ScrollText");
                iTween.PunchScale(scoreText.gameObject, new Vector3(.2f, .2f, .2f), 1f);
                WinCounting = false;

                //  Destroy(GetComponent<ScrollTextScript>());
            }
        }

        scoreText.text = "" + ((float)(tempScore + _initialScore)).ToString("F2");
    }

   
}
