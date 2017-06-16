using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rankingScore : MonoBehaviour {

//#if UNITY_EDITOR

    [ Tooltip("テスト用のマイスコア") ]
    public int m_testMyScore;

    [ Tooltip("テスト用のマイスコアを使うか") ]
    public bool m_isUseTestMyScore;

    [ Tooltip("デフォルトのレコードを使うか") ]
    public bool m_isUseDefaultRecord;

    [ Tooltip("PlayerPrefsを初期化するか") ]
    public bool m_isUseInitializePrefs;

//#endif

    public List<int> m_recordsDefault = new List<int>();    // デフォルトレコードデータ

    public float m_interval;                                // 点滅周期
    public Color m_FlashColor;                              // 点滅色

    private Transform m_myScoreObj;                         // マイスコアオブジェクト
    private Transform m_rankInRecordObj;                    // ランクインしたレコードのオブジェクト
    private Color m_myScoreDefaultColor;                    // マイスコアのデフォルト色
    private Color m_rankInDefaultColor;                     // ランクインしたランク画像のデフォルト色
    private Color m_defaultColor;                           // スコアのデフォルト色
    private float m_nextTime;                               // 点滅用カウンタ
    private bool m_flashing = false;                        // 光っている

    static private int m_myScore = 0;                      // マイスコア
    static private bool m_isMyScoreChanged = false;        // マイスコアが変更されたか



    private void Awake()
    {
        Debug.Log("Ranking Awaken");

//#if UNITY_EDITOR

        if (m_isUseInitializePrefs)
        {
            PlayerPrefs.DeleteAll();
        }

//#endif

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

//#if UNITY_EDITOR

        if ( m_isUseDefaultRecord )
        {
            records = new List<int>(m_recordsDefault);
        }

//#endif

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
        m_nextTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if ( m_rankInRecordObj != null /*&& m_nextTime < Time.time*/ )
        {
            /* 点滅処理 */

            //m_flashing = !m_flashing;
            m_flashing = true;

            m_myScoreObj.FindChild("Rank").GetComponent<Image>().color = m_flashing ? m_FlashColor : m_myScoreDefaultColor;
            SetRecordColor(m_myScoreObj.FindChild("Digits"), m_flashing ? m_FlashColor : m_defaultColor);

            m_rankInRecordObj.FindChild("Rank").GetComponent<Image>().color = m_flashing ? m_FlashColor : m_rankInDefaultColor;
            SetRecordColor(m_rankInRecordObj.FindChild("Digits"), m_flashing ? m_FlashColor : m_defaultColor);

            // 次の周期へ
            m_nextTime += m_interval;
        }
	}

    // レコードオブジェクトの初期化
    void InitializeRecord( ref List<int> records )
    {
        PointManager pointManager;

//#if UNITY_EDITOR

        if ( m_isUseTestMyScore )
        {
            SetMyScore(m_testMyScore);
        }

//#endif

        for (int i = 0; i < records.Count; i++)
        {
            Transform child = transform.FindChild("Records").GetChild(i); ;

            // ランクイン判定
            if (m_isMyScoreChanged && m_rankInRecordObj == null && m_myScore > records[i])
            {
                m_rankInRecordObj = child;
                m_rankInDefaultColor = m_rankInRecordObj.FindChild("Rank").GetComponent<Image>().color;
                m_defaultColor = m_rankInRecordObj.FindChild("Digits/Number").GetComponent<Image>().color;

                records.Insert(i, m_myScore);
                records.RemoveAt(records.Count - 1);

                Debug.Log("Rank In! : " + (i + 1));
            }

            pointManager = child.FindChild("PointManager").GetComponent<PointManager>();
            pointManager.AddPoint(records[i]);
        }

        // マイスコア

        m_myScoreObj = transform.FindChild("myScore");
        m_myScoreDefaultColor = m_myScoreObj.FindChild("Rank").GetComponent<Image>().color;
        pointManager = m_myScoreObj.FindChild("PointManager").GetComponent<PointManager>();
        pointManager.AddPoint(m_myScore);
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

    // マイレコードへセット
    static public void SetMyScore( int record )
    {
        m_myScore = record;
        m_isMyScoreChanged = true;
    }
}
