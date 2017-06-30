using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public float GameTime = 60;
    [SerializeField]
    List<GameObject> numberPointList;
    public Pauseable Pause;
    public GameMain gameMain;

    private float Minutes = 0;
    private float Seconds = 0;
    private const float FPS = 60;
    private bool timeOut = false;
    private bool stop = false;


    private GameObject finish;

    // Use this for initialization
    void Start () {
        Minutes = GameTime / FPS;       //  分取得。
        Seconds = GameTime % FPS;       //  秒取得。

        finish = GameObject.Find("Start&Finish_Canvas").transform.Find("Finish").gameObject;
        finish.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //  タイム減少。
        if (timeOut == false && Pause.pausing == false)
        {
            Seconds -= Time.deltaTime;
            if (Seconds < 0.0f)
            {       //  秒が0になったら分を削って秒を最大に
                --Minutes;
                if (Minutes < 0 && Seconds <= 0)
                {
                    timeOut = true;
                    finish.SetActive(true);
                }
                else
                {
                    Seconds = FPS;
                }
            }
        }
        else if (timeOut == true)
        {
            if (gameMain.Phase == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PointManager pointManager = GameObject.Find("PointManager").GetComponent<PointManager>();
                    rankingScore.SetMyScore(pointManager.GetPoint());

                    GameObject.Find("Fade").GetComponent<fadeScript>().SetFade("ranking");
                }
            }
        }

        if (timeOut == false)
        {
            ResetTime();
        }
        else
        {
            Seconds = FPS;
        }


        if (timeOut == true && gameMain.Phase == 0)
        {
            Pause.pausing = true;
        }

        if (timeOut == false)
        {
            int second = (int)Seconds;
            int minutes = (int)Minutes;
            int work;

            for (int i = 0; i < numberPointList.Count; i++)
            {
                if (i < 2)
                {    //  秒の計算
                    work = second % 10;
                    Point number = numberPointList[i].GetComponent<Point>();
                    number.SetNumber(work);
                    second = second / 10;
                }
                else
                {      //  分の計算
                    work = minutes % 10;
                    Point number = numberPointList[i].GetComponent<Point>();
                    number.SetNumber(work);
                    minutes = minutes / 10;
                }
            }
        }
    }

    void SetTimeStop(bool timeStop)
    {
        stop = timeStop;
    }

    void ResetTime()
    {
        int second = (int)Seconds;
        int minutes = (int)Minutes;
        int work;

        for (int i = 0; i < numberPointList.Count; i++)
        {
            if (i < 2)
            {    //  秒の計算
                work = second % 10;
                Point number = numberPointList[i].GetComponent<Point>();
                number.SetNumber(work);
                second = second / 10;
            }
            else
            {      //  分の計算
                work = minutes % 10;
                Point number = numberPointList[i].GetComponent<Point>();
                number.SetNumber(work);
                minutes = minutes / 10;
            }
        }
    }

    public void AddTimeSecond(float time)
    {
        Seconds += time;

        if (Seconds >= FPS)
        {
            Seconds -= FPS;
            ++Minutes;

            ResetTime();
        }
    }
}
