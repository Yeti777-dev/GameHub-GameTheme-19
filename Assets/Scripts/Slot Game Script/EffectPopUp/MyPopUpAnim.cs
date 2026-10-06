using UnityEngine;
using System.Collections;

public class MyPopUpAnim : MonoBehaviour
{

    public iTween.EaseType easeType;
    public GameObject multiPlyerGo;
    public GameObject blackFade;
    private GameObject fadeclone;
    public float ShowNormal = 5f;
    public float ShowTurbo = 3f;
    // Use this for initialization
    void Start()
    {

        // transform.position = new Vector3(45.5f, -0.75f, -3);
       // gameObject.transform.localScale = Vector3.zero;
        fadeclone = (GameObject)  Instantiate(blackFade, blackFade.transform.position, Quaternion.identity);
        // multiPlyerGo.SetActive(false);
         transform.position = new Vector3(48.8f, -0.27f, -96);
       // Time.timeScale = 0.2f;
        Invoke("ShowText", 0.1f);

    }

    void ShowText()
    {
    
        iTween.Defaults.easeType = easeType;
        if (!GUIManager.instance.TurboBool)
        {
            iTween.ScaleFrom(gameObject, Vector3.zero, 1f);
            Invoke("HideText", ShowNormal);
        }
        else {
            iTween.ScaleFrom(gameObject, Vector3.zero, 0.5f);
            Invoke("HideText", ShowTurbo);
        }
     
    }

    void HideText()
    {
        iTween.Defaults.easeType = easeType;
        iTween.ScaleTo(gameObject, Vector3.zero, 0.5f);
        Destroy(fadeclone, 0.1f);

        Destroy(gameObject, 0.6f);
    }
}
