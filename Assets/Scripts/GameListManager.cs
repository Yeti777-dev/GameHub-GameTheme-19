using BestHTTP.JSON.LitJson;
using BestHTTP.SocketIO;
using gm.api.domain;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameListManager : MonoBehaviour
{
    public GameObject LoadingPanel;

    private float fillTarget;
    public Image FillProgress;
    public int show_Games_Num;
    public Transform ShowGameTransform;


    public Transform GameTypesTransform;
    public int Games_types_num;
    public string[] CategoryNamesArray;
    public Sprite[] CategorySpritesArray;

    private List<GmGame> games;
    private GmWallet wallet = new GmWallet();
    public GameObject gamePrefab;
    public GameObject gameCategoryPrefab;

    public Text Username, UserBalance;
    public List<GameObject> AllGames = new List<GameObject>();
    public static GameListManager Instance;

  

    void Awake() {
        Time.timeScale = 1;
           LoadGameList();

        LoadWallet();

       loadGamesTypes();
        if (Instance == null)
            Instance = this;

    }
    void Start()
    {
        //  DontDestroyOnLoad(this.gameObject);
        GameSocketManager.Instance.InvokeGetGameListAsync();

        GameSocketManager.Instance.InvokeGetWalletAsync();


     


        Username.text = GameSocketManager.Instance.auth.Username.ToString();
    }

    public void LoadGameList() {
        bool Callonce = false;
        GameSocketManager.OnGetGameList += (res) => {

            if (Callonce)
                return;
            Callonce = true;
            print("Game list called");
            if (res.HasError)
            {
                DisplayErrorOnScreen.Instance.DisplayError("Loading Failed!", res.ErrorMessage);
                print("Error Detected" + res.ErrorMessage.ToString());

               
            }
            else
            {
                games = res.Data;
                print("Number of Games are " + games.Count);
                for (int i = 0; i < games.Count; i++)
                {
                    var gm = games[i];
                    print("this game is active "+gm.IsActive);

                    if (gm.IsActive)
                    {
                        GameObject gameObj = GameObject.Instantiate(gamePrefab, GameObject.FindGameObjectWithTag("ShowGames").transform);

                        gameObj.transform.name = games[i].Name;

                        gameObj.GetComponent<GameData>().Name = games[i].Name;
                        gameObj.GetComponent<GameData>().GameType = games[i].GameType;
                        gameObj.GetComponent<GameData>().GameId = games[i].Id;
                        gameObj.GetComponent<GameData>().SceneName = games[i].SceneName;
                        gameObj.GetComponent<GameData>().logoUrl = games[i].LogoUrl;
                        gameObj.GetComponent<GameData>().IscomingSoon = games[i].IsComingSoon;
                        gameObj.GetComponent<GameData>().isNewGame = games[i].IsNewGame;
                        gameObj.GetComponent<GameData>().GameOrderNumber = games[i].GameOrder;
#if UNITY_EDITOR
                        gameObj.GetComponent<GameData>().AssetUrl = games[i].UnitySceneUrl;
                        print("receive bundle for Unity editor");
#elif UNITY_ANDROID
 gameObj.GetComponent<GameData>().AssetUrl = games[i].AndroidSceneUrl;
  print("receive bundle for android");
#elif UNITY_IOS
gameObj.GetComponent<GameData>().AssetUrl = games[i].IosSceneUrl;
 print("receive bundle for Iphone");
#elif UNITY_WEBGL
                                         gameObj.GetComponent<GameData>().AssetUrl = games[i].WebSceneUrl;
                                          print("receive bundle for Webgl");

#endif
                        //   gameObj.GetComponent<GameData>().Rtp = games[i].Config.Rtp;


                        gameObj.GetComponent<GameData>().GetmyPath();

                        //     var payout = JsonMapper.ToObject<CasinoPayoutModel>(gm.Config.Payouts);

                        //  gameObj.GetComponent<GameData>().Payouts = payout;

                        if (!GameSocketManager.Instance.IsGamelistLoaded)
                        {
                            var img = gameObj.transform.GetChild(0).GetComponent<Image>();
                            var imgLoader = gameObj.transform.GetChild(0).GetComponent<ImageLoader>();
                            StartCoroutine(imgLoader.LoadImage(gm.LogoUrl, img, "Image no_"+i.ToString()));
                            if (i + 1 == games.Count)
                                GameSocketManager.Instance.IsGamelistLoaded = true;
                        }
                        else {
                            var img = gameObj.transform.GetChild(0).GetComponent<Image>();
                            var imgLoader = gameObj.transform.GetChild(0).GetComponent<ImageLoader>();
                            imgLoader.LoadIconsFromDisk("Image no_" +i.ToString(), img);
                        }

                        AllGames.Add(gameObj);
                    }
                }

            }
           
        };

     



    }


  void  LoadWallet() {
        bool callonce = false;
        GameSocketManager.OnGetWallet += (res) =>
        {
            if (callonce)
                return;
            callonce = true;
            if (res.HasError)
            {
                DisplayErrorOnScreen.Instance.DisplayError("Failed to load wallet details!", res.ErrorMessage);

            }
            else
            {

                wallet = res.Data;
                //  print("User balance is " + wallet.Coin.ToString());
                Game.noOfCoinsLeft = (float)wallet.Coin;
                UserBalance.text = Game.noOfCoinsLeft.ToString();

            }

        };
    }

    void loadGamesTypes() {


        for (int i = 0; i < Games_types_num; i++)
        {

            GameObject CatObj = GameObject.Instantiate(gameCategoryPrefab, GameTypesTransform);
            CatObj.transform.name = CategoryNamesArray[i];
            CatObj.GetComponent<GameCategory>().categoryName = CategoryNamesArray[i];
            CatObj.transform.GetChild(0).GetComponent<Image>().sprite = CategorySpritesArray[i];

        }

    }
    // Update is called once per frame
    void Update()
    {
      
    }

    public void LoadingScene(string scene) {

        LoadingPanel.GetComponent<loadingScript>().SceneToLoad = scene; 
        LoadingPanel.SetActive(true);

    }

    public void DisableLoadingScene()
    {

        LoadingPanel.SetActive(false);

    }

    public void GamesToShow(string Category) {

        if (Category == "All")
        {

            for (int i = 0; i < AllGames.Count; i++)
                AllGames[i].gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < AllGames.Count; i++)
                if (AllGames[i].GetComponent<GameData>().GameType.ToString() == Category)
                AllGames[i].gameObject.SetActive(true);
            else
                    AllGames[i].gameObject.SetActive(false);
        }
    
    }

    private void OnDestroy()
    {

        AllGames.Clear();
        games.Clear();
        GameSocketManager.OnGetGameList -= (res) =>{ };
        //  StopAllCoroutines();
        // AllGames.ForEach(x => Destroy(x));
        //  Destroy(Instance);
    }

}
