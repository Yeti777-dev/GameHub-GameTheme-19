using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPaytable : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Pages;
    private int counter = 0;
    private int maxCounter;
    private int MinCounter = 0;
    void Start()
    {
        DisplayPage(counter);
        maxCounter = Pages.Length-1; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayPage(int index) {

        for (int i = 0; i < Pages.Length; i++) {

            if (i == index)
                Pages[i].SetActive(true);
            else
                Pages[i].SetActive(false);
        }
    
    }

    public void OnNextPage() { 
    
      if(counter < maxCounter)
            counter++;
      else
            counter = 0;
        DisplayPage(counter);

    }

    public void OnPreviousPage() {
        if (counter > 0)
            counter--;
        else counter = maxCounter;
        DisplayPage(counter);
    
    }
}
