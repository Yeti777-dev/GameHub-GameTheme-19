using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class callerToFreeSpinsTotal : MonoBehaviour
{
    private TextMeshPro winningtext;
    // Start is called before the first frame update
    void Start()
    {
        winningtext = GetComponent<TextMeshPro>();

        Invoke("startScrolling", 1f);

        // winningtext.text = ""+SlotManager.instance.currentSpinWinningAmount.ToString("#,##0");
        winningtext.text = "";
    }
    void startScrolling()
    {

        FreeSpinTotal.inst_Freescroll.ScrollTo(winningtext, 0, SlotManager.instance.FreeSpinsWinSum, 6, 0);



    }
    public void ReachedTarget()
    {
        if (GUIManager.instance.SpinNumbers > 0 || GameOperations.instance.noOfFreeSpin > 0)
            iTween.PunchScale(GUIManager.instance.totalCoinText.gameObject, this.transform.localScale * 2f, 0.5f);
        else
            iTween.PunchScale(GUIManager.instance.totalCoinText.gameObject, this.transform.localScale * 2f, 1f);

        GUIManager.instance.UpdateGUI();
        GameOperations.instance.FreeSpinsCompleted();
        Destroy(GameObject.FindGameObjectWithTag("FreeSpinsTotal"),1f);
    }
}
