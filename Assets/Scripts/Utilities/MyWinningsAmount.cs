using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MyWinningsAmount : MonoBehaviour
{
    private TextMeshPro winningtext;
    // Start is called before the first frame update
    void Start()
    {
        winningtext = GetComponent<TextMeshPro>();
          
          winningtext.text = "";
        startScrolling();
    }
    void startScrolling() {
        if (SlotManager.instance.IsFreeSpinsEnabled)
        {
            SlotManager.instance.FreeSpinsWinSum += SlotManager.instance.currentSpinWinningAmount;
            SlotManager.instance.UpdateFreespinsTotal();
        }
        if (GUIManager.instance.SpinNumbers > 0 || GameOperations.instance.noOfFreeSpin > 0) {

            MyWinningScroll.inst_myscroll.ScrollTo(winningtext, 0, SlotManager.instance.currentSpinWinningAmount, 2, 0);
        }
        else
        {
           // winningtext.text = SlotManager.instance.currentSpinWinningAmount.ToString("F2");

          //  GUIManager.instance.SetGuiButtonState(true);
          //  GUIManager.instance.StopSpinButton.SetActive(false);
            if (!GUIManager.instance.TurboBool)
                MyWinningScroll.inst_myscroll.ScrollTo(winningtext, 0, SlotManager.instance.currentSpinWinningAmount, 10, 0);
            else
                MyWinningScroll.inst_myscroll.ScrollTo(winningtext, 0, SlotManager.instance.currentSpinWinningAmount, 10f, 0);
        }

    }

    public void ReachedTarget()
    {
        if (GUIManager.instance.SpinNumbers > 0 || GameOperations.instance.noOfFreeSpin > 0)
            iTween.PunchScale(GUIManager.instance.totalCoinText.gameObject, this.transform.localScale * 2f, 0.5f);
        else
            iTween.PunchScale(GUIManager.instance.totalCoinText.gameObject, this.transform.localScale * 2f, 1f);

        GUIManager.instance.UpdateGUI();
        OnCountComplete();

    }
    public void OnCountComplete()
    {

        SlotManager.instance.checkNextSpinAfterWin();
        if (SlotManager.instance.IsFreeSpinsEnabled)
            GameOperations.instance.CheckFreeSpinOnCountComplete();
    }
}
