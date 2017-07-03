using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // Image型を使うため必要

public class titleGearScript : MonoBehaviour
{
    // 設定用
    public GameObject[] buttonPrefab; // ボタンプレファブ
    public float length;              // 配置長さ
    public int rotTime;               // 回転時間

    // オブジェ
    private GameObject[] buttonObj;   // ボタンオブジェ
    private GameObject[] buttonChild; // ボタンの子
    private GameObject fade;          // フェード

    // 内部数値
    private bool rotFlag;       // 回転フラグ
    private float rotSpeed;     // 回転速度
    private float rotVecZ;      // 回転方向
    private int time;           // 時間
    private float angle;        // 角度

    // フリック
    public Vector2 flickRange = new Vector2( 40, 60 );  // フリックの受付範囲
    private Vector3 inClickPos;     // 押したクリック位置
    private Vector3 outClickPos;    // 離したクリック位置

    void Start()
    {
        buttonObj = new GameObject[buttonPrefab.Length];
        buttonChild = new GameObject[buttonPrefab.Length];

        float angle = 360.0f / buttonPrefab.Length;         // 角度
        rotSpeed = (float)(angle / rotTime);                // 目標位置への速度


        Vector3 gearPos = transform.position;   // ギアの位置
        Vector3 pos;                            // 位置

        
        for (int i = 0; i < buttonPrefab.Length; i++)
        {
            // 位置設定
            pos.x = gearPos.x + Mathf.Sin(( angle * i) * Mathf.PI / 180) * length;
            pos.y = gearPos.y + Mathf.Cos(( angle * i) * Mathf.PI / 180) * length;
            pos.z = 0.0f;

            // ボタン生成
            buttonObj[i] = Instantiate(buttonPrefab[i]);
            buttonObj[i].transform.parent = this.transform;
            buttonObj[i].transform.position = pos;
            buttonObj[i].transform.localScale = new Vector3(1,1,1);
            buttonObj[i].transform.Rotate(0, 0, 0);

            Image img = buttonObj[i].GetComponent<Image>();
            pos = img.rectTransform.localPosition;
            pos.z = 0;
            img.rectTransform.localPosition = pos;

            // 子取得 ( button )のImage
            buttonChild[i] = buttonObj[i].transform.FindChild("Image").gameObject;
        }
        fade = GameObject.Find("Fade");

        rotFlag = false;    // フラグ
    }

    // Update is called once per frame
    void Update()
    {
        // ここでボタン歯車の向きを設定してる
        for (int i = 0; i < buttonPrefab.Length; i++)
        {
            buttonObj[i].transform.Rotate(0, 0, -0.2f);

            // 文字設定
            buttonChild[i].transform.Rotate(new Vector3(0, 0, +0.2f));
        }


        // 回転中は操作不能に
        if (rotFlag == true)
        {
            // 回転時の時間が 0 ならフリック可能に
            if (time == 0)
            {
                rotFlag = false;
            }
            else
            {
                // 時間があるため回転中
                transform.Rotate(new Vector3(0, 0, rotVecZ));
                time--;

                // 文字を回転しないように
                for (int i = 0; i < buttonPrefab.Length; i++)
                {
                    buttonChild[i].transform.Rotate(new Vector3(0, 0, -rotVecZ));
                }
            }
        }
        // フリック
        else
        {
            Flick();
        }
    }

    // ギア回転
    public void GearRotation(int vec)
    {
        if (rotFlag == false)
        {
            // 左
            if (vec == 0)
            {
                rotVecZ = -rotSpeed;      // 回転方向Z
                time = rotTime;         // 回転時間
                rotFlag = true;         // フラグ
            }
            // 右
            else if (vec == 1)
            {
                rotVecZ = rotSpeed;      // 回転方向Z
                time = rotTime;         // 回転時間
                rotFlag = true;         // フラグ
            }
            AudioManager.Instance.PlaySE("ギア");
        }
    }

    // 回転速度取得
    public float GetRotSpeed()
    {
        return rotSpeed;
    }

    // 回転中かのフラグ
    public bool GetRotFlag()
    {
        return rotFlag;
    }

    // クリック位置取得
    public Vector3 GetClickPos()
    {
        return inClickPos;
    }

    // フリック処理
    public void Flick()
    {
        // クリックした位置
        if ( Input.GetMouseButtonDown(0) )
        {
            inClickPos = Input.mousePosition;
            outClickPos = inClickPos;
        }
        // クリック離した位置
        else if( Input.GetMouseButtonUp(0) )
        {
            if (GameObject.Find("Fade").GetComponent<fadeScript>().GetFadeMode() == fadeScript.FADE_MODE.FADE_NONE)
            {
                // 離した位置更新
                outClickPos = Input.mousePosition;

                // 補正値より動いていたら
                if (inClickPos.x + flickRange.x < outClickPos.x &&
                    Mathf.Abs(inClickPos.y - outClickPos.y) < flickRange.y )
                {
                    GearRotation(0);
                }
                else if (inClickPos.x - flickRange.x > outClickPos.x &&
                        Mathf.Abs( inClickPos.y - outClickPos.y ) < flickRange.y)
                {
                    GearRotation(1);
                }
            }
        }
    }
}