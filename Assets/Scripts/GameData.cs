using gm.api.domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{

    public string GameType;
    public string Name;

    public string GameId;

    public string SceneName;

    public string logoUrl;

    public string AssetUrl;

    public bool IscomingSoon = false;

    public bool isNewGame = false;

    public int GameOrderNumber = 0;

 
    public int Rtp;

    public string AssetBundle_Path;
    public GameObject ComingSoonBanner, NewGameBanner;
    public int progress_Download;

    public Button DownloadButton;
    public Text downloadText;
    public Image GameLogo;
    public GameObject DownloadProgress;
    public Image ProgressFillImage;
  public  Color logocolor = new Color();
    void Awake()
    {
        downloadText.gameObject.SetActive(false);
        ComingSoonBanner.SetActive(false);

        NewGameBanner.SetActive(false);





    }

    public string GetGameAssetsVersion() {
        var splits = this.AssetUrl.Split('/');
        return splits[splits.Length-2];
    }

    public void GetmyPath() {
        if (!IscomingSoon)
        {
            
                NewGameBanner.SetActive(isNewGame);


            var verison = GetGameAssetsVersion();
            AssetBundle_Path = Path.Combine(Application.persistentDataPath, "AssetData", SceneName, verison);
            AssetBundle_Path = Path.Combine(AssetBundle_Path, SceneName + ".unity3d");
            Debug.Log("Asset path is " + AssetBundle_Path);

            if (File.Exists(AssetBundle_Path))
            {
                gameObject.GetComponent<Button>().interactable = true;
                DownloadButton.gameObject.SetActive(false);
                DownloadProgress.gameObject.SetActive(false);

                logocolor.a = 1;
                GameLogo.color = logocolor;
            }
            else
            {

                gameObject.GetComponent<Button>().interactable = false;
                DownloadButton.gameObject.SetActive(true);
                logocolor.a = 0.7f;
                GameLogo.color = logocolor;
            }
        }
        else {

            DownloadButton.gameObject.SetActive(false);
            DownloadProgress.gameObject.SetActive(false);
            ComingSoonBanner.SetActive(true);

        }
    }
    public void OnDownloadGameButton()
    {

        StartCoroutine(downloadAsset());
    }

    IEnumerator downloadAsset()
    {
        DownloadButton.interactable = false;
        //  AssetUrl = "Fortune Casino/bundle/2023/2/28/638131770023456713/fortuneslot";
        DownloadProgress.gameObject.SetActive(true);

        downloadText.gameObject.SetActive(true);
        UnityWebRequest www = UnityWebRequest.Get(AssetUrl);
        DownloadHandler handle = www.downloadHandler;
        www.SendWebRequest();
        while (!www.isDone)
        {
            ProgressFillImage.fillAmount = www.downloadProgress;
            progress_Download = (int)(www.downloadProgress * 100);
            downloadText.text = progress_Download.ToString() + " %";
            print("Downloading game progress is " + www.downloadProgress.ToString());


            yield return null;
        }
        //Send Request and wait
          yield return www.isDone;

        if (www.isNetworkError)
        {

            Debug.Log("Error while Downloading Data: " + www.error);
        }
        else
        {
            Debug.Log("Success");

            //handle.data

            //Construct path to save it


            //Save
            DownloadButton.gameObject.SetActive(false);
            downloadText.gameObject.SetActive(false);
            DownloadProgress.gameObject.SetActive(false);

            gameObject.GetComponent<Button>().interactable = true;
            logocolor.a = 1;
            GameLogo.color = logocolor;
            save(handle.data, AssetBundle_Path);
        }
    }

    void save(byte[] data, string path)
    {
        //Create the Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try
        {
            File.WriteAllBytes(path, data);
            Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }
}
