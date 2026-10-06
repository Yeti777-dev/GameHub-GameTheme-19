using UnityEngine;
using System.Collections;

public class ButtonMessage : MonoBehaviour {
    public GameObject target;
    public string message;
    public int no;

    public void OnClick()
    {
        if (target) target.SendMessage(message, no, SendMessageOptions.DontRequireReceiver);
    }
}
