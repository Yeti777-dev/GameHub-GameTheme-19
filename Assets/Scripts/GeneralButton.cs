using UnityEngine;
using System.Collections;
//using ChartboostSDK;

public class GeneralButton : MonoBehaviour
{
    public MonoBehaviour scriptToCall;
    public string methodToInvoke;
    public bool ShowInfo = false;
    public GameObject info;
    private void Start()
    {
     
    }
    private void OnMouseEnter()
    {
        if (ShowInfo) {
            info.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        if (ShowInfo)
        {
            info.SetActive(false);
        }
    }

    private void OnMouseDown()
    {

        SoundFxManager.instance.buttonTapSound.Play();
    }

  

    void OnMouseUp()
    {
      
        if (scriptToCall != null)
            scriptToCall.Invoke(methodToInvoke, .1f);
       
    }
}
