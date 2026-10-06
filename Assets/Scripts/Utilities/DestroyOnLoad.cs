using UnityEngine;
using System.Collections;

public class DestroyOnLoad : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        Destroy(gameObject);

    }
}
