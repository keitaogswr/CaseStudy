using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour {

    private AudioManager audioManager;
    public Toggle BGMToggle;
    public Toggle SEToggle;

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
}
