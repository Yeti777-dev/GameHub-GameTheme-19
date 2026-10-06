using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Bigwinscroll : MonoBehaviour
{
    float scrollStep = 0;
    float tempScore;
    TextMeshPro scoreText;
    float _initialScore;
    float _difference;
   private AudioSource clickSound;
    public AudioClip incrementSound, FinishedSound;
    public static Bigwinscroll instant_Bscroll;
  public bool  WinCounting = false;

    private void Start()
    {
        clickSound = GetComponent<AudioSource>();
        instant_Bscroll = this;
    }

    public void ScrollTo(TextMeshPro text, float initialScore, float finalScore, float duration, float initialDelay)
    {
        tempScore = 0;
        float frequency = .08f;
        GUIManager.instance.SetGuiButtonState(false);

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
        if (WinCounting) {
            if (Input.GetMouseButtonUp(0))
            {
                tempScore = _difference;
            }
        }
    }

    void _ScrollText()
    {
        WinCounting = true;
        tempScore += scrollStep;

        if (!clickSound.isPlaying)
            clickSound.Play();

        if (_difference > 0)
        {
            if (tempScore >= _difference)
            {
                clickSound.clip = FinishedSound;
                clickSound.Play();
                tempScore = _difference;
                CancelInvoke("_ScrollText");
                iTween.PunchScale(scoreText.gameObject, new Vector3(0.03f,0.03f,1f),0.1f);
                CanEndInvoke();
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
                iTween.PunchScale(scoreText.gameObject, new Vector3(0.03f, 0.03f, 1f), 0.1f);
                CanEndInvoke();


            }
        }

        scoreText.text = "" + ((float)(tempScore + _initialScore)).ToString("F2");
    }

    void CanEndInvoke() {
        WinCounting = false;
        float movetime = 1;
        float scaletime = 1.1f;
        if (GUIManager.instance.SpinNumbers > 0 || GameOperations.instance.noOfFreeSpin > 0)
        {
            movetime = 0.5f;
            scaletime = 0.6f;
        }


        iTween.MoveTo(scoreText.gameObject, iTween.Hash("x", GUIManager.instance.totalCoinText.transform.position.x, "y", GUIManager.instance.totalCoinText.transform.position.y, "time", movetime, "easetype", iTween.EaseType.easeInExpo, "oncomplete", "ReachedTarget"));


        iTween.ScaleTo(scoreText.gameObject, iTween.Hash("scale", Vector3.zero, "time", scaletime, "easetype", iTween.EaseType.easeInExpo));

    }

}
