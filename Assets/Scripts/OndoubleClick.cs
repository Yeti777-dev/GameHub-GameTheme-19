using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndoubleClick : MonoBehaviour
{
    float clicktime = 0;
  public  float clickdelay = 0.5f;
    int clicknumber = 0;

    public string ClipName;
    public float jumptoAnimTime = 0.9f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - clicktime > clickdelay) {
            clicktime = 0;
            clicknumber = 0;
        }
     
    }

    private void OnMouseDown()
    {
        clicknumber++;
        clicktime = Time.time;
        
    }

    private void OnMouseUp()
    {
        if (clicknumber == 2 && Time.time - clicktime < clickdelay)
        {
            clicktime = 0;
            clicknumber = 0;
    
            GetComponent<Animator>().Play(ClipName, 0, 0.9f);

        }
        else {
            if (clicknumber > 2 || Time.time - clicktime > clickdelay) {
                clicktime = 0;
                clicknumber = 0;
              
            }
        }
        
    }
}
