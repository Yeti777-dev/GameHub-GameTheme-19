using UnityEngine;
using System.Collections;
using UnityEngine.PlayerLoop;

public class LineManager : MonoBehaviour
{

    /// LineManager Script Instance..
    public static LineManager instance;
    public LineItem []lineItemScripts;
    public bool BigBigWins = false;
    public int Items_In_Column = 3;
    public GameObject[] PayColumnItems;
    public bool IsshowPayline = false;
    public int showLineNumber = 1;
    void Awake()
    {
        instance = this;
        lineItemScripts = transform.GetComponentsInChildren<LineItem>();
    


    }

    void Update()
    {

        if (IsshowPayline)
            for (int i = 0; i < lineItemScripts.Length; i++)
            {
                if (showLineNumber == 0)
                    showLineNumber = 1;

                if (i == showLineNumber-1)
                    lineItemScripts[i].ShowItemPayline();
              
            }
    }
    void Start()
    {
        SlotManager.instance.noOfLinesSelected = lineItemScripts.Length;

        GUIManager.instance.CalculateTotalBetAmount();        
        ShowEabledLine(false);
    }
  
    internal void SetLinesItems()
    {
        for (int i = 0; i < SlotManager.instance.noOfLinesSelected; i++)
        {
            lineItemScripts[i].SetCurrentLineItems();
            lineItemScripts[i].lineNumberIndex = i;
        }   
    }
    public void HideAllItemPayline()
    {
        for (int i = 0; i < lineItemScripts.Length; i++)
        {
            lineItemScripts[i].HideItemPayline();
        }
    }
    internal void TraceForCombinations()
    {
        for (int i = 0; i < SlotManager.instance.noOfLinesSelected; i++)
        {
            lineItemScripts[i].TraceForCombinations();   
        }   
    }

    internal void TransforBonusCombination()
    {
        int bonusItemsCount = 0;

        bonusItemsCount = 0;
        for (int i = 0; i < LineManager.instance.Items_In_Column; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                SlotItem slotItem = (SlotItem)ColumnManager.instance.GetSlotItemAt(j, i);
                if (slotItem.itemType == SlotItemType.Bonus)
                {
                    bonusItemsCount++;

                }

            }

        }

        if (bonusItemsCount >= SlotManager.instance.BonusTriggerCounter && !SlotManager.instance.IsFreeSpinsEnabled)
        {

            if (GameOperations.instance.GameHasBonus)
            {
                GUIManager.instance.SetGuiButtonState(false);
                SlotManager.CanSpinAgain = false;
                SlotManager.instance.IsBonusEnabled = true;
                OnSlotItemClicked.CanShowSlotInfo = false;
                GameEffects.instance.ShowBonusPopupEffect();
            }
            else
            {
                SlotManager.CanSpinAgain = false;
                GameOperations.instance.RewardBonusSpins(GameOperations.instance.FixedBonusSpin);

            }


        }
    }

    internal void CheckForSpecialEffect()
    {

        if (CheckForJackPot())
            GameEffects.instance.JackPotShow();
        else
        if (CheckForCleanSweep())
            GameEffects.instance.CleanSweepShow();
        else if (CheckForSuperWin())
            GameEffects.instance.SuperWinShow();
        else if (CheckForSpecialWin())
            GameEffects.instance.SpecialWinShow();
        else if (CheckForMegaWin())
            GameEffects.instance.MegaWinShow();
        else if (CheckForBigWin())
           GameEffects.instance.BigWinShow();  
     
       
    
    }

  


   



    bool CheckForJackPot()
    {
        bool isJackpot = false;

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 30) && SlotManager.instance.currentSpinWinningAmount >5)
        {
            isJackpot = true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

           // Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
           // GUIManager.instance.UpdateGUI();

        }
        return isJackpot;
    }

    bool CheckForCleanSweep()
    {
        bool isCleanSweep = false;

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 25) && SlotManager.instance.currentSpinWinningAmount > 5)
        {
            isCleanSweep = true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

          //  Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
          //  GUIManager.instance.UpdateGUI();

        }

        return isCleanSweep;
    }

    bool CheckForSuperWin()
    {
        bool IsSuperWin = false;

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 20) && SlotManager.instance.currentSpinWinningAmount > 5)
        {
            IsSuperWin = true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

          //  Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
           // GUIManager.instance.UpdateGUI();

        }

        return IsSuperWin;
    }
    bool CheckForSpecialWin()
    {
        bool IsSpecialWin = false;

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 15) && SlotManager.instance.currentSpinWinningAmount > 5)
        {
            IsSpecialWin = true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

         //   Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
          //  GUIManager.instance.UpdateGUI();

        }

        return IsSpecialWin;
    }
    bool CheckForMegaWin()
    {
        bool IsMegaWin = false;
   

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 10) && SlotManager.instance.currentSpinWinningAmount > 5)
        {
            IsMegaWin = true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

          //  Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
          //  GUIManager.instance.UpdateGUI();

        }

        return IsMegaWin;
    }
    bool CheckForBigWin()
    {
        bool IsBigWin = false;

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 5) && SlotManager.instance.currentSpinWinningAmount > 5)
        {
            IsBigWin= true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

         //   Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
           // GUIManager.instance.UpdateGUI();

        }

        return IsBigWin;
    }

    bool CheckForBonusWin()
    {
        bool isBonusWin = false;

        
       
            for (int i = 0; i < SlotManager.instance.noOfLinesSelected; i++)
            {
                if (lineItemScripts[i].bonusSlotItemCount >= 12)
                {
                    if (!SlotManager.instance.IsFreeSpinsEnabled)
                    {
                        isBonusWin = true;
                    BigBigWins = true;
               
                    }
                    else
                    {
                        SlotManager.instance.bonusinFreeSpin = true;
                    }
                    break;
                }
            }

      
        return isBonusWin;
    }


    

    internal void ResetAllLines()
    {   
        for (int i = 0; i < SlotManager.instance.noOfLinesSelected; i++)
        {
            lineItemScripts[i].Reset();
        }
    }





  
   

    internal void ShowEabledLine(bool enable)
    {
        for (int i = 0; i < SlotManager.instance.noOfLinesSelected;i++ )

        {
            lineItemScripts[i].EnableLine(enable);
        }

       HideAllItemPayline();

        if (enable == false)
        {

            if (LineAnimationScript.instance.linewin)
                LineAnimationScript.instance.linewin.gameObject.SetActive(enable);
        }
    }

}// End Script..
