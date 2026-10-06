using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomInOutObj : MonoBehaviour
{
    public Vector3 zoomScaleOut;
    public Vector3 zoomScaleIn;
    private Vector3 inititalScale;
    public Vector3 differvalue = new Vector3(0.2f, 0.2f, 0.2f);
    public float timer = 0;
    public bool IszoomOut = false;
    public float speed =1;
    public bool CanInimate = true;
    private void Awake()
    {
        inititalScale = transform.localScale;
        zoomScaleIn = inititalScale + differvalue;
        zoomScaleOut = inititalScale - differvalue;
        transform.localScale = zoomScaleOut;
    }

    private void OnEnable()
    {
        timer = 0;
        IszoomOut = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy && CanInimate) {

            if (timer > 1) {

                IszoomOut = true;
            }
            if (timer < 0) {
                IszoomOut = false;
            }


            if (!IszoomOut)
            {

                timer += Time.deltaTime*speed;
                transform.localScale = Vector3.Lerp(zoomScaleOut, zoomScaleIn, timer);
            }
            else {
                timer -= Time.deltaTime *speed;
               // transform.localScale = Vector3.Lerp(zoomScaleIn, zoomScaleOut, timer);
                transform.localScale = Vector3.Lerp(zoomScaleOut, zoomScaleIn, timer);
            }



     
         

        }
        
    }
}
