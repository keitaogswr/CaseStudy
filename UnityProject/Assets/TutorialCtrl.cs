using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCtrl : MonoBehaviour {

    public GameObject Charcter1;
    public GameObject Charcter2;
    public Canvas SelectCanvas;

    private DrawGuide m_DG1;
    private DrawGuide m_DG2;
    private GameObject point;

    private enum Scene
    {
        Game = 0,
        Title,
    };
    private string[] Scene_Name =
    {
        "game",
        "title",
    };

    private enum CHARCTER
    {
        BOY = 0,
        GIRL,
    };

    private string[] Tutorial_Text =
    {
        "はさんでウィッチの\n世界へようこそ！",
        "これからこのゲームの\n世界について\n説明するね！",
        "これは薬石といって、\n薬を作るのに必要なんだ",
        "薬石はタテ、ヨコ3つ\n揃えるときえるよ",
        "フリックしたところに\n薬石が挟み込まれるよ",
        "薬石を消していくと\nゲージがたまるよ♪",
        "ゲージがたまると、\nタップした行を\nまとめて消せるよ！",
        "ここで次の薬石が\n確認できるよ",
        "これはスコア。\n薬石を消すたびに\n増えていくよ",
        "メニューを押すと\nゲームが一時停止して、\nメニュー画面が開くよ",
        "たくさん薬石を消して、\nハイスコアを目指そう！"
    };

    private CHARCTER[] Responsible = { CHARCTER.BOY,
                                       CHARCTER.GIRL,
                                       CHARCTER.BOY,
                                       CHARCTER.BOY,
                                       CHARCTER.BOY,
                                       CHARCTER.GIRL,
                                       CHARCTER.GIRL,
                                       CHARCTER.GIRL,
                                       CHARCTER.BOY,
                                       CHARCTER.BOY,
                                       CHARCTER.GIRL };
    private int page = 0;

    // Use this for initialization
    void Start () {
        m_DG1 = Charcter1.GetComponent<DrawGuide>();
        m_DG2 = Charcter2.GetComponent<DrawGuide>();
        point = GameObject.Find("Tutorial_Point");
        SelectCanvas.enabled = false;
        point.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        //左クリックされたら
        if (page < Tutorial_Text.Length)
        {
            if (page == 0 && GameObject.Find("Fade").GetComponent<fadeScript>().GetFadeMode() == 0)
            {
                m_DG1.SetText(Tutorial_Text[page]);
                m_DG1.SetReverse(false);
                m_DG1.SetScaleTermination(true);
                m_DG2.SetScaleTermination(false);
                ++page;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (Responsible[page] == CHARCTER.BOY)
                {
                    m_DG1.SetText(Tutorial_Text[page]);
                    m_DG1.SetReverse(false);
                    m_DG1.SetScaleTermination(true);
                    m_DG2.SetScaleTermination(false);
                }
                else if (Responsible[page] == CHARCTER.GIRL)
                {
                    m_DG2.SetText(Tutorial_Text[page]);
                    m_DG2.SetReverse(false);
                    m_DG1.SetScaleTermination(false);
                    m_DG2.SetScaleTermination(true);
                }
                AudioManager.Instance.PlaySE("動作音_1");
                ++page;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectCanvas.enabled = true;
            }
        }

        if (page > 2) {
            point.SetActive(true);
            Tutorial_Point tutorial_point = point.GetComponent<Tutorial_Point>();
            tutorial_point.SetNum(page-1);
        }

        if (page >= 11)
        {
            Charcter1.transform.localPosition += new Vector3(0, 0 - Charcter1.transform.localPosition.y, 0) * 0.2f;
            Charcter2.transform.localPosition += new Vector3(0, 0 - Charcter2.transform.localPosition.y, 0) * 0.2f;
        }
        else if (page >= 9)
        {
            Charcter1.transform.localPosition += new Vector3(0, - 350 - Charcter1.transform.localPosition.y, 0) * 0.2f;
            Charcter2.transform.localPosition += new Vector3(0, - 350 - Charcter2.transform.localPosition.y, 0) * 0.2f;
        }
        else if (page >= 6)
        {
            Charcter1.transform.localPosition += new Vector3(0, 140 - Charcter1.transform.localPosition.y, 0) * 0.2f;
            Charcter2.transform.localPosition += new Vector3(0, 140 - Charcter2.transform.localPosition.y, 0) * 0.2f;
        }
    }

    public void TransitionGame()
    {
        GameObject.Find("Fade").GetComponent<fadeScript>().SetFade(Scene_Name[(int)Scene.Game]);
    }

    public void TransitionTitle()
    {
        GameObject.Find("Fade").GetComponent<fadeScript>().SetFade(Scene_Name[(int)Scene.Title]);
    }
}
