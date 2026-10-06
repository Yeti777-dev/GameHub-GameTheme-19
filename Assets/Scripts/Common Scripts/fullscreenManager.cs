using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fullscreenManager : MonoBehaviour
{
    public static fullscreenManager instance;
    private void Start()
    {
        instance = this;
#if UNITY_WEBGL
#else
           fullscreenbutton.SetActive(false);

#endif

        fullscreenbutton.SetActive(false);
    }
    public GameObject fullscreenbutton;
    public Sprite fullscreenSp, windowSp;

   
    public void fullscreenOnOff() {

        ResolutionSelector.instance.FullScreenToggle();
        Invoke("changeImage", 0.2f);
      //  print("fullscreen is " + Screen.fullScreen);
    }

    public void changeImage() {

        fullscreenbutton.GetComponent<Image>().sprite = (Screen.fullScreen ? windowSp : fullscreenSp);
    }
}
