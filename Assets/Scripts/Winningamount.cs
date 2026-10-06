using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winningamount : MonoBehaviour
{
    private TextMesh winningtext;
    // Start is called before the first frame update
    void Start()
    {
        winningtext = GetComponent<TextMesh>();
        winningtext.text = SlotManager.instance.currentSpinWinningAmount.ToString("F2");
      //  ScrollTextScript.Scroll(winningtext, 0, SlotManager.instance.currentSpinWinningAmount, 2, 0);
     //   winningtext.text = "$"+SlotManager.instance.currentSpinWinningAmount.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
