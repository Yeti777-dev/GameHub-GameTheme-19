using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OnSlotItemClicked : MonoBehaviour
{
    public GameObject InfoBox;
    private GameObject infoClone;
    public static bool ItemClicked;
    public GameObject mySprite;
    private GameObject slotclone;
    public Sprite[] slotsprites;
    public GameObject SimpleWildInfo,FreeSpinsInfo,BonusInfo;
    public static bool CanShowSlotInfo = true;

    private void Start()
    {
        ItemClicked = false;
    }
    private void OnMouseDown()
    {
       

    }

    
    void OnMouseUp()
    {
        //if (!CanShowSlotInfo || gameObject.GetComponent<SlotItem>().indexInColumn >(LineManager.instance.Items_In_Column-1) || SlotManager.instance.IsFreeSpinsEnabled)
        //    return;

        //int itemIndex = gameObject.GetComponent<SlotItem>().animationIndex;


      
        //if (!ItemClicked)
        //{
        //    if (itemIndex == 9) {
        //        infoClone = (GameObject)Instantiate(SimpleWildInfo, gameObject.transform.position + new Vector3(0, 0, -7f), Quaternion.identity);
        //        slotclone = (GameObject)Instantiate(mySprite, gameObject.transform.position + new Vector3(0, 0, -8.3f), Quaternion.identity, infoClone.transform);
        //        slotclone.GetComponent<SpriteRenderer>().sprite = slotsprites[itemIndex];
        //    }
        //    else {
        //        if (itemIndex == 10)
        //        {

        //            infoClone = (GameObject)Instantiate(FreeSpinsInfo, gameObject.transform.position + new Vector3(0, 0, -7f), Quaternion.identity);
        //            slotclone = (GameObject)Instantiate(mySprite, gameObject.transform.position + new Vector3(0, 0, -8.3f), Quaternion.identity, infoClone.transform);
        //            slotclone.GetComponent<SpriteRenderer>().sprite = slotsprites[itemIndex];
        //        }
        //        else {
        //            if (itemIndex == 11)
        //            {

        //                infoClone = (GameObject)Instantiate(BonusInfo, gameObject.transform.position + new Vector3(0, 0, -7f), Quaternion.identity);
        //                slotclone = (GameObject)Instantiate(mySprite, gameObject.transform.position + new Vector3(0, 0, -8.3f), Quaternion.identity, infoClone.transform);
        //                slotclone.GetComponent<SpriteRenderer>().sprite = slotsprites[itemIndex];
        //            }

        //            else {

        //                infoClone = (GameObject)Instantiate(InfoBox, gameObject.transform.position + new Vector3(0, 0, -7f), Quaternion.identity);
        //                slotclone = (GameObject)Instantiate(mySprite, gameObject.transform.position + new Vector3(0, 0, -8.3f), Quaternion.identity, infoClone.transform);
        //                slotclone.GetComponent<SpriteRenderer>().sprite = slotsprites[itemIndex];
        //                infoClone.transform.GetChild(0).GetComponent<TextMeshPro>().text = "" + PayTable.instance.CkeckInPayTable(itemIndex,5).ToString() +"\n" + PayTable.instance.CkeckInPayTable(itemIndex, 4).ToString() + "\n" + PayTable.instance.CkeckInPayTable(itemIndex, 3).ToString();


        //            }
        //        }
        //            }


      


        //    ItemClicked = true;
        //    LineItem.Instance.InfoObj = infoClone;
        //}
        //else {
        //    Destroy(GameObject.FindGameObjectWithTag("info"));
        //    ItemClicked = false;
        //  //  Invoke("Delay", 0.2f);
        //}
    }

    void Delay()
    {
        ItemClicked = false;


    }
}

