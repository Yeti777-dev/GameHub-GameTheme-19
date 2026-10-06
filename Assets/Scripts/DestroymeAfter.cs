using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroymeAfter : MonoBehaviour
{
    public float Showtime;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }
    private void OnEnable()
    {
        Invoke("HideMe", Showtime);
    }
     void HideMe() {
        this.gameObject.SetActive(false);
    }

}
