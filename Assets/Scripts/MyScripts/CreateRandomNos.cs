using System.Collections.Generic;
using UnityEngine;

public class CreateRandomNos : MonoBehaviour
{
    public int NoOfItemsPerColumn = 5;
    public int NoOfColumns = 5;
    public int animationIndex, columnIndex, indexInColumn;

    public int randnum;
    public int randnum2;
    public int randnum3;
    public LineItem randompayline;
    public LineItem randompayline2;
    public LineItem randompayline3;
    internal int lowitem;
    internal int hightitem;
    internal float chance;

    internal int Rand_For_HighWin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<List<int>> Columns = new() {

        [0] = new List<int>(),
        [1] = new List<int>(),
        [2] = new List<int>(),
        [3] = new List<int>(),
        [4] = new List<int>()

    };
    public bool[] IsWiled = {false,false,false,false,false };
    public int[] WildInSequanceCount = { 0, 0, 0, 0, 0 };


 public   void CreateRandomNumber()
    {
        SetLowHighRange();

        for (int colIndx = 0; colIndx < NoOfColumns; colIndx++)
        {
            columnIndex = colIndx;
            for (int ItmIndx=0; ItmIndx < NoOfItemsPerColumn; ItmIndx++)
            {
                indexInColumn = ItmIndx;

                animationIndex = GetItemOnProb();


                if (animationIndex == 9 && colIndx == 0)
                {
                    animationIndex = Random.Range(0, 9);
                }
                else if (SlotManager.instance.IsFreeSpinsEnabled && animationIndex == 11)
                {
                 animationIndex = Random.Range(0, 9);
                 
                }

                if (animationIndex == 9)
                {
                    IsWiled[colIndx] = true;
                }

                float maxRtp = SlotManager.instance.pakar_nakha + (SlotManager.instance.pakar_nakha * 0.10f);

                if (SlotManager.instance.game_nakha > maxRtp)
                {

                    ApiManager.instance.maxRtpExceed = true;
                  //  ApiManager.instance.StartCoroutine(ApiManager.instance.SetMaxRtpBool());
                }
                // print($"user rtp {SlotManager.instance.game_nakha} max rtp {maxRtp} exceed {GameSocketManager.Instance.maxRtpExceed}");
                if (SlotManager.instance.IsFreeSpinsEnabled && SlotManager.instance.FreeSpinsWinSum <= SlotManager.instance.totalBetAmount)
                {

                    if (chance <= 0.8f)
                        LowWin();
                    else
                        VeryLowWin();
                }

                else
                       if (SlotManager.instance.game_nakha > maxRtp || ApiManager.instance.maxRtpExceed)
                {
                    if (chance <= 0.5f)
                        Zerowin();
                    else if (chance > 0.5f && chance <= 0.9f)
                        VeryLowWin();
                    else if (chance > 0.9f)
                        LowWin();
                }
                else if (SlotManager.instance.totalBetAmount > 1)
                {
                    if (chance <= 0.3f)
                        Zerowin();
                    else if (chance > 0.3f && chance <= 0.9f)
                        VeryLowWin();
                    else if (chance > 0.9f)
                        HighWin();
                }
                else if (SlotManager.instance.game_nakha > SlotManager.instance.pakar_nakha)
                {
                    if (chance > 0 && chance <= 0.8)
                        LowWin();
                    else
                        HighWin();
                }
                else
                {
                    Optimize_gata();
                }
          

           

                if (animationIndex == 11 && indexInColumn < LineManager.instance.Items_In_Column)
                {
                    GameOperations.instance.BonusMatched += 1;
                }
                else
                     if (animationIndex == 9 && indexInColumn < LineManager.instance.Items_In_Column)
                     {
                    GameOperations.instance.scattersMatched += 1;

                    }

            }
        }
    }

    private void VeryLowWin()
    {



        if (columnIndex == 0)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[0])
            {
                animationIndex = lowitem;
               // itemType = SlotItemType.Normal;
            }

        }
        else
             if (columnIndex == 1)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[1])
            {


                animationIndex = Random.Range(5, 10);
                //if (animationIndex < 9)
                //    itemType = SlotItemType.Normal;
                //else
                //    itemType = SlotItemType.Wild;

            }

        }
        else
             if (columnIndex == 2)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[2])
            {
                animationIndex = lowitem;
              //  itemType = SlotItemType.Normal;
            }

        }
        else if (columnIndex == 3)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[3])
            {

                animationIndex = Random.RandomRange(0, 5);
               // itemType = SlotItemType.Normal;

            }

        }
        else
             if (columnIndex == 4)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[4])
            {
                animationIndex = Random.RandomRange(0, 5);
               // itemType = SlotItemType.Normal;
            }

        }

    }

    private void Optimize_gata()
    {

        if (columnIndex == 1)
        {
            while (ColumnManager.instance.columnScripts[0].column0indexes.Contains(animationIndex))
            {
                animationIndex = Random.Range(0, 9);
               // itemType = SlotItemType.Normal;
            }
        }


        if (SlotManager.instance.IsFreeSpinsEnabled)
        {
            // normal spin when user rtp is low and playing low bets and free spins round enabled 
            if (SlotManager.instance.FreeSpinsWinSum < (SlotManager.instance.totalBetAmount * 10f))
                HighWin();
            else
                LowWin();

        }
        else
        {
            // normal spin when user rtp is low and playing low bets

            if (chance <= 0.2f)
                LowWin();
            else if (chance > 0.2f && chance <= 0.9f)
                HighWin();
            else VeryHighWin();
        }
    }
    private void Zerowin()
    {

        if (columnIndex == 2)
        {
            while (ColumnManager.instance.columnScripts[0].column0indexes.Contains(animationIndex) || animationIndex == 9)
            {
                animationIndex = Random.Range(0, 9);
               // itemType = SlotItemType.Normal;
            }
        }
    }
    private void LowWin()
    {



        if (columnIndex == 0)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[0])
            {
                animationIndex = lowitem;
               // itemType = SlotItemType.Normal;
            }

        }
        else
             if (columnIndex == 1)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[1])
            {
                if (Random.value > 0.5 && !GameOperations.instance.WildAsMultiplier)
                {

                    animationIndex = 9;
                    //itemType = SlotItemType.Wild;
                }
                else
                {


                    animationIndex = Random.Range(5, 10);
                    //if (animationIndex < 9)
                    //    itemType = SlotItemType.Normal;
                    //else
                    //    itemType = SlotItemType.Wild;
                }
            }

        }
        else
             if (columnIndex == 2)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[2])
            {
                animationIndex = lowitem;
              //  itemType = SlotItemType.Normal;
            }

        }
        else if (columnIndex == 3)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[3])
            {
                animationIndex = Random.Range(6, 10);
                //if (animationIndex < 9)
                //    itemType = SlotItemType.Normal;
                //else
                //    itemType = SlotItemType.Wild;
            }

        }
        else
             if (columnIndex == 4)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[4])
            {
                animationIndex = Random.RandomRange(0, 5);
               // itemType = SlotItemType.Normal;
            }

        }

    }
    private void HighWin()
    {

        //  GameSocketManager.Instance.spins_counter = 0;

        if (columnIndex == 0)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[0])
            {
                animationIndex = hightitem;
               // itemType = SlotItemType.Normal;
            }

        }
        else
             if (columnIndex == 1)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[1])
            {
                animationIndex = hightitem;
               // itemType = SlotItemType.Normal;
            }

        }
        else
             if (columnIndex == 2)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[2])
            {
                animationIndex = hightitem;
              //  itemType = SlotItemType.Normal;
            }

        }
        else if (columnIndex == 3)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[3])
            {
                animationIndex = Random.Range(0, 10);
                //if (animationIndex < 9)
                //    itemType = SlotItemType.Normal;
                //else
                //    itemType = SlotItemType.Wild;
            }

        }
        else
             if (columnIndex == 4)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[4])
            {
                animationIndex = Random.Range(0, 10);
                //if (animationIndex < 9)
                //    itemType = SlotItemType.Normal;
                //else
                //    itemType = SlotItemType.Wild;

            }

        }

    }
    private void VeryHighWin()
    {

        //  GameSocketManager.Instance.spins_counter = 0;


        if (columnIndex == 0)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[0] || indexInColumn == ColumnManager.instance.randompayline2.rowIndex[0] || indexInColumn == ColumnManager.instance.randompayline3.rowIndex[0])
            {
                animationIndex = hightitem;
             //   itemType = SlotItemType.Normal;
            }

        }
        else
             if (columnIndex == 1)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[1] || indexInColumn == ColumnManager.instance.randompayline2.rowIndex[1] || indexInColumn == ColumnManager.instance.randompayline3.rowIndex[1])
            {
                animationIndex = hightitem;
               // itemType = SlotItemType.Normal;
            }

        }
        else
             if (columnIndex == 2)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[2] || indexInColumn == ColumnManager.instance.randompayline2.rowIndex[2] || indexInColumn == ColumnManager.instance.randompayline3.rowIndex[2])
            {
                animationIndex = hightitem;
               // itemType = SlotItemType.Normal;
            }

        }
        else if (columnIndex == 3)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[3] || indexInColumn == ColumnManager.instance.randompayline2.rowIndex[3] || indexInColumn == ColumnManager.instance.randompayline3.rowIndex[3])
            {
                if (chance > 0.1f)
                {
                    animationIndex = hightitem;
                   // itemType = SlotItemType.Normal;
                }


                else
                {
                    animationIndex = 9;
                   // itemType = SlotItemType.Wild;
                }
            }

        }
        else
             if (columnIndex == 4)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[4] || indexInColumn == ColumnManager.instance.randompayline2.rowIndex[4] || indexInColumn == ColumnManager.instance.randompayline3.rowIndex[4])
            {

                if (chance > 0.1f)
                {
                    animationIndex = hightitem;
                    //itemType = SlotItemType.Normal;
                }


                else
                {
                    animationIndex = 9;
                   // itemType = SlotItemType.Wild;
                }

            }

        }

    }
    
    public void CalculateTempWin()
    {
        LineManager.instance.SetLinesItems();
        LineManager.instance.TraceForCombinations();
    }

    internal void TraceForWildOnly()
    {
        for (int linesCount = 0; linesCount < SlotManager.instance.noOfLinesSelected; linesCount++)
        {
            for (int i = 0; i < 5; i++)
            {
                if (LineManager.instance.lineItemScripts[linesCount].lineSlotItems[i].itemType == SlotItemType.Wild)
                {
                    LineManager.instance.lineItemScripts[linesCount].WildInSequanceCount++;

                }
                else
                {
                    break;
                }
            }
        }
    }
    internal void TraceForNormalCombinations()
    {
        //Wild_in_Winline = 0;
        //firstItemIndex = -1;
        //count = 0;

        //for (int i = 0; i < 5; i++)
        //{

        //    if (lineSlotItems[i].itemType == SlotItemType.Wild)
        //    {
        //        wildItem = lineSlotItems[i];
        //        Wild_in_Winline++;
        //        count++;
        //        continue;
        //    }
        //    if (firstItemIndex == -1 && lineSlotItems[i].itemType == SlotItemType.Normal)
        //    {

        //        count++;
        //        firstItemIndex = lineSlotItems[i].animationIndex;
        //    }
        //    else if (lineSlotItems[i].animationIndex == firstItemIndex)
        //    {
        //        count++;
        //    }
        //    else
        //        break;

        //}
    }

    int GetItemOnProb()
    {
        int randomNo = Random.Range(0, 1000);
        if (randomNo <= ProbabilityManager.instance.ItemsWeightsRange[0])
        {
            animationIndex = 0;
        }
        for (int i = 1; i < 12; i++)
        {
            if (randomNo > ProbabilityManager.instance.ItemsWeightsRange[i - 1] && randomNo <= ProbabilityManager.instance.ItemsWeightsRange[i])
            {
                animationIndex = i;
                if (GameOperations.instance.NoBonusAtAll)
                {
                    animationIndex = Random.Range(0, 9);
                }
                

                if (animationIndex == 10 && GameOperations.instance.BonusMatched >= 3 && i < LineManager.instance.Items_In_Column)
                {
                    animationIndex = 8;
                }
                else if (animationIndex == 10 && GameOperations.instance.scattersMatched >= 3 && i < LineManager.instance.Items_In_Column)
                {
                    animationIndex = 7;
                }

            }
        }


       
        return animationIndex;
    }

    private void SetLowHighRange()
    {
        Rand_For_HighWin = Random.Range(30, 100);
        //  GameSocketManager.Instance.spins_counter++;
        randnum = Random.Range(0, LineManager.instance.lineItemScripts.Length);
        randompayline = LineManager.instance.lineItemScripts[randnum];
        randnum2 = Random.Range(0, LineManager.instance.lineItemScripts.Length);
        while (randnum2 == randnum)
        {
            randnum2 = Random.Range(0, LineManager.instance.lineItemScripts.Length);
        }
        randompayline2 = LineManager.instance.lineItemScripts[randnum2];

        randnum3 = Random.Range(0, LineManager.instance.lineItemScripts.Length);
        while (randnum3 == randnum || randnum3 == randnum2)
        {
            randnum3 = Random.Range(0, LineManager.instance.lineItemScripts.Length);
        }
        randompayline3 = LineManager.instance.lineItemScripts[randnum3];

        chance = Random.value;
        lowitem = Random.Range(5, 9);
        hightitem = Random.Range(0, 7);
      
        GameOperations.instance.BonusMatched = 0;
        GameOperations.instance.scattersMatched = 0;
      
    }
}
