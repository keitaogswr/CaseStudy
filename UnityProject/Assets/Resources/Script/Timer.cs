using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public float GameTime = 60;
    [SerializeField]
    List<GameObject> numberPointList;
    public Pauseable Pause;

    private float Minutes = 0;
    private float Seconds = 0;
    private float FPS = 60;
    private bool timeOut = false;

    // Use this for initialization
    void Start () {
        Minutes = GameTime / FPS;       //  分取得。
        Seconds = GameTime % FPS;       //  秒取得。
    }
	
	// Update is called once per frame
	void Update () {
        //  タイム減少。
        if (timeOut == false && Pause.pausing == false) {
            Seconds -= Time.deltaTime;
            if (Seconds < 0.0f)
            {       //  秒が0になったら分を削って秒を最大に
                --Minutes;
                if (Minutes < 0 && Seconds <= 0)
                {
                    timeOut = true;
                    Pause.pausing = true;
                }
                else
                {
                    Seconds = FPS - 1.0f;
                }
            }
        }
        else if (timeOut == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject.Find("Fade").GetComponent<fadeScript>().SetFade("ranking");
            }
        }

        int second = (int)Seconds;
        int minutes = (int)Minutes;
        int work;

        for (int i = 0; i < numberPointList.Count; i++) {
            if (i < 2) {    //  秒の計算
                work = second % 10;
                Point number = numberPointList[i].GetComponent<Point>();
                number.SetNumber(work);
                second = second / 10;
            }
            else {      //  分の計算
                work = minutes % 10;
                Point number = numberPointList[i].GetComponent<Point>();
                number.SetNumber(work);
                minutes = minutes / 10;
            }
        }
    }
}
