using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChoosePackageClicked : MonoBehaviour
{

    private void Start()
    {
        Invoke("FunctionCaller", GameEffects.instance.freespinStartDelay);
    }


    void FunctionCaller()
    {

        GameOperations.instance.ContinueToFreeSpins();
        if (this.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer render))
            Destroy(this.transform.root.gameObject, 0.1f);
        else
           Destroy(gameObject, 0.1f);
    }
}
