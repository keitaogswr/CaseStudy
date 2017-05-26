using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitButton : MonoBehaviour {

    public void OnClickExit()
    {
        Debug.Log("ゲーム終了ボタンクリック");
        Application.Quit();
    }
}
