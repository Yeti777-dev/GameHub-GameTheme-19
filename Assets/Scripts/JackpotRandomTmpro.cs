using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JackpotRandomTmpro : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshPro m_TextMeshPro;
    public float PrizeRangeMin;
    public float PrizeRangeMax;

    public float RandomPrize;
    void Start()
    {
        m_TextMeshPro = GetComponent<TextMeshPro>();
        RandomPrize = Random.Range(PrizeRangeMin, PrizeRangeMax);
        m_TextMeshPro.text = "$" + RandomPrize.ToString("F2");

        InvokeRepeating("IncrementPrize", 10, 5);
    }

    void IncrementPrize()
    {

        float increment = RandomPrize * 0.005f;
        RandomPrize += increment;
        m_TextMeshPro.text = "$" + RandomPrize.ToString("F2");


    }

}
