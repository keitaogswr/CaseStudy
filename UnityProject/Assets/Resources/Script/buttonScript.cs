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
        TUTORIAL,
    }

    // シーンの名前
    private string[] sceneName = {
            "title",
            "game",
            "ranking",
            "tutorial",
        };

    private float range =30;

    // ゲーム開始ボタン
    public void ClickGameStart()
    {
        if (GetComponentInParent<titleGearScript>().GetRotFlag() == false)
        {
            Vector3 inPos = GetComponentInParent<titleGearScript>().GetClickPos();
            Vector3 outPos = Input.mousePosition;

            if (inPos.x + range > outPos.x &&
                inPos.x - range < outPos.x)
            {
                Debug.Log("ゲームスタートボタンクリック");
                GameObject.Find("Fade").GetComponent<fadeScript>().SetFade(sceneName[(int)sceneNum.GAME]);
                AudioManager.Instance.PlaySE("動作音_3");
            }
        }
    }

    // ランキングボタン
    public void ClickRanking()
    {
        if (GetComponentInParent<titleGearScript>().GetRotFlag() == false)
        {
            Vector3 inPos = GetComponentInParent<titleGearScript>().GetClickPos();
            Vector3 outPos = Input.mousePosition;

            if (inPos.x + range > outPos.x &&
                inPos.x - range < outPos.x)
            {
                Debug.Log("ランキングボタン");
                GameObject.Find("Fade").GetComponent<fadeScript>().SetFade(sceneName[(int)sceneNum.RANKING]);
                AudioManager.Instance.PlaySE("動作音_1");
            }
        }
    }


    // ゲーム修了ボタン
    public void ClickTutorial()
    {
        if (GetComponentInParent<titleGearScript>().GetRotFlag() == false)
        {
            Vector3 inPos = GetComponentInParent<titleGearScript>().GetClickPos();
            Vector3 outPos = Input.mousePosition;

            if (inPos.x + range > outPos.x &&
                inPos.x - range < outPos.x)
            {
                Debug.Log("ランキングボタン");
                GameObject.Find("Fade").GetComponent<fadeScript>().SetFade(sceneName[(int)sceneNum.TUTORIAL]);
                AudioManager.Instance.PlaySE("動作音_1");
            }
        }
    }
}
