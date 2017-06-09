using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour
{
    // シーンのタグ
    public enum sceneNum
    {
        TITLE = 0,
        GAME,
        RANKING,
    }

    // シーンの名前
    private string[] sceneName = {
            "title",
            "testscene",
            "ranking",
        };

    // ゲーム開始ボタン
    public void ClickGameStart()
    {
        if ( GetComponentInParent<titleGearScript>().GetRotFlag() == false)
        {
            Debug.Log("ゲームスタートボタンクリック");
            GameObject.Find("Fade").GetComponent<fadeScript>().SetFade(sceneName[(int)sceneNum.GAME]);
        }
    }

    // ランキングボタン
    public void ClickRanking()
    {
        if (GetComponentInParent<titleGearScript>().GetRotFlag() == false)
        {
            Debug.Log("ランキングボタン");
            GameObject.Find("Fade").GetComponent<fadeScript>().SetFade(sceneName[(int)sceneNum.RANKING]);
        }
    }


    // ゲーム修了ボタン
    public void ClickExit()
    {
        if (GetComponentInParent<titleGearScript>().GetRotFlag() == false)
        {
            Debug.Log("ゲーム終了ボタンクリック");
            Application.Quit();
        }
    }
}
