using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject Cube;
    public string CubeName = "";

    public Vector3 StartPos;
    public Vector3 MovedPos;
    
    GameMain Ctrl;

    private void Awake()
    {
        StartPos = new Vector3(0, 0, 0);
        MovedPos = new Vector3(0, 0, 0);
    }

    // Use this for initialization
    void Start ()
    {
        Ctrl = GameObject.Find("GameCtrl").GetComponent<GameMain>();
        //CreateBlock();
	}

    void Update()
    {
    }

    // Update is called once per frame
    public void CreateBlock()
    {
        CubeName = Ctrl.CubeMats[Random.Range(0, Ctrl.CubeMats.Length)];

        Material CubeMat = Resources.Load("Materials/" + CubeName) as Material;

        //Debug.Log(CubeName);

        this.GetComponent<Renderer>().material = CubeMat;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void SetStartPos(Vector3 Pos)
    {
        StartPos = Pos;
        //Debug.Log(MovedPos);
    }

    public void SetMovePos(Vector3 Pos)
    {
        MovedPos = Pos;
        //Debug.Log(MovedPos);
    }
}
