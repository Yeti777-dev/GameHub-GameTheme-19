using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceAnim : MonoBehaviour
{
    public float animationTime = 5;
    void Start()
    {

        Invoke("DisableAnimator", animationTime);
    }

    void DisableAnimator()
    {

        GetComponent<Animator>().enabled = false;

    }
}
