using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeCanvasPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    void DestroyWhenFadeCompletes() {
        Destroy(gameObject);
    }
}
