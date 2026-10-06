using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;
using AvoEx;
using gm.api.domain;
using UnityEngine.Rendering;
using System.Globalization;
using System.Threading;

public enum GameState
{
    none,
    isSpining,
    isReady,
    isSpiningComplete
}


public class Game : MonoBehaviour
{
    public static Game Instance;
    public static bool isReadyForAppStore = false;

    public static float noOfCoinsLeft = 1000f;
    public static ObscuredFloat MinBonusWin = 300;

   
    public static GameState currentGameState = GameState.none;

    public TextMeshPro UsernameField;
    public Image userPhoto;
    public GameObject CheatWarningPanel;

    public GameObject BonusAtStartPanel;
   public string[] DataString = new string[2];

    public string payString, probString;

    public bool Once = false;

    private Texture2D dumypic;
    private void Awake()
    {
        Instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
      //  UsernameField.text = GameSocketManager.Instance.UserName.ToString();

    }

    //ENR
    public void SetCulture(string cultureCode)
    {
        CultureInfo culture = new CultureInfo(cultureCode);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        //Debug.Log("Culture changed to: " + cultureCode);
    }

    private void Start()
    {
        //ENR
        SetCulture("en-US");

        Game.currentGameState = GameState.isReady;
        GUIManager.instance.SetGuiButtonState(true);
        //UsernameField.text = ApiManager.instance.myDeserializedClassUpdate.data.first_name;
        OnSlotItemClicked.CanShowSlotInfo = false;
        ObscuredCheatingDetector.StartDetection(OncheaterDetected);
        dumypic = Resources.Load("pic") as Texture2D;
      //  userPhoto.sprite = GlobalAccess.Instance.GetSelectedAvatar();
            }

    void Update() {

       
       
    
    }

    public void AddBalance() {

        noOfCoinsLeft = (float) ApiManager.instance.Balance;
        GUIManager.instance.UpdateGUI();
    }

    private void OncheaterDetected()
    {
        GUIManager.instance.SetGuiButtonState(false);
     
        Invoke("newdirection", 1.5f);
    }
  public void newdirection()
    {
        GUIManager.instance.LobbyYes();
        //  Application.OpenURL(ApiManager.instance.DashboardUrl);
    }


    internal static void Reset()
    {

    }

  

    internal void LoadPlayerData()
    {







        GUIManager.instance.UpdateAllValuesAtStart();

        GUIManager.instance.UpdateGUI();
    
        GUIManager.instance.SetGuiButtonState(true);
        loadPaytableValues();

    }
    public string[] items;
   public void loadPaytableValues() {


        //  Debug.Log("Pay data is " + SlotManager.instance.Pay_data);

       

        string readyPaydata = SlotManager.instance.Pay_data.Trim("\"".ToCharArray());

     ///  print("Ready pay data as " + readyPaydata);

       DataString = readyPaydata.Split('$');

        payString = DataString[0];
        probString = DataString[1];

      //  Debug.Log("ready data is " + readyPaydata);


      items =payString.Split('/');
            //new string[] { }; //ApiManager.instance.Mydata.pay_data.Split('/');
  
     
   
       string[] item = items[0].Split(',');
       PayTable.instance.item1 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[1].Split(',');
        PayTable.instance.item2 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[2].Split(',');
        PayTable.instance.item3 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[3].Split(',');
        PayTable.instance.item4 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[4].Split(',');
        PayTable.instance.item5 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[5].Split(',');
        PayTable.instance.item6 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[6].Split(',');
        PayTable.instance.item7 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[7].Split(',');
        PayTable.instance.item8 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[8].Split(',');
        PayTable.instance.item9 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[9].Split(',');
        PayTable.instance.autoSpinActive = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };


        item = items[10].Split(',');
        PayTable.instance.WildInSequanceAmount = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        if (items.Length >10)
        {
            item = items[11].Split(',');
            for (int i = 0; i < 5; i++)
            {
                PayTable.instance.WinPerBonusRound[i] = int.Parse(item[i]);
            }
        }
        //if (items.Length > 11) {
        //    GameOperations.instance.FixedBonusSpin = int.Parse(items[12]);

        //}
        loadProbValues();         


    }
    void loadProbValues() {


        string[] items_probs = probString.Split(','); // ApiManager.instance.Mydata.prob_data.Split(',');

        for (int i = 0; i < items_probs.Length; i++) {

            ProbabilityManager.instance.ItemsProbabilities[i] = int.Parse(items_probs[i]);
        }

        ProbabilityManager.instance.getProbSum();

        GUIManager.instance.Startbet();


    }


    public void PlayBonusAtStart() {

        SlotManager.instance.betPerLineAmount = 0.02f;
        SlotManager.instance.noOfLinesSelected = LineManager.instance.lineItemScripts.Length;
        GUIManager.instance.CalculateTotalBetAmount();
        GUIManager.instance.UpdateGUI();
        SlotManager.instance.MybonusReward();






    }
}
