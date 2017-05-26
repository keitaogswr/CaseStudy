using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSqript : MonoBehaviour {

    public void ClickGameStart()
    {
        Debug.Log("ゲームスタートボタンクリック");
        GameObject.Find("Fade").GetComponent<fadeSqript>().SetFade("testscene");
    }

    public void ClickExit()
    {
        Debug.Log("ゲーム終了ボタンクリック");
        Application.Quit();
    }
}
