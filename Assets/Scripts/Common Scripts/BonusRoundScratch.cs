using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRoundScratch : MonoBehaviour
{
    public OnCardSelected[] Cards;
    public List<int> rewards = new List<int> { 0,25,50, 250,500,750,1000,1250,1500 };
    public int Passes_remains = 3;
    public static BonusRoundScratch instance;
    public int TotalBonusWinnings = 0;
    private void Start()
    {
        instance = this;
        GUIManager.instance.SetGuiButtonState(false);
        StartbonusRound();
    }

    void StartbonusRound() {

        for (int i = 0; i < Cards.Length; i++) {
          int RandomBonus =  Random.Range(0, rewards.Count);

            Cards[i].Reward_Inside = rewards[RandomBonus];
            rewards.RemoveAt(RandomBonus);
           }
    }

    private void Update()
    {
        if (Passes_remains == 0) {
            BlockCards();
        }
    }
    private void BlockCards() {

        foreach (OnCardSelected cd in Cards) {
            cd.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

}
