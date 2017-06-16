using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // Image型を使うため必要

public class ArmScript : MonoBehaviour
{
    public enum ARM_STATE           // アームの状態の状態
    {
        ARM_STATE_NONE = 0,
        ARM_STATE_ADD,
        ARM_STATE_BACK,
        ARM_STATE_MAX,
    }


    private Image armImg;           // arm取得
    private Vector2 defSize;        // デフォルトのサイズ
    private Vector2 nowSize;        // 現在の幅
    private float addWidth;         // 加算Width
    private float subWidth;
    public ARM_STATE state;        // 状態
    private int addTime;            // 加算する時間
    private int backTime;           // 戻す時間

    // Use this for initialization
    void Start()
    {
        // Imgを取得
        armImg = GetComponent<Image>();

        // デフォルトの幅を取得
        defSize = armImg.rectTransform.sizeDelta;
        // 現在の幅に設定
        nowSize = defSize;

        state = ARM_STATE.ARM_STATE_NONE;
    }

    // Update is called once per frame
    void Update()
    {
        // 加算フラグONなら
        switch (state)
        {
            case ARM_STATE.ARM_STATE_ADD:
                {
                    StateAddUpdate();
                    break;
                }

            case ARM_STATE.ARM_STATE_BACK:
                {
                    StateBackUpdate();
                    break;
                }
            case ARM_STATE.ARM_STATE_NONE:
                {
                   break;
                }
            default:
                OnArmAdd(4, 60);
                break;
        }
    }

    // 加算実行
    public void OnArmAdd(float addWidthMax, int time)
    {
        if (state == ARM_STATE.ARM_STATE_NONE)
        {
            state = ARM_STATE.ARM_STATE_ADD;

            addWidth = addWidthMax / time;      // 一度に加算する量を設定
            subWidth = addWidthMax / 2;
            addTime = time;                     // 加算する時間を設定
            backTime = 2;                 // 戻す時間も設定
        }
    }

    // 加算状態の処理
    public void StateAddUpdate()
    {
        if (addTime > 0)
        {
            addTime--;                                      // 時間減算
            nowSize.x += addWidth;                         // 幅加算
            armImg.rectTransform.sizeDelta = nowSize;      // 位置更新
        }
        else
        {
            // 戻し状態へ
            state = ARM_STATE.ARM_STATE_BACK;
        }
    }

    // 戻す状態の処理
    public void StateBackUpdate()
    {
        if (backTime > 0)
        {
            backTime--;                                 // 時間減算
            nowSize.x -= subWidth;                      // 幅 戻すため減算
            armImg.rectTransform.sizeDelta = nowSize;   // 位置更新
        }
        else
        {
            // 状態なしへ
            state = ARM_STATE.ARM_STATE_NONE;
        }
    }

    // アームの状態を取得
    public ARM_STATE GetArmState()
    {
        return state;
    }

    // 方向の設定
    public void SetPivotPos( float pos )
    {
        armImg.rectTransform.pivot = new Vector2( pos, 0.5f );
    }
}
