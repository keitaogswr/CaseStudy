using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // Image型を使うため必要

public class berScript : MonoBehaviour {
    private Image berImage; // バーのImg取得

    private float addNum;
    private int addTime;

    // Use this for initialization
    void Start () {
        berImage = GetComponent<Image>();
        addNum = 0;
        addTime = 0;

        //SetAddMode(60, 0.01f);
    }
	
	// Update is called once per frame
	void Update () {
        // 指定時間内加算
        if (addTime > 0)
        {
            addTime--;
            AddAmount(addNum);
        }
    }

    // 指定時間内加算 ( 時間, 加算数値 )
    public void SetAddMode(int time, float add)
    {
        addNum = add;
        addTime = time;
    }


    // バーの表示領域 追加
    public void AddAmount(float add)
    {
        berImage.fillAmount += add;
    }

    // バーのゲージを0にする
    public void ResetAmount()
    {
        berImage.fillAmount = 0;
    }

    // 現在の表示割合の取得
    public float GetFillAmount()
    {
        return berImage.fillAmount;
    }
}
