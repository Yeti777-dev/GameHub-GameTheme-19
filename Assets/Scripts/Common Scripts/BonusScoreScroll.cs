using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BonusScoreScroll : MonoBehaviour
{
    private TextMeshPro winningtext;
    // Start is called before the first frame update
    void Start()
    {
        winningtext = GetComponent<TextMeshPro>();
        if (!GUIManager.instance.TurboBool)
            Invoke("startScrolling", 0.5f);
        else
            Invoke("startScrolling", 0.3f);
        winningtext.text = "";

    }
    void startScrolling()
    {
       
        if (!GUIManager.instance.TurboBool)
            MyWinningScroll.inst_myscroll.ScrollTo(winningtext, 0, SlotManager.instance.currentSpinWinningAmount, 7, 0);
        else
            MyWinningScroll.inst_myscroll.ScrollTo(winningtext, 0, SlotManager.instance.currentSpinWinningAmount, 5, 0);


    }
}
