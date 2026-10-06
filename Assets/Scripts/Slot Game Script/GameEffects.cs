using UnityEngine;
using System.Collections;
using UnityEngine.Video;
public class GameEffects : MonoBehaviour
{
    public static GameEffects instance;
    public GameObject coinImagePrefab;
    public GameObject freeSpinPopupPrefeb;
    public GameObject bonusPopupPrefeb;
    public GameObject bigWinPrefeb;  // rank 1
    public GameObject MegaWinPrefab;       // rank 2
    public GameObject SpecialWinPrefab;      // rank 3
    public GameObject SuperWinPrefab;     // rank 4
    public GameObject CleanSweepPrefab;   // rank 5
    public GameObject JackPotPrefab;   // rank 6
    public GameObject AwardedSpinsPrefab;
    [Header("Displays")]
    [Header("VideoPlayers")]

    [Header("Other")]

    public GameObject CoinShower;
    public GameObject FireWorksEmitter;
    public static bool onceBonus = false;
    public bool IsVideoEffect;
    public GameObject WinningsShow;
    public bool WinningsCeleberation =false;
    public bool CanEndCelebration = false;
    public GameObject FreeSpinsInfo;
    GameObject celebClone;
    public float freespinStartDelay = 2f;
    private int BigWinShowTime = 10;

    void Awake()
    {
        freespinStartDelay = 2f;
        IsVideoEffect = false;
        instance = this;



    }

    void Start()
    {
    }
    private void Update()
    {
       

    }


  public  void CreateFreeSpinInfo() {
        Instantiate(FreeSpinsInfo, FreeSpinsInfo.transform.position, Quaternion.identity);
        ColumnManager.instance.isStickyWild = false;
        Invoke("StartFreespins", freespinStartDelay);
    }

    void StartFreespins()
    {

        BgManager.instance.SetFreespinBg();

    }

        public void CreateAwardedSpinMessage()
    {
        Instantiate(AwardedSpinsPrefab, AwardedSpinsPrefab.transform.position, Quaternion.identity);
        BgManager.instance.SetFreespinBg();
        ColumnManager.instance.isStickyWild = false;
      //  GUIManager.instance.TurboMode();

    }
 

   
    void continueAutoSpin()
    {
        SlotManager.CanSpinAgain = true;
        SlotManager.instance.SpinAgain();
    }
 
 
  
   
 




    public void CelebrationEnds()
    {
     
            WinningsCeleberation = false;
            Invoke("CelebrationEndsLate", 0.5f);
      
    }
    public void BonusEnds(float BonusAmount) {

        SlotManager.instance.IsBonusEnabled = false;
        SlotManager.CanSpinAgain = true;
        GUIManager.instance.SetGuiButtonState(true);
        onceBonus = false;
        if (GUIManager.instance.SpinNumbers > 0 || GameOperations.instance.noOfFreeSpin >0)
        {
            this.Invoke("continueAutoSpin", 1f);


        }

    }

    void CelebrationEndsLate() {
        // SoundFxManager.instance.jackpotPopupSound.Play();
        if(!SlotManager.instance.IsBonusEnabled)
        GUIManager.instance.GenerateFlyingBalance(SlotManager.instance.currentSpinWinningAmount);

        if (!SlotManager.instance.IsFreeSpinsEnabled)
        BgManager.instance.SetNormlBg();

        SlotManager.instance.IsBonusEnabled = false;
    
       onceBonus = false;
       // celebClone = GameObject.FindGameObjectWithTag("Celebration");
       if(celebClone !=null)
        Destroy(celebClone);
        SoundFxManager.instance.WinBigSound.Stop();
        //GameObject coinClone = GameObject.FindGameObjectWithTag("Coins");
        //Destroy(coinClone);
        LineManager.instance.BigBigWins = false;
        if (GUIManager.instance.SpinNumbers > 0 && !SlotManager.instance.IsFreeSpinsEnabled)
        {
            this.Invoke("continueAutoSpin", 1f);
            print("auto spin continue");


        }
        else if (SlotManager.instance.IsFreeSpinsEnabled)
        {
            print("free spin continue");
            GameOperations.instance.NextFreeSpin();
        }
        else {
            SlotManager.CanSpinAgain = true;
            GUIManager.instance.SetGuiButtonState(true);
            print("no spin");
        }

    }
  

    internal void FreeSpinPopUpEffect()
    {
      
        SoundFxManager.instance.FreeSpinsPopUpSound.Play();
        GUIManager.instance.SetGuiButtonState(false);
        Invoke("ShowfreeSpinsPopup", 0.5f);

       
    }
    void ShowfreeSpinsPopup()
    {
        SoundFxManager.instance.StopBgSoundOnly();
    
        Instantiate(freeSpinPopupPrefeb, freeSpinPopupPrefeb.transform.position, Quaternion.identity);
        LineAnimationScript.instance.StopPaylineAnims();
       SlotManager.instance.ClearFreespinsPreviousSum();
        //if(!GUIManager.instance.TurboBool)
        //GUIManager.instance.TurboMode();

        SlotManager.CanSpinAgain = false;

    }


   

    internal void ShowBonusPopupEffect()
    {

        if (!onceBonus)
        {
          
            onceBonus = true;
            GUIManager.instance.SetGuiButtonState(false);
          //  ApiManager.instance.PostBonusEventStatus("bonus_saved");
            SoundFxManager.instance.BonusPopUpSound.Play();

            SoundFxManager.instance.StopBgSoundOnly();


            SlotManager.CanSpinAgain = false;
            Invoke("ShowbonusSprites", 0.2f);

       }
    }
    private void ShowbonusSprites() {

        Instantiate(bonusPopupPrefeb, bonusPopupPrefeb.transform.position, Quaternion.identity);
    }
 

 
   


 




    internal void AddToTatalCoin()
    {

    }


    



    // All Celebration Effects

    internal void BigWinShow()          // Rank 1
    {
        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);
     //   GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);
        SoundFxManager.instance.WinBigSound.Play();
        WinningsCeleberation = true;
        // CoinShower.SetActive(true);
        GUIManager.instance.SetGuiButtonState(false);
        Invoke("BigWinLate", 2);

      
    
    }
    void BigWinLate() {
        celebClone = (GameObject)Instantiate(bigWinPrefeb, bigWinPrefeb.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", BigWinShowTime);
    }


    internal void MegaWinShow()    //Rank 2
    {
        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);
        SoundFxManager.instance.WinBigSound.Play();

      //  GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);

        WinningsCeleberation = true;

        //  CoinShower.SetActive(true);
        GUIManager.instance.SetGuiButtonState(false);
        Invoke("MegaWinLate", 2);

    }
    void MegaWinLate() {
        celebClone = (GameObject)Instantiate(MegaWinPrefab, MegaWinPrefab.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", BigWinShowTime);
    }

    internal void SpecialWinShow()         // Rank 3
    {
        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);
        SoundFxManager.instance.WinBigSound.Play();

     //   GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);
             
        WinningsCeleberation = true;

        GUIManager.instance.SetGuiButtonState(false);
       

        Invoke("SpecialWinLate", 2);
  
    }
    void SpecialWinLate() {
        celebClone = (GameObject)Instantiate(SpecialWinPrefab, SpecialWinPrefab.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", BigWinShowTime);

    }


    internal void SuperWinShow()       // Rank 4

    {
        SoundFxManager.instance.WinBigSound.Play();

        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);

      //  GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);

        WinningsCeleberation = true;
        SlotManager.CanSpinAgain = false;
        GUIManager.instance.SetGuiButtonState(false);
      
        Invoke("SuperWinLate", 2);

    }
    void SuperWinLate() {
        celebClone = (GameObject)Instantiate(SuperWinPrefab, SuperWinPrefab.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", BigWinShowTime);

    }
    internal void CleanSweepShow() // Rank 5
    {
        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);
        SoundFxManager.instance.WinBigSound.Play();

        //  GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);

        WinningsCeleberation = true;
        GUIManager.instance.SetGuiButtonState(false);
        SlotManager.CanSpinAgain = false;
     
        Invoke("CleanSweepLate", 2);
      //  CoinClone.transform.SetParent(myclone.transform);

    }
    void CleanSweepLate() {
        celebClone = (GameObject)Instantiate(CleanSweepPrefab, CleanSweepPrefab.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", BigWinShowTime);

    }
    internal void JackPotShow()            // Rank 6
    {
        SoundFxManager.instance.WinBigSound.Play();


        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);

     //   GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);

 
        WinningsCeleberation = true;
        GUIManager.instance.SetGuiButtonState(false);

        // CoinClone.transform.SetParent(myclone.transform);
        Invoke("JackpotLate", 2);
    }
    void JackpotLate() {
        celebClone = (GameObject)Instantiate(JackPotPrefab, JackPotPrefab.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", BigWinShowTime);

    }
}
