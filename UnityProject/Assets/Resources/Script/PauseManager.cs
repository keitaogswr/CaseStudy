using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    private enum Scene {
        Title = 0,
        Game,
    };

    public string[] SceneName = {
        "title",
        "game",
    };

    [SerializeField]
    private Canvas pauseCanvas;
    private bool pause;
    private Start_UI Start_UI;
    private Finish_UI Finish_UI;
    public Pauseable Pause;

    private void Awake()
    {
        Start_UI = GameObject.Find("Start").GetComponent<Start_UI>();
        Finish_UI = GameObject.Find("Finish").GetComponent<Finish_UI>();
    }

    // Use this for initialization
    void Start () {
        pause = false;
        pauseCanvas.enabled = false;
        Pause.pausing = true;

        Debug.Log(SceneName[(int)Scene.Title]);
        Debug.Log(SceneName[(int)Scene.Game]);
    }

    void Update()
    {
        pauseCanvas.enabled = pause;
    }

    public void PushMenu()
    {
        if (Start_UI.GetFinish() == true && Finish_UI.isActiveAndEnabled == false)
        {
            pause = !pause;
            Pause.pausing = !Pause.pausing;
        }
    }

    public void Resume()
    {
        pause = false;
        Pause.pausing = false;
    }

    public void ReturnTitle()
    {
        GameObject.Find("Fade").GetComponent<fadeScript>().SetFade(SceneName[(int)Scene.Title]);
        pauseCanvas.enabled = false;
    }

    public void ReTry()
    {
        GameObject.Find("Fade").GetComponent<fadeScript>().SetFade(SceneName[(int)Scene.Game]);
        pauseCanvas.enabled = false;
    }
}
