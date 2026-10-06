using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class huntSpecialSpinManager : MonoBehaviour
{
    public Sprite[] slotItemsHunt;
    public float huntSpinSpeed =30f;
    public static huntSpecialSpinManager instance;
    public bool startSpinBool = false;
    public float HuntColumnSpinTime = 3f;
   public GameObject huntedclonesParent;
    public int NumberoFHuntSpins =0;
    public GameObject[] Columns;
    public int huntSpinsUsed =0;
    public TextMesh spinsleft, spinsUsed;
    public int HuntCounter;
    public TextMesh huntCounterTextM;
    public int RewardPerHunt = 50;
    public int HuntTotalWinnings;
    public GameObject StartInfo;
    public TextMeshPro bonusspinstext;
    public GameObject FinalBonusWinningsPanel;
    public TextMeshPro RewardPerItemText, CurrentTotal;
    public Animator BonusCharacter;
    // Start is called before the first frame update
    void Start()
    {
        NumberoFHuntSpins = LineItem.Instance.bonusSlotItemCount + 2;
        StartInfo.SetActive(true);
        GUIManager.instance.SetCanvasButtonsState(false);
        RewardPerHunt = (int) SlotManager.instance.totalBetAmount *2;
        RewardPerItemText.text = RewardPerHunt.ToString();
        CurrentTotal.text = "0";
        OnSlotItemClicked.CanShowSlotInfo = false;
        GUIManager.instance.SetGuiButtonState(false);
        instance = this;
        huntedclonesParent = new GameObject();
        huntedclonesParent.transform.name = "HuntedClonesParent";
        UpdateHuntGUI();
        spinsleft.text = NumberoFHuntSpins.ToString();
        spinsUsed.text = huntSpinsUsed.ToString();
        
        //  NumberoFHuntSpins = GameOperations.instance.huntSpinNumbers;
      

        bonusspinstext.text = NumberoFHuntSpins.ToString()+ " Bonus Spins";

    }
  
    public void UpdateHuntGUI() {
        huntCounterTextM.text = HuntCounter.ToString();
        CurrentTotal.text = (HuntCounter * RewardPerHunt).ToString();
    }
   public void  StartBonusPlay() {
        spinsleft.text = NumberoFHuntSpins.ToString();
        spinsUsed.text = huntSpinsUsed.ToString();
        StartInfo.SetActive(false);
        SoundFxManager.instance.PlayBonusBg();
        StartCoroutine(ManageHuntSpins());

    }

    public IEnumerator ManageHuntSpins()
    {
        yield return new WaitForSeconds(1);
        NumberoFHuntSpins--;
        huntSpinsUsed++;

        spinsleft.text = NumberoFHuntSpins.ToString();
        spinsUsed.text = huntSpinsUsed.ToString();
    
        for (int i = 0; i < Columns.Length; i++)
        {
            Columns[i].GetComponent<HuntColumnScript>().startHuntSpin();

        }
      
            spinsleft.text = NumberoFHuntSpins.ToString();
            spinsUsed.text = huntSpinsUsed.ToString();

    
  
    

    }

   
    public void NextSpin() {
        if (NumberoFHuntSpins > 0)
            StartCoroutine(ManageHuntSpins());
        else
        {
            spinsleft.text = NumberoFHuntSpins.ToString();
            spinsUsed.text = huntSpinsUsed.ToString();
            StopCoroutine(ManageHuntSpins());
            CalculateHuntResults();
        }
       
    }
    void CalculateHuntResults() {
        HuntTotalWinnings = HuntCounter * RewardPerHunt;
        SlotManager.instance.currentSpinWinningAmount = (float)HuntTotalWinnings;
        ShowFinalWinningsPanel();
    }
    void ShowFinalWinningsPanel() {

        FinalBonusWinningsPanel.SetActive(true);
        GUIManager.instance.AddBonusWinToTotalCredit(HuntTotalWinnings);
    }

    public void collectBonusWinnings() {
       
        GUIManager.instance.UpdateGUI();
        GameEffects.instance.CelebrationEnds();
        OnSlotItemClicked.CanShowSlotInfo = true;
        GUIManager.instance.SetCanvasButtonsState(true);


        Destroy(gameObject.transform.root.gameObject,0.05f);

    }
    public void DestroyHuntedClones() {
        Destroy(huntedclonesParent);
     

    }
    private void OnDisable()
    {
        DestroyHuntedClones();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
