using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickSprite : MonoBehaviour
{
   
    public MonoBehaviour scriptToCall;
    public string methodToInvoke;
    private BoxCollider2D col;

    private Color Normal;
    public Color Highlight;
    public bool ifCanHighlight = true;
    private void Start()
    {
        Normal = GetComponent<SpriteRenderer>().color;
        Highlight.a = 1;
        if (!TryGetComponent(out col)) {
          col = gameObject.AddComponent<BoxCollider2D>();
           
        }

    }
    private void OnMouseEnter()
    {
        if(ifCanHighlight)
        GetComponent<SpriteRenderer>().color = Highlight;
        
    }
    private void OnMouseExit()
    {
        if (ifCanHighlight)
            GetComponent<SpriteRenderer>().color = Normal;
    }

    void OnMouseUp()
    {
       

        if (scriptToCall != null)
            scriptToCall.Invoke(methodToInvoke, 0.01f);
    }
}
