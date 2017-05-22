using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rankingScript : MonoBehaviour {
    private GameObject fade;

    // Use this for initialization
    void Start()
    {
        Debug.Log("ランキング開始");
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
