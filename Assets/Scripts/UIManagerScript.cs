using UnityEngine;
using System.Collections;

public class UIManagerScript : MonoBehaviour {
    public int videoPannelEnableCoinsLimit;
    public GameObject congrassPannelGO;
    public float rewardedAmount;
    public GameObject videoPannelGO;

   public static bool isVideoPannelShowing;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( Game.noOfCoinsLeft <= videoPannelEnableCoinsLimit && !isVideoPannelShowing )//&& AdsManagerNew.instance.isChartboostVideoAvaliable())
        {
            isVideoPannelShowing = true;
            videoPannelGO.SetActive(true);
        }
	
	}


    public void VideoPannelButton()
    {
        videoPannelGO.SetActive(false);
       // AdsManagerNew.instance.ShowRewardedVideo(OnVideoAdsComplete,OnVideoAdsFailed);
    }

//    void OnVideoAdsComplete(ChartboostSDK.CBLocation loc,int i)
//    {
//        congrassPannelGO.SetActive(true);
//        ChartboostSDK.Chartboost.didCompleteRewardedVideo -= OnVideoAdsComplete;
//        ChartboostSDK.Chartboost.didDismissRewardedVideo -= OnVideoAdsFailed;
//    }
//
//    void OnVideoAdsFailed(ChartboostSDK.CBLocation loc)
//    {
//        ChartboostSDK.Chartboost.didDismissRewardedVideo -= OnVideoAdsFailed;
//        Invoke("ResetPannelShowing", 1f);
//    }
//
    public void CongrassOKButton()
    {
        congrassPannelGO.SetActive(false);
        //MainScreen.instance.CoinsLerb(Game.noOfCoinsLeft + rewardedAmount);
        Invoke("ResetPannelShowing", 1f);
    }

    void ResetPannelShowing()
    {
        isVideoPannelShowing = false;
    }
  
}
