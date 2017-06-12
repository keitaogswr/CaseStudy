using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public float GameTime = 60;
    [SerializeField]
    List<GameObject> numberPointList;

    private float Minutes = 0;
    private float Seconds = 0;
    private float FPS = 60;

    // Use this for initialization
    void Start () {
        Minutes = GameTime / FPS;       //  分取得。
        Seconds = GameTime % FPS;       //  秒取得。
    }
	
	// Update is called once per frame
	void Update () {
        //  タイム減少。
        Seconds -= Time.deltaTime;
        if (Seconds < 0.0f) {       //  秒が0になったら分を削って秒を最大に
            --Minutes;
            Seconds = FPS - 1.0f;
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
