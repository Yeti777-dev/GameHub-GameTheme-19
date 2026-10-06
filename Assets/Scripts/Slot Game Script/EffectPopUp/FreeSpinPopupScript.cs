using UnityEngine;
using System.Collections;

public class FreeSpinPopupScript : MonoBehaviour 
{
    public GameObject bg;
    public GameObject freeSpinText;
   
  //  public ParticleEmitter starPfx;
    public iTween.EaseType easeType;
    

    void Start()
    {
       
        transform.position = new Vector3(50, 0, -3);
        bg.SetActive(false);
        freeSpinText.SetActive(false);

        Invoke("ShowBG", .1f);
        Invoke("ShowText", .1f);
        Invoke("DestroyPopup", 3f);
            GetComponent<AudioSource>().Play();
    }

    void ShowBG()
    {
        bg.SetActive(true);
        iTween.Defaults.easeType = easeType;
       // iTween.ScaleFrom(bg, new Vector3(18, 12, 1), 2f);
        iTween.FadeFrom(bg, 0, 1);
    }

    void ShowText()
    {
        freeSpinText.SetActive(true);
        iTween.Defaults.easeType = easeType;
        iTween.MoveFrom(freeSpinText, new Vector3(freeSpinText.transform.position.x - 10, freeSpinText.transform.position.y, freeSpinText.transform.position.z), 2f);
    }

    void StopPfxEmission()
    {
     //   starPfx.emit = false;
       
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
        freeSpinText.SetActive(true);
        iTween.Defaults.easeType = easeType;
        iTween.MoveTo(freeSpinText, new Vector3(freeSpinText.transform.position.x + 10, freeSpinText.transform.position.y, freeSpinText.transform.position.z), 2f);
        Destroy(gameObject, 2f);
    }
}
