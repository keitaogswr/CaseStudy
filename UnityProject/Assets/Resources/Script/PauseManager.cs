using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    public string TitleScene = null;
    [SerializeField]
    private Canvas pauseCanvas;
    private bool pause;

	// Use this for initialization
	void Start () {
        pause = false;
        pauseCanvas.enabled = false;
    }

    void Update()
    {
        pauseCanvas.enabled = pause;
    }

    public void PushMenu()
    {
        pause = !pause;
    }

    public void Resume()
    {
        pause = !pause;
    }

    public void ReturnTitle()
    {
        GameObject.Find("Fade").GetComponent<fadeScript>().SetFade(TitleScene);
        pauseCanvas.enabled = false;
    }

    public void ReTry()
    {
        GameObject.Find("Fade").GetComponent<fadeScript>().SetFade("testscene");
        pauseCanvas.enabled = false;
    }
}
