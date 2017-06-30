using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour {

    public GameMain m_GameMain;
    private Image m_Image;
    private int comboNum;           // コンボの数値
    public PointManager point;      // 数値を変更するためのスクリプト
    public Image[] number;          // イメージナンバーのデータ
    private float digit;            // 桁数

    // Use this for initialization
    void Start () {
        comboNum = 0;
        digit = Mathf.Pow(10, number.Length);


        m_Image = GetComponent<Image>();
        m_Image.color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update() {
        Color temp;
        float alfaSpeed;
        temp = m_Image.color;
        if (m_GameMain.Phase != PHASE.STAY)
        {
            alfaSpeed = 0.1f;
        }
        else
        {
            alfaSpeed = -0.1f;

            // コンボの値をリセット
            point.SubPoint(comboNum);
            comboNum = 0;
        }
        temp.a += alfaSpeed;
        temp.a = temp.a > 1.0f ? 1.0f : temp.a;
        temp.a = temp.a < 0.0f ? 0.0f : temp.a;

        m_Image.color = temp;

        for (int i = 0; i < number.Length; i++)
        {
            number[i].color = temp;
        }
    }


    // コンボの加算の処理
    public int addComb( int num )
    {
        if (num < digit)
        {
            comboNum += num;
            point.AddPoint(num);
        }
        return comboNum;
    }
}
