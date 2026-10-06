using UnityEngine;
using System.Collections;

public class SuperBigWinPopupScript : MonoBehaviour 
{
    public GameObject bg;
    public GameObject megaWinText;
   // public ParticleEmitter coinPfx;
   // public ParticleEmitter starPfx;
    public iTween.EaseType easeType;


    void Start()
    {
        
        transform.position = new Vector3(50, 0, -3);
        bg.SetActive(false);
        megaWinText.SetActive(false);

        Invoke("ShowBG", .1f);
        Invoke("ShowText", .1f);
        Invoke("DestroyPopup", 3f);

            GetComponent<AudioSource>().Play();
    }

    void ShowBG()
    {
        bg.SetActive(true);
       
        // iTween.ScaleFrom(bg, new Vector3(18, 12, 1), 2f);
        iTween.FadeFrom(bg, 0, 1);
    }

    void ShowText()
    {
        megaWinText.SetActive(true);
        iTween.Defaults.easeType = easeType;
        iTween.ScaleFrom(megaWinText, Vector3.zero, 1f);

        iTween.RotateFrom(megaWinText, new Vector3(0, 0, 90), 1f);
    }

    void StopPfxEmission()
    {
        //starPfx.emit = false;
       // coinPfx.emit = false;
    }

    internal void DestroyPopup()
    {
        StopPfxEmission();
        HideText();
        HideBG();
       
    }

    void HideBG()
    {
        bg.SetActive(true);
        iTween.Defaults.easeType = easeType;
        iTween.FadeTo(bg, 0, 1.2f);
    }

    void HideText()
    {
        iTween.Defaults.easeType = easeType;
        iTween.ScaleTo(megaWinText, Vector3.zero, 1);
        Destroy(gameObject, 1.1f);     
    }
}
