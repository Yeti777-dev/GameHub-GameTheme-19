using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;

    public Camera guiCamera;

    public TextMesh enabledLineText;
    public TextMesh betAmountText;
    public TextMesh totalBetAmountText;
    public TextMesh winingAmountText;
    public TextMesh totalCoinText;
    public TextMesh perLineWinText;

    public GameObject specialPackPrefeb;
    public GameObject payTablePopupPrefab;
    public GameObject storeScreenPopupPrefab;

    public GameObject[] AllGuiButton;
    public Button[] CanvasButtons;


    public float[] betAmmountArray = {0.01f,0.02f,0.04f,0.06f,0.08f,0.1f,0.15f,0.2f,0.3f,0.5f,0.8f,1f,1.5f,2f};
    private int betAmmountIndex = 6;
    public int SpinNumbers;
    public TextMeshPro AutoSpinText;
    public GameObject autostartButton, autostopbutton,SpinButton, StopSpinButton;
    public GameObject SpinEffects;
    public GameObject AutoSpinPanel;
    public bool SpinEnable;
    public GameObject AutoSpinInfo;
    public GameObject TurboButton;
    public bool TurboButtonStatus = false;
    public bool TurboBool = false;
    public Texture2D TurboOff,TurboON;
    private float currenTotalBet;
    private bool profileshow = true;
    public Transform profileShowT, ProfileHideT;
    public Animator TurboAnimator;
    private bool paylineShowing = false;
    internal bool Isauto = false;
    public GameObject AddtoCreditTextprefab;
    public float ratio;
    public GameObject LobbyPanel;
    Transform betminus, betplus;

    void Awake()
    {
        instance = this;

        SetGuiButtonState(false);
        AutoSpinInfo.SetActive(false);
        SpinEnable = true;
        AutoSpinPanel.SetActive(false);
        SpinNumbers = 0;
      
        LobbyPanel.SetActive(false);

        betminus = AllGuiButton[0].transform;
        betplus = AllGuiButton[1].transform;

        


        AllGuiButton[2].SetActive(false);
        AllGuiButton[3].SetActive(false);

    

        betAmmountIndex = 0;
    }



    void Start()
    {

        autostopbutton.SetActive(false);
        StopSpinButton.SetActive(false);

      //  totalCoinText.text = Game.noOfCoinsLeft.ToString("F2");
        betAmountText.text = betAmmountArray[betAmmountIndex].ToString("F2");
        winingAmountText.text = "0";
     

        enabledLineText.text = SlotManager.instance.noOfLinesSelected.ToString();
        ChangeLineInfoToTotalBet();
        CalculateTotalBetAmount();
       

            if (PlayerPrefs.GetInt("Sound", 1) == 1)
                AudioListener.volume = 1;
            else
                AudioListener.volume = 0;

        
       

    }

    public void Startbet() {
        betAmmountIndex = 0;

        SlotManager.instance.betPerLineAmount = betAmmountArray[betAmmountIndex];

        betAmountText.text = SlotManager.instance.betPerLineAmount.ToString("F2");

        SlotManager.instance.totalBetAmount = (float)(SlotManager.instance.betPerLineAmount * SlotManager.instance.noOfLinesSelected);
        totalBetAmountText.text = SlotManager.instance.totalBetAmount.ToString("F2");


    }

    internal void ChangeLineInfoToTotalBet()
    {
        perLineWinText.text = "Playing " + SlotManager.instance.noOfLinesSelected.ToString() + " Lines " + "( Total Bet = " + (SlotManager.instance.noOfLinesSelected * SlotManager.instance.betPerLineAmount).ToString("F2") + " )";
    }

    public void OnLobbyButton()
    {
        ApiManager.instance.Home(Game.noOfCoinsLeft);
     

    }
    public void LobbyYes() {

        ApiManager.instance.Home(Game.noOfCoinsLeft);
    }
    public void LobbyNo()
    {
        SetGuiButtonState(true);
        LobbyPanel.SetActive(false);

    }

    public void OnRechargeButton()
    {
        LobbyYes();

    }
    public void OnTurboButtonClicked()
    {
        TurboButtonStatus = !TurboButtonStatus;
        if (!TurboButtonStatus)
        {

            SoundFxManager.instance.TurboOff.Play();


        }
        else
        {
            SoundFxManager.instance.TurboOn.Play();

        }
        TurboMode();
    }

    public void TurboMode() {
        TurboBool = !TurboBool;

        if (!TurboBool)
        {

     
            SlotManager.instance.timeOfSpin = 1f;
            TurboButton.GetComponent<MeshRenderer>().material.mainTexture = TurboOff;
        }
        else
        {
            SlotManager.instance.timeOfSpin = 0.5f;
            TurboButton.GetComponent<MeshRenderer>().material.mainTexture = TurboON;
        }

    }

    public void GetFreeCoins() {
    
    }
 
    
   
    void Update()
    {



        if (SpinEnable && !SlotManager.instance.IsFreeSpinsEnabled)
        {
            SpinButton.GetComponent<MyButtonScript>().ClearTheImage();



        }
        else
        {

            SpinButton.GetComponent<MyButtonScript>().blurTheImage();


        }

        if (SpinNumbers < 0)
            SpinNumbers = 0;
        if (SpinNumbers > 0) {
            SpinButton.GetComponent<MyButtonScript>().blurTheImage();

        }

        if (Game.noOfCoinsLeft < 0)
            Game.noOfCoinsLeft = 0;

       
       
    }

      public void OnSpinButtonClicked()
    {
        if (Game.currentGameState == GameState.isSpining || LineManager.instance.BigBigWins)
            return;
        if (SlotManager.instance.totalBetAmount <= Game.noOfCoinsLeft)
        {

            DisableAllLineOnSpinStart();
            SetGuiButtonState(false);
            SpinButton.GetComponent<MyButtonScript>().blurTheImage();
            SlotManager.instance.SpinButtonClicked();
            Game.currentGameState = GameState.isReady;
            LineManager.instance.BigBigWins = false;
            AutoSpinPanel.GetComponent<Animator>().SetTrigger("AutoDown");

        }
        else
        {

            if (specialPackPrefeb)
            {
              //  Instantiate(specialPackPrefeb, specialPackPrefeb.transform.position, Quaternion.identity);
                ApiManager.instance.StartCoroutine(ApiManager.instance.GetUserBalance());
            }
            StopSpinButton.SetActive(false);
        }

    }

    public void GenerateFlyingBalance(float Amount) {
      
    }

    public void AutoSpinButtonPressed() {
        AutoSpinPanel.SetActive(true);
        SoundFxManager.instance.buttonTapSound.Play();
        if (AutoSpinPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Idle") || AutoSpinPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Down"))
        {
            SoundFxManager.instance.AutoSpinUp.Play();
            AutoSpinPanel.GetComponent<Animator>().SetTrigger("AutoUp");
        }
        else
        {
            SoundFxManager.instance.AutoSpinDown.Play();

            AutoSpinPanel.GetComponent<Animator>().SetTrigger("AutoDown");
        }
    }
    public int totalAutoSpins;

    public void AutoSpinsCounting() {

        AutoSpinInfo.transform.GetChild(0).GetComponent<TextMeshPro>().text = (SpinNumbers).ToString(); // auto left
        AutoSpinInfo.transform.GetChild(1).GetComponent<TextMeshPro>().text = (totalAutoSpins-SpinNumbers).ToString(); // auto used
    }

    public void OnAutoSpinClicked()
    {

        if (SlotManager.instance.totalBetAmount <= Game.noOfCoinsLeft)
        {
            SoundFxManager.instance.AutoSpinDown.Play();

            AutoSpinPanel.GetComponent<Animator>().SetTrigger("AutoDown");
            AutoSpinInfo.SetActive(true);


            SpinNumbers--;
            SpinButton.GetComponent<MyButtonScript>().blurTheImage();
            AutoSpinInfo.transform.GetChild(0).GetComponent<TextMeshPro>().text = (SpinNumbers).ToString(); // auto left
            AutoSpinInfo.transform.GetChild(1).GetComponent<TextMeshPro>().text = (totalAutoSpins - SpinNumbers).ToString(); // auto used
            Isauto = true;
            ///SpinNumbers = spinNumberSelected;
            autostopbutton.SetActive(true);
        }
        this.OnSpinButtonClicked();



    }
    public void IncrementSpinNumber(int spin) {
        
            SpinNumbers += spin;
       

        }
    public void DecrementSpinNumber(int spin)
    {
        if(SpinNumbers>0)
        SpinNumbers -= spin;


    }
    public void StopAutoSpin() {
    
        autostopbutton.SetActive(false);
        SpinNumbers = 0;
        AutoSpinInfo.SetActive(false);
        StopSpinButton.SetActive(false);
        Isauto = false;
        //ColumnManager.instance.StopSpinnigButtonClicked();

    }
    public void StopSpin()
    {

        StopSpinButton.SetActive(false);

        ColumnManager.instance.StopSpinnigButtonClicked();

    }
    public void AddWinningsToTotalCredit(float amount)
    {

        Game.noOfCoinsLeft += amount;

      // UpdateGUI();
    }
    public void AddBonusWinToTotalCredit(float amount)
    {
        SlotManager.instance.winnings_session += (int)amount;

        Game.noOfCoinsLeft += amount;
      
      
          
         

        GenerateFlyingBalance(amount);

        UpdateGUI();
        ApiManager.instance.StartCoroutine(ApiManager.instance.UpdateWalletCor(amount,0,SlotManager.instance.GamePlayOutPut,true));

    }

    public void OnPayTableButtonClicked()
    {


             
        SoundFxManager.instance.buttonTapSound.Play();
        OnSlotItemClicked.CanShowSlotInfo = false;
        Instantiate(payTablePopupPrefab, Vector3.zero, Quaternion.identity);
        SetGuiButtonState(false);
        if (Game.currentGameState == GameState.isSpiningComplete)
        {
            LineManager.instance.ShowEabledLine(false);
            Game.currentGameState = GameState.isReady;
        }
    }

    public void OnBuyButtonClicked()
    {
        if (StoreScreen.instance)
            return;

        SoundFxManager.instance.buttonTapSound.Play();
        SlotManager.instance.ResetAll();
        Instantiate(storeScreenPopupPrefab, Vector3.zero, Quaternion.identity);

    }

    public void OnMaxLineButtonClicked()
    {
        SlotManager.instance.ResetAll();
        SoundFxManager.instance.MaxLineButtonSound.Play();
        SlotManager.instance.noOfLinesSelected = LineManager.instance.lineItemScripts.Length;
        betAmmountIndex = betAmmountArray.Length - 1;
        SlotManager.instance.betPerLineAmount = betAmmountArray[betAmmountIndex];
        if (!paylineShowing)
        {
            paylineShowing = true;
            LineManager.instance.ShowEabledLine(true);
        }
        else {

            paylineShowing = false;
            LineManager.instance.ShowEabledLine(false);
        }
        
        enabledLineText.text = SlotManager.instance.noOfLinesSelected.ToString();
        CalculateTotalBetAmount();
        ChangeLineInfoToTotalBet();
        Game.currentGameState = GameState.isReady;
        UpdateGUI();
    }

    public void LinesButton()
    {

        if (!paylineShowing)
        {
            paylineShowing = true;
            LineManager.instance.ShowEabledLine(true);
        }
        else
        {

            paylineShowing = false;
            LineManager.instance.ShowEabledLine(false);
        }
    }

    public void OnLinePlusButtonClicked()
    {
        currenTotalBet = SlotManager.instance.totalBetAmount;

        SlotManager.instance.ResetAll();
        SoundFxManager.instance.buttonTapSound.Play();

        //########################
        if (SlotManager.instance.noOfLinesSelected < 25)
            SlotManager.instance.noOfLinesSelected += 1;
        else
            return;
        //########################

        LineManager.instance.ShowEabledLine(true);
        enabledLineText.text = SlotManager.instance.noOfLinesSelected.ToString();
        CalculateTotalBetAmount();
        ChangeLineInfoToTotalBet();
        Game.currentGameState = GameState.isReady;
    }

    public void OnLineMinusButtonClicked()
    {
       
        currenTotalBet = SlotManager.instance.totalBetAmount;

        SoundFxManager.instance.buttonTapSound.Play();     

        SlotManager.instance.ResetAll();
        LineManager.instance.ShowEabledLine(false);

        //##############
        if (SlotManager.instance.noOfLinesSelected > 1)
            SlotManager.instance.noOfLinesSelected -= 1;
        //else
        //    return;
        //##############

        LineManager.instance.ShowEabledLine(true);
        enabledLineText.text = SlotManager.instance.noOfLinesSelected.ToString();
        CalculateTotalBetAmount();
        ChangeLineInfoToTotalBet();
        Game.currentGameState = GameState.isReady;
    }

    public void OnBetPlusButtonClicked()
    {
        currenTotalBet = SlotManager.instance.totalBetAmount;
        SlotManager.instance.ResetAll();
        SoundFxManager.instance.buttonTapSound.Play();

        if(betAmmountIndex < betAmmountArray.Length-1)
          betAmmountIndex++;
        else
            betAmmountIndex = 0;

        SlotManager.instance.betPerLineAmount = betAmmountArray[betAmmountIndex];

        betAmountText.text = SlotManager.instance.betPerLineAmount.ToString("F2");
        CalculateTotalBetAmount();
        ChangeLineInfoToTotalBet();
        if (Game.currentGameState == GameState.isSpiningComplete)
        {
            LineManager.instance.ShowEabledLine(false);
            Game.currentGameState = GameState.isReady;
        }
    }

    public void OnBetMinusButtonClicked()
    {
        currenTotalBet = SlotManager.instance.totalBetAmount;

        SlotManager.instance.ResetAll();
        SoundFxManager.instance.buttonTapSound.Play();

        if (betAmmountIndex > 0)
            betAmmountIndex--;
        else {
            betAmmountIndex = betAmmountArray.Length - 1;
        }

        SlotManager.instance.betPerLineAmount = betAmmountArray[betAmmountIndex];

        betAmountText.text =SlotManager.instance.betPerLineAmount.ToString("F2");
        CalculateTotalBetAmount();
        ChangeLineInfoToTotalBet();
        if (Game.currentGameState == GameState.isSpiningComplete)
        {
            LineManager.instance.ShowEabledLine(false);
            Game.currentGameState = GameState.isReady;
        }
    }





    public void CalculateTotalBetAmount()
    {
        currenTotalBet = SlotManager.instance.totalBetAmount;
        SlotManager.instance.totalBetAmount = (float)(SlotManager.instance.betPerLineAmount * SlotManager.instance.noOfLinesSelected);
        totalBetAmountText.text = SlotManager.instance.totalBetAmount.ToString("F2");
       
    }

    public void UpdateAllValuesAtStart() {

        CalculateTotalBetAmount();
        ChangeLineInfoToTotalBet();
    }

    internal void UpdateWiningAmount()
    {
        if(winingAmountText)
        winingAmountText.text = SlotManager.instance.currentSpinWinningAmount.ToString("F2");
      //  PlayerAnalytics.Instance.CurrentCredit.text = Game.noOfCoinsLeft.ToString();
    }

    public void UpdateGUI()
    {


        totalCoinText.text =  Game.noOfCoinsLeft.ToString("F2");
        enabledLineText.text = SlotManager.instance.noOfLinesSelected.ToString();
        betAmountText.text = SlotManager.instance.betPerLineAmount.ToString("F2");

       
        totalBetAmountText.text = SlotManager.instance.totalBetAmount.ToString("F2");
         
    }

    internal void ShowPerLineWinAmount(string winAmount)
    {
        perLineWinText.text = winAmount;
    }

    internal void DisableAllLineOnSpinStart()
    {
        for (int i = 0; i < SlotManager.instance.noOfLinesSelected; i++)
        {
            LineManager.instance.lineItemScripts[i].EnableLine(false);
        }
        if (LineManager.instance.Items_In_Column > 3) {

            LineManager.instance.HideAllItemPayline();
        }
        if(LineAnimationScript.instance.linewin)
            LineAnimationScript.instance.linewin.gameObject.SetActive(false);
    }

    public void SetGuiButtonState(bool enable)
    {
        SpinEnable = enable;
        for (int i = 0; i < AllGuiButton.Length; i++)
        {
            AllGuiButton[i].GetComponent<BoxCollider>().enabled = enable;
        }
       
    }
    public void SetCanvasButtonsState(bool state) {

        for (int i = 0; i < CanvasButtons.Length; i++)
        {
            CanvasButtons[i].interactable = state;
        }
    }

}
