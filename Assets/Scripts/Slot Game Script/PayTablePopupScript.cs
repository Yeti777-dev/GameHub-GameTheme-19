using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class PayTablePopupScript : MonoBehaviour
{

    // Use this for initialization

    public Text platformsettingInfo;
    public TextMeshProUGUI[] itemsText;
  private  Vector2[] currentArray;
    public bool Show2Columns = true;

    private void Awake()
    {
        setcurrentArray(0);
      StartCoroutine(UpdatePayouts());
    }
    void Start()
    {
        if (SystemInfo.operatingSystem.Contains("Windows") || SystemInfo.operatingSystem.Contains("Mac"))
        {
            platformsettingInfo.text = "Settings Set for PC user";
        }
        else
        {
            platformsettingInfo.text = "Settings Set for Mobile user";
        }
    }
    IEnumerator UpdatePayouts() {
      
        for (int j = 0; j < itemsText.Length; j++)
        {
            itemsText[j].text = "";

            setcurrentArray(j);
            for (int i = 0; i < 3; i++)
            {
                if (Show2Columns)
                    itemsText[j].text += "" + currentArray[i].x + "x    " + currentArray[i].y + "\n";
                else
                {

                    itemsText[j].text += currentArray[i].y + "\n";

                }

            }
         
        
            yield return new WaitForEndOfFrame();

        }
        StopCoroutine(UpdatePayouts());
    }
    void setcurrentArray(int itemIndex) {

      



        switch (itemIndex)
        {
            case 0:
                currentArray = PayTable.instance.item1;
                break;
            case 1:
                currentArray = PayTable.instance.item2;
                break;
            case 2:
                currentArray = PayTable.instance.item3;
                break;
            case 3:
                currentArray = PayTable.instance.item4;
                break;
            case 4:
                currentArray = PayTable.instance.item5;
                break;
            case 5:
                currentArray = PayTable.instance.item6;
                break;
            case 6:
                currentArray = PayTable.instance.item7;
                break;
            case 7:
                currentArray = PayTable.instance.item8;
                break;
            case 8:
                currentArray = PayTable.instance.item9;
                break;
            case 9:
                currentArray = PayTable.instance.WildInSequanceAmount;
                break;
            case 10:
                currentArray = PayTable.instance.autoSpinActive;
                break;

        }


    }

    public void CloseButtonClicked()
    {
   
        SoundFxManager.instance.buttonTapSound.Play();
     
        //FaderScript.instance.BringItBack();
        GUIManager.instance.SetGuiButtonState(true);
        OnSlotItemClicked.CanShowSlotInfo = true;
        Destroy(gameObject,0.1f);
    }

  

 

   

}
