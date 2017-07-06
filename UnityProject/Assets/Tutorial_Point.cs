using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Point : MonoBehaviour {

    public List<Vector2> T_Point;
    private Icon_Move Tutorial_Icon;
    private int num = 0;
	// Use this for initialization
	void Start () {
        Tutorial_Icon = GameObject.Find("Icon").GetComponent<Icon_Move>();
    }
	
	// Update is called once per frame
	void Update () {
        Tutorial_Icon.SetTargetPos(T_Point[num]);
    }

    public void SetNum(int page)
    {
        num = page;
    }
}
