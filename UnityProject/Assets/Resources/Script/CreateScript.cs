using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateScript : MonoBehaviour {
    public GameObject[] prefab;
    static private GameObject[] obj;
	// Use this for initialization
	void Awake() {
        obj = new GameObject[prefab.Length];

        for (int i = 0; i < prefab.Length; i++)
        {
            obj[i] = null;
            obj[i] = GameObject.Find(prefab[i].name);

            if (obj[i] == null)
            {
                // ないなら生成
                obj[i] = Instantiate(prefab[i]) as GameObject;
                obj[i].name = prefab[i].name;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
