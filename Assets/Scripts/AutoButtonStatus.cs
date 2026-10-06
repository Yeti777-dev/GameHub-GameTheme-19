using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoButtonStatus : MonoBehaviour
{
    public GameObject[] buttons;

    public void ShowButtons() {
 
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].SetActive(true);
        }
    }

    public void HideButtons() {


        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

}
