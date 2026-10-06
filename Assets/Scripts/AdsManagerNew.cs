using UnityEngine;
using System.Collections;
//using ChartboostSDK;
using System;

public class AdsManagerNew : MonoBehaviour
{
    public static AdsManagerNew instance;
    public int admobInterval;
    public int chartboostInterval;

//    public string adUnitId = "ca-app-pub-8460304617173164/5790176534";
//    private int admobSpinLeft;
//    private int chartBoostSpinLeft;
//    private bool isInit;
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

    }

    void Start()
    {
//        if (!isInit)
//        {
//            isInit = true;
////            Chartboost.setAutoCacheAds(true);
////            Chartboost.cacheInterstitial(CBLocation.Default);
////            Chartboost.cacheRewardedVideo(CBLocation.Default);
////       
//            admobSpinLeft = admobInterval;
//            chartBoostSpinLeft = chartboostInterval;
//        }

    }

    

//    public bool isChartboostVideoAvaliable()
//    {
////        return Chartboost.hasRewardedVideo(CBLocation.Default);
//    }

//    public void ShowRewardedVideo(Action<CBLocation,int> Finished, Action<CBLocation> Failed)
//    {
//         
////        if(Chartboost.hasRewardedVideo(CBLocation.Default))
////        {
////            Chartboost.showRewardedVideo(CBLocation.Default);
////            Chartboost.didCompleteRewardedVideo += Finished;
////            Chartboost.didDismissRewardedVideo += Failed;
////        }
//    }

//    private void Failed(CBLocation obj)
//    {
//        
//    }

    

    public void OneSpinned()
    {
//        if (admobSpinLeft > 0)
//            admobSpinLeft--;
//
//        if (chartBoostSpinLeft > 0)
//            chartBoostSpinLeft--;
//
//        if (chartBoostSpinLeft == 0)
//            ShowChartboost();


    }



    public void OnInterstialAdsFinished()
    {
//        admobSpinLeft = admobInterval;
    }

    

    void ShowChartboost()
    {
//        if (Chartboost.hasInterstitial(CBLocation.Default))
//        {
//            Chartboost.showInterstitial(CBLocation.Default);
//            chartBoostSpinLeft = chartboostInterval;
//        }
    }



    // Update is called once per frame
    void Update()
    {

    }
}
