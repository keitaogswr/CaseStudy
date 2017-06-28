using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Instance.PlayBGM("BGM01", true);
	}

}
