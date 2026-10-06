using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinForhunt : MonoBehaviour
{
    public bool isHuntSpinning;
    private Transform HuntItemTransform;
    public bool IsSpecialItem = false;
    private HuntColumnScript HuntColumn;
    public bool IsSpin = false;
    public int HuntIndexInColumn;
    private float initialYaxis;
    int ThisItemType;
    private Vector3 InitialScale;

    // Start is called before the first frame update
    void Awake()
    {
        InitialScale = transform.localScale;

        HuntColumn = transform.parent.GetComponent<HuntColumnScript>();
        HuntItemTransform = transform;
        isHuntSpinning = true;
        IsSpecialItem = false;
        initialYaxis = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsSpin)
            return;

     
        if (HuntColumn.IsHuntColumnSpinning) {
            HuntItemTransform.position -= new Vector3(0, huntSpecialSpinManager.instance.huntSpinSpeed * Time.deltaTime, 0);
            if (HuntItemTransform.position.y < -2.5f)
            {
                HuntColumn.MoveLastItemUp();
                HuntItemTransform.position = new Vector3(HuntItemTransform.position.x, 2f, HuntItemTransform.position.z);
                ChangeItemType();
            }
        }else if (!HuntColumn.IsHuntColumnSpinning)
        {
            
            iTween.Defaults.easeType = iTween.EaseType.linear;
            switch (HuntIndexInColumn)
            {
              
                case 0:
                     iTween.MoveTo(gameObject, new Vector3(transform.position.x, HuntColumn.initialYaxisArray[0], -3f), 0.1f);

                    break;
                case 1:
                   iTween.MoveTo(gameObject, new Vector3(transform.position.x, HuntColumn.initialYaxisArray[1], -3f), 0.1f);

                    break;
                case 2:
                    iTween.MoveTo(gameObject, new Vector3(transform.position.x, HuntColumn.initialYaxisArray[2], -3f), 0.1f);

                    break;
                case 3:
                    iTween.MoveTo(gameObject, new Vector3(transform.position.x, HuntColumn.initialYaxisArray[3], -3f), 0.1f);

                    break;
                case 4:
                     iTween.MoveTo(gameObject, new Vector3(transform.position.x, HuntColumn.initialYaxisArray[4], -3f), 0.1f);

                    break;
                case 5:
                     iTween.MoveTo(gameObject, new Vector3(transform.position.x, HuntColumn.initialYaxisArray[5], -3f), 0.1f);

                    break;

                   
            }

            if (ThisItemType == 8 && !transform.parent.GetComponent<HuntColumnScript>().CheckForFillIndex(HuntIndexInColumn))
            {
             
                IsSpecialItem = true;
               StartCoroutine(transform.parent.GetComponent<HuntColumnScript>().CreateHuntedSlot(HuntIndexInColumn));
                transform.parent.GetComponent<HuntColumnScript>().FillTheIndex(HuntIndexInColumn);
               
                huntSpecialSpinManager.instance.HuntCounter++;
                huntSpecialSpinManager.instance.UpdateHuntGUI();
           ;

                Vector3 newscale = new Vector3(0.5f, 0.5f, 1);
                gameObject.transform.localScale = newscale;
                iTween.Defaults.easeType = iTween.EaseType.linear;
                iTween.PunchScale(gameObject, InitialScale, 1f);
                newscale = new Vector3(0.468f, 0.468f, 1f);
                gameObject.transform.localScale = newscale;
                iTween.ScaleFrom(gameObject, newscale, 1f);

            }
            IsSpin = false;
           
        }
    }
    internal void ChangeItemType() {
        ThisItemType = Random.Range(0, huntSpecialSpinManager.instance.slotItemsHunt.Length);
        GetComponent<SpriteRenderer>().sprite = huntSpecialSpinManager.instance.slotItemsHunt[ThisItemType];
       

    }
}
