using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemCaptureSystem : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private SlotItemCaptureDataSO slotItemCaptureDataSO;
    [SerializeField] private int slotItemIndexToCapture = 0;
    [Header("References")]
    // [SerializeField] private Image[] itemBgImage;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image itemBG;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // for (int i = 0; i < itemBgImage.Length; i++)
        // {
        //     itemBgImage[i].sprite = slotItemCaptureDataSO.ItemBackgroundFrames[i];
        // }
    }
    [ContextMenu("Capture Slot Item By Index")]
    private void CaptureByIndex()
    {
        if (slotItemIndexToCapture >= slotItemCaptureDataSO.slotItemCaptureDatas.Count)
        {
            Debug.LogError("Please Give valid Index to capture OR -1 to capture all slot item");
            return;
        }
        if (slotItemIndexToCapture == -1)
        {
            foreach (SlotItemCaptureData targetItem in slotItemCaptureDataSO.slotItemCaptureDatas)
            {
                if (targetItem.isItemActive)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        itemIcon.sprite = targetItem.frames[i];
                        itemBG.sprite = slotItemCaptureDataSO.ItemBackgroundFrames[i];

                        if (!Directory.Exists(slotItemCaptureDataSO.RootFileAddress))
                        {
                            Directory.CreateDirectory(slotItemCaptureDataSO.RootFileAddress);
                        }

                        string fullPath = Path.Combine(slotItemCaptureDataSO.RootFileAddress, targetItem.GetFileNamebyIndex(i) + ".png");
                        // Capture from camera
                        Texture2D screenshot = CaptureScreenshot();

                        // Save to file
                        byte[] pngData = screenshot.EncodeToPNG();
                        File.WriteAllBytes(fullPath, pngData);


                        if (i == 0)
                        {
                            if (!Directory.Exists(slotItemCaptureDataSO.RootSlotItemsAddress))
                            {
                                Directory.CreateDirectory(slotItemCaptureDataSO.RootSlotItemsAddress);
                            }
                            string fullPathOne = Path.Combine(slotItemCaptureDataSO.RootSlotItemsAddress, targetItem.slotItemsName + ".png");
                            // Capture from camera
                            Texture2D screenshotOne = CaptureScreenshot();

                            // Save to file
                            byte[] pngDataOne = screenshotOne.EncodeToPNG();
                            File.WriteAllBytes(fullPathOne, pngDataOne);
                        }
                    }
                }
            }
        }
        else
        {
            SlotItemCaptureData targetItem = slotItemCaptureDataSO.slotItemCaptureDatas[slotItemIndexToCapture];

            for (int i = 0; i < 10; i++)
            {
                itemIcon.sprite = targetItem.frames[i];
                itemBG.sprite = slotItemCaptureDataSO.ItemBackgroundFrames[i];

                if (!Directory.Exists(slotItemCaptureDataSO.RootFileAddress))
                {
                    Directory.CreateDirectory(slotItemCaptureDataSO.RootFileAddress);
                }

                string fullPath = Path.Combine(slotItemCaptureDataSO.RootFileAddress, targetItem.GetFileNamebyIndex(i) + ".png");
                // Capture from camera
                Texture2D screenshot = CaptureScreenshot();

                // Save to file
                byte[] pngData = screenshot.EncodeToPNG();
                File.WriteAllBytes(fullPath, pngData);
            }
        }
        Debug.Log("[SlotItemCaptureSystem] Operation Done");
    }
    private Texture2D CaptureScreenshot()
    {
        int width = targetCamera.pixelWidth;
        int height = targetCamera.pixelHeight;

        // Create a temporary RenderTexture
        RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        renderTexture.Create();

        // Store original target
        RenderTexture originalTarget = targetCamera.targetTexture;

        // Render camera to RenderTexture
        targetCamera.targetTexture = renderTexture;
        targetCamera.Render();

        // Read pixels from RenderTexture
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGBA32, false);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        // Restore original settings
        RenderTexture.active = null;
        targetCamera.targetTexture = originalTarget;

        // Cleanup
        // Destroy(renderTexture);

        return screenshot;
    }



    [ContextMenu("Capture the Screen Only")]
    private void CaptureScreen()
    {
        string savePath = "Assets/Captures/";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        // Create filename
        string time = DateTime.Now.ToSafeString();
        time = time.Replace(':', '_');
        string fileName = $"ScreenShot {time}.png";
        string fullPath = Path.Combine(savePath, fileName);

        // Handle overwrite
        // if (!settings.overrideCaptures && File.Exists(fullPath))
        // {
        //     int counter = 1;
        //     string pathWithoutExtension = Path.Combine(savePath, Path.GetFileNameWithoutExtension(fileName));
        //     string extension = Path.GetExtension(fileName);

        //     while (File.Exists($"{pathWithoutExtension}_{counter}{extension}"))
        //     {
        //         counter++;
        //     }

        //     fullPath = $"{pathWithoutExtension}_{counter}{extension}";
        // }

        // Capture from camera
        Texture2D screenshot = CaptureScreenshot();

        // Save to file
        byte[] pngData = screenshot.EncodeToPNG();
        File.WriteAllBytes(fullPath, pngData);

        // Cleanup
        // Destroy(screenshot);

        Debug.Log($"Captured and saved: {fullPath}");
    }
}
