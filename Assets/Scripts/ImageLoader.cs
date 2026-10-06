using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System;

public class ImageLoader : MonoBehaviour
{
    Texture2D myTexture;
    byte[] textureBytes;
    // Start is called before the first frame update

    //public string[] filenames;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public string GetGameAssetsVersion(string url)
    {
        var splits = url.Split('/');
        return splits[splits.Length - 2];
    }

    public IEnumerator LoadImage(string imageUrl, Image img,string sceneName)
    {

        var version = GetGameAssetsVersion(imageUrl);
        var path = Path.Combine(Application.persistentDataPath, "IconData", sceneName, version);
        path = Path.Combine(path, sceneName + ".png");

        if (File.Exists(path))
        {
            LoadIconsFromDisk(path,img);
            StopCoroutine(LoadImage(imageUrl, img, sceneName));
        }
        else
        {
           

        using    UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
               {
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogError("Error while loading image");
                }
                else
                {
                    if (img.sprite)
                    {

                        DestroyImmediate(img.sprite);
                    }
                    myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                    Sprite imageSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));

                    img.sprite = imageSprite;

                    byte[] textureBytes = myTexture.EncodeToPNG();
                    if (!Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                    }

                    File.WriteAllBytes(path, textureBytes);
                    request.downloadHandler.Dispose();
                    request.Dispose();
                    Array.Clear(textureBytes, 0, textureBytes.Length - 1);
                    Debug.Log("Icon Saved On Disk!");

                }
            }
            yield return null;
            StopCoroutine(LoadImage(imageUrl, img, sceneName));
        }



    }

    public void LoadIconsFromDisk(string fileName, Image img)
    {
        //if (img.sprite)
        //{

        //    Destroy(img.sprite);
        //}
        textureBytes = File.ReadAllBytes(fileName);
        myTexture = new Texture2D(0, 0);

        myTexture.LoadImage(textureBytes);
        Sprite imageSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
        img.sprite = imageSprite;

        //Debug.Log("Image successfully loaded from disk");

       

    }

    private void OnDestroy()
    {
        Destroy(myTexture);
        if (textureBytes != null && textureBytes.Length > 0) {
            Array.Clear(textureBytes, 0, textureBytes.Length - 1);
        }
        
    }
}
