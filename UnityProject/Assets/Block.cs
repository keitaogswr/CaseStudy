using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    public GameObject Cube;
    public string CubeName = "";
    bool move;

    //検索対象のマテリアル名
    string[] CubeMats = { "Red", "Blue", "Green","Yellow" };

    //追加用の色
    //,"Purple"

    GameMain Ctrl;

	// Use this for initialization
	void Start () {
        move = true;
        Ctrl = GameObject.Find("GameCtrl").GetComponent<GameMain>();
        CreateBlock();
	}

    void Update()
    {
        int x, y;
        if (move == true)
        {
            this.GetComponent<Rigidbody2D>().gravityScale = 10;
            //Debug.Log(this.GetComponent<Rigidbody2D>().velocity);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            //Debug.Log("Stop:" + this.GetComponent<Rigidbody2D>().velocity);
        }

        if (this.GetComponent<Rigidbody2D>().velocity.y < 0.05f)
        {
            x = ((int)(this.transform.position.x * 10.0f) / 7);
        }
    }

    // Update is called once per frame
    public void CreateBlock()
    {
        CubeName = CubeMats[Random.Range(0, CubeMats.Length)];

        Material CubeMat = Resources.Load("Materials/" + CubeName) as Material;

        this.GetComponent<Renderer>().material = CubeMat;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(Mathf.Round(this.transform.position.y));
    }

    public void ChangeBlock(string MatName)
    {
        Material CubeMat = Resources.Load("Materials/" + CubeName) as Material;

        this.GetComponent<Renderer>().material = CubeMat;
    }
    
}
