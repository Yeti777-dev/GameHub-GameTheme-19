using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProbabilityManager : MonoBehaviour
{
    public static ProbabilityManager instance;
    // Start is called before the first frame update
    public float[] ItemsProbabilities;

    public float[] ItemsWeights = new float[12];
    public float[] ItemsWeightsRange = new float[12];
    public float sumofWeight;
    public float SumofProb=0;
    private void Awake()
    {
        //ItemsProbabilities = new float[] { 7, 7, 8, 9, 10, 10, 13, 13, 16, 3, 2, 2 };
        instance = this;
    }
    void Start()
    {
        getProbSum();
    }

   public void getProbSum() {
        SumofProb = 0;
        for (int i = 0; i < ItemsProbabilities.Length; i++) {
            SumofProb += ItemsProbabilities[i];
        }
        GetItemsWeights();
       
    }
    void GetItemsWeights() {

        sumofWeight = 0;
        for (int i = 0; i < ItemsProbabilities.Length; i++)
        {
            ItemsWeights[i] = (ItemsProbabilities[i] / SumofProb) * 1000;
            sumofWeight += ItemsWeights[i];
        }
        setWeightRange();

    }
    void setWeightRange() {
        ItemsWeightsRange[0] = ItemsWeights[0];

        for (int i = 1; i < ItemsProbabilities.Length; i++)
        {
            ItemsWeightsRange[i] = ItemsWeights[i] + ItemsWeightsRange[i-1];
        }

    }
  
 

    // Update is called once per frame
    void Update()
    {
        
    }
}
