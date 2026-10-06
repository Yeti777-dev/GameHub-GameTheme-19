using UnityEngine;
using TMPro;

public class LineItem : MonoBehaviour
{
    public static LineItem Instance;
    public int[] rowIndex;
    public   Vector2[] lineCoordinates;
    public SlotItem[] lineSlotItems;
    public int[] lineitemIndexes = new int[5];
    public GameObject lineGfx;
    int firstItemIndex = -1;
    int firstIndexWild = 0;
    internal int count = 0;
    internal int bonusSlotItemCount;
    public int WildInSequanceCount;
    internal GameObject normalItemBoxContainer, wildItemBoxContainer, bonusItemContainer;
    private int animationSelection;
    private GameObject wildItemIndication;
    private bool showEffect;
    internal int winingMultiplyer;
    internal float totalWin;
    internal int lineNumberIndex = 0;
    public GameObject[] matchedslots = new GameObject[5];
    public GameObject InfoObj;
    public SlotItem wildItem;
    private int Wild_in_Winline = 0;
    public int TotalItemsMatchedOnline = 0;


    void Awake()
    {
        Instance = this;
        WildInSequanceCount = 0;
    }


    void Start()
    {
        lineCoordinates = new Vector2[5];

        for (int i = 0; i < rowIndex.Length; i++)
        {
            lineCoordinates[i] = new Vector2(i, rowIndex[i]);
        }

        lineSlotItems = new SlotItem[5];
       // if(LineManager.instance.PayColumnItems.Length>0)
       // ShowItemPayline();
    }

    public void ShowItemPayline() {

        for (int i = 0; i < rowIndex.Length; i++)
        {
            for (int k = 0; k < LineManager.instance.PayColumnItems[i].GetComponent<ColumnPaylineItems>().columnItems.Length; k++)
            {
                if (k == rowIndex[i])
                    LineManager.instance.PayColumnItems[i].GetComponent<ColumnPaylineItems>().columnItems[k].SetActive(true);
                else
                    LineManager.instance.PayColumnItems[i].GetComponent<ColumnPaylineItems>().columnItems[k].SetActive(false);

            }
        }

      //  Invoke("HideItemPayline", 2);

    }

    public  void HideItemPayline()
    {
        if(LineManager.instance.PayColumnItems.Length>0)
        for (int i = 0; i < rowIndex.Length; i++)
        {
            for (int k = 0; k < LineManager.instance.PayColumnItems[i].GetComponent<ColumnPaylineItems>().columnItems.Length; k++)
            {
                if (k == rowIndex[i])
                    LineManager.instance.PayColumnItems[i].GetComponent<ColumnPaylineItems>().columnItems[k].SetActive(false);
            }
        }
    }



    internal void Reset()
    {
      
        winingMultiplyer = 0;
        totalWin = 0;
        StopAllCoroutines();
        StopSlotitemAnimation();
        showEffect = false;
        count = 0;
        if(lineGfx)
        lineGfx.transform.localPosition = new Vector3(lineGfx.transform.localPosition.x, lineGfx.transform.localPosition.y, 0);
        bonusSlotItemCount = 0;
        WildInSequanceCount = 0;
        GameOperations.instance.ScatterSlotItemCount = 0;
        GameEffects.onceBonus = false;
        GameOperations.instance.DestroyIndication();
        if (InfoObj != null) {
            Destroy(InfoObj);
            OnSlotItemClicked.ItemClicked = false;

        }
        for (int i = 0; i < 5; i++)
        {
            matchedslots[i] = null;
        }
    }


    internal void SetCurrentLineItems()
    {
        for (int i = 0; i < 5; i++)
        {
            lineSlotItems[i] = ColumnManager.instance.GetSlotItemAt((int)lineCoordinates[i].x, (int)lineCoordinates[i].y);
        }
    }



    internal void TraceForCombinations()
    {
        TraceForWildOnly();
        TraceForNormalCombinations();
        CalculateReward();
     
    }

   


    internal void TraceForNormalCombinations()
    {
        Wild_in_Winline = 0;
        firstItemIndex = -1;
        count = 0;
        for (int i = 0; i < 5; i++)
        {
            lineitemIndexes[i] = lineSlotItems[i].animationIndex;
                if (lineSlotItems[i].itemType == SlotItemType.Wild)
                {
                wildItem = lineSlotItems[i];
                Wild_in_Winline++;
                count++;
                    continue;
                }
                if (firstItemIndex == -1 && lineSlotItems[i].itemType == SlotItemType.Normal)
                {
                    
                    count++;
                    firstItemIndex = lineSlotItems[i].animationIndex;
                }
                else if (lineSlotItems[i].animationIndex == firstItemIndex)
                {
                    count++;
                }
                else
                    break;

               
            
        }
        TotalItemsMatchedOnline = count;
    }


    void CalculateReward()
    {

        winingMultiplyer = PayTable.instance.CkeckInPayTable(firstItemIndex, count);


        totalWin = winingMultiplyer * SlotManager.instance.betPerLineAmount;
        animationSelection = Random.Range(0, 3);

        if (GameOperations.instance.WildAsMultiplier && totalWin > 0)
        {
            if (SlotManager.instance.spinType == "paid")
            {

                if (Wild_in_Winline > 0)
                {
                    int wildMultiplier = 0;
                    PayTable.instance.SetWildMultiplier(Wild_in_Winline, PayTable.instance.WildInSequanceAmount);
                    wildMultiplier = PayTable.instance.WiningMultiPlyerOnALine();
                    totalWin = totalWin * wildMultiplier;
                    if (wildItem)
                    {
                        wildItem.transform.GetChild(0).GetComponent<TextMeshPro>().text = wildMultiplier.ToString() + "X";
                        wildItem.transform.GetChild(0).GetComponent<TextMeshPro>().gameObject.SetActive(true);
                    }
                    animationSelection = 6;

                }

            }
            else
            {
                if (GameOperations.instance.noOfFreeSpin > 0)
                {
                    float linewin = totalWin;
                    totalWin = linewin * GameOperations.instance.multiplierfs;
                    GameOperations.instance.toplinetext.text = "" + linewin.ToString() + " x " + GameOperations.instance.multiplierfs.ToString() + " = " + totalWin.ToString();

                }



            }
        }



    }

    void CalculateRewardForWild()
    {
       
    }

    /// Trace For Bonus Item
    internal void TraceForBonus()
    {

        bonusSlotItemCount = 0;
        for (int i = 0; i < LineManager.instance.Items_In_Column; i++)
        {
            for (int j = 0; j < 5; j++)
            {
              SlotItem  Item = (SlotItem)ColumnManager.instance.GetSlotItemAt(j, i);
                if (Item.itemType == SlotItemType.Bonus)
                {
                    bonusSlotItemCount++;

                }
            }
        }
        if (bonusSlotItemCount >= SlotManager.instance.BonusTriggerCounter && !SlotManager.instance.IsFreeSpinsEnabled)
        {

            if (GameOperations.instance.GameHasBonus)
            {
                GUIManager.instance.SetGuiButtonState(false);
                SlotManager.CanSpinAgain = false;
                SlotManager.instance.IsBonusEnabled = true;
                OnSlotItemClicked.CanShowSlotInfo = false;
                GameEffects.instance.ShowBonusPopupEffect();
            }
            else {
                SlotManager.CanSpinAgain = false;
                GameOperations.instance.RewardBonusSpins(GameOperations.instance.FixedBonusSpin);
            
            }

          
        }
    }

    // Trace For Wild Item..
    internal void TraceForWildOnly()
    {
        for (int i = 0; i < 5; i++)
        {
            if (lineSlotItems[i].itemType == SlotItemType.Wild)
            {
                WildInSequanceCount++;
         
            }
            else
            {
                break;
            }
        }
    }

    internal int NoOfItemMatched()
    {
        if (count == 0)
            return WildInSequanceCount;
        else
        {
            return count;
        }
    }
  public  void showmatchedSlots()
    {
        for (int i = 0; i < 5; i++)
        {
            if (matchedslots[i] != null)
                matchedslots[i].GetComponent<MeshRenderer>().enabled = true;
         
        }
    }
   public void HideMatchedSlots()
    {
        for (int i = 0; i < 5; i++)
        {
            if (matchedslots[i] != null)
              matchedslots[i].GetComponent<MeshRenderer>().enabled = false;
         
        }
    }
    // Show Slot Item Animation
    internal void ShowSlotItemAnimation()
    {
        for (int i = 0; i < NoOfItemMatched(); i++) {
            if (!lineSlotItems[i].ParentColumn.IsColumnWild)
            {
                Vector3 newtrans = lineSlotItems[i].transform.position;
                newtrans.z = -3f;
                lineSlotItems[i].transform.position = newtrans;
            }
          //  iTween.PunchScale(lineSlotItems[i].gameObject, new Vector3(0.3f, 0.3f, 0.3f), 2);
    }
        if(LineAnimationScript.instance.HaveAnimations)
        HideMatchedSlots();

        //for (int i = 0; i < NoOfItemMatched(); i++)
        //{
        //    lineSlotItems[i].myEffectScript.Highlight(animationSelection);
        //}
    }

    internal void ShowBonusItemAnimation()
    {

        for (int i = 0; i < 5; i++)
        {
            if (lineSlotItems[i].itemType == SlotItemType.Bonus)
                lineSlotItems[i].myEffectScript.Highlight(animationSelection);
        }
    }

    internal void StopSlotitemAnimation()
    {

        for (int i = 0; i < NoOfItemMatched(); i++)
        {
            Vector3 newtrans = lineSlotItems[i].transform.position;
            newtrans.z = 1f;
            lineSlotItems[i].transform.position = newtrans;
           

        }
        if (LineAnimationScript.instance.HaveAnimations)
                 showmatchedSlots();
        //for (int i = 0; i < NoOfItemMatched(); i++)
        //{

        //    lineSlotItems[i].myEffectScript.UnHighlight();
        //}
    }

    internal void EnableLine(bool enable)
    {
        if(lineGfx)
        lineGfx.SetActive(enable);

        if (wildItem && GameOperations.instance.WildAsMultiplier)
        {
            wildItem.transform.GetChild(0).GetComponent<TextMeshPro>().gameObject.SetActive(enable);


        }
    }

}// End Script
