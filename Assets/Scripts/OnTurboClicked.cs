using UnityEngine;
using System.Collections;
//using ChartboostSDK;

public class OnMyButtonClicked : MonoBehaviour
{
    public MonoBehaviour scriptToCall;
    public string methodToInvoke;

    private Vector3 InitialScale;
    public GameObject spinParticle;
   
    private void Start()
    {
        InitialScale = transform.localScale;
  
    }
    private void OnMouseDown()
    {
      

    }

  

    void OnMouseUp()
    {
      
        if (scriptToCall != null)
            scriptToCall.Invoke(methodToInvoke, 0.01f);
    }
}
