using UnityEngine;
using System.Collections;

public class RayRotationScript : MonoBehaviour {

	public float speed = 150f;

	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime, Space.Self);  
	}
}
