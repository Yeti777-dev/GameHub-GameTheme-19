using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraSpinsManager : MonoBehaviour
{
    public static ExtraSpinsManager instance;
    // Start is called before the first frame update
    
    void Start()
    {
        instance = this;
    
    }
    public void ShowExtraSpins(int ExtraSpinFromServer) {

        StartCoroutine(StartExtraSpins(ExtraSpinFromServer));
    }

    public IEnumerator StartExtraSpins(int ExtraSpins) {
        yield return new WaitForSeconds(0.001f);
        SlotManager.instance.IsFreeSpinsEnabled = true;
        GameOperations.instance.noOfFreeSpin = ExtraSpins;
        GameEffects.instance.CreateAwardedSpinMessage();



    }
}
