using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class getWinningAmount : MonoBehaviour
{
    private TextMeshPro Winamount;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Winamount.text = BonusRoundManager.instance.CurrentBonusWinnings.ToString("F2");
    }
}
