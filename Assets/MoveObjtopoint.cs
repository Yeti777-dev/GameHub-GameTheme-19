using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjtopoint : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 SpawnPos;
    public Vector3 Point1;
    public float stay;
    public Vector3 point2;

    public iTween.EaseType easeType = iTween.EaseType.easeOutSine;

   void Awake() { 
    
    }
    void Start()
    {
        
    }

    void OnEnable() {
        this.transform.position = SpawnPos;
        iTween.Defaults.easeType = iTween.EaseType.easeOutSine;

        iTween.MoveTo(this.gameObject, Point1, stay/4);
        Invoke("MoveAside", stay);

    }
    void MoveAside() {
        iTween.Defaults.easeType = iTween.EaseType.easeOutSine;
        iTween.MoveTo(this.gameObject, point2, stay / 4);
        Invoke("offlinewin", 1);

    }
    void offlinewin() { 
    
        this.gameObject.SetActive(false);
    }
    void OnDisable() {
        this.transform.position = SpawnPos;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
