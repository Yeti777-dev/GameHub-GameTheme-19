using UnityEngine;

public class OrientationError : MonoBehaviour
{
    public GameObject OrientationErroPanel;
    public Canvas ApiMangerCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
           
            OrientationErroPanel.SetActive(true);
        }
        else
        {
            OrientationErroPanel.SetActive(false);
           // ResolutionManager.Instance.SetFullScreen();
        }

        if (ApiMangerCanvas.worldCamera == null)
            ApiMangerCanvas.worldCamera = Camera.main;

    }
    public void CloseOrientationError()
    {
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
           
            return;
        }

        else
            OrientationErroPanel.SetActive(false);

        
    }
}
