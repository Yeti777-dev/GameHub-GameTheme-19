using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GetFreeSpins : MonoBehaviour
{
    private TextMeshPro mesh;
    private void Start()
    {
        mesh = GetComponent<TextMeshPro>();
        mesh.text = GameOperations.instance.noOfFreeSpin.ToString() + " Free Spins";

    }
    private void OnEnable()
    {
        
    }
}
