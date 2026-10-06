using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class WheelGame : MonoBehaviour {
    public int idx = 0;
    public float dist = 0f;
    public int counter = 20;

    public int bet = 0;
    int saveBet = 0;

    public Text betLabel;

    public GameObject historyGo;

    public Transform indicatorTr;

    public AudioSource spinSound, stopSound;

    Image[] historyItems;

    bool isInput = true;

    bool isDouble = true;
    bool isGamblingShowing = false;

    int choice = -1;

    string[] sprites = new string[4] { "spade", "heart", "club", "diamond" };
    public Sprite[] icons;

  public GameObject spinButton;
  public GameObject gamblingCanvas;

	void Start () {
        HistoryItem[] tlist = historyGo.GetComponentsInChildren<HistoryItem>();
        historyItems = new Image[tlist.Length];
        for (int i = 0; i < tlist.Length; i++)
            historyItems[i] = tlist[i].GetComponent<Image>();
        InitHistory();
	}

    public void InitBet(int val)
    {
        saveBet = val;
        bet = val;
        betLabel.text = bet.ToString("#,##0");
        isInput = true;
    }

    void InitHistory()
    {
        for (int i = 0; i < historyItems.Length; i++)
        {
            Image item = historyItems[i];
            Transform tr = item.transform;
            //tr.localPosition = new Vector3(-245f + 70 * i, 0f, 0f);
            RectTransform rt = item.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(-245f + 70 * i, 0f);
        }
    }

    void NextHistory()
    {
        string str = sprites[idx % 4];
        Debug.Log("NextHistory : " + str);
        List<Image> tlist = new List<Image>();
        tlist.Add(historyItems[historyItems.Length-1]);
        for (int i = 0; i < historyItems.Length-1; i++)
        {
            tlist.Add(historyItems[i]);
        }
        historyItems = tlist.ToArray();
        historyItems[1].sprite = icons[idx % 4];
        for (int i = 0; i < historyItems.Length; i++)
        {
            Image item = historyItems[i];
            Transform tr = item.transform;
            tr.localPosition = new Vector3(-245f + 70 * (i-1), 0f, 0f);
            TweenParms parms = new TweenParms().Prop("localPosition", new Vector3(-245f + 70 * i, 0f, 0f)).Ease(EaseType.Linear);
            HOTween.To(tr, 0.2f, parms);
        }
    }
	
    public void Spin()
    {
        if (!isInput) return;
        isInput = false;
        idx = 0;
        MoveTo(0f);
        counter = 16 + Random.Range(0, 4) % 4;
        Roll(true);
        Debug.Log("Spin");
        spinSound.Play();
    }

    void OnUpdateBet()
    {
        betLabel.text = bet.ToString("#,##0");
    }

    void TweenBet(int val)
    {
        TweenParms parms = new TweenParms().Prop("bet", val).Ease(EaseType.Linear).OnUpdate(OnUpdateBet);
        HOTween.To(this, 0.5f, parms);
    }


    void MoveTo(float val)
    {
        transform.localRotation = Quaternion.AngleAxis(val * -1f, Vector3.forward);
    }

    void OnUpdateTweenMove()
    {
        MoveTo(idx * 30f - 30f + dist * 30f / 100f);
        indicatorTr.localRotation = Quaternion.AngleAxis(Mathf.Sin(dist/16f) * 30f, Vector3.forward);
    }

    void OnDoneTweenMove()
    {
        MoveTo(idx * 30f);
        Roll(true);
    }

    void TweenMove()
    {
        dist = 0f;
        HOTween.Complete(name);
        TweenParms parms = new TweenParms().Prop("dist", 100f).Ease(EaseType.Linear).Id(name).OnUpdate(OnUpdateTweenMove).OnComplete(OnDoneTweenMove);
        HOTween.To(this, 0.1f, parms);
    }

    void OnUpdateTweenMoveBack()
    {
        MoveTo(idx * 30f + dist * 30f / 100f);
        indicatorTr.localRotation = Quaternion.AngleAxis(Mathf.Sin(dist/16f) * 30f, Vector3.forward);
    }

    void TweenMoveBack()
    {
        dist = 0f;
        SequenceParms sparams = new SequenceParms();
        Sequence mySequence = new Sequence(sparams);
        TweenParms parms;
        parms = new TweenParms().Prop("dist", 30f).Ease(EaseType.Linear).OnUpdate(OnUpdateTweenMoveBack);
        mySequence.Append(HOTween.To(this, 0.1f, parms));
        parms = new TweenParms().Prop("dist", 0f).Ease(EaseType.Linear).OnUpdate(OnUpdateTweenMoveBack);
        mySequence.Append(HOTween.To(this, 0.1f, parms));
        mySequence.Play();
    }

    public void Roll(bool isLinear)
    {
        if (counter < 1)
        {
            DoneRoll();
            return;
        }
        counter--;
        idx = (idx + 1) % 12;
        TweenMove();
    }

    void Failure()
    {
        //isInput = false;
        bet = 0;
        TweenBet(0);
        if(gameObject.activeInHierarchy)
        StartCoroutine(DelayAction(0.8f, () =>
        {
        }));
    }

    public void Correct()
    {
    SlotManager.instance.currentSpinWinningAmount = bet;
    Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
    GUIManager.instance.UpdateWiningAmount();
    PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
    PlayerPrefs.Save();
    GUIManager.instance.UpdateGUI();
        Debug.Log("Correct Fun");
        if(transform.parent.gameObject.activeInHierarchy)
    spinButton.SetActive(true);
    //gamblingCanvas.SetActive(false);

    InitBet(0);
    
        //isInput = false; 
        /*
        InitBet(SlotManager.instance.currentSpinWinningAmount);
        StartCoroutine(DelayAction(0.8f, () =>
        {
        })); */
    }

    void DoneRoll()
    {
        TweenMoveBack();
        NextHistory();
        spinSound.Stop();
        stopSound.Play();

        if (isDouble)
        {
            if (idx % 2 == choice) TweenBet(bet * 2);
            else Failure();
        }
        else
        {
            if (idx % 4 == choice) TweenBet(bet * 4);
            else Failure();
        }
        
        isInput = true;

    }

    public void OnClickSpade(){
        if (!isInput) return;
        isDouble = false;
        choice = 0;
        Spin();
    }

    public void OnClickHeart(){
        if (!isInput) return;
        isDouble = false;
        choice = 1;
        Spin();
    }

    public void OnClickClub(){
        if (!isInput) return;
        isDouble = false;
        choice = 2;
        Spin();
    }

    public void OnClickDiamond(){
        if (!isInput) return;
        isDouble = false;
        choice = 3;
        Spin();
    }

    public void OnClickRed()
    {
        if (!isInput) return;
        isDouble = true;
        choice = 1;
        Spin();
    }

    public void OnClickBlack()
    {
        if (!isInput) return;
        isDouble = true;
        choice = 0;
        Spin();
    }

    void Update()
    {
      //OnUpdateBet();
      if (bet == 0) {
       Invoke("Correct", 2.0f); // When the bet equals zero this causes the gambling to end
    }
    }

    IEnumerator DelayAction(float dTime, System.Action callback)
    {
        yield return new WaitForSeconds(dTime);
        callback();
    }
}
