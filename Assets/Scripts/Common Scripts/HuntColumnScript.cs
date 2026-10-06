using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntColumnScript : MonoBehaviour
{
    public ArrayList huntItemsList;
    public SpinForhunt[] itemsinColumn;
    public float huntVerticalGap = 0.1f;
    public bool IsHuntColumnSpinning;
    public float[] initialYaxisArray = new float[5];
    public int ColumnIndex;
    public bool[] IndexesFilledCheckArray = new bool[5];
    public GameObject HuntedSlotPrefb;
    // Start is called before the first frame update
    void Awake()
    {
        
        huntVerticalGap = 1.2f;
        huntItemsList = new ArrayList();
        for (int i = 0; i < itemsinColumn.Length; i++) {
            huntItemsList.Add(itemsinColumn[i]);
            initialYaxisArray[i] = itemsinColumn[i].transform.position.y;
            IndexesFilledCheckArray[i] = false;
           // print("item " +  huntItemsList[i]);

        }
    }
    public IEnumerator CreateHuntedSlot(int huntedIndex) {
        yield return new WaitForSeconds(0.5f);
      
        GameObject HuntedClone = (GameObject)  Instantiate(HuntedSlotPrefb, ((SpinForhunt)huntItemsList[huntedIndex]).gameObject.transform.position,Quaternion.identity);
         Vector3 Pos = HuntedClone.transform.position;
        Pos.z = -5f;
        HuntedClone.transform.position = Pos;
        HuntedClone.transform.parent = huntSpecialSpinManager.instance.huntedclonesParent.transform;
        iTween.PunchScale(HuntedClone, new Vector3(1f, 1f, 1f), 1f);
        if (!SoundFxManager.instance.BonusItemAppear.isPlaying)
        {
            SoundFxManager.instance.BonusItemAppear.Play();
          //  huntSpecialSpinManager.instance.BonusBlowKiss();
        }

    }

    public bool CheckForFillIndex(int huntindex) {
       
        
        return IndexesFilledCheckArray[huntindex];
     }

    public void FillTheIndex(int HuntIndex) {
        IndexesFilledCheckArray[HuntIndex] = true;
    }
  

    public void startHuntSpin() {
        IsHuntColumnSpinning = true;
        SoundFxManager.instance.spinSound.Play();

        for (int i = 0; i < itemsinColumn.Length; i++) {
           
                ((SpinForhunt)huntItemsList[i]).IsSpin = true;
                ((SpinForhunt)huntItemsList[i]).HuntIndexInColumn = i;
            }
           
        

        Invoke("StopHuntColumnSpin", huntSpecialSpinManager.instance.HuntColumnSpinTime);
    }

    void StopHuntColumnSpin() {
        SoundFxManager.instance.spinSound.Stop();

        IsHuntColumnSpinning = false;
        if (ColumnIndex == 5)
            Invoke("CheckNextSpin", 2);
    }
    void CheckNextSpin() {
        huntSpecialSpinManager.instance.NextSpin();

    }
    public void MoveLastItemUp() {

        ((SpinForhunt)huntItemsList[itemsinColumn.Length - 1]).transform.position = ((SpinForhunt)huntItemsList[0]).transform.position + new Vector3(0, huntVerticalGap, 0);
        huntItemsList.Insert(0, (SpinForhunt)huntItemsList[itemsinColumn.Length - 1]);
        huntItemsList.Remove(itemsinColumn.Length);
        // reseting the indexes
        for (int i = 0; i < itemsinColumn.Length; i++)
        {
            
                ((SpinForhunt)huntItemsList[i]).HuntIndexInColumn = i;
            
        }
     
    }
    // Update is called once per frame
    void Update()
    {
       
        
    }
}
