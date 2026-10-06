using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class UltimateBonusRecorder : MonoBehaviour
{
   public static UltimateBonusRecorder Instance;
    internal List<Vector2> bonusPos = new List<Vector2>();
    internal List<float> reward = new List<float>();


    void Awake() { 
    
    Instance = this;
    }

    public void ClearBonusRecords() { 
    
    bonusPos.Clear();
        reward.Clear();
    }

}
