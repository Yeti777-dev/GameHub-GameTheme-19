using UnityEngine;
using System.Collections;

public class BonusPopupScript : MonoBehaviour
{
    public GameObject bg;
    public iTween.EaseType easeType;

 //   public ParticleEmitter starPfx;

    void Start()
    {
       // transform.position = new Vector3(50, 0, -3.5f);
        bg.SetActive(false);
        Invoke("ShowBG", .1f);
        Destroy(gameObject, 3f);        
    }

    void ShowBG()
    {
        bg.SetActive(true);
        iTween.ScaleFrom(gameObject, new Vector3(1, 1, 1), 1.5f);
    }
}
