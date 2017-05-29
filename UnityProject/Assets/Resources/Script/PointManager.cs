using UnityEngine;
using System.Collections;

public class PointManager : MonoBehaviour {
    public int point = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void AddPoint(int num)
    {
        point += num;
    }

    public void SubPoint(int num)
    {
        point -= num;
    }

    public int GetPoint()
    {
        return point;
    }
}
