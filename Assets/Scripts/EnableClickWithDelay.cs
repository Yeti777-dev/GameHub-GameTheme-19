using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableClickWithDelay : MonoBehaviour
{
    public GameObject[] SpriteButtons;
    public float EnableButtonDelay = 5f;


    void Awake()
    {
        if (SpriteButtons.Length > 1)
        {
            SpriteButtons[0].SetActive(false);
            SpriteButtons[1].SetActive(false);
        }

    }

    void OnEnable()
    {

        Invoke("Nextround", 5f);

    }

    void Nextround()
    {

        BonusRoundManager.instance.ContinueToNextRound();
    }
}
