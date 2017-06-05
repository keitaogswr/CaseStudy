using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rankingScore : MonoBehaviour {

    public List<int> m_records = new List<int>();           // レコードデータ
    public int m_myRecord;                                  // マイスコア
    public bool m_isRecordTest;                             // レコードのテストデータを使うかどうか

    public float m_interval;                                // 点滅周期
    public Color m_color;                                   // 通常色
    public Color m_FlashColor;                              // 点滅色

    private Transform m_myScore;                            // マイスコアオブジェクト
    private Transform m_rankInRecord;                       // ランクインしたレコードのオブジェクト
    private float m_nextTime;                               // 点滅用カウンタ
    private bool m_flashing = false;                        // 光っている



    private void Awake()
    {
        Debug.Log("Ranking Awaken");

        /* ランキングデータを読み込む */

        if ( m_isRecordTest == false )
        {
            Transform child;

            for (int i = 0; i < m_records.Count; i++)
            {
                child = transform.GetChild(i);
                m_records[i] = PlayerPrefs.GetInt(child.name);
            }
        }

        InitializeRecord(); // オブジェクトへセット

        /*
         * ランキングデータの保存
         * InitializeRecord()を読んだ時点でレコードの変更はないので、
         * 確実に保存しておく
         */

        for (int i = 0; i < m_records.Count; i++)
        {
            Transform child = transform.GetChild(i);
            PlayerPrefs.SetInt(child.name, m_records[i]);
        }

        PlayerPrefs.Save();

        Debug.Log("RankingRecord is saved.");
    }

    // Use this for initialization
    void Start ()
    {
        m_nextTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if ( m_rankInRecord != null && m_nextTime < Time.time )
        {
            /* 点滅処理 */

            m_flashing = !m_flashing;

            SetRecordColor(m_myScore.FindChild("Digits"), m_flashing ? m_FlashColor : m_color);

            m_rankInRecord.FindChild("Text").GetComponent<Text>().color = m_flashing ? m_FlashColor : m_color;
            SetRecordColor(m_rankInRecord.FindChild("Digits"), m_flashing ? m_FlashColor : m_color);

            // 次の周期へ
            m_nextTime += m_interval;
        }
	}

    // レコードオブジェクトの初期化
    void InitializeRecord()
    {
        PointManager pointManager;

        for (int i = 0; i < m_records.Count; i++)
        {
            Transform child = transform.GetChild(i);

            // ランクイン判定
            if (m_rankInRecord == null && m_myRecord > m_records[i])
            {
                m_rankInRecord = child;

                m_records.Insert(i, m_myRecord);
                m_records.RemoveAt(m_records.Count - 1);

                Debug.Log("Rank In! : " + (i + 1));
            }

            pointManager = child.FindChild("PointManager").GetComponent<PointManager>();
            pointManager.AddPoint(m_records[i]);

            SetRecordColor(child.FindChild("Digits"), m_color);
        }

        // マイスコア

        m_myScore = transform.FindChild("myScore");
        pointManager = m_myScore.FindChild("PointManager").GetComponent<PointManager>();
        pointManager.AddPoint(m_myRecord);

        SetRecordColor(m_myScore.FindChild("Digits"), m_color);
    }

    // レコードのスコアへ色をセット
    void SetRecordColor( Transform digits, Color color )
    {
        foreach (Transform obj in digits.transform)
        {
            Image image = obj.GetComponent<Image>();
            image.color = color;
        }
    }
}
