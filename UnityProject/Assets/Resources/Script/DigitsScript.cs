using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DigitsScript : MonoBehaviour {

    public Vector2 size = new Vector2(1.0f, 1.0f);
    public Color color = new Color();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        foreach (RectTransform t in transform)
        {
            t.sizeDelta = size;
            t.GetComponent<Image>().color = color;
        }
    }
}
