using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titleSqript : MonoBehaviour {
    public GameObject fadeObj;      // フェードのPrefab
    public string nextScene;        // 次のシーン

    // Use this for initialization
    void Start() {
        Debug.Log("タイトル開始");

        GameObject fade;                    // フェードの
        fade = null;                        // フェードをNULLに
        fade = GameObject.Find("Fade");     // フェードの有無を確認
        if (fade == null)
        {
            // ないなら生成
            GameObject obj  = Instantiate(fadeObj) as GameObject;
            obj.name = "Fade";
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log("ボタン入力確認");
        //    GameObject.Find("Fade").GetComponent<fadeSqript>().SetFade(nextScene);
        //}
    }
}
