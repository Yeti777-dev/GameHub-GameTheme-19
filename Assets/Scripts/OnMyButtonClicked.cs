using UnityEngine;
using System.Collections;
//using ChartboostSDK;

public class OnTurboClicked : MonoBehaviour
{
    public MonoBehaviour scriptToCall;
    public string methodToInvoke;

   
    private void Start()
    {
     
    }
    private void OnMouseDown()
    {

        SoundFxManager.instance.buttonTapSound.Play();
    }

  

    void OnMouseUp()
    {
      
        if (scriptToCall != null)
            scriptToCall.Invoke(methodToInvoke, 0.01f);
       
    }
}
