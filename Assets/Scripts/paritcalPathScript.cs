using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paritcalPathScript : MonoBehaviour
{
    public string pathname;
    public iTween.EaseType eastype;
    public float time;
    void OnEnable()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(pathname), "easytype", eastype, "time", time));
    }

}
