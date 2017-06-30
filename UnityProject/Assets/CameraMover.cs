using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

    private Vector3 OrignPos;
    private bool bQueiq;
    private float MoveTime,CurrentTime;

	// Use this for initialization
	void Start () {
        bQueiq = false;
        OrignPos = this.transform.position;
        MoveTime = 0;
        CurrentTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (bQueiq == true)
        {
            float x, y;
            x = Random.Range(-0.2f, 0.2f);
            y = Random.Range(-0.01f, 0.01f);

            this.transform.position += new Vector3(x, y, 0);

            CurrentTime += Time.deltaTime;

            if(MoveTime < CurrentTime)
            {
                bQueiq = false;
                MoveTime = 0;
                CurrentTime = 0;

                this.transform.position = OrignPos;
            }
        }
    }

    public void Queiq(float Time)
    {
        MoveTime = Time;
        bQueiq = true;
    }
}
