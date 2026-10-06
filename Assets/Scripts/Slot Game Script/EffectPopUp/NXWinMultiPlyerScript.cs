using UnityEngine;
using System.Collections;

public class NXWinMultiPlyerScript : MonoBehaviour
{

    public iTween.EaseType easeType;
    public GameObject multiPlyerGo;
    // Use this for initialization
    void Start()
    {

        transform.position = new Vector3(50, 0.5f, -3);
        Invoke("ShowText", .1f);
        
    }

    void ShowText()
    {
        multiPlyerGo.SetActive(true);
        iTween.Defaults.easeType = easeType;
        iTween.ScaleFrom(gameObject, Vector3.zero, 2f);

        Invoke("HideText", 2f);

    }

    void HideText()
    {
        iTween.Defaults.easeType = easeType;
        iTween.ScaleTo(gameObject, Vector3.zero, 0.5f);
        Destroy(gameObject, 0.6f);
    }
}
