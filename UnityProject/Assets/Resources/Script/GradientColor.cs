using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// グラデーションキー
[System.Serializable]
public class GradientElement
{
    public GradientElement() { }
    public GradientElement(Color color, float time)
    {
        this.color = color;
        this.time = time;
    }

    public Color color;
    public float time;
};

public class GradientColor : MonoBehaviour {

    public float gradientTime;                                              // グラデーションが一周する時間
    public List<GradientElement> element = new List<GradientElement>();     // グラデーションキー

    private Gradient gradient;          // グラデーションマネージャ

	// Use this for initialization
	void Start () {

        gradient = new Gradient();

        // キー値が２つ以上じゃなきゃダメ
        Debug.Assert(element.Count >= 2);

        GradientColorKey[] gck = new GradientColorKey[element.Count];
        GradientAlphaKey[] gak = new GradientAlphaKey[element.Count];

        for( int i = 0; i < element.Count; i++ )
        {
            gck[i].color = element[i].color;
            gck[i].color.a = 1.0f;
            gck[i].time = element[i].time;

            gak[i].alpha = element[i].color.a;
            gak[i].time = element[i].time;
        }

        gradient.SetKeys(gck, gak);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public Color GetColor()
    {
        float theta = 2 * Mathf.PI * Mathf.Repeat(Time.time, gradientTime) / gradientTime;
        return gradient.Evaluate(Mathf.Abs(Mathf.Sin( theta )));
    }
}
