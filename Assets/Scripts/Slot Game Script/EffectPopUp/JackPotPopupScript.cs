using UnityEngine;
using System.Collections;

public class JackPotPopupScript : MonoBehaviour 
{
    public GameObject bg;
    public GameObject jackPotText;
    public float speed = 120f;
    public iTween.EaseType easeType;
   // public ParticleEmitter coinPfx;

    void Start()
    {
       
        transform.position = new Vector3(50, 0, -3);
        bg.SetActive(false);
        jackPotText.SetActive(false);

        Invoke("ShowBG", .1f);
        Invoke("ShowText", .1f);
        Invoke("DestroyPopup", 3f);

            GetComponent<AudioSource>().Play();
    }

    void StopPfxEmission()
    {
      
       // coinPfx.emit = false;
    }

    void ShowBG()
    {
        bg.SetActive(true);
        iTween.Defaults.easeType = easeType;
      
        iTween.FadeFrom(bg, 0, 1);
    }

    void ShowText()
    {
        jackPotText.SetActive(true);
        iTween.Defaults.easeType = easeType;
        iTween.ScaleFrom(jackPotText, Vector3.zero, 1f);
        iTween.RotateUpdate(jackPotText, new Vector3(0,0,270), 1f);
    }



    internal void DestroyPopup()
    {
        StopPfxEmission();
        HideBG();
        HideText();
    }

    void HideBG()
    {
        bg.SetActive(true);
        iTween.Defaults.easeType = easeType;
        iTween.FadeTo(bg, 0, 1);
    }

    void HideText()
    {
      
        iTween.Defaults.easeType = easeType;
        iTween.ScaleTo(jackPotText, Vector3.zero, 1);
        Destroy(gameObject, 1.1f);     
    }
}
