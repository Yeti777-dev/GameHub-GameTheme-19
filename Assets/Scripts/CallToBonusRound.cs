using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallToBonusRound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void BonusRoundCaller() {
        Invoke("CallBonus", 1f);

    }
    void CallBonus() {
        SlotManager.instance.MybonusReward();
        Destroy(gameObject);

    }
}
