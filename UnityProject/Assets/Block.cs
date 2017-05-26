using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject Cube;
    public string CubeName = "";

    private Vector3 MovedPos;
    
    GameMain Ctrl;

	// Use this for initialization
	void Start ()
    {
        MovedPos = new Vector2(0, 10);
        Ctrl = GameObject.Find("GameCtrl").GetComponent<GameMain>();
        CreateBlock();
	}

    void Update()
    {
    }

    // Update is called once per frame
    public void CreateBlock()
    {
        CubeName = Ctrl.CubeMats[Random.Range(0, Ctrl.CubeMats.Length)];

        Material CubeMat = Resources.Load("Materials/" + CubeName) as Material;

        Debug.Log(CubeName);

        this.GetComponent<Renderer>().material = CubeMat;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(Mathf.Round(this.transform.position.y));
    }
    
    public void SetMovePos(Vector3 Pos)
    {
        MovedPos = Pos;
        //Debug.Log(MovedPos);
    }
}
