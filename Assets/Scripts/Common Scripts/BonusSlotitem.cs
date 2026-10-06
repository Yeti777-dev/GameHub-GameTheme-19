using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusSlotitem : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshPro bonusMulMesh;
    private float[] rewardArray = { 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.2f, 1.3f, 1.5f };
    internal float reward;
    private SlotItem bonusitem;
    void Start()
    {
        bonusitem = GetComponent<SlotItem>();
        randomReward();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (bonusMulMesh)
        {

            if (bonusitem.itemType == SlotItemType.Bonus && bonusitem.indexInColumn < LineManager.instance.Items_In_Column)
            {
              
                bonusMulMesh.gameObject.SetActive(true);
            }
            else
            {
                bonusMulMesh.gameObject.SetActive(false);
            }
        }
    }

    void randomReward() {
        reward = rewardArray[Random.RandomRange(0, rewardArray.Length)];
        reward *= SlotManager.instance.totalBetAmount;
        bonusMulMesh.text = reward.ToString("F1");

    }
    public void BonusItemPosandReward() {

        reward = rewardArray[Random.RandomRange(0, rewardArray.Length)];
        reward *= SlotManager.instance.totalBetAmount;
        bonusMulMesh.text = reward.ToString("F1");

        Vector2 colrowpos = new Vector2(bonusitem.ParentColumn.columnIndex, bonusitem.indexInColumn);
        UltimateBonusRecorder.Instance.bonusPos.Add(colrowpos);
        UltimateBonusRecorder.Instance.reward.Add(reward);

    }

}
