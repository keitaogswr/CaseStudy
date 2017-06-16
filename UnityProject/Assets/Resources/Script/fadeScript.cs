using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;               // UIを使用可能にする
using UnityEngine.SceneManagement;  // 

public class fadeScript : MonoBehaviour {
    //---------------------------
    // 注意 : Fadeは配置の問題からHierarchyの上のほうの位置に置くこと
    //---------------------------
    private GameObject fade;        // 自分の
    private GameObject _child;      // パネルに使われるオブジェクト
    public Sprite texture;          // テクスチャ
    public float speed = 0.01f;     // 透明化の速さ
    public enum FADE_MODE           // フェードの状態
    {
        FADE_NONE = 0,
        FADE_IN,
        FADE_OUT,
    }

    FADE_MODE fadeMode;             // フェードの状態
    Color color;                    // 色の値
    string nextScene;               // 次のシーン


    // Startの前に行われる処理
    private void Awake()
    {
        // 子情報の取得
        _child = transform.FindChild("FadePanel").gameObject;
        // 破棄を無効化
        DontDestroyOnLoad(this.gameObject);
    }


    // Use this for initialization
    void Start() {
        nextScene   = null;           // 次のシーン初期化
        fadeMode = FADE_MODE.FADE_NONE;

        // 色の初期化
        color.r = _child.GetComponent<Image>().color.r;
        color.g = _child.GetComponent<Image>().color.g;
        color.b = _child.GetComponent<Image>().color.b;
        color.a = 0;

        SetTexture(texture);
    }

    // Update is called once per frame
    void Update() {

        if (fadeMode != FADE_MODE.FADE_NONE)
        {
            // 画面を暗転化する
            if (fadeMode == FADE_MODE.FADE_OUT)
            {
                _child.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a);
                color.a += speed;
            }
            // 画面を見えるように戻す
            else if (fadeMode == FADE_MODE.FADE_IN)
            {
                _child.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a);
                color.a -= speed;
            }

            // 制御
            if (color.a > 1.0f)
            {
                Debug.Log("画面暗転終了");
                // フェードINへ移行
                fadeMode = FADE_MODE.FADE_IN;
                // シーン移行
                SceneManager.LoadScene(nextScene);

            }
            else if (color.a < 0)
            {
                Debug.Log("フェード終了");
                // フェード終了
                color.a = 0;
                fadeMode = FADE_MODE.FADE_NONE;
            }
        }
    }


    // テクスチャの貼り付け
    void SetTexture( Sprite tex )
    {
        _child.GetComponent<Image>().sprite = tex;
    }


    // フェード状態取得
    public FADE_MODE GetFadeMode()
    {
        return fadeMode;
    }

    // フェードをセットする
    public bool SetFade(string next)
    {
        if (fadeMode == FADE_MODE.FADE_NONE)
        {
           Debug.Log("フェード呼び出し : " + next);

            color.a = 0;                    // アルファ値初期化
            nextScene = next;               // 次のシーン
            fadeMode = FADE_MODE.FADE_OUT;  // フェードOUTへ
            return true;
        }
        return false;
    }
}
