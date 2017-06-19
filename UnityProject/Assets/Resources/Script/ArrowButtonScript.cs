using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButtonScript : MonoBehaviour {
    public GameObject[] button;

    public float sinRange;      // sinの範囲
    public float addTime;       // 加算時間量


    private Vector3 defScale;           // デフォルトスケール
    private Vector3 scale;              // スケール
    private float time;                 // 時間

    // Use this for initialization
    void Start () {
        time = 0;
        defScale = button[0].transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {
        scale = defScale;
        scale.x = defScale.x + Mathf.Sin(time) * sinRange;
        scale.y = defScale.y + Mathf.Sin(time) * sinRange;
        time += addTime;

        // 時間の制御
        if (time > Mathf.PI)
        {
            time = 0;
        }

        // スケール更新
        for (int i = 0; i < button.Length; i++)
        {
            button[i].transform.localScale = scale;
        }
    }
}
