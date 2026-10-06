using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenesManager : MonoBehaviour
{
    public GameObject PlayButton;

    private void Awake()
    {
        PlayButton.SetActive(false);
    }
    void Start()
    {
        Invoke("ActivatePlayButton", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivatePlayButton() {
        // PlayButton.SetActive(true);
      //  SceneManager.LoadScene(1);
    }



    public void OnPlayGameButton() {
        GetComponent<AudioSource>().Play();
        PlayButton.SetActive(false);
        PlayMainScene();
    }
    void PlayMainScene() {

     
    }
}
