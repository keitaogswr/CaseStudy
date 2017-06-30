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
        "はさんでウィッチの世界へ\nようこそ！",
        "これからこのゲームの世界に\nついて説明するね！",
        "これはスコア。\n薬石を消すたびに増えていくよ",
        "メニューを押すとゲームが一時停止して、メニュー画面が開くよ",
        "薬石を3つ揃えて薬の素材にしよう",
        "薬石を3つ揃えるときえるよ♪",
        "タップしたところに薬石が挟み込まれるよ",
        "連鎖するとゲージがたまるよ♪",
        "ここで次の薬石が確認できるよ"
    };

    private CHARCTER[] Responsible = { CHARCTER.BOY,
                                       CHARCTER.BOY,
                                       CHARCTER.BOY,
                                       CHARCTER.BOY,
                                       CHARCTER.BOY,
                                       CHARCTER.GIRL,
                                       CHARCTER.GIRL,
                                       CHARCTER.GIRL,
                                       CHARCTER.BOY };
    private int page = 0;

    // Use this for initialization
    void Start () {
        m_DG1 = Charcter1.GetComponent<DrawGuide>();
        m_DG2 = Charcter2.GetComponent<DrawGuide>();
        SelectCanvas.enabled = false;
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
                
                ++page;
            }
        }
        else
        {
            SelectCanvas.enabled = true;
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
