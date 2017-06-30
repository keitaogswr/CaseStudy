using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour {

    [SerializeField]
    private GameMain m_GameMain;
    private Image m_Image;
	// Use this for initialization
	void Start () {
        m_Image = GetComponent<Image>();
        m_Image.color = new Color(1f, 1f, 1f, 0f);
    }
	
	// Update is called once per frame
	void Update () {
        Color temp;
        float alfaSpeed;
        temp = m_Image.color;
        if (m_GameMain.Phase != PHASE.STAY)
        {
            alfaSpeed = 0.1f;
        }
        else
        {
            alfaSpeed = -0.1f;
        }
        temp.a += alfaSpeed;
        temp.a = temp.a > 1.0f ? 1.0f : temp.a;
        temp.a = temp.a < 0.0f ? 0.0f : temp.a;

        m_Image.color = temp;
    }
}
