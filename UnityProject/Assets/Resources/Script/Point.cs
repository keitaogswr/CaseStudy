using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Point : MonoBehaviour {

    [SerializeField]
    List<Sprite> numberImageList;

    Image point;

    int nunberIndex;
	// Use this for initialization
	void Start () {
        point = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        point.sprite = numberImageList[nunberIndex];
    }

    public void SetNumber(int index)
    {
        nunberIndex = index;
    }
}
