using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientColor : MonoBehaviour {

    // グラデーションキー
    [ System.Serializable ]
    public class GradientElement
    {
        public GradientElement( Color color, float time )
        {
            this.color = color;
            this.time = time;
        }

        public Color color;
        public float time;
    };

    public float gradientTime;                                              // グラデーションが一周する時間
    public List<GradientElement> element = new List<GradientElement>();     // グラデーションキー

    private Gradient gradient;          // グラデーションマネージャ
    private float count;                // カウンタ

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
        count += Time.deltaTime;
        if ( count >= gradientTime )
        {
            count -= gradientTime;
        }
	}

    public Color GetColor()
    {
        return gradient.Evaluate(count / gradientTime);
    }
}
