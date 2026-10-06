using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleZoomIn : MonoBehaviour
{
    public iTween.EaseType easeType;

    // Start is called before the first frame update
    void Start()
    {
        ZoomStarts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ZoomStarts() {
        iTween.Defaults.easeType = easeType;
        iTween.ScaleFrom(gameObject, Vector3.zero, 1.345f);

    }
}
