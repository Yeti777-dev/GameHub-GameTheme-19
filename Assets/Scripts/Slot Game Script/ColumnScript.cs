using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ColumnScript : MonoBehaviour
{
    /// Coloum Script Instance..
    public static ColumnScript instance;
    /// Set No Of Item in Each Coloum..
    private int noOfItemsInAColumn = 12;
    /// Colom Index..
    public int columnIndex = 0;
    // All SlotItem According to Coloum..
    public List<SlotItem> slotItemList;
    /// slot Is spining 
    internal bool isSpinning = false;
    /// Slot Item Spin Using Itwwen When Spining Stop to Reposition slot Item..
    public bool shouldStopSpinning = false;
    /// Slot Item Current Anim Idex..
    internal int slotItemAnim;
    internal int rareItemChance;
    //50-50 chance for the lucky item to be shown
    public int luckyItemPercent ;
    public float heightadjust;
    public GameObject fireWall;
    public float TubroSpinTime;
    public bool IsColumnWild = false;
    public GameObject WildExpandedPrefab;
    public float LongSpinTime = 1f;
    private bool LongSpinBool = false;
    public List<int> column0indexes = new List<int>();
    void Awake()
    {
        instance = this;

    }

    public SlotItem[] slotItems;

    void UpdateSlotItemList()
    {
        slotItems = new SlotItem[slotItemList.Count];
        for (int i = 0; i < slotItemList.Count; i++)
        {
            slotItems[i] = (SlotItem)slotItemList[i];
        }

    }

    void Start()
    {
        slotItemList.Clear();
        PopulateColumn();

        InvokeRepeating("UpdateSlotItemList", 2, 2);
    }

    /// Populate all Column With Slot Item..And Add Slot Item In ArrayList..
    internal void PopulateColumn()
    {
        for (int i = 0; i < noOfItemsInAColumn; i++)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y - i * SlotManager.instance.verticalGap -0.2f, 2);

            GameObject temp = (GameObject)Instantiate(SlotManager.instance.slotItemPrefab, pos, Quaternion.identity);
            temp.transform.parent = transform;
            temp.GetComponent<SlotItem>().indexInColumn = i;
            slotItemList.Add(temp.GetComponent<SlotItem>());
        }
        rareItemChance = Random.Range(0, slotItemList.Count);
    }

  

    /// Start Slot Item Spining ..
    internal void StartSpin()
    {
        if (!SlotManager.instance.IsFreeSpinsEnabled || !ColumnManager.instance.isStickyWild)
            IsColumnWild = false;
       
      
        shouldStopSpinning = false;

        isSpinning = true;
        for (int i = 0; i < slotItemList.Count; i++)
        {
            ((SlotItem)slotItemList[i]).spin = true;
            // ((SlotItem)slotItemList[i]).setSprite();

        }
        /// Call Stop Spining..


        float spintime = SlotManager.instance.timeOfSpin + ((float)columnIndex /1.5f);
        float turbotime = spintime/1.5f;
        if (!GUIManager.instance.TurboBool)
            Invoke("StopSpin", spintime);
        else
            Invoke("StopSpin", turbotime);

        if (IsColumnWild) {
            StopSpin();
        }


    }



    // Spining Stop Iween Start to Reset Position Of slot Item....
    void StopSpin()
    {
        shouldStopSpinning = true;
    }

  



    /// Move Last item Of Coloum To First Index..
    internal void MoveLastItem()
    {

        ((SlotItem)slotItemList[noOfItemsInAColumn - 1]).transform.position = ((SlotItem)slotItemList[0]).transform.position + new Vector3(0, SlotManager.instance.verticalGap, 0);
      //  slotItemList.Insert(0, (SlotItem)slotItemList[noOfItemsInAColumn - 1]);
    //    slotItemList.RemoveAt(noOfItemsInAColumn);

        /// Reset Slot Item Position..
        //for (int i = 0; i < slotItemList.Count; i++)
        //{

        //    ((SlotItem)slotItemList[i]).indexInColumn = i;
        //}
     
         
       

        if (shouldStopSpinning)
        {
            if (LongSpinBool)
                return;
            if (ColumnManager.instance.LongSpin && !ColumnManager.instance.ShouldStopAtOnce)
            {
                
                LongSpinBool = true;
                fireWall.SetActive(true);
                if (!SoundFxManager.instance.ReelBoomSound.isPlaying)
                    SoundFxManager.instance.ReelBoomSound.Play();
                Invoke("StopColumnSpin", LongSpinTime + (columnIndex - 2));
            }
            else {

                StopColumnSpin();
            }
        }
    }

    void StopColumnSpin() {
        if(isSpinning)
        ColumnManager.instance.ColumnsStopped--;
        isSpinning = false;
        
        fireWall.SetActive(false);
        LongSpinBool = false;

        if (!IsColumnWild)
          Invoke("PlayDelayedStop", 0.01f);
        if (ColumnManager.instance.ColumnsStopped ==0)
        {
            for (int i = 0; i < ColumnManager.instance.columnScripts.Length; i++)
            {
                if (ColumnManager.instance.columnScripts[i].isSpinning)
                ColumnManager.instance.columnScripts[i].isSpinning = false;
            }
            SoundFxManager.instance.ReelBoomSound.Stop();
            SlotManager.instance.OnSpinComplete();
            ColumnManager.instance.LongSpin = false;
           
        }
    }
    void PlayDelayedStop()
    {
       // print("stop sound");
        SoundFxManager.instance.columnSpinCompleteSound.Play();

    }
}// End Script..
