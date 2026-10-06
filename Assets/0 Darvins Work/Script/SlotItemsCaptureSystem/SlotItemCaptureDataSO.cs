using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlotItemCaptureDataSO", menuName = "Darvin/SlotItemCaptureDataSO")]
public class SlotItemCaptureDataSO : ScriptableObject
{
    public string RootFileAddress = "D:/Work/Mohit/Game Hub/Unity/GameHubSlots-Unity/Assets/Game Data/All Graphics/ReplaceGraphics/Testing";
    public string RootSlotItemsAddress = "D:/Work/Mohit/Game Hub/Unity/GameHubSlots-Unity/Assets/Game Data/All Graphics/ReplaceGraphics/slot items normal";
    public Sprite[] ItemBackgroundFrames;
    public List<SlotItemCaptureData> slotItemCaptureDatas;
}
[System.Serializable]
public class SlotItemCaptureData
{
    public string itemFileName = "00_";
    public string slotItemsName = "item";
    public Sprite[] frames;
    public bool isItemActive = true;
    public string[] frameNames =
    {
        "00000",
        "00003",
        "00006",
        "00009",
        "00012",
        "00015",
        "00018",
        "00021",
        "00024",
        "00027"
    };
    public string GetFileNamebyIndex(int index)
    {
        return itemFileName + frameNames[index];
    }
}