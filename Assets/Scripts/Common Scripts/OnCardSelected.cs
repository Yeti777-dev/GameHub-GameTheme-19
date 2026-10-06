using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCardSelected : MonoBehaviour
{
    public int Reward_Inside;
    private void OnMouseUp()
    {
        Debug.Log("Reward inside is " + Reward_Inside);
        gameObject.SetActive(false);
        BonusRoundScratch.instance.Passes_remains--;
        BonusRoundScratch.instance.TotalBonusWinnings += Reward_Inside;
        Debug.Log("total bonus winnings are " + BonusRoundScratch.instance.TotalBonusWinnings);

    }
}
