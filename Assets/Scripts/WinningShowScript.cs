using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningShowScript : MonoBehaviour
{
    public iTween.EaseType easeType;
    public GameObject multiPlyerGo;
    public GameObject blackFade;
    private GameObject fadeclone;
    public float ShowNormal = 5f;
    public float ShowTurbo = 3f;

    // public Vector3 showPos = new Vector3(49.58f, 0.39f, -1);
    // Use this for initialization
    void Start()
    {

        // fadeclone = (GameObject)Instantiate(blackFade, blackFade.transform.position, Quaternion.identity);
      //  multiPlyerGo.SetActive(false);
        //    transform.position = showPos;
        //    GetComponentInChildren<WinningShowScript>().enabled = false;
        //    Vector3 pos0 = new Vector3();

        //    if(transform.childCount >1)
        //        pos0 = transform.GetChild(0).position;
        //    Vector3 pos1 = new Vector3();
        //    if (transform.childCount > 1)
        //        pos1 = transform.GetChild(1).position;



        //    pos0.z = 0;
        //    pos1.z = -1;

        //    if (transform.childCount > 1)
        //        transform.GetChild(0).position = pos0;

        //    if (transform.childCount > 1)
        //        transform.GetChild(1).position = pos1;

        //    if (transform.childCount > 2) {
        //        for (int i = 2; i < transform.childCount; i++) { 

        //            Vector3 tempos = transform.GetChild(i).position;
        //            tempos.z = 0;
        //           transform.GetChild (i).position = tempos;

        //        }


        //    }
        //   SlotManager.instance.FadePanel = fadeclone;
        SlotManager.instance.WinShowPanel = gameObject;
       // Invoke("ShowText", 0.1f);

    }
    
    void ShowText()
    {
        multiPlyerGo.SetActive(true);

        iTween.Defaults.easeType = easeType;
        if (!GUIManager.instance.TurboBool)
        {
           // iTween.ScaleFrom(gameObject, Vector3.zero, 1f);
           // Invoke("HideText", ShowNormal);
        }
        else
        {
          //  iTween.ScaleFrom(gameObject, Vector3.zero, 0.5f);
          //  Invoke("HideText", ShowTurbo);
        }

    }
  
    void HideText()
    {
        iTween.Defaults.easeType = easeType;
        iTween.ScaleTo(gameObject, Vector3.zero, 0.5f);
      //  Destroy(fadeclone, 0.1f);

        Destroy(gameObject, 0.6f);
    }
}
