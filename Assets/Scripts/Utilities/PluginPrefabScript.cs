using UnityEngine;
using System.Collections;

public class PluginPrefabScript : MonoBehaviour {

	// Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
     
    }



}
