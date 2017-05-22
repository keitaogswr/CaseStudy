using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSceneButton : MonoBehaviour {

    public string nextScene = null;

    public void OnClickMoveScene()
    {
        Debug.Log("MoveTo:" + nextScene);
        GameObject.Find("Fade").GetComponent<fadeSqript>().SetFade(nextScene);
    }
}
