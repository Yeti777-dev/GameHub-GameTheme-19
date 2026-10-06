using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragonscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DrakasFunction", 2f, 5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DrakasFunction() {
        GetComponent<Animator>().SetTrigger("drakas");
    }
}
