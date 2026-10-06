using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardsAnimation : MonoBehaviour {

    public List<GameObject> cards = new List<GameObject>();
    public List<Material> blackFaces = new List<Material>();
    public List<Material> redFaces = new List<Material>();
    public List<Material> numbers = new List<Material>();
    int cardsIndex = 0;


	// Use this for initialization
	void Start () {
        StartCoroutine(StartAnimation(0.1f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}



    IEnumerator StartAnimation(float time)
    {
        if (cardsIndex < cards.Count) { 
        yield return new WaitForSeconds(time);
        cards[cardsIndex].SetActive(true);
        cardsIndex++;
        StartCoroutine(StartAnimation(0.1f));
        }
    }
}
