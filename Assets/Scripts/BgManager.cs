using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgManager : MonoBehaviour
{
    public static BgManager instance;
    public GameObject[] Bgs;
    void Start()
    {
        instance = this;
        SetNormlBg();
    }

    public void SetFreespinBg() {
        Bgs[0].SetActive(false);
        Bgs[1].SetActive(true);
        SoundFxManager.instance.PlayFreeSpinBg();
    }

    public void SetNormlBg() {
        Bgs[1].SetActive(false);
        Bgs[0].SetActive(true);
        SoundFxManager.instance.PlayNormalBg();
    }


}
