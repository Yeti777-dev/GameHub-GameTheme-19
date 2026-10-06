using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusWheelManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static BonusWheelManager Instance;

    public GameObject WheelGrpahics;
    public GameObject FreeGameShow;
    public TextMeshPro freeGameMesh;

    internal int FreeGamesWon;


    void Awake() { 
    
    Instance = this;
        WheelGrpahics.SetActive(true);
        FreeGameShow.SetActive(false);
    
    }


   public void HideWheelGraphics() { 
    
        WheelGrpahics.SetActive(false);

        freeGameMesh.text = FreeGamesWon.ToString();

        FreeGameShow.SetActive(true);
        SoundFxManager.instance.PlayFreeSpinBg();

        Invoke("GoToFreeSpins", 3f);
    
    }

    void GoToFreeSpins() {
    
    SlotManager.instance.IsFreeSpinsEnabled = true;
        GameOperations.instance.noOfFreeSpin = FreeGamesWon;
        BgManager.instance.SetFreespinBg();
        GameOperations.instance.ContinueToFreeSpins();

        Destroy(this.gameObject,0.1f);
    
    }








}
