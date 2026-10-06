using UnityEngine;
using System.Collections;

public class SpacialPopupScript : MonoBehaviour 
{
    public GameObject bg;
    public GameObject cleanSweepText;

    public iTween.EaseType easeType;


    void Start()
    {
       
        transform.position = new Vector3(50, 0, -3);
        bg.SetActive(false);
        cleanSweepText.SetActive(false);

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
        cleanSweepText.SetActive(true);
        iTween.Defaults.easeType = easeType;
        iTween.MoveFrom(cleanSweepText, new Vector3(cleanSweepText.transform.position.x + 10, cleanSweepText.transform.position.y, cleanSweepText.transform.position.z), 1f);
    }



    internal void DestroyPopup()
    {
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
        cleanSweepText.SetActive(true);
        iTween.Defaults.easeType = easeType;
        iTween.MoveTo(cleanSweepText, new Vector3(cleanSweepText.transform.position.x - 10, cleanSweepText.transform.position.y, cleanSweepText.transform.position.z), 1f);
        Destroy(gameObject, 2f);
    }
}
