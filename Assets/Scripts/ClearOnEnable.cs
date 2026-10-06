using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearOnEnable : MonoBehaviour
{
    public GameObject[] LockerArray;
    private void OnEnable()
    {
        for (int i = 0; i < LockerArray.Length; i++)
        {
            LockerArray[i].SetActive(false);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < LockerArray.Length; i++)
        {
            LockerArray[i].SetActive(true);
        }
    }
}
