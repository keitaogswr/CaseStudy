using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DrawGuide : MonoBehaviour {

    private float Inertia = 0.08f;

    [SerializeField]
    private Text GuideText;
    [SerializeField]
    private RectTransform Hukidasi;
    
    private bool ScaleTermination = false;
    private Vector3 defScal;
    private bool Reverse = false;
    private bool ReverseOld = false;

    // Use this for initialization
    void Start () {
        defScal = Hukidasi.localScale;
        Hukidasi.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
        if (ScaleTermination)
            Hukidasi.localScale += (defScal - Hukidasi.localScale) * Inertia;
        else
            Hukidasi.localScale += (new Vector3(0.0f, 0.0f, 0.0f) - Hukidasi.localScale) * Inertia;
    }

    public void SetText(string text)
    {
        GuideText.text = text;
        ScaleTermination = false;
        Hukidasi.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void SetReverse(bool reverse)
    {
        Reverse = reverse;
        if (Reverse != ReverseOld)
            defScal.x *= -1.0f;

        ReverseOld = Reverse;
    }

    public void SetScaleTermination(bool scaleTermination)
    {
        ScaleTermination = scaleTermination;
    }
}
