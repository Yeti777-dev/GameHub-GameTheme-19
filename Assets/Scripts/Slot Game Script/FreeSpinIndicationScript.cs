using UnityEngine;
using System.Collections;

public class FreeSpinIndicationScript : MonoBehaviour
{

    public static FreeSpinIndicationScript instance;

    void Awake()
    {
        SlotManager.CanSpinAgain = false;

        instance = this;
    }

    void Start()
    {
     //   iTween.Defaults.easeType = iTween.EaseType.easeOutExpo;
      //  transform.position = new Vector3(0.04f, 2.57f, -2);
       // iTween.MoveFrom(gameObject, transform.position + new Vector3(0, 3f, 0), 3);
    }


    internal void DestroyFreeSpinIndication()
    {
        gameObject.GetComponent<Animator>().SetTrigger("FreeSpinUp");
      //  iTween.MoveTo(gameObject, transform.position + new Vector3(0, 3f, 0), 0);
        SlotManager.CanSpinAgain = true;
        if (GUIManager.instance.SpinNumbers > 0 && SlotManager.CanSpinAgain)
        {
            this.Invoke("playspin", 0.2f);
        }
        Destroy(gameObject, 4);
    }
    void playspin() {
        SlotManager.instance.SpinAgain();
    }
}
