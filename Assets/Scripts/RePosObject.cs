using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePosObject : MonoBehaviour
{

   

    public void CallBonusRoundContr() {
        SlotManager.instance.MybonusReward();
        Destroy(gameObject);
    }
    void Update()
    {
        
    }
}
