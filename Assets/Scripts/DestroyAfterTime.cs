using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float Showtime;
    // Start is called before the first frame update
    void Start()
    {


    }
    private void OnEnable()
    {
 
        Invoke("destroy", Showtime);

    }
    void destroy()
    {
        Destroy(gameObject);
    }

}
