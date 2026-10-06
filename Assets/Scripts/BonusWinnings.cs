using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BonusWinnings : MonoBehaviour
{
    private TextMeshPro winningtext;
    // Start is called before the first frame update
    void OnEnable()
    {
        winningtext = GetComponent<TextMeshPro>();
        if (!GUIManager.instance.TurboBool)
            Invoke("startScrolling", 0.5f);
        else
            Invoke("startScrolling", 0.2f);
        winningtext.text = "";

    }
    void startScrolling()
    {

        winningtext.text = BonusRoundManager.instance.CurrentBonusWinnings.ToString("F2");

        //if (!GUIManager.instance.TurboBool)
        //    MyWinningScroll.inst_myscroll.ScrollTo(winningtext, 0, BonusRoundManager.instance.CurrentBonusWinnings, 2, 0);
        //else
        //    MyWinningScroll.inst_myscroll.ScrollTo(winningtext, 0, BonusRoundManager.instance.CurrentBonusWinnings, 1, 0);


    }

}
