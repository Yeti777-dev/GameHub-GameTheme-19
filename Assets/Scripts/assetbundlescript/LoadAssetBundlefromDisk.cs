using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadAssetBundlefromDisk : MonoBehaviour
{
    private GameObject assetObj;
    public GameObject LoadingPanel;

    void Awake()
    {

        LoadingPanel.SetActive(true);
    }
    void Start()
    {
#if UNITY_EDITOR
        StartCoroutine(LoadObject(GamePlaySocketManager.AssetPath));

#elif UNITY_ANDROID

        StartCoroutine(LoadObject(GamePlaySocketManager.AssetPath));
#endif




    }


    IEnumerator LoadObject(string path)
    {
        AssetBundleCreateRequest bundle = AssetBundle.LoadFromFileAsync(path);
        yield return bundle;

        AssetBundle myLoadedAssetBundle = bundle.assetBundle;
        if (myLoadedAssetBundle == null)
        {
            DisplayErrorOnScreen.Instance.DisplayError("Asset Bundle failed!", "No asset bundle found by that name");

            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }



        assetObj = myLoadedAssetBundle.LoadAsset<GameObject>(GamePlaySocketManager.SceneName);
        print("Loading asset");
        yield return assetObj;
        GameObject dataclone = GameObject.Instantiate(assetObj);
        Debug.Log("Asset Bundle Instantitated successfully");

        yield return dataclone;
        LoadingPanel.SetActive(false);
        GUIManager.instance.SetGuiButtonState(false);
        yield return new WaitForSeconds(2f);
        GUIManager.instance.SetGuiButtonState(true);


        myLoadedAssetBundle.Unload(false);
    }
}

