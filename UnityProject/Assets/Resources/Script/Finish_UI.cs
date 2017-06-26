using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish_UI : MonoBehaviour {
    public Vector3  stagingScale;       // 演出スケール
    public Vector3  lastScale;          // 最終スケール
    public float    stagingTime;        // 演出時間
    public float    lastTime;           // 最終地点へのタイム

    private bool    stagingEnd = false; // 演出終了フラグ
    private bool    end = false;        // 修了フラグ
    private Vector3 addNum;             // 加算値
    private float   time;               // 時間

    // Use this for initialization
    void Start () {
        time = 0;
        addNum = (stagingScale - transform.localScale) / stagingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (stagingEnd == false)
        {
            transform.localScale += addNum;

            time++;

            if (time > stagingTime)
            {
                stagingEnd = true;
                time = 0;
                addNum = (lastScale - transform.localScale) / lastTime;
            }
        }
        else
        {
            if (end == false)
            {
                transform.localScale += addNum;
                time++;

                if (time > lastTime)
                {
                    end = true;
                }
            }
        }

    }
}
