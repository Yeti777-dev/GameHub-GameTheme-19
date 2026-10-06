using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class CoinsUpdate : MonoBehaviour {
    public TextMesh texMesh;
    public float coins;
    // Use this for initialization
    void Start () {
	
	}

    void OnEnable()
    {

       //JackPotPopupScriptSlot.OnCoinsTextUpdate += CoinsLerb;
        NotifactionCenter.OnNotifactionTextUpdate += CoinsLerb;
        coins = Game.noOfCoinsLeft;
        OnUpdateBet();
    }

    void OnDisable()
    {
       // JackPotPopupScriptSlot.OnCoinsTextUpdate -= CoinsLerb;
        NotifactionCenter.OnNotifactionTextUpdate -= CoinsLerb;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void CoinsLerb(float traget)
    {
        TweenBet(traget);
    }

    void TweenBet(float val)
    {
        coins = Game.noOfCoinsLeft;
        TweenParms parms = new TweenParms().Prop("coins", val).Ease(EaseType.Linear).OnUpdate(OnUpdateBet);
        HOTween.To(this, 0.5f, parms);
        Game.noOfCoinsLeft = val;

    }

    void OnUpdateBet()
    {
        texMesh.text = coins.ToString("#,##0");
        
    }
}
