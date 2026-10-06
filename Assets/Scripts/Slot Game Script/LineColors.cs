using UnityEngine;
using System.Collections;

public class LineColors : MonoBehaviour
{
    public static LineColors instance;

    public Color[] lineColors;


    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        
    }

}
