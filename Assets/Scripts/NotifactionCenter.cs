using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotifactionCenter : MonoBehaviour
{
    public delegate void Ontrigger(float amount);
    public static Ontrigger OnNotifactionTextUpdate;
    public static NotifactionCenter instance;
    public int notifactionInterval;
    public float winAmount;
    public static string notifactionString = "Notifaction";
    public GameObject congrassPannelGO;
    
    public Text winAmountText;

    NotificationService notificationService = new NotificationService();
    private bool inited = false;
    private int notifactionInSeconds = 60;

    private int currentNotifactionNO = 1;
    private bool isStart;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        notifactionInSeconds = notifactionInterval;
    }

    // Use this for initialization
    void Start()
    {

        if (!PlayerPrefs.HasKey(notifactionString))
        {
            PlayerPrefs.SetString(notifactionString, System.DateTime.Now.AddSeconds(notifactionInSeconds).ToBinary() + "");
        }

        if (!inited)
        {
           
            notificationService.Init();
            inited = true;
            
          //  
            notificationService.RemoveAllPreviousNotifications();
            
          //  notificationService.SetActivityClassName("com.unity3d.player.UnityPlayerNativeActivity");
          //  notificationService.KeepNotificationsAfterReboot();
          //  SetupNotifaction(1, 30);
          //  SetThenotifationAtStart();
           
        }


    }



    void SetThenotifationAtStart()
    {
        string str = PlayerPrefs.GetString(notifactionString);

        System.TimeSpan span = System.DateTime.Now.Subtract(System.DateTime.FromBinary(long.Parse(str)));

        if (span.TotalSeconds >= notifactionInSeconds * 2)
        {
            PlayerPrefs.SetString(notifactionString, System.DateTime.Now.AddSeconds(notifactionInSeconds).ToBinary() + "");
          //  SetupNotifaction(1, notifactionInterval);
            congrassPannelGO.SetActive(true);
        }
        else if (span.TotalSeconds >= notifactionInSeconds * 1)
        {
            string s = PlayerPrefs.GetString(notifactionString);
            PlayerPrefs.SetString(notifactionString, (System.DateTime.FromBinary(long.Parse(s)).AddSeconds(notifactionInSeconds * 2)).ToBinary() + "");
          //  SetupNotifaction(1, (int)((System.DateTime.FromBinary(long.Parse(PlayerPrefs.GetString(notifactionString)))).Subtract(System.DateTime.Now).TotalSeconds));
            congrassPannelGO.SetActive(true);
        }
        else if (span.TotalSeconds >= 0)
        {
            string s = PlayerPrefs.GetString(notifactionString);
            PlayerPrefs.SetString(notifactionString, (System.DateTime.FromBinary(long.Parse(s)).AddSeconds(notifactionInSeconds)).ToBinary() + "");
          //  SetupNotifaction(1, (int)(System.DateTime.FromBinary(long.Parse(PlayerPrefs.GetString(notifactionString)))).Subtract(System.DateTime.Now).TotalSeconds);
            congrassPannelGO.SetActive(true);
        }
        else
        {
         //   SetupNotifaction(1, (int)(System.DateTime.FromBinary(long.Parse(PlayerPrefs.GetString(notifactionString)))).Subtract(System.DateTime.Now).TotalSeconds);
        }
    }

    void SetupNotifaction(int id, int delay)
    {
        notificationService.SetActivityClassName("com.unity3d.player.UnityPlayerNativeActivity");
        NotificationParams paramsForFirstNotification = new NotificationParams(id, "This is first Notification", "You have got 1000 coins", "Come back!", "icon1");
        paramsForFirstNotification.Vibrate = true;
        paramsForFirstNotification.LargeIconPath = "large1";
        paramsForFirstNotification.DelayInSeconds = delay;
        paramsForFirstNotification.Sound = true;
        paramsForFirstNotification.NewIconStyleColor = Color.blue;
        paramsForFirstNotification.LEDParameters = new LedParams(Color.green, 200, 200);
        notificationService.CreateNotificationEvent(paramsForFirstNotification);
    }


    void SetupNotifactionInQuit()
    {

        notificationService.RemoveAllPreviousNotifications();
        
        SetupNotifaction(1, (int)(System.DateTime.FromBinary(long.Parse(PlayerPrefs.GetString(notifactionString)))).Subtract(System.DateTime.Now).TotalSeconds);
        SetupNotifaction(2, (int)(System.DateTime.FromBinary(long.Parse(PlayerPrefs.GetString(notifactionString)))).AddSeconds(notifactionInSeconds).Subtract(System.DateTime.Now).TotalSeconds);
        SetupNotifaction(3, (int)(System.DateTime.FromBinary(long.Parse(PlayerPrefs.GetString(notifactionString)))).AddSeconds(notifactionInSeconds * 2).Subtract(System.DateTime.Now).TotalSeconds);
        notificationService.StartNotificationService();
    }

    // Update is called once per frame
    void Update()
    {
        long val = long.Parse(PlayerPrefs.GetString(notifactionString,System.DateTime.Now.AddSeconds(10).ToBinary()+""));
        int intVal = (int)((System.DateTime.Now.Subtract(System.DateTime.FromBinary(val))).TotalSeconds);
         if (intVal > 0)
         {
             if (!congrassPannelGO.activeSelf)
             {
                 congrassPannelGO.SetActive(true);
                 // PlayerPrefs.SetString(notifactionString, System.DateTime.Now.AddSeconds(notifactionInSeconds).ToBinary() + "");
                 // SetupNotifaction(1, notifactionInterval);
                 PlayerPrefs.SetString(notifactionString, System.DateTime.Now.AddSeconds(notifactionInSeconds).ToBinary() + "");
                notificationService.StopNotificationService();
                currentNotifactionNO++;
                 SetupNotifaction(currentNotifactionNO, 0);
                notificationService.StartNotificationService();

            }
             else
             {

             }
         }

    }

    void ResetNotifaction()
    {
       
    }

    public void CongrassOkbutton()
    {
        congrassPannelGO.SetActive(false);
        if (OnNotifactionTextUpdate != null)
            OnNotifactionTextUpdate(winAmount + Game.noOfCoinsLeft);
        
    }

    public void ShowNotifaction()
    {
        SetupNotifaction(1, 5);
    }

    void OnApplicationQuit()
    {
        //SetupNotifactionInQuit();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        { // Application goes to background.

           SetupNotifactionInQuit();
        }
        else
        {
           // SetThenotifationAtStart();
        }
    }

    void OnDestroy()
    {
       // SetupNotifactionInQuit();
    }

}