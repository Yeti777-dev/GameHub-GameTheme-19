using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnGameButtonClicked : MonoBehaviour
{
    public GameObject SmallLoadingBar;
    public void Start()
    {
    }


    public void Update() {

        GetComponent<Button>().interactable = !gameObject.GetComponent<GameData>().IscomingSoon;


    }
    public void gameClicked()
    {

    }

    IEnumerator GamePlayScene()
    {

        yield return null;

        AsyncOperation GameScene = SceneManager.LoadSceneAsync("GamePlay");
        GameScene.allowSceneActivation = false;
        print("GameScene progress  at start is " + GameScene.progress.ToString());

        while (!GameScene.isDone)
        {

            if (GameScene.progress >= 0.85f)
            {

                GameScene.allowSceneActivation = true;

            }
            yield return null;
        }

        print("GameScene progress is " + GameScene.progress.ToString());
    }
}
