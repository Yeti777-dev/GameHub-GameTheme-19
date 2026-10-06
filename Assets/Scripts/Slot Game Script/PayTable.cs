using UnityEngine;
using System.Collections;
using TMPro;
public class PayTable : MonoBehaviour
{
    /// PayTable Instacne..
    public static PayTable instance;
    /// Item First Reward Info..
    public Vector2[] item1;
    /// Item Second Reward Info..
    public Vector2[] item2;
    /// Item Third Reward Info..
    public Vector2[] item3;
    /// Item Fourth Reward Info..
    public Vector2[] item4;
    /// Item Fifth Reward Info..
    public Vector2[] item5;
    /// Item Sixth Reward Info..
    public Vector2[] item6;
    /// Item Seventh Reward Info..
    public Vector2[] item7;
    /// Item Eigth Reward Info..
    public Vector2[] item8;
    /// Item Ninth Reward Info..
    public Vector2[] item9;

    public Vector2[] autoSpinActive;
    public Vector2[] WildInSequanceAmount;

    public float MinWinPerBonusItem = 25;  // based on total bet and bonus items matched
    public float[] WinPerBonusRound;        
    private int winingMultiplyer;

    private void Awake()
    {
        //item1 = new Vector2[] { new Vector2(5, 500), new Vector2(4, 100), new Vector2(3, 45) };
        //item2 = new Vector2[] { new Vector2(5, 250), new Vector2(4, 80), new Vector2(3, 40) };
        //item3 = new Vector2[] { new Vector2(5, 200), new Vector2(4, 70), new Vector2(3, 35) };
        //item4 = new Vector2[] { new Vector2(5, 100), new Vector2(4, 60), new Vector2(3, 30) };
        //item5 = new Vector2[] { new Vector2(5, 80), new Vector2(4, 50), new Vector2(3, 25) };
        //item6 = new Vector2[] { new Vector2(5, 60), new Vector2(4, 45), new Vector2(3, 20) };
        //item7 = new Vector2[] { new Vector2(5, 50), new Vector2(4, 40), new Vector2(3, 15) };
        //item8 = new Vector2[] { new Vector2(5, 40), new Vector2(4, 30), new Vector2(3, 10) };
        //item9 = new Vector2[] { new Vector2(5, 30), new Vector2(4, 25), new Vector2(3, 10) };

        //WildInSequanceAmount = new Vector2[] { new Vector2(5, 150), new Vector2(4, 100), new Vector2(3, 50) };
        //autoSpinActive = new Vector2[] { new Vector2(5, 18), new Vector2(4, 12), new Vector2(3, 7) };
    }
    void Start()
    {
        instance = this;
  
    
    }


    /// Privide Item First Reward ..
    internal void SetMultiplier(int matchCount, Vector2[] payTable)
    {
        switch (matchCount)
        {
            case 5:
                winingMultiplyer = (int)payTable[0].y;
                break;
            case 4:
                winingMultiplyer = (int)payTable[1].y;
                break;
            case 3:
                winingMultiplyer = (int)payTable[2].y;
                break;
            //case 2:
            //    winingMultiplyer = (int)payTable[3].y;
            //    break;
            //case 1:
            //    winingMultiplyer = (int)payTable[4].y;
            //    break;

        }

     
    }
    internal void SetWildMultiplier(int matchCount, Vector2[] payTable)
    {
        switch (matchCount)
        {
            case 3:
                winingMultiplyer = (int)payTable[0].y;
                break;
            case 2:
                winingMultiplyer = (int)payTable[1].y;
                break;
            case 1:
                winingMultiplyer = (int)payTable[2].y;
                break;
             

        }


    }
    internal int WiningMultiPlyerOnALine()
    {
        return winingMultiplyer;
    }

    internal int CkeckInPayTable(int itemIndex, int matchCount)
    {
        Vector2[] currentArray = item1;
        winingMultiplyer = 0;
       


        switch (itemIndex)
        {
            case 0:
                currentArray = item1;
                break;
            case 1:
                currentArray = item2;
                break;
            case 2:
                currentArray = item3;
                break;
            case 3:
                currentArray = item4;
                break;
            case 4:
                currentArray = item5;
                break;
            case 5:
                currentArray = item6;
                break;
            case 6:
                currentArray = item7;
                break;
            case 7:
                currentArray = item8;
                break;
            case 8:
                currentArray = item9;
                break;

        }

        if(5 - matchCount < currentArray.Length )
            SetMultiplier(matchCount, currentArray);

        return winingMultiplyer;
    }

    public float GivemeBonusRoundWin(int round) {
        float RoundWin = 0;
        RoundWin = SlotManager.instance.totalBetAmount * WinPerBonusRound[round];

        return RoundWin;
    }

    public float GiveMeMinBonusWin()
    {
        float MinBonusWin = 0;
        MinBonusWin =GameOperations.instance.BonusMatched * SlotManager.instance.totalBetAmount;

        return MinBonusWin;
    }
    public float GiveMeMaxBonusWin() {
        float MaxWin = 0;
        for (int i = 0; i < WinPerBonusRound.Length; i++) {
            MaxWin += GivemeBonusRoundWin(i);
        }

        return MaxWin;

    }


}// End Script..
