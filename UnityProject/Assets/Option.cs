using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour {

    private Canvas optionCanvas;

	// Use this for initialization
	void Start () {
        optionCanvas = GameObject.Find("OptionCanvas").GetComponent<Canvas>();
        optionCanvas.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickOption()
    {
        optionCanvas.enabled = true;
    }

    public void Return()
    {
        optionCanvas.enabled = false;
    }
}
