using UnityEngine;
using System.Collections;
using System;

public class UpdateJackpot : MonoBehaviour {

    public Rooms room;

    public TextMesh texMesh;

	// Use this for initialization
	void Start () {

       

    }
	
    void OnEnable()
    {
        JackpotManagerScript.OnUpdateJackpot += OnUpdateJackPot;
    }

    void OnDisable()
    {
        JackpotManagerScript.OnUpdateJackpot -= OnUpdateJackPot;
    }

    private void OnUpdateJackPot(Rooms r, float coins)
    {
        if (room != r)
            return;

        texMesh.text = coins.ToString();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
