using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleGearScript : MonoBehaviour
{
    // 設定用
    public GameObject[] button; // ボタン
    private GameObject[] obj;    // オブジェ
    public float length;        // 配置長さ
    public int rotTime;         // 回転時間

    // 内部数値
    private bool rotFlag;       // 回転フラグ
    private float rotSpeed;     // 回転速度
    private float rotVecZ;      // 回転方向
    private int time;           // 時間
    private float angle;        // 角度

    // フリック
    private float flickRange = 40;   // フリックの受付範囲
    private Vector3 inClickPos;     // 押したクリック位置
    private Vector3 outClickPos;    // 離したクリック位置

    void Start()
    {
        obj = new GameObject[button.Length];

        length *= 0.44f;                        // 長さの修正
        float angle = 360.0f / button.Length;   // 角度
        rotSpeed = (float)(angle / rotTime);    // 目標位置への速度


        Vector3 gearPos = transform.position;   // ギアの位置
        Vector3 pos;                            // 位置

        
        for (int i = 0; i < button.Length; i++)
        {
            // 位置設定
            pos.x = gearPos.x + Mathf.Sin(( angle * i) * Mathf.PI / 180) * length;
            pos.y = gearPos.y + Mathf.Cos(( angle * i) * Mathf.PI / 180) * length;
            pos.z = 0.0f;

            obj[i] = Instantiate(button[i]);
            obj[i].transform.parent = transform;
            obj[i].transform.position = pos;
            obj[i].transform.localScale = new Vector3(1,1,1);
            obj[i].transform.Rotate(0, 0, 0);
        }

        // 初期化
        rotFlag = false;                        // フラグ
    }

    // Update is called once per frame
    void Update()
    {
        // ここでボタン歯車の向きを設定してる
        for (int i = 0; i < button.Length; i++)
        {
            obj[i].transform.Rotate(0, 0, -0.2f);
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
            }
        }
        // フリック
        else
        {
            Flick();
        }
    }

    // ギア回転
    public void GearRotation(float rot)
    {
        rotVecZ = rot;      // 回転方向Z
        time = rotTime;     // 回転時間
        rotFlag = true;     // フラグ
    }

    // 回転中かのフラグ
    public bool GetRotFlag()
    {
        return rotFlag;
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
            // 離した位置更新
            outClickPos = Input.mousePosition;

            // 補正値より動いていたら
            if (inClickPos.x + flickRange < outClickPos.x)
            {
                GearRotation(-rotSpeed);
            }
            else if (inClickPos.x - flickRange > outClickPos.x)
            {
                GearRotation(rotSpeed);
            }
        }
    }
}