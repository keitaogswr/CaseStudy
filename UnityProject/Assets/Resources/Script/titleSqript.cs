using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titleSqript : MonoBehaviour {
    private GameObject fade;
    public string nextScene;

    // Use this for initialization
    void Start() {
        Debug.Log("タイトル開始");
        fade = GameObject.Find("Fade");
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log("ボタン入力確認");
        //    fade.GetComponent<fadeSqript>().SetFade(nextScene);
        //}
    }
}
