using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour {

    private AudioManager audioManager;
    public Toggle BGMToggle;
    public Toggle SEToggle;
    public Slider BGMSlider;
    public Slider SESlider;

    // Use this for initialization
    void Start () {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        bool exit = PlayerPrefs.HasKey("BGMPlay");
        if (exit == true)
        {
            int flag = PlayerPrefs.GetInt("BGMPlay");
            if (flag == 0)
            {
                BGMToggle.isOn = false;
            }
            else
            {
                BGMToggle.isOn = true;
            }
        }
        else
        {
            PlayerPrefs.SetInt("BGMPlay", 1);
        }

        exit = PlayerPrefs.HasKey("SEPlay");
        if (exit == true)
        {
            int flag = PlayerPrefs.GetInt("SEPlay");
            if (flag == 0)
            {
                SEToggle.isOn = false;
            }
            else
            {
                SEToggle.isOn = true;
            }
        }
        else
        {
            PlayerPrefs.SetInt("SEPlay", 1);
        }

        exit = PlayerPrefs.HasKey("BGMValue");
        if (exit == true)
        {
            BGMSlider.value = PlayerPrefs.GetFloat("BGMValue");
        }
        else
        {
            PlayerPrefs.SetFloat("BGMValue", 1.0f);
        }

        exit = PlayerPrefs.HasKey("SEValue");
        if (exit == true)
        {
            SESlider.value = PlayerPrefs.GetFloat("SEValue");
        }
        else
        {
            PlayerPrefs.SetFloat("SEValue", 1.0f);
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetVolumeBGM(float val)
    {
        audioManager.SetVolumeBGM(val);
    }
    public void SetVolumeSE(float val)
    {
        audioManager.SetVolumSE(val);
    }
    public void NoVolumeBGM(bool val)
    {
        audioManager.NoVolumeBGM(val);
    }
    public void NoVolumeSE(bool val)
    {
        audioManager.NoVolumeSE(val);
    }

    public void OnClick()
    {
        AudioManager.Instance.PlaySE("動作音_1");
    }
}
