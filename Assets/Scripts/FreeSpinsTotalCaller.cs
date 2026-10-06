using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FreeSpinsTotalCaller : MonoBehaviour
{
    private Text winningtext;
    // Start is called before the first frame update
    void Start()
    {
        winningtext = GetComponent<Text>();
        Invoke("startScrolling", 1.5f);
        //   winningtext.text = "$"+SlotManager.instance.currentSpinWinningAmount.ToString("F2");
    }
    void startScrolling()
    {
     //   FreeSpinsTotalCaller..ScrollTo(winningtext, 0, SlotManager.instance.currentSpinWinningAmount, 2, 0);

    }
}
