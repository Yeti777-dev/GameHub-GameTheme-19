using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Splash : MonoBehaviour {

	public Image loadBar;
	public float loadTime = 5.0f;
	private float timer;
	
	void Start(){
	
//        if (GoogleAnalytics.instance)
//            GoogleAnalytics.instance.LogScreen("Splash");

		if(PlayerPrefs.GetInt("FirstPlay") == 0){
			Game.Reset();
        	PlayerPrefs.SetInt("FirstPlay",1);
        	Debug.Log("Resetting values, first time playing.");
        }


	
    }
    
  


}
