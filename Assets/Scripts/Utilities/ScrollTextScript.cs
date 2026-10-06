using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScrollTextScript : MonoBehaviour 
{
    float scrollStep = 0;    
    float tempScore;
    TextMesh scoreText;
    float _initialScore;
    float _difference;
    AudioSource clickSound;

    void AssignAudio(AudioSource _audio)
    {
        clickSound = _audio;
    }

    internal void ScrollTo(TextMesh text, float initialScore, float finalScore, float duration,float initialDelay)
    {
        float frequency = .08f;

        _initialScore = initialScore;
        scoreText = text;
        float functionCallPerSecond = 1.0f / frequency;
        float difference = finalScore - initialScore;
        scrollStep = difference / (functionCallPerSecond * duration);
        _difference = difference;
        InvokeRepeating("_ScrollText", initialDelay, frequency);
    }

  

    void _ScrollText()
    {
        tempScore += scrollStep;

        if(clickSound)
            clickSound.Play();

        if (_difference > 0)
        {
            if (tempScore >= _difference)
            {
                tempScore = _difference;
                CancelInvoke("_ScrollText");
                iTween.PunchScale(scoreText.gameObject, new Vector3(.2f, .2f, .2f), 1f);
                Destroy(GetComponent<ScrollTextScript>());
            }
        }
        else
        {
            if (tempScore <= _difference)
            {
                tempScore = _difference;
                CancelInvoke("_ScrollText");
                iTween.PunchScale(scoreText.gameObject, new Vector3(.2f, .2f, .2f), 1f);
                Destroy(GetComponent<ScrollTextScript>());
            }
        }

        scoreText.text =  ((float)(tempScore + _initialScore)).ToString("#,##0");
    }

    public static void Scroll(TextMesh text, float initialScore, float finalScore, float duration,float initialDelay)
    {
        text.gameObject.AddComponent<ScrollTextScript>();
        text.GetComponent<ScrollTextScript>().ScrollTo(text, initialScore, finalScore, duration, initialDelay);
    }
   

    public static void Scroll(TextMesh text, float initialScore, float finalScore, float duration, float initialDelay,AudioSource audio)
    {
        text.gameObject.AddComponent<ScrollTextScript>();
        text.GetComponent<ScrollTextScript>().AssignAudio(audio);
        text.GetComponent<ScrollTextScript>().ScrollTo(text, initialScore, finalScore, duration, initialDelay);
    }
}
