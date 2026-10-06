using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class GetMeVideoClip : MonoBehaviour
{
    private VideoPlayer player;
    private void Awake()
    {

        player = GetComponent<VideoPlayer>();
    }
    private void Start()
    {
        player.loopPointReached += CheckOver;
    }

    void CheckOver(VideoPlayer vp) {

        SceneManager.LoadScene(1);
    }

    public void SkipVideo() {
        player.Stop();
        SceneManager.LoadScene(1);
    }
    private void Update()
    {
        
       
    }
}
