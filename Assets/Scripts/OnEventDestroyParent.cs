using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEventDestroyParent : MonoBehaviour
{

    public float delayDestroy;

    public MonoBehaviour ScriptToCall;
    public string MethodToCall;
    public void DestroyParentObj() {

        Invoke("destroyafterDelay", delayDestroy);
    }
    private void destroyafterDelay() {
        if (ScriptToCall !=null) {
            ScriptToCall.Invoke(MethodToCall, 0.1f);
        }
        Destroy(transform.parent.gameObject,1f);
    }
}
