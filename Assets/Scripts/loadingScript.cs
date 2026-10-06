using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;

#pragma warning disable IDE1006 // Naming Styles
public class loadingScript : MonoBehaviour
#pragma warning restore IDE1006 // Naming Styles
{
    public string SceneToLoad;
    private Image FillProgress;
    private float fillTarget;

    void Awake() {

        FillProgress = gameObject.transform.GetChild(1).GetComponent<Image>();
    }

    void Start() {

        StartCoroutine(LoadGameScene());


    }
   

    // Update is called once per frame
    void Update()
    {
      //  print("fill target is " + fillTarget.ToString());
        if (gameObject.activeInHierarchy)
            FillProgress.fillAmount = Mathf.MoveTowards(FillProgress.fillAmount, fillTarget, 0.3f * Time.deltaTime);
    }

 

    IEnumerator LoadGameScene()
    {
       
        yield return null;

        AsyncOperation GameScene = SceneManager.LoadSceneAsync(SceneToLoad);
        GameScene.allowSceneActivation = false;
      //  print("GameScene progress  at start is " + GameScene.progress.ToString());

        while (!GameScene.isDone)
        {
            fillTarget = GameScene.progress;

            if (GameScene.progress >= 0.85f)
            {

                GameScene.allowSceneActivation = true;

            }
            yield return null;
        }

      //  print("GameScene progress is " + GameScene.progress.ToString());
      
    
    
    }
}
