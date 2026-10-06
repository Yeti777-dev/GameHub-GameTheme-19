using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideThisOnEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Hidethis() {

        gameObject.SetActive(false);
    }
}
