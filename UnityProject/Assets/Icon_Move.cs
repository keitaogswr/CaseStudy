using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon_Move : MonoBehaviour {

    public float Inertia = 0.02f;
    private Vector2 TargetPoint;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 localPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        localPos += (TargetPoint - localPos) * Inertia;
        transform.localPosition = localPos;
    }

    public void SetTargetPos(Vector2 pos)
    {
        TargetPoint = pos;
    }
}
