
#define TEST_RANKING
#if TEST_RANKING
#define UNITY_EDITOR
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rankingScore : MonoBehaviour {

#if UNITY_EDITOR

    [Tooltip("テスト用のマイスコア") ]
    public int m_testMyScore;

    [ Tooltip("テスト用のマイスコアを使うか") ]
    public bool m_isUseTestMyScore;

    [ Tooltip("デフォルトのレコードを使うか") ]
    public bool m_isUseDefaultRecord;

    [ Tooltip("PlayerPrefsを初期化するか") ]
    public bool m_isUseInitializePrefs;

#endif

    public List<int> m_recordsDefault = new List<int>();    // デフォルトレコードデータ

    public float m_interval;                                // 点滅周期
    public Color m_FlashColor;                              // 点滅色

    private Transform m_myScoreObj;                         // マイスコアオブジェクト
    private Transform m_rankInRecordObj;                    // ランクインしたレコードのオブジェクト

    static private int m_myScore = 0;                      // マイスコア
    static private bool m_isMyScoreChanged = false;        // マイスコアが変更されたか



    private void Awake()
    {
        Debug.Log("Ranking Awaken");

#if UNITY_EDITOR

        if (m_isUseInitializePrefs)
        {
            PlayerPrefs.DeleteAll();
        }

#endif

        /* ランキングデータを読み込む */

        List<int> records = new List<int>( m_recordsDefault );
        Transform recordObject = transform.FindChild("Records");

        Debug.Assert(records.Count == recordObject.childCount, "レコードサイズとレコードオブジェクトの数があっていません");

        // PlayerPrefs内にデータがあれば読み込む
        if (PlayerPrefs.HasKey(recordObject.GetChild(0).name))
        {
            Debug.Log("Loading Ranking Records.");

            for ( int i = 0; i < records.Count; i++ )
            {
                Transform child = recordObject.GetChild(i);
                records[i] = PlayerPrefs.GetInt(child.name);
            }
        }

#if UNITY_EDITOR

        if ( m_isUseDefaultRecord )
        {
            records = new List<int>(m_recordsDefault);
        }

#endif

        InitializeRecord( ref records ); // オブジェクトへセット

        /*
         * ランキングデータの保存
         * InitializeRecord()を読んだ時点でレコードの変更はないので、
         * 確実に保存しておく
         */

        for (int i = 0; i < records.Count; i++)
        {
            Transform child = recordObject.GetChild(i);
            PlayerPrefs.SetInt(child.name, records[i]);
        }

        PlayerPrefs.Save();

        Debug.Log("RankingRecord is saved.");
    }

    private void OnDestroy()
    {
        m_isMyScoreChanged = false;
    }

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_rankInRecordObj != null)
        {
            Transform t;

            t = m_rankInRecordObj.FindChild("Rank");
            t.GetComponent<Image>().color = t.GetComponent<GradientColor>().GetColor();
            t = m_rankInRecordObj.FindChild("Digits");
            t.GetComponent<DigitsScript>().color = t.GetComponent<GradientColor>().GetColor();

            t = m_myScoreObj.FindChild("Rank");
            t.GetComponent<Image>().color = t.GetComponent<GradientColor>().GetColor();
            t = m_myScoreObj.FindChild("Digits");
            t.GetComponent<DigitsScript>().color = t.GetComponent<GradientColor>().GetColor();
        }
    }

    // レコードオブジェクトの初期化
    void InitializeRecord( ref List<int> records )
    {
        PointManager pointManager;

#if UNITY_EDITOR

        if ( m_isUseTestMyScore )
        {
            SetMyScore(m_testMyScore);
        }

#endif

        for (int i = 0; i < records.Count; i++)
        {
            Transform child = transform.FindChild("Records").GetChild(i); ;

            // ランクイン判定
            if (m_isMyScoreChanged && m_rankInRecordObj == null && m_myScore > records[i])
            {
                m_rankInRecordObj = child;

                records.Insert(i, m_myScore);
                records.RemoveAt(records.Count - 1);

                Debug.Log("Rank In! : " + (i + 1));
            }

            pointManager = child.FindChild("PointManager").GetComponent<PointManager>();
            pointManager.AddPoint(records[i]);
        }

        // マイスコア

        m_myScoreObj = transform.FindChild("myScore");
        pointManager = m_myScoreObj.FindChild("PointManager").GetComponent<PointManager>();
        pointManager.AddPoint(m_myScore);

        if (m_rankInRecordObj != null)
        {
            SetRankInRecord(m_rankInRecordObj);
            SetRankInRecord(m_myScoreObj);
        }
    }

    // ランクインレコードへコンポーネントを付ける
    void SetRankInRecord( Transform record )
    {
        GradientColor gc;
        GameObject obj;

        /* ランクへGradientColorをAdd */

        obj = record.FindChild("Rank").gameObject;
        gc = obj.AddComponent<GradientColor>();
        gc.gradientTime = m_interval;
        gc.element.Add(new GradientColor.GradientElement(obj.GetComponent<Image>().color, 0.0f));
        gc.element.Add(new GradientColor.GradientElement(m_FlashColor, 0.5f));
        gc.element.Add(new GradientColor.GradientElement(obj.GetComponent<Image>().color, 1.0f));

        /* ディジットへGradientColorをAdd */

        obj = record.FindChild("Digits").gameObject;
        gc = obj.AddComponent<GradientColor>();
        gc.gradientTime = m_interval;
        gc.element.Add(new GradientColor.GradientElement(obj.GetComponent<DigitsScript>().color, 0.0f));
        gc.element.Add(new GradientColor.GradientElement(m_FlashColor, 0.5f));
        gc.element.Add(new GradientColor.GradientElement(obj.GetComponent<DigitsScript>().color, 1.0f));
    }

    // マイレコードへセット
    static public void SetMyScore( int record )
    {
        m_myScore = record;
        m_isMyScoreChanged = true;
    }
}
