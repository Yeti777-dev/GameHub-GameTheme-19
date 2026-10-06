using UnityEngine;
using System.Collections;

public class BigWinPopupScript : MonoBehaviour 
{
 
    public GameObject bigWinText;
    public GameObject starLeft;
    public GameObject starRight;
  //  public ParticleEmitter coinPfx;
  //  public ParticleEmitter starPfx;
    public iTween.EaseType easeType;


    void Start()
    {
      
        transform.position = new Vector3(50, 0, -3.5f);


        bigWinText.SetActive(false);
        starLeft.SetActive(false);
        starRight.SetActive(false);

      
        Invoke("ShowText", .1f);
        Invoke("ShowStar", 1.2f);
        Invoke("DestroyPopup", 3f);
        Invoke("HideStar", 3f);

            GetComponent<AudioSource>().Play();
    }

   
    void ShowText()
    {
       // FaderScript.instance.BringItForward();
        bigWinText.SetActive(true);
        iTween.Defaults.easeType = easeType;
        iTween.ScaleFrom(bigWinText, Vector3.zero, .2f);

    }


    void ShowStar()
    {
        starLeft.SetActive(true);
        starRight.SetActive(true);
        iTween.Defaults.easeType = easeType;
        iTween.ScaleFrom(starLeft, Vector3.zero, .2f);
        iTween.ScaleFrom(starRight, Vector3.zero, .2f);
    }

    void HideStar()
    {
        iTween.ScaleFrom(starLeft, Vector3.zero, 1f);
        iTween.ScaleFrom(starRight, Vector3.zero, 1f);
        Destroy(gameObject, 2f);
    }

    internal void DestroyPopup()
    {
       
        HideText();
        Invoke("StopPfxEmission", 0.8f);
    }

    void StopPfxEmission()
    {
      //  starPfx.emit = false;
     //   coinPfx.emit = false;
    }

    void HideText()
    {
        bigWinText.SetActive(true);
      //  FaderScript.instance.Invoke("BringItBack", 1f);
        iTween.ScaleTo(gameObject, Vector3.zero, 1);
      
    }
}
