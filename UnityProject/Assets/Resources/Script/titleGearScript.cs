using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleGearScript : MonoBehaviour
{
    public int rotTime;         // 回転時間
    public int buttonNum;       // ボタン数
    private bool rotFlag;       // 回転フラグ
    private float rotSpeed;     // 回転速度
    private float rotVecZ;      // 回転方向
    private int time;           // 時間

    private int moveRight;
    private int moveLeft;

    void Start()
    {
        // 初期化
        rotFlag = false;
        rotSpeed = (float)((360.0f / buttonNum) / rotTime);

        moveRight = 0;
        moveLeft = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotFlag == true)
        {
            if (time == 0)
            {
                rotFlag = false;
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, rotVecZ));
                time--;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                GearRotation(rotSpeed);
            }
            if (Input.GetMouseButtonDown(1))
            {
                GearRotation(-rotSpeed);
            }

        }
    }

    // ギア回転
    public void GearRotation(float rot)
    {
        rotVecZ = rot;      // 回転方向Z
        time = rotTime;  // 回転時間
        rotFlag = true;     // フラグ
    }

    // 回転中かのフラグ
    public bool GetRotFlag()
    {
        return rotFlag;
    }
}