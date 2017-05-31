using UnityEngine;
using System.Collections.Generic;

public class Point_UI_Manager : MonoBehaviour {

    [SerializeField]
    List<GameObject> numberPointList;

    private GameObject pointManager;

    // Use this for initialization
    void Start () {
        pointManager = transform.FindChild("PointManager").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        PointManager manager = pointManager.GetComponent<PointManager>();

        int point = manager.GetPoint();
        int workPoint;

        for(int i = 0; i < numberPointList.Count; i++)
        {
            workPoint = point % 10;

            Point number = numberPointList[i].GetComponent<Point>();
            number.SetNumber(workPoint);

            point = point / 10;
        }
    }
}
