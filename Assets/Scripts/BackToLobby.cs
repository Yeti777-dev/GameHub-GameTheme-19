using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLobby : MonoBehaviour
{



    public void OnBackToLobbyButton() {

        SceneManager.LoadScene("02_Gamelist");
       // bgsoundmanager.instance.playMainBG();
    
    }
}
