using UnityEngine;
using System.Collections;
//using System;
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;
using System;

public class SlotManager : MonoBehaviour
{
    /// SlotManager Script Instance ..
    internal static SlotManager instance;
    public TextMeshPro TranSactionID;
    /// Slot Item Prefeb GameObject
    /// 
    public GameObject themeSlotItemPrefab;
    public GameObject themePayTablePrefab;

    public float probality;
    public GameObject spinButton;
    public GameObject BonusMenuPrefab;
  
    public GameObject WinningFly;

    internal GameObject slotItemPrefab;
    /// Item Spining Speed.
    public float spinSpeed = 18;
    /// Vertical Gap B/W Slot Item..
    public float verticalGap = 0.5f;
    /// Item Matched In Line Info
    private int countElement;
    /// Slot Machine Rolling Time..
    public float timeOfSpin = 4;
    internal bool bonusinFreeSpin;
    /// Slot Is Spining Or Not..
    internal bool isSpinning = false;

    public ObscuredInt noOfLinesSelected = 25;
    internal ObscuredFloat betPerLineAmount = 0.01f;
    public ObscuredFloat totalBetAmount;
    public ObscuredFloat currentSpinWinningAmount;

    [Header("Min Limits")]
    private float minForGambling;
    private float minForWheel;
    private float minForTeasure;
    public static bool CanSpinAgain;
    internal ObscuredFloat totalNoOfLines = 25;
    public GameObject GuiCam;
    public ObscuredFloat FreeSpinsWinSum;
    public ObscuredFloat TotalFreeSpins;
    public bool spinagainability;
    public TextMesh FreespinsTotalSumMesh;
    public GameObject WinShowPanel;
    public GameObject FadePanel;
    public bool IsFreeSpinsEnabled, IsBonusEnabled;

    public int playerSpins;
   internal string spinType = "paid";

    public int GameSessionFreeSpins = 0;

    public float game_nakha;
    public float pakar_nakha;
    internal bool nakha_shta = true;
    public float winnings_session;
    public float totalgamebets;
    public string Pay_data;
    public bool GameDetailReceived = false;

    private int wincounter = 0;
    public int BonusTriggerCounter = 3;

    public string GamePlayOutPut;
    void Awake()
    {
       
        instance = this;
        spinSpeed = 20;
        Game.noOfCoinsLeft = 1000;
        playerSpins = 0;
        CanSpinAgain = true;
        timeOfSpin = 1;

        slotItemPrefab = themeSlotItemPrefab;
        
        // apicomment
          pakar_nakha = ApiManager.instance.TargetRTP;
          Pay_data = ApiManager.instance.PayoutString;
         //Pay_data=  "60,50,40/50,40,30/50,30,25/40,30,20/30,20,15/25,15,10/20,12,8/15,10,5/15,10,5/15,10,5/6,4,2/2,3,5,10,50$4,5,6,7,8,9,16,18,19,3,3,4";


    }






    private void Start()
    {
        // apicomment
        winnings_session = (float)ApiManager.instance.TotalWinings;
        totalgamebets = (float)ApiManager.instance.TotalbetPlaced;
        Game.noOfCoinsLeft = (float)ApiManager.instance.Balance;
      
        


        game_nakha = (winnings_session / totalgamebets) * 100;

     
        GUIManager.instance.UpdateGUI();


        Game.Instance.Invoke("LoadPlayerData", 1);
    }
    
    private void Update()
    {
        spinagainability = CanSpinAgain;
       
    }

  
   
    internal void SpinButtonClicked()
    {
        if (WinShowPanel != null)
        {
            Destroy(WinShowPanel);
        }
        if (FadePanel != null)
        {
            Destroy(FadePanel);
        }
        GameSessionFreeSpins = 0;
       
        //if (UltimateBonusRecorder.Instance)
        //{

        //    UltimateBonusRecorder.Instance.ClearBonusRecords();
        //}
        GameOperations.instance.isFixedSpins = false;

        if (ColumnManager.instance.wildclones.Count != 0 && !ColumnManager.instance.isStickyWild)
        {
            for (int i = 0; i < ColumnManager.instance.wildclones.Count; i++)
            {

                Destroy(ColumnManager.instance.wildclones[i]);

            }
            ColumnManager.instance.wildclones.Clear();

        }

        if (isSpinning)
            return;


        if (IsFreeSpinsEnabled)
            spinType = "bonus";
        else
            spinType = "paid";


        SoundFxManager.instance.spinSound.Play();

        ResetAll();

        GUIManager.instance.ShowPerLineWinAmount("Good Luck !!");

        isSpinning = true;
        Game.currentGameState = GameState.isSpining;
        OnSlotItemClicked.CanShowSlotInfo = false;
        if (!IsFreeSpinsEnabled)
        {
           
            Game.noOfCoinsLeft -= totalBetAmount;

           
            // send spin info

            playerSpins++;
            totalgamebets += totalBetAmount;
        }
        GUIManager.instance.totalCoinText.text = Game.noOfCoinsLeft.ToString("F2");
        ResetAllAmount();

        

        StartCoroutine(ColumnManager.instance.StartSpinningColumn());
        GUIManager.instance.StopSpinButton.SetActive(true);

               // if player plays with a free spin


    }

   


    /// Call Back When Spining Complete..
    internal void OnSpinComplete()
    {

     

            Invoke("_StopSpinSound",0.3f);
            isSpinning = false;
            Invoke("_SpinCompleted", 1f);
      
    }

 

  

 

    public void MybonusReward() {
        CanSpinAgain = false;
        StartCoroutine(ShowBonusMenu());
        GUIManager.instance.SetGuiButtonState(false);
    }
    

    
    public IEnumerator ShowBonusMenu()
    {
        Instantiate(BonusMenuPrefab, BonusMenuPrefab.transform.position, Quaternion.identity);

       

        yield return null;
    }

   

    void _StopSpinSound()
    {
        SoundFxManager.instance.spinSound.Stop();
    }

    void _SpinCompleted()
    {
        LineManager.instance.TransforBonusCombination();
        //  AutoMoveAndRotate.instance.isRotate = false;
        if (!SlotManager.instance.IsBonusEnabled)
        {


            GameOperations.instance.TraceForScatterItem();


        }




        // trace for free spins only if Bonus round is not applicable


        ShowWinAfterRequest();

       

     

    }

    public void checkNextSpinAfterWin() {
        if (GUIManager.instance.SpinNumbers > 0 && CanSpinAgain && !SlotManager.instance.IsBonusEnabled && !LineManager.instance.BigBigWins && !GameOperations.instance.FreeSpinBoolDelay)
            SpinAgain();


    }

    public void SpinAgain() {
        if (!IsFreeSpinsEnabled && CanSpinAgain) {
            GUIManager.instance.SpinNumbers--;
           
            GUIManager.instance.AutoSpinsCounting();
            CanSpinAgain = false;
            GUIManager.instance.OnSpinButtonClicked();
            CancelInvoke("SpinAgain");
         }
        
    }

    

 
   

   public void ShowWinAfterRequest() {

        LineAnimationScript.instance.ShowLineAnimations();

        CanSpinAgain = true;


        if (!IsFreeSpinsEnabled)
        {
            LineManager.instance.CheckForSpecialEffect();
        }


        Game.currentGameState = GameState.isSpiningComplete;
        //  currentSpinWinningAmount = 3000;
        if (currentSpinWinningAmount > 0 && !LineManager.instance.BigBigWins)
        {
            //  Vector3 pos = WinningFly.transform.position;
            // pos.z = 0;

            if (!SlotManager.instance.IsBonusEnabled)
                Instantiate(WinningFly, WinningFly.transform.position, Quaternion.identity);
            
            // GameEffects.instance.BigWinShow();

        }
        SlotItem.onceCheck = true;


        if (GUIManager.instance.SpinNumbers > 0 && CanSpinAgain && !SlotManager.instance.IsBonusEnabled && !LineManager.instance.BigBigWins && !GameOperations.instance.FreeSpinBoolDelay)
        {
            if (currentSpinWinningAmount == 0)
                this.Invoke("SpinAgain", 1f);
        }
        else if (GUIManager.instance.SpinNumbers == 0)
        {
            GUIManager.instance.StopAutoSpin();
            OnSlotItemClicked.CanShowSlotInfo = true;
            //   GUIManager.instance.SetGuiButtonState(true);

        }

        if (currentSpinWinningAmount > 0)
            wincounter++;

        if (currentSpinWinningAmount > (totalBetAmount * 30))
        {
            int randomX = UnityEngine.Random.Range(25, 31);
            currentSpinWinningAmount = randomX * totalBetAmount;


        }
        //  print("Win Hit counter is " + wincounter.ToString());
        

       

        winnings_session += currentSpinWinningAmount;
        GUIManager.instance.UpdateWiningAmount();


        GUIManager.instance.AddWinningsToTotalCredit(currentSpinWinningAmount);
        PlayerPrefs.SetFloat("LastWinAmm", currentSpinWinningAmount);
        PlayerPrefs.Save();
        GUIManager.instance.UpdateGUI();

        if (currentSpinWinningAmount == 0 && !IsFreeSpinsEnabled && !IsBonusEnabled)
        {
            GUIManager.instance.SetGuiButtonState(Game.currentGameState != GameState.isSpining);
            GUIManager.instance.StopSpinButton.SetActive(false);

        }
        string SpinResult = "Null";
        float resultamount = 0;
        int SpinBet = 0;


        if (spinType == "paid")
        {

            resultamount = currentSpinWinningAmount;
            SpinResult = "won";




            SpinBet = (int)betPerLineAmount;
        }
        else
        {
            SpinResult = "won";
            resultamount = currentSpinWinningAmount;
            SpinBet = 0;
        }
        // print("Session win is " + winnings_session.ToString());

        // print("Total GameBets is " + totalgamebets);

        if (totalgamebets != 0)
            game_nakha = ((winnings_session / totalgamebets) * 100);
        else
            game_nakha = winnings_session / 2f;
    }

    public void SendServerBonusWin(float BonusWin) {



      


    }

   

    internal void ResetAll()
    {
        LineManager.instance.ResetAllLines();
        LineAnimationScript.instance.StopAllAnimationAndEffect();
    }

    internal void ResetAllAmount()
    {
        countElement = 0;
        currentSpinWinningAmount = 0;
       // if (GameOperations.instance.noOfFreeSpin == 0)
       
     //  GUIManager.instance.UpdateGUI();
        GUIManager.instance.UpdateWiningAmount();
        
    }

  

    public void UpdateFreespinsTotal() {

        FreespinsTotalSumMesh.text = FreeSpinsWinSum.ToString("F2");
       // ScrollTextScript.Scroll(FreespinsTotalSumMesh, float.Parse(FreespinsTotalSumMesh.text.ToString()), FreeSpinsWinSum, 2f, 0f);
    }
    public void ClearFreespinsPreviousSum() {
        FreespinsTotalSumMesh.text = "0.00";
    }


  
} // End Script..
