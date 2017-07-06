using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour {

    private GameObject optionCanvas;
    public GameObject debug;
	// Use this for initialization
	void Start () {
        optionCanvas = GameObject.Find("OptionCanvas");
        debug = optionCanvas;
        optionCanvas.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickOption()
    {
        optionCanvas.SetActive(true);
    }

    public void Return()
    {
        optionCanvas.SetActive(false);
    }
}
