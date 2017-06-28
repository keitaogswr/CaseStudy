using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCtrl : MonoBehaviour {

    public GameObject Charcter1;
    public GameObject Charcter2;

    private DrawGuide m_DG1;

    private enum STATE {
        NONE = 0,
        BOY,
        GIRL,
        MAX
    };

    private string[] Tutorial_Text =
    {
        "これはスコア。\n薬石を消すたびに増えていくよ",
        "メニューを押すとゲームが一時停止して、メニュー画面が開くよ",
        "薬石を3つ揃えて薬の素材にしよう",
        "薬石を3つ揃えるときえるよ♪",
        "タップしたところに薬石が挟み込まれるよ",
        "連鎖するとゲージがたまるよ♪",
        "次の薬石が確認できるよ"
    };

    private STATE state;
    private int page = 0;

    // Use this for initialization
    void Start () {
        state = STATE.NONE;
        m_DG1 = Charcter1.GetComponent<DrawGuide>();
    }
	
	// Update is called once per frame
	void Update () {
        //左クリックされたら
        if (page < Tutorial_Text.Length)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_DG1.SetText(Tutorial_Text[page]);
                ++page;
            }
        }
    }
}
