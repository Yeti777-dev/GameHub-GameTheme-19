using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjOnAnimEvent : MonoBehaviour
{
    public GameObject Anim;
    // Start is called before the first frame update

    public void SetAnimActive()
    {

        Anim.SetActive(true);

    }
}
