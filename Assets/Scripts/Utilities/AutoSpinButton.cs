using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpinButton : MonoBehaviour
{
  


    public MonoBehaviour scriptToCall;
    public string methodToInvoke;
    public int NumberOfAutoSpins;

    private void Start()
    {
    
    }
    private void OnMouseDown()
    {
       

    }
    void OnMouseUp()
    {
        //Chartboost.showInterstitial(CBLocation.HomeScreen);
        /*
        #if UNITY_ANDROID
                        string adUnitId = "ca-app-pub-8460304617173164/5790176534";
        #elif UNITY_IPHONE
                        string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
        #else
                string adUnitId = "unexpected_platform";
        #endif

                // Initialize an InterstitialAd.
                InterstitialAd interstitial = new InterstitialAd(adUnitId);
                // Create an empty ad request.
                AdRequest request = new AdRequest.Builder().Build();
                // Load the interstitial with the request.
                interstitial.LoadAd(request);

                if (interstitial.IsLoaded())
                {
                    interstitial.Show();
                }
                */
        if (UIManagerScript.isVideoPannelShowing)
            return;
        if (scriptToCall != null)
            scriptToCall.Invoke(methodToInvoke, .1f);

    }
}


