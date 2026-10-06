using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;
public class GameOperations : MonoBehaviour
{

    public static GameOperations instance;
    public int ScatterSlotItemCount;
    public SlotItem slotItem;
    public ObscuredInt noOfFreeSpin, noOfFreeSpinUsed;
    private GameObject getIndiaction, sequareIndication;
    public SlotItem[] scatterItems;
    public GameObject FreeSpinInfo;
    private TextMeshPro noOfSpinRemainingText;
    private TextMeshPro noOfFreeSpinsUsedText;

    public GameObject FreeSpinsWinShow;
    public GameObject spinButton, AutoSpinButton;
    public bool FreeSpinBoolDelay;


    public GameObject PauseButton, ResumeButton;
    public GameObject SoundONObj, SoundOFFObj;
    internal bool IsGamePaused = false;
    internal bool isSoundOFF = false;
    public int scattersMatched =0;
    public int BonusMatched = 0;
    bool Mute = false;
    public ObscuredInt ExtraSpinsLeft;
    public GameObject AddSpinsText;
    public bool GameHasBonus = true;
    public bool BonusToFreeSpin = true;
    public bool NoBonusAtAll = false;
    public bool isFixedSpins = false;
    public bool WildAsMultiplier = false;
    public bool HasExpandedWild = true;
    public bool CanWildAlsoTriggerFreespins = false;


    internal int FixedBonusSpin = 10;

    public TextMeshPro collectedmesh, multiplierfsmesh, toplinetext;
    internal int collect = 1;
    internal int multiplierfs = 2;
    private int innermultiplier = 1;
    // Use this for initialization
    void Start()
    {
        instance = this;
        scatterItems = new SlotItem[15];
        FreeSpinBoolDelay = false;
        PauseButton.SetActive(true);
        ResumeButton.SetActive(false);

        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            Mute = false;
            AudioListener.volume = 1;
            SoundOFFObj.SetActive(true);
            SoundONObj.SetActive(false);
   

        }
        else
        {
            AudioListener.volume = 0;
            Mute = true;
            SoundOFFObj.SetActive(false);
            SoundONObj.SetActive(true);
      
        }



        // AudioListener.volume = PlayerPrefs.GetInt("Sound");
    }

    // Update is called once per frame
    void Update()
    {
      

    }
    public void CalculateFreespinMultiplier()
    {

        collectedmesh.text = collect.ToString();
        multiplierfs = collect * innermultiplier;
        multiplierfsmesh.text = multiplierfs.ToString();

        toplinetext.text = "Free spin  " + noOfFreeSpinUsed.ToString() + " -  All win x  " + multiplierfs.ToString();

    }
    public void ClearFsMultipliers()
    {

        collect = 1;
        multiplierfs = 2;
        CalculateFreespinMultiplier();

    }

    public void PauseGame()
    {
       // Time.timeScale = 0;
        AudioListener.volume = 0;
        PauseButton.SetActive(false);
        ResumeButton.SetActive(true);
        IsGamePaused = true;
    }
    public void ResumeGame() {

        Time.timeScale = 1;
        if (!Mute)
            AudioListener.volume = 1;
        PauseButton.SetActive(true);
        ResumeButton.SetActive(false);
        IsGamePaused = false;

    }
    public void SoundOFF()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            AudioListener.volume = 0;
            Mute = true;
            SoundOFFObj.SetActive(false);
            SoundONObj.SetActive(true);
            PlayerPrefs.SetInt("Sound", 0);
        }
    }
    public void SoundON()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 0)
        {
            Mute = false;
            AudioListener.volume = 1;
            SoundOFFObj.SetActive(true);
            SoundONObj.SetActive(false);
            PlayerPrefs.SetInt("Sound", 1);
        }
        else
        {
            AudioListener.volume = 0;
            Mute = true;
            SoundOFFObj.SetActive(false);
            SoundONObj.SetActive(true);
            PlayerPrefs.SetInt("Sound", 0);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!Mute) {
            AudioListener.volume = focus ? 1 : 0;
        }
    }

    internal void TraceForScatter()
    {
        ScatterSlotItemCount = 0;
        for (int i = 0; i < LineManager.instance.Items_In_Column; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                slotItem =(SlotItem) ColumnManager.instance.GetSlotItemAt(j, i);
                if (slotItem.itemType == SlotItemType.Scatter)
                {
                    scatterItems[ScatterSlotItemCount] = slotItem;
                    ScatterSlotItemCount++;

                }
                if (CanWildAlsoTriggerFreespins)
                {
                    if (slotItem.itemType == SlotItemType.Wild)
                    {
                        scatterItems[ScatterSlotItemCount] = slotItem;
                        ScatterSlotItemCount++;

                    }


                }
            }
        }

        if (ScatterSlotItemCount >= 3)
        {
            sequareIndication = new GameObject();
            sequareIndication.tag = "ItemInication";
            sequareIndication.transform.position = Vector3.zero;
            SoundFxManager.instance.singleLineIndicationSound.Play();
          
            for (int i = 0; i < ScatterSlotItemCount; i++)
            {
                 SetItemIndication(scatterItems[i].transform.position);
                iTween.PunchScale(scatterItems[i].gameObject, scatterItems[i].transform.localScale*1.5f, 1f);

            }
        }
      
    }

    public void StartFreeSpinRound() {

       
        if (ScatterSlotItemCount == 3)
        {
            noOfFreeSpin = (int)PayTable.instance.autoSpinActive[2].y;
        }
        else if (ScatterSlotItemCount == 4)
        {
            noOfFreeSpin = (int)PayTable.instance.autoSpinActive[1].y;
        }
        else if (ScatterSlotItemCount >= 5)
        {
            noOfFreeSpin = (int)PayTable.instance.autoSpinActive[0].y;
        }
        if (WildAsMultiplier)
            ClearFsMultipliers();
        Invoke("GenerateFreeSpinPopUp", 0.5f);

    }

    public void RewardBonusSpins(int Bonus_spins) {

        isFixedSpins = true;
        noOfFreeSpin = Bonus_spins;
        SlotManager.instance.IsFreeSpinsEnabled = true;
        FreeSpinBoolDelay = true;
      
        Invoke("GenerateFreeSpinPopUp", 0.1f);

    }

    public void AddExtraFreeSpinsInRound() {

        int MorefreeSpins = 0;
        if (ScatterSlotItemCount == 3)
            MorefreeSpins = 5;
        else
            if (ScatterSlotItemCount == 4)
            MorefreeSpins = 10;
        else
        {
            MorefreeSpins = 15;
        }
        SlotManager.instance.GameSessionFreeSpins += MorefreeSpins;

        noOfFreeSpin += MorefreeSpins;
        noOfSpinRemainingText.text = noOfFreeSpin.ToString();
        GameObject clone = (GameObject)Instantiate(AddSpinsText, AddSpinsText.transform.position, Quaternion.identity);
        clone.transform.GetChild(0).GetComponent<TextMesh>().text = "+" + MorefreeSpins.ToString();
        Destroy(clone, 2f);
    }

    internal void TraceForScatterItem()
    {
        if (!SlotManager.instance.IsFreeSpinsEnabled)
        {
            TraceForScatter();
            if (ScatterSlotItemCount >=3)
            {
                SlotManager.instance.IsFreeSpinsEnabled = true;
                StartFreeSpinRound();
            }
          
        }
        else
        {
            SlotManager.CanSpinAgain = false;
            TraceForScatter();
            if (ScatterSlotItemCount >2)
            {

                AddExtraFreeSpinsInRound();

            }
            if(!isFixedSpins)
            Invoke("CheckNextFreeSpin", 1f);
         


        }
    }

    void CheckNextFreeSpin() {
        if (SlotManager.instance.currentSpinWinningAmount == 0)
            Invoke("EnableAutoSpin", 0.3f);
    }

    public void CheckFreeSpinOnCountComplete() {
        if(GameOperations.instance.noOfFreeSpinUsed >0)
        Invoke("EnableAutoSpin", 0.5f);
    }
  public  void GenerateFreeSpinPopUp()
    {
        if (SlotManager.instance.IsBonusEnabled)
            return;

        StartCoroutine(SquareGfxBlinkingForScatter());

         if (SlotManager.instance.currentSpinWinningAmount != 0)
            Invoke("_GetNoOfAutoSpin", 2);
        else
            _GetNoOfAutoSpin();
    }

     void _GetNoOfAutoSpin()
    {
        if (!LineManager.instance.BigBigWins)
        {
            StartAutoSpin();
            StartCoroutine(SquareGfxBlinkingForScatter());
            LineManager.instance.ShowEabledLine(false);
            Invoke("_StartAutoSpin", 1f);
            GameEffects.instance.FreeSpinPopUpEffect();
        }
        else
        {
            Invoke("LateFreeSpins", 5f);
        }

    }
    void LateFreeSpins() {
        StartAutoSpin();
        StartCoroutine(SquareGfxBlinkingForScatter());
        LineManager.instance.ShowEabledLine(false);
        Invoke("_StartAutoSpin", 2f);
        GameEffects.instance.FreeSpinPopUpEffect();
    }

    public void _StartAutoSpin()
    {

        DestroyIndication();
     
        FreeSpinBoolDelay = false;
    }


     void OnAutoSpinComplete()
    {
        if (noOfFreeSpin == 1)
        {
        
            if (FreeSpinIndicationScript.instance != null)
            {

                FreeSpinIndicationScript.instance.DestroyFreeSpinIndication();
                GUIManager.instance.SetGuiButtonState(true);
               
            }

            
        }
    }

   

    internal void StartAutoSpin()
    {

       
        SlotManager.instance.GameSessionFreeSpins += noOfFreeSpin;
        ScatterSlotItemCount = 0;
        // scene change Animation
         // Invoke("EnableAutoSpin", 2);
     }
 
    private int totalFreeSpins;
 
    public void ContinueToFreeSpins() {
        SlotManager.instance.IsBonusEnabled = false;
        if (GameEffects.onceBonus)
        {
            GameEffects.onceBonus = false;
        }
        totalFreeSpins = noOfFreeSpin;
        SlotManager.instance.FreeSpinsWinSum = 0;
        SlotManager.instance.IsFreeSpinsEnabled = true;
        FreeSpinInfo.SetActive(true);
        GameObject noOfSpinText = FreeSpinInfo.transform.GetChild(0).gameObject;
        noOfSpinRemainingText = noOfSpinText.GetComponent<TextMeshPro>();
        noOfSpinRemainingText.text = noOfFreeSpin.ToString();
        GameObject noOfSpinUsedText = FreeSpinInfo.transform.GetChild(1).gameObject;
        noOfFreeSpinsUsedText = noOfSpinUsedText.GetComponent<TextMeshPro>();
        noOfFreeSpinsUsedText.text = "0";

        Invoke("EnableAutoSpin", 1.2f);

    }



    private void EnableAutoSpin()
    {
        SlotManager.instance.ResetAll();
        if (!LineManager.instance.BigBigWins)
        {
            NextFreeSpin();
        }
       

       
    }

    public void NextFreeSpin() {

        // spinButton.SetActive(false);
        spinButton.GetComponent<MyButtonScript>().blurTheImage();
        AutoSpinButton.SetActive(false);
        OnAutoSpinComplete();
        if (noOfFreeSpin == 0)
        {

            if (SlotManager.instance.currentSpinWinningAmount > 0)
                Invoke("ShowfreeSpinsWin", SlotManager.instance.currentSpinWinningAmount+1);
            else
                ShowfreeSpinsWin();

            noOfFreeSpinUsed = 0;
            ColumnManager.instance.isStickyWild = false;

            CancelInvoke("EnableAutoSpin");
            return;
        }

        SlotManager.instance.IsFreeSpinsEnabled = true;
        LineManager.instance.ShowEabledLine(false);
        SlotManager.instance.SpinButtonClicked();
        noOfFreeSpin--;
        noOfFreeSpinUsed ++;
        if (WildAsMultiplier)
            CalculateFreespinMultiplier();
        noOfSpinRemainingText.text = noOfFreeSpin.ToString();
        noOfFreeSpinsUsedText.text = noOfFreeSpinUsed.ToString();
        if(ExtraSpinsLeft > 0)
        {
            ExtraSpinsLeft--;

        }
    }

    void ShowfreeSpinsWin() {
        Instantiate(FreeSpinsWinShow, FreeSpinsWinShow.transform.position, Quaternion.identity);

    }
    public void FreeSpinsCompleted() {
        BgManager.instance.SetNormlBg();
        //if (!GUIManager.instance.TurboButtonStatus)
        //    GUIManager.instance.TurboMode();
        if (WildAsMultiplier)
            ClearFsMultipliers();
        SlotManager.instance.IsFreeSpinsEnabled = false;
        FreeSpinInfo.SetActive(false);
        SlotManager.instance.noOfLinesSelected = LineManager.instance.lineItemScripts.Length;
      //  GUIManager.instance.UpdateGUI();
        GUIManager.instance.GenerateFlyingBalance(SlotManager.instance.FreeSpinsWinSum);
       // spinButton.SetActive(true);
        spinButton.GetComponent<MyButtonScript>().ClearTheImage();
        //PlayerAnalytics.Instance.WinningsLifeTime += SlotManager.instance.FreeSpinsWinSum;
        AutoSpinButton.SetActive(true);
        SlotManager.CanSpinAgain = true;
        SlotManager.instance.FreeSpinsWinSum = 0;
        //  GetBonusRoundinSpin();
        if (GUIManager.instance.SpinNumbers > 0)
            Invoke("ContinueAutoSpinAfterFreeSpins", 2f);
        else
        {
            GUIManager.instance.SetGuiButtonState(true);
            GUIManager.instance.StopSpinButton.SetActive(false);
        }

    }

    void ContinueAutoSpinAfterFreeSpins() {
        SlotManager.instance.SpinAgain();
    }

    internal void SetItemIndication(Vector3 pos)
    {
         Vector3 newpos = pos;
        newpos.z = -4;
        pos = newpos;
        getIndiaction = Instantiate(LineAnimationScript.instance.SlotItemSpiritesAnim,pos,Quaternion.identity) as GameObject;
        getIndiaction.transform.parent = sequareIndication.transform;
      

    }

    // Sequare For Blinking Effect
    internal IEnumerator SquareGfxBlinkingForScatter()
    {
        PlayScatterItemAnimation();
        yield return new WaitForSeconds(.1f);
        //for (int i = 0; i < 5; i++)
        //{

        //    sequareIndication.SetActive(true);
        //    yield return new WaitForSeconds(.1f);
        //    sequareIndication.SetActive(false);
        
        //}
    }

    void PlayScatterItemAnimation()
    { 
        for (int i = 0; i < ScatterSlotItemCount; i++)
        {
            int animationIndex;
            animationIndex = scatterItems[i].animationIndex;

           // scatterItems[i].ItemPackedSprite.PlayAnim(animationIndex);
        }
    }

   
    internal void DestroyIndication()
    {
        Destroy(sequareIndication);
       
    }

}
