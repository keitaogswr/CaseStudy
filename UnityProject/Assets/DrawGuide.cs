using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DrawGuide : MonoBehaviour {

    [SerializeField]
    private Text GuideText;
    private string TutorialText;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        //GuideText.text = TutorialText;
    }

    public void SetText(string text)
    {
        GuideText.text = text;
    }
}
