using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Rooms
{
    Casino,Witch,Egypt,Zoombie,Vegas,Farm
}

public class JackpotManagerScript : MonoBehaviour {
    public static JackpotManagerScript instance;

    public delegate void JackPotDel(Rooms room,float coins);
    public delegate void JackPotTextUpdate(float i);
    public static JackPotDel OnUpdateJackpot;
    public static JackPotTextUpdate OnCoinsTextUpdate;
  

    public static string jackpotTimeString = "JACKPOT_START";
    public static string jackpotValSTring = "JACKPOT_COINS";

    public int restartMin;
    public Text congrassText;
    public GameObject congrassPannel;

    private string strAtStart;
    private float currentWin;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        strAtStart = congrassText.text;
    }

    // Use this for initialization
    void Start()
    {

        if (!PlayerPrefs.HasKey(jackpotTimeString + (Rooms)0))
        {
            for (int i = 0; i < 6; i++)
            {
                PlayerPrefs.SetString(jackpotTimeString + (Rooms)i, System.DateTime.Now.ToBinary() + "");
                PlayerPrefs.SetInt(jackpotValSTring + (Rooms)i, restartMin);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                long val = long.Parse(PlayerPrefs.GetString(jackpotTimeString + (Rooms)i));
                int intVal = (int)((System.DateTime.Now.Subtract(System.DateTime.FromBinary(val) )).TotalSeconds+ restartMin);
                PlayerPrefs.SetInt(jackpotValSTring + (Rooms)i, intVal);
                if (OnUpdateJackpot != null)
                    OnUpdateJackpot((Rooms)i,intVal);
            }

        }
       
    }
	
	// Update is called once per frame
	void Update (){
        for (int i = 0; i < 6; i++)
        {
            long val = long.Parse(PlayerPrefs.GetString(jackpotTimeString + (Rooms)i));
            int intVal = (int)((System.DateTime.Now.Subtract(System.DateTime.FromBinary(val))).TotalSeconds + restartMin);
            PlayerPrefs.SetInt(jackpotValSTring + (Rooms)i, intVal);
            if (OnUpdateJackpot != null)
                OnUpdateJackpot((Rooms)i, intVal);
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            JackPotWin(Rooms.Egypt);
        }
    }


    public void OkButtonInCongrass()
    {
        congrassPannel.SetActive(false);
        if(OnCoinsTextUpdate != null)
        {
            OnCoinsTextUpdate(currentWin + Game.noOfCoinsLeft);
        }
    }

    public void JackPotWin(Rooms room)
    {
        congrassPannel.SetActive(true);
        long val = long.Parse(PlayerPrefs.GetString(jackpotTimeString + (room)));
        currentWin = ((int)((System.DateTime.Now.Subtract(System.DateTime.FromBinary(val))).TotalSeconds + restartMin));
        congrassText.text = strAtStart + currentWin;
        PlayerPrefs.SetString(jackpotTimeString + room, System.DateTime.Now.ToBinary() + "");
        
    }


}
