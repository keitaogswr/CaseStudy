using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour
{

    public void ClickGameStart()
    {
        if (GetComponentInParent<titleGearScript>().GetRotFlag())
        {
            Debug.Log("ゲームスタートボタンクリック");
            GameObject.Find("Fade").GetComponent<fadeScript>().SetFade("testscene");
        }
    }

    public void ClickExit()
    {
        if (GetComponentInParent<titleGearScript>().GetRotFlag())
        {
            Debug.Log("ゲーム終了ボタンクリック");
            Application.Quit();
        }
    }
}
