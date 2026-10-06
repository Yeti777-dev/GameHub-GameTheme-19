using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColumnManager : MonoBehaviour
{
    /// Coloum Manager Script Instance..
    public static ColumnManager instance;
    /// All Coloum Reference Array..
    public ColumnScript[] columnScripts;
    public List<GameObject> wildclones;
    public float MyGap;
    public bool LongSpin = false;
    public int ColumnsStopped = 5;
    public bool ShouldStopAtOnce = false;
    public bool isStickyWild = false;
    public iTween.EaseType easetype = iTween.EaseType.linear;
    public float setTime = 0.5f;

    public int randnum;
    public int randnum2;
    public int randnum3;
    public LineItem randompayline;
    public LineItem randompayline2;
    public LineItem randompayline3;
    internal int lowitem;
    internal int hightitem;
    internal float chance;

    // Backend RTP outcome values for deterministic wins
    internal bool backendWon = false;
    internal float suggestedMultiplier = 0f;
    internal float maxAllowedMultiplier = 0f;
    internal WinType backendWinType = WinType.Zero;

    // Pre-calculated winning configuration
    internal int targetMatchCount = 0;  // How many symbols should match (3, 4, or 5)
    internal int targetSymbol = 0;      // Which symbol to use for matching
    internal int targetPaylineCount = 1; // How many paylines should win

    internal int Rand_For_HighWin;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Set Coloum Indentifire Index..
        for (int i = 0; i < columnScripts.Length; i++)
        {
            columnScripts[i].columnIndex = i;
        }
        if (setTime > 0.8f)
            setTime = 0.5f;
    }
    public void AssingLuckyPercentage(int value)
    {
        for (int i = 0; i < columnScripts.Length; i++)
        {
            columnScripts[i].luckyItemPercent = value;
        }

    }

    /// When Called Slot Item Start Spining..
    public IEnumerator StartSpinningColumn()
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
        ColumnsStopped = 5;
        GameOperations.instance.BonusMatched = 0;
        GameOperations.instance.scattersMatched = 0;
        ShouldStopAtOnce = false;
        SetSpinResult();
        yield return null;
        LineManager.instance.SetLinesItems();
        yield return null;
        LineManager.instance.TraceForCombinations();
        yield return null;

        CalculateWinningAmount();
        yield return null;
        StopCoroutine(StartSpinningColumn());

    }

    public void StartSpin()
    {

        for (int i = 0; i < columnScripts.Length; i++)
        {
            columnScripts[i].StartSpin();
        }
    }

    void SetSpinResult()
    {

        for (int c = 0; c < columnScripts.Length; c++)
        {
            int index = 0;
            for (int r = 0; r < columnScripts[c].slotItemList.Count; r++)
            {

                SlotItem item = columnScripts[c].slotItemList[r];
                item.indexInColumn = index;
                index++;

                if (index < LineManager.instance.Items_In_Column)
                    item.SetItemOnProb();


            }

        }



    }
    public void CalculateWinningAmount()
    {
        SlotManager.instance.currentSpinWinningAmount = 0;

        for (int i = 0; i < LineManager.instance.lineItemScripts.Length; i++)
        {

            SlotManager.instance.currentSpinWinningAmount += LineManager.instance.lineItemScripts[i].totalWin;
        }
        SlotManager.instance.GamePlayOutPut = "";
        for (int i = 0; i < ColumnManager.instance.columnScripts.Length; i++)
        {
            for (int k = 0; k < LineManager.instance.Items_In_Column; k++)
            {
                SlotManager.instance.GamePlayOutPut = string.Concat(SlotManager.instance.GamePlayOutPut, ColumnManager.instance.columnScripts[i].slotItemList[k].animationIndex + ",");
            }
        }


        // print(SlotManager.instance.GamePlayOutPut);
        // print("currentSpinWinningAmount = " + SlotManager.instance.currentSpinWinningAmount);
        //  print("totalBetAmount = " + SlotManager.instance.totalBetAmount);


        if (!SlotManager.instance.IsFreeSpinsEnabled)
            ApiManager.instance.StartCoroutine(ApiManager.instance.UpdateWalletCor(System.MathF.Round(SlotManager.instance.currentSpinWinningAmount, 2), System.MathF.Round(SlotManager.instance.totalBetAmount, 2), SlotManager.instance.GamePlayOutPut, false));
        else
            ApiManager.instance.StartCoroutine(ApiManager.instance.UpdateWalletCor(SlotManager.instance.currentSpinWinningAmount, 0, SlotManager.instance.GamePlayOutPut, false));


    }

    public void StopSpinnigButtonClicked()
    {
        StartCoroutine(StopSpinningCor());
    }
    public IEnumerator StopSpinningCor()
    {
        ShouldStopAtOnce = true;
        for (int i = 0; i < columnScripts.Length; i++)
        {
            if (columnScripts[i].isSpinning)
            {
                columnScripts[i].shouldStopSpinning = true;
                columnScripts[i].CancelInvoke("StopSpin");
                yield return new WaitForSeconds(0.3f);
            }

        }
        StopCoroutine(StopSpinningCor());
    }

    internal SlotItem GetSlotItemAt(int columnIndex, int rowIndex)
    {
        return (SlotItem)columnScripts[columnIndex].slotItemList[rowIndex];

    }

}// End Script


public enum WinType
{
    Zero,
    VeryLow,
    Low,
    Medium,
    High,
    VeryHigh
}
