using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSpecialItemClicked : MonoBehaviour
{
    public GameObject SpecialInfoPrefab;


    private void OnMouseUp()
    {
      //  if (!OnSlotItemClicked.CanShowSlotInfo || SlotManager.instance.IsFreeSpinsEnabled)
            return;


        if (!OnSlotItemClicked.ItemClicked )
        {

          GameObject infoClone = (GameObject)  Instantiate(SpecialInfoPrefab, SpecialInfoPrefab.transform.position, Quaternion.identity);
            OnSlotItemClicked.ItemClicked = true;
            LineItem.Instance.InfoObj = infoClone;

        }
        else
        {
            Destroy(GameObject.FindGameObjectWithTag("info"));
            Invoke("Delay", 0.2f);
        }
    }

    void Delay()
    {
        OnSlotItemClicked.ItemClicked = false;


    }

}



