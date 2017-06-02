using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    public string TitleScene = null;
    [SerializeField]
    private Canvas pauseCanvas;
    private bool pause;
    public Pauseable Pause;

	// Use this for initialization
	void Start () {
        pause = false;
        pauseCanvas.enabled = false;
        Pause.pausing = false;
    }

    void Update()
    {
        pauseCanvas.enabled = pause;
    }

    public void PushMenu()
    {
        pause = !pause;
        Pause.pausing = !Pause.pausing;
    }

    public void Resume()
    {
        pause = !pause;
        Pause.pausing = false;
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
