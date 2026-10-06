using UnityEngine;
using System.Collections;


public class StoreScreen : MonoBehaviour
{
    public static StoreScreen instance;

    public GameObject coinPurchaseScreenPrefab;
    private GameObject coinPurchaseScreenClone;


    void Awake()
    {
        instance = this;


    }

    void Start()
    {
        iTween.Defaults.easeType = iTween.EaseType.easeOutExpo;

        if (Application.loadedLevelName == "Main Menu")
        {
            transform.position = new Vector3(0, -0.2f, -3);
        }
        else
        {
            transform.position = new Vector3(50f, -0.2f, -3);
        }

        iTween.MoveFrom(gameObject, transform.position + new Vector3(0, 7, 0), 1);
    }

    void BuyCoinPack1Clicked()
    {
        SoundFxManager.instance.buttonTapSound.Play();

//        InAppController.instance.PurchaseProduct(InAppController.coinPack5IAPIdentifier);
    }

    void BuyCoinPack2Clicked()
    {
//        InAppController.instance.PurchaseProduct(InAppController.coinPack4IAPIdentifier);
        SoundFxManager.instance.buttonTapSound.Play();
    }

    void BuyCoinPack3Clicked()
    {
//        InAppController.instance.PurchaseProduct(InAppController.coinPack3IAPIdentifier);
        SoundFxManager.instance.buttonTapSound.Play();
    }

    void BuyCoinPack4Clicked()
    {
//        InAppController.instance.PurchaseProduct(InAppController.coinPack2IAPIdentifier);
        SoundFxManager.instance.buttonTapSound.Play();
    }

    void BuyCoinPack5Clicked()
    {
//        InAppController.instance.PurchaseProduct(InAppController.coinPack1IAPIdentifier);
        SoundFxManager.instance.buttonTapSound.Play();
    }

    void CloseButtonClicked()
    {
        SoundFxManager.instance.buttonTapSound.Play();
        instance = null;
        iTween.Defaults.easeType = iTween.EaseType.easeOutExpo;
        iTween.MoveTo(gameObject, transform.position + new Vector3(0, 7, 0), 1);
        Destroy(gameObject, 1);
    }

}
