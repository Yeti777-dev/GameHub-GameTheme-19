using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiviateObjects : MonoBehaviour
{
    public bool ifPunchScale = false;
    public GameObject[] children;
    public bool disableChildrenOnDisable = false;
    // Start is called before the first frame update

    public void activeChildOjects() {

        for (int i = 0; i < children.Length; i++) {
            children[i].SetActive(true);

            if (ifPunchScale) {
                iTween.PunchScale(gameObject, new Vector3(0.2f, 0.2f, 1f), 1f);
            }
        }
    }

    public void OnDisable()
    {
        if(disableChildrenOnDisable)
        for (int i = 0; i < children.Length; i++)
        {
                children[i].SetActive(false);
        }
    }
}
