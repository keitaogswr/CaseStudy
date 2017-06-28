using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Instance.PlayBGM("BGM02", true);
	}
}
