using UnityEngine;
using System.Collections;

public class ResolutionSelector : MonoBehaviour {
    public static ResolutionSelector instance;
    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        instance = this;
        DontDestroyOnLoad(gameObject);
      
    }
    private void Update()
    {
      
    }

  

    public void FullScreenToggle() {
        if (ResolutionManager.Instance == null) return;
        ResolutionManager resolutionManager = ResolutionManager.Instance;
        resolutionManager.ToggleFullscreen();

    }

  


}
