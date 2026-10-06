using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MyWinningScroll : MonoBehaviour
{
    float scrollStep = 0;
    float tempScore;
    TextMeshPro scoreText;
    float _initialScore;
    float _difference;
    private AudioSource clickSound;
    public AudioClip incrementSound, FinishedSound;
    public static MyWinningScroll inst_myscroll;
    public bool WinCounting = false;
    private void Start()
    {
        clickSound = GetComponent<AudioSource>();
        inst_myscroll = this;
    }
    public void ScrollTo(TextMeshPro text, float initialScore, float finalScore, float duration, float initialDelay)
    {
        WinCounting = true;
        tempScore = 0;
        float frequency = .08f;
        duration = (finalScore - initialScore);
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
                if(scoreText !=null)
                iTween.PunchScale(scoreText.gameObject, new Vector3(.2f, .2f, .2f), 0.1f);
                if (!SlotManager.instance.IsFreeSpinsEnabled && !SlotManager.instance.IsBonusEnabled) {
                    GUIManager.instance.SetGuiButtonState(true);
                    GUIManager.instance.StopSpinButton.SetActive(false);
                }
                Invoke("DestroyWinShow", 1);
            
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
                if (scoreText != null)
                    iTween.PunchScale(scoreText.gameObject, new Vector3(.2f, .2f, .2f), 0.1f);
                if (!SlotManager.instance.IsFreeSpinsEnabled && !SlotManager.instance.IsBonusEnabled)
                {
                    GUIManager.instance.SetGuiButtonState(true);
                    GUIManager.instance.StopSpinButton.SetActive(false);

                }
                Invoke("DestroyWinShow", 1);

                //  Destroy(GetComponent<ScrollTextScript>());
            }
        }
        if (scoreText != null)
            scoreText.text = "" + ((float)(tempScore + _initialScore)).ToString("F2");
    }

    void DestroyWinShow()
    {
        WinCounting = false;
        if (SlotManager.instance.WinShowPanel)
        {
            float movetime = 1;
            float scaletime = 1.1f;
            if (GUIManager.instance.SpinNumbers > 0 || GameOperations.instance.noOfFreeSpin > 0)
            {
                movetime = 0.5f;
                scaletime = 0.6f;
            }


            iTween.MoveTo(scoreText.gameObject, iTween.Hash("x", GUIManager.instance.totalCoinText.transform.position.x, "y", GUIManager.instance.totalCoinText.transform.position.y, "time", movetime, "easetype", iTween.EaseType.easeInExpo, "oncomplete", "ReachedTarget"));


            iTween.ScaleTo(scoreText.gameObject, iTween.Hash("scale", Vector3.zero, "time", scaletime, "easetype", iTween.EaseType.easeInExpo));

         


            Destroy(SlotManager.instance.WinShowPanel, 2f);
        }

    }
}
