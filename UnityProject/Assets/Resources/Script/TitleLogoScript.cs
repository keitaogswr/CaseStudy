using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoScript : MonoBehaviour {

    public enum TITLE_LOGO_STATE
    {
        STATE_NONE = 0,
        STATE_UP,
        STATE_MAX,
    }

    public float addNum = 0.01f;        // 加算値
    public float range = 30;            // 移動範囲

    private TITLE_LOGO_STATE state;     // 状態
    private Vector3 defPos;             // デフォルト位置
    private Vector3 pos;                // 現在の位置
    private float time;                 // 時間


    private int upTime;                 // ↑に行くときの時間
    public int downTime = 30;           // ↑に行くとき   下がる時間
    public int downNum = 1;             // ↑に行くとき   下がる数値
    public int upNum = 8;               // ↑に行くとき   上がる数値


    private GameObject fade;

    // Use this for initialization
    void Start()
    {
        defPos = transform.position;
        time = 0;
        state = TITLE_LOGO_STATE.STATE_NONE;
    }

    // Update is called once per frame
    void Update() {

        switch (state)
        {
            case TITLE_LOGO_STATE.STATE_NONE:
                {
                    StateNoneUpdate();
                    break;
                }

            case TITLE_LOGO_STATE.STATE_UP:
                {
                    StateUpUpdate();
                    break;
                }
            default:break;
        }
    }

    // 上下移動状態
    private void StateNoneUpdate()
    {
        pos = defPos;
        pos.y = defPos.y + Mathf.Sin(time) * range;
        time += addNum;

        // 時間の制御
        if (time > Mathf.PI * 2)
        {
            time = 0;
        }

        // 位置更新
        transform.position = pos;


        if (GameObject.Find("Fade").GetComponent<fadeScript>().GetFadeMode() == fadeScript.FADE_MODE.FADE_OUT)
        {
            state = TITLE_LOGO_STATE.STATE_UP;
        }
    }

    // ↑にふっとぶ
    private void StateUpUpdate()
    {
        upTime++;

        if (upTime > downTime)
        {
            pos = transform.position;
            pos.y += upNum;
            transform.position = pos;
        }
        else
        {
            pos = transform.position;
            pos.y -= downNum;
            transform.position = pos;
        }
    }
}
