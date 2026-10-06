using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplayErrorOnScreen : MonoBehaviour
{
    public static DisplayErrorOnScreen Instance;
    public Text titleTF;
    public Text messageTF;
    public GameObject ErrorPanel;


    public GameObject NetworkPanel;
    public Text NetworktitleTF;
    public Text NetworkmessageTF;

    public GameObject RetryPanel;
    public Text RetrytitleTF;
    public Text RetrymessageTF;

    private bool Retrying_Connect = false;

    private int counter = 10;
    void Start()
    {
        Instance = this;
        ErrorPanel.SetActive(false); 
        NetworkPanel.SetActive(false);

        DontDestroyOnLoad(this.gameObject);
        InvokeRepeating("checkConnection", 5f, 0.1f);
    }

    void Update() {
      
    
    }

    IEnumerator Retry() {
        RetrytitleTF.text = "Server Connection Failed";
        GameSocketManager.Instance.Connect();
        RetrymessageTF.text = "Connecting......";
        RetryPanel.SetActive(true);
        yield return new WaitForSeconds(5);

        while (counter != 0 || GameSocketManager.Instance.IsServerConnected) {

            RetrymessageTF.text = "Trying Reconnection in ... " + counter.ToString();
           
            yield return new WaitForSeconds(1);
            counter -= 1;
            if (counter == 0 && !GameSocketManager.Instance.IsServerConnected)
            {
                counter = 10;
                yield return Retry();
                //NetworkError("Failed to reconnect!", "Try to Login back");

            }
            else {
                if (GameSocketManager.Instance.IsServerConnected) {

                    RetrytitleTF.text = "Connection restored!";
                    RetrymessageTF.text = "Reconnection was successful. Now you can play the game";

                    yield return new WaitForSeconds(3);
                    DisableAllPanels();
                    Retrying_Connect = false;
                    break;
                }


            }




            yield return null;
        }
        
    
    }

    void checkConnection() {
        if (!GameSocketManager.Instance.IsServerConnected && !Retrying_Connect)
        {

            StartCoroutine(Retry());
            Retrying_Connect = true;
        }
    }

    public void DisplayError(string title, string Message) {

        titleTF.text = title;
        messageTF.text = Message;
        ErrorPanel.SetActive(true);
        RetryPanel.SetActive(false);
        NetworkPanel.SetActive(false);

    }

    public void NetworkError(string title, string Message)
    {

        NetworktitleTF.text = title;
        NetworkmessageTF.text = Message;
        NetworkPanel.SetActive(true);
        ErrorPanel.SetActive(false);
        RetryPanel.SetActive(false);


    }

    public void DisableAllPanels() {
        NetworkPanel.SetActive(false);
        ErrorPanel.SetActive(false);
        RetryPanel.SetActive(false);

    }
    public void ConnectAgain() {

        SceneManager.LoadScene("01_Login");
        DisableAllPanels();
    }

}
