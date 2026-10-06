using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;
public class OnClickBonusBox : MonoBehaviour
{
    public GameObject textMesh;
    GameObject textobject;
    public bool Clickable =true;
    public string BoxAlphabet;
    public iTween.EaseType movetype;
    public static int rounds = 0;
    private void Start()
    {
        // Invoke("ShowType", 1f);
        movetype = iTween.EaseType.easeInCubic;
        iTween.Defaults.easeType = movetype;
        Clickable = false;
    }

   public void ShowType() {
         textobject = new GameObject("TextObj");
        textobject.transform.SetParent(transform.root.transform);

       textobject.AddComponent(typeof(TextMesh));
        textobject.GetComponent<TextMesh>().fontSize = 2;
         Vector3 pos = transform.position + new Vector3(-0.4f,0.6f,-6);
        textobject.transform.position = pos;
        textobject.layer = 9;
        textobject.GetComponent<TextMesh>().text = BoxAlphabet;
        
    }
    public void UpdateType() {
        textobject.GetComponent<TextMesh>().text = BoxAlphabet;
    }

    public void OnMouseDown()
    {
       
    }
    private void OnMouseEnter()
    {
    }
    private void OnMouseExit()
    {
      
    }

    public void OnMouseUp()
    {
        if (Clickable)
        {
            BonusRoundManager.instance.BlockBoxesfromClick();
            iTween.PunchRotation(gameObject, new Vector3(0f, 1600f, 0f), 2.5f);
            if (!BonusRoundManager.instance.sendOnce)
            {
                BonusRoundManager.instance.sendOnce = true;
            }
            StartCoroutine(ClickResult());
            SoundFxManager.instance.FlippingSound.Play();
          

        }
    }

    IEnumerator ClickResult() {
        if (rounds == 4 && !BonusRoundManager.instance.TakenAlphabets.Contains(BoxAlphabet)) {
            BoxAlphabet = BonusRoundManager.instance.TakenAlphabets[2];

        }
        yield return new WaitForSeconds(1f);
        // GetComponent<SpriteRenderer>().sprite = null;
        GetComponent<Animator>().SetBool("blast", true);

        yield return new WaitForSeconds(1.5f);
        if (!BonusRoundManager.instance.TakenAlphabets.Contains(BoxAlphabet))
        {
            Instantiate(BonusRoundManager.instance.WinParticles, gameObject.transform.position, Quaternion.identity, transform.root.transform);
            yield return new WaitForSeconds(2f);
            BonusRoundManager.instance.TakenAlphabets.Add(BoxAlphabet);
            BonusRoundManager.instance.correctBoxSelected();
            BonusRoundManager.instance.MarkCorrectAlphabet(BoxAlphabet);
            rounds++;
        }
     
        else
        {
            yield return new WaitForSeconds(1.5f);
            Instantiate(BonusRoundManager.instance.LossCross, gameObject.transform.position + new Vector3(0, 0, -1f), Quaternion.identity, transform.root.transform);
            yield return new WaitForSeconds(2f);
            BonusRoundManager.instance.IncorrectBoxSelected();
        }

    }

    public void ShowChosenAlpha() {
        GameObject textclone = Instantiate(textMesh, gameObject.transform.position + new Vector3(0,0,0.5f), Quaternion.identity) as GameObject;
        switch (BoxAlphabet) {
            case "Item1":
                textclone.GetComponent<TextMeshPro>().text = "B";
                break;
            case "Item2":
                textclone.GetComponent<TextMeshPro>().text = "O";
                break;
            case "Item3":
                textclone.GetComponent<TextMeshPro>().text = "N";
                break;
            case "Item4":
                textclone.GetComponent<TextMeshPro>().text = "U";
                break;
            case "Item5":
                textclone.GetComponent<TextMeshPro>().text = "S";
                break;

        }
        
        textclone.transform.SetParent(transform.root.transform);
        SoundFxManager.instance.AlphabetAppearSound.Play();
        BonusRoundManager.instance.PickedAlphaClone = textclone;
      

    }

}
