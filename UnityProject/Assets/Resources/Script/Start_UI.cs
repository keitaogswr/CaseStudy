using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_UI : MonoBehaviour {

    public float second = 1.0f;
    public float rotSoeed = 10.0f;
    public Vector3 TargetPoint;
    public float ScalingValue = 0.01f;
    public Pauseable Pause;

    private Vector3 Addend;
    private int counter = 0;
    private bool scalEnd = false;

    const float FPS = 60.0f;

    // Use this for initialization
    void Start () {
        Addend = (TargetPoint - transform.localPosition) / (second * FPS);
        Pause.pausing = true;
    }
	
	// Update is called once per frame
	void Update () {
		if (counter != second * FPS)
        {
            ++counter;
            transform.localPosition += Addend;
        }
        else
        {
            if (scalEnd == false)
            {
                transform.localScale -= new Vector3(ScalingValue, ScalingValue, ScalingValue);
                transform.Rotate(new Vector3(0.0f, 0.0f, rotSoeed));
                if (transform.localScale.x <= 0.0f && transform.localScale.y <= 0.0f)
                {
                    scalEnd = true;
                    Pause.pausing = false;
                }
            }
        }
	}

    bool GetFinishFlag()
    {
        return scalEnd;
    }
}
