using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEventDisableParent : MonoBehaviour
{
    public float delayDisable;

    public void DestroyThisObjandCall()
    {

        GameEffects.instance.CreateFreeSpinInfo();
        
        Invoke("DestroyThis", delayDisable);
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
       
    }

    public void DisableONEvent() {
        gameObject.SetActive(false);

    }
}
