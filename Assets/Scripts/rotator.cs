using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour
{
    // Start is called before the first frame update

    public float angle_inc = 3f;
    Vector3 rotationEuler;
    void Update()
    {
        rotationEuler += Vector3.back * angle_inc * Time.deltaTime; //increment 30 degrees every second
        transform.rotation = Quaternion.Euler(rotationEuler);

    }
}