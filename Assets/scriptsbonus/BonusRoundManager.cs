using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;
public class BonusRoundManager : MonoBehaviour
{
    public OnClickBonusBox[] Boxes;
    public GameObject BonusBarParent,OptionBoxesParent,  BonusLossPanel,BonusWinPanel;

    public static BonusRoundManager instance;
    public float CurrentWinRewards;
    public List<string> TakenAlphabets = new List<string>();
    public List<string> DynamicAlphaList = new List<string> { "Item1", "Item2", "Item3", "Item4", "Item5" };
    public GameObject[] CorrectedAlphabets;
    public GameObject hightlightPrefab;
    private GameObject highlightclone;
    public GameObject WinParticles, LossCross;
    public OnClickBonusBox TobeRemoved = null;
    public TextMeshPro alphainfo;
    public GameObject FullBonusWinPanel;
    public float CurrentBonusWinnings =0;
    internal GameObject PickedAlphaClone;
    public TextMeshPro CurrentWinningsTF;
    public TextMeshPro shufflingtext;
    public int currentRound;
    public TextMeshPro MaxWin;
    public GameObject BonusInfo;
    public bool sendOnce = false;
    private void Awake()
    {
        BlockBoxesfromClick();

    }
    private void Start()
    {
        LineManager.instance.ShowEabledLine(false);

    
        instance = this;
        MaxWin.text = "" + PayTable.instance.GiveMeMaxBonusWin().ToString();
        BonusBarParent.SetActive(true);
        OptionBoxesParent.SetActive(true);
        BonusLossPanel.SetActive(false);
        BonusWinPanel.SetActive(false);
        CurrentWinningsTF.text = "" + CurrentBonusWinnings.ToString();
    }
    public void HideInfo()
    {
        BonusInfo.SetActive(false);
        ShuffuleList();
    }

    public void ShuffuleList() {
        BlockBoxesfromClick();
       
        for (int i = 0; i < DynamicAlphaList.Count; i++)
        {
            string temp = DynamicAlphaList[i];
            int randomIndex = Random.Range(i, DynamicAlphaList.Count);
            DynamicAlphaList[i] = DynamicAlphaList[randomIndex];
            DynamicAlphaList[randomIndex] = temp;

        }
        for (int i = 0; i < Boxes.Length; i++) {
            Boxes[i].BoxAlphabet = DynamicAlphaList[i];

        }
        StartCoroutine(ChangeBoxePos());
    }


    IEnumerator ChangeBoxePos() {
   
        for (int j = 0; j < 5; j++)
        {
            if (!SoundFxManager.instance.Shuffling.isPlaying)
                SoundFxManager.instance.Shuffling.Play();
            for (int i = 0; i < Boxes.Length; i++)
            {

                Vector3 tempPos = Boxes[i].transform.position;

                int randomIndex = Random.Range(i, Boxes.Length);
                Vector3 randomboxpos = Boxes[randomIndex].gameObject.transform.position;
                shufflingtext.text = "Shuffling ...";

                //   Boxes[i].transform.position = randomboxpos;
                //  Boxes[randomIndex].transform.position = tempPos;

                iTween.Defaults.easeType = iTween.EaseType.linear;
                iTween.MoveTo(Boxes[i].gameObject, randomboxpos, 0.05f);

                iTween.Defaults.easeType = iTween.EaseType.linear;
                iTween.MoveTo(Boxes[randomIndex].gameObject, tempPos, 0.05f);

                yield return new WaitForSeconds(0.1f);
              

            }
        }

        shufflingtext.text = "Ready";
        SoundFxManager.instance.Shuffling.Stop();
        yield return new WaitForSeconds(1f);
      
        UnblockBoxesToClick();
       
        shufflingtext.text = "";
        StopCoroutine(ChangeBoxePos());
    }
    public void MarkCorrectAlphabet(string Alpha) {
        switch (Alpha) {
            case "Item1":
                CorrectedAlphabets[0].SetActive(true);
                break;
            case "Item2":
                CorrectedAlphabets[1].SetActive(true);
                break;
            case "Item3":
                CorrectedAlphabets[2].SetActive(true);
                break;
            case "Item4":
                CorrectedAlphabets[3].SetActive(true);
                break;
            case "Item5":
                CorrectedAlphabets[4].SetActive(true);
                break;


        }

    }

    public void BlockBoxesfromClick() {
        foreach (OnClickBonusBox box in Boxes)
        {
            box.Clickable = false;
        }
    }
    public void UnblockBoxesToClick()
    {
        foreach (OnClickBonusBox box in Boxes)
        {
            box.Clickable = true;
            
        }
    }

    public void LightupCorrectedAlphabet(int num) {

        CorrectedAlphabets[num].SetActive(true);

    }


    public void IncorrectBoxSelected() {
        BonusLossPanel.SetActive(true);
        

    }
    public void correctBoxSelected()
    {
      
        CurrentBonusWinnings += PayTable.instance.GivemeBonusRoundWin(currentRound);
        currentRound++;
        if (TakenAlphabets.Count < DynamicAlphaList.Count)
            BonusWinPanel.SetActive(true);
        else
        {
            FullBonusWinPanel.SetActive(true);
        }
        CurrentWinningsTF.text = "" + CurrentBonusWinnings.ToString();
    }


    public void ContinueToNextRound() {
        foreach (OnClickBonusBox box in Boxes)
        {
            box.GetComponent<Animator>().SetBool("blast", false);
         Color spritealpha =    box.GetComponent<SpriteRenderer>().color;
            spritealpha.a = 1f;
            box.GetComponent<SpriteRenderer>().color = spritealpha;
        }

        // OptionBoxesList.Sort();
        Invoke("ShuffuleList", 0.5f);
       // ShuffuleList();
        BonusWinPanel.SetActive(false);

        //foreach (OnClickBonusBox box in OptionBoxesList) {
        //    box.UpdateType();
        //}
        if (PickedAlphaClone != null) {
            Destroy(PickedAlphaClone);
        }
      
    }

   
    public void QuitBonus() {
        GUIManager.instance.AddBonusWinToTotalCredit(CurrentBonusWinnings);
        GUIManager.instance.UpdateGUI();
        Destroy(gameObject, 0.2f);
        GameEffects.instance.CelebrationEnds();
    }

    public void CollectBonus() {
       GUIManager.instance.AddBonusWinToTotalCredit(CurrentBonusWinnings);
        GUIManager.instance.UpdateGUI();
        GameEffects.instance.CelebrationEnds();

        Destroy(gameObject, 0.1f);

    }

  
 

}
