using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BigAmounts : MonoBehaviour
{
    private  TextMeshPro winningtext;
    // Start is called before the first frame update
    void Start()
    {
        if (SlotManager.instance.IsFreeSpinsEnabled)
        {
            SlotManager.instance.FreeSpinsWinSum += SlotManager.instance.currentSpinWinningAmount;
            SlotManager.instance.UpdateFreespinsTotal();
        }
        winningtext = GetComponent<TextMeshPro>();
        LineManager.instance.BigBigWins = true;
        Bigwinscroll.instant_Bscroll.ScrollTo(winningtext, 0, SlotManager.instance.currentSpinWinningAmount, 6, 0);
        //   winningtext.text = "$"+SlotManager.instance.currentSpinWinningAmount.ToString("F2");
    }
    public void ReachedTarget()
    {
        if (GUIManager.instance.SpinNumbers > 0 || GameOperations.instance.noOfFreeSpin > 0)
            iTween.PunchScale(GUIManager.instance.totalCoinText.gameObject, this.transform.localScale * 2f, 0.5f);
        else
            iTween.PunchScale(GUIManager.instance.totalCoinText.gameObject, this.transform.localScale * 2f, 1f);

        GameEffects.instance.CelebrationEnds();

        GUIManager.instance.UpdateGUI();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
