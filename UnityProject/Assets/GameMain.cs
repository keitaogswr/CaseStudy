using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FIELD
{
    public bool Alive;
    public bool Break;
    public bool Lock;
    public GameObject Cube;
};

public enum PHASE
{
    STAY = 0,
    PUSH,
    SERACH,
    VANISH,
    DROP,
    GENERATE,
};


public enum CountType
{
    Y_Count = 0,
    X_Count,
};

public class GameMain : MonoBehaviour {

    public int gridWidth;       //オブジェクトの横配列量
    public int gridHeight;      //オブジェクトの縦配列量
    public GameObject Prefab;
    // Use this for initialization

    public float span;

    public PHASE Phase;

    //GameObject[,] Blocks = new GameObject[5, 10];

    float time = 0.0f;

    public FIELD[,] Field = new FIELD[7,10];

    int BlockCounter = 0;

    void Start() {
        Phase = PHASE.STAY;
        for (int x = -(gridWidth/2); x < gridWidth - (gridWidth / 2); x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Debug.Log(x);
                GameObject g = Instantiate(Prefab, new Vector3(x, y + 1, 0), Quaternion.identity) as GameObject;

                //生成したオブジェクとの親にこのオブジェクトを設定
                g.transform.parent = gameObject.transform;

                SetCubeData(x + (gridWidth / 2), y, g);
            }
        }
	}

    // Update is called once per frame
    void Update()
    {
        //int Cube_Cnt = 0;
        int x, y;

        bool VanishCaller = false;

        //GameObject FirstObj, NextObj;

        time += Time.deltaTime;
        if (time > span)
        {
            time = 0;
            for (x = 0; x < gridWidth; x++)
            {
                for(y = 0;y < gridHeight;y++)
                {
                    if(Field[x,y].Cube != null)
                    {
                        //Debug.Log("Field[" + x + "," + y + "]:" + Field[x, y].Cube.GetComponent<Block>().CubeName);
                    }
                    else
                    {
                        //Debug.Log("Field[" + x + "," + y + "]:キューブは存在しません");
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            var tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var collition2d = Physics2D.OverlapPoint(tapPoint);

            if (collition2d)
            {
                var hit = Physics2D.Raycast(tapPoint, -Vector2.up);
                if (hit)
                {
                    Debug.Log("HitObj's Color = " + Field[(int)hit.collider.gameObject.transform.position.x + 3, (int)hit.collider.gameObject.transform.position.y].Cube.GetComponent<Block>().CubeName);
                }
            }
        }

        switch (Phase)
        {
            case PHASE.STAY:
                if (Input.GetMouseButtonDown(0))
                {
                    var tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    var collition2d = Physics2D.OverlapPoint(tapPoint);

                    if (collition2d)
                    {
                        var hit = Physics2D.Raycast(tapPoint, -Vector2.up);
                        if (hit)
                        {
                            int C_X = (int)hit.collider.gameObject.transform.position.x + 3;
                            int C_Y = (int)hit.collider.gameObject.transform.position.y;
                            Destroy(Field[(int)hit.collider.gameObject.transform.position.x + 3, (int)hit.collider.gameObject.transform.position.y].Cube);

                            GameObject g = Instantiate(Prefab, new Vector3(C_X - 3, C_Y, 0), Quaternion.identity) as GameObject;

                            //生成したオブジェクとの親にこのオブジェクトを設定
                            g.transform.parent = gameObject.transform;

                            SetCubeData(C_X, C_Y, g);
                        }
                    }

                    Debug.Log("クリック！");
                    Phase = PHASE.SERACH;
                }
                break;
            case PHASE.PUSH:
                break;
            case PHASE.SERACH:
                Debug.Log("サーチフェイズ");

                VanishCaller = false;

                for (x = 0; x < gridWidth; x++)
                {
                    for (y = 0; y < gridHeight; y++)
                    {
                        BlockCounter = 0;
                        CountBlocks(x, y, CountType.X_Count);

                        BlockCounter = 0;
                        CountBlocks(x, y, CountType.Y_Count); 
                    }
                }
                Phase = PHASE.VANISH;
                break;
            case PHASE.VANISH:
                Debug.Log("ヴァニッシュフェイズ");
                for (x = 0; x < gridWidth; x++)
                {
                    for (y = 0; y < gridHeight; y++)
                    {
                        if (Field[x, y].Break == true)
                        {
                            Destroy(Field[x, y].Cube, 0.1f);
                            Field[x, y].Alive = false;
                            Field[x, y].Break = false;

                            VanishCaller = true;

                            Debug.Log("X" + x + "Y" + y);
                        }
                    }
                }
                Phase = PHASE.DROP;
                break;
            case PHASE.DROP:
                int DropCnt = 0;
                int EmpCnt = 0;
                for (x = 0; x < gridWidth; x++)
                {
                    for (y = 0; y < gridHeight; y++)
                    {
                        //フィールドに空きを見つけた
                        if (Field[x, y].Alive == false)
                        {
                            DropCnt++;
                            //空き領域より上にブロックが存在するか探す
                            for(EmpCnt = y + 1;EmpCnt < gridHeight; EmpCnt++)
                            {
                                if(Field[x,EmpCnt].Alive == true)
                                {
                                    Debug.Log("列:" + x + "空き領域 :" + EmpCnt + "移動量:" + DropCnt);
                                    break;
                                }
                                Debug.Log("列:" + x + "空き領域 :" + EmpCnt + "移動量:" + DropCnt);
                                //空き領域までの数をカウント
                                DropCnt++;
                            }
                            Debug.Log("抜けてる");
                            //存在しない場合はカウントが不必要になるので0へ
                            if (EmpCnt == gridHeight)
                            {
                                DropCnt = 0;
                            }

                            if(DropCnt > 0)
                            {
                                for(int i = EmpCnt;EmpCnt < gridHeight;EmpCnt++)
                                {
                                    Debug.Log("移動してる行：" + EmpCnt + "下がった量:" + DropCnt);
                                    Field[x, EmpCnt - DropCnt] = Field[x, EmpCnt];
                                    Field[x, EmpCnt].Cube = null;
                                    Field[x, EmpCnt].Alive = false;
                                }
                                DropCnt = 0;
                            }
                        }
                    }
                }
                Phase = PHASE.GENERATE;
                break;
            case PHASE.GENERATE:
                for(x = 0;x < gridWidth;x++)
                {
                    for(y = 0;y < gridHeight;y++)
                    {
                        if(Field[x,y].Cube == null)
                        {
                            GameObject g = Instantiate(Prefab, new Vector3(x - 3, y + 10, 0), Quaternion.identity) as GameObject;

                            //生成したオブジェクとの親にこのオブジェクトを設定
                            g.transform.parent = gameObject.transform;

                            SetCubeData(x , y, g);
                        }
                    }
                }
                if(VanishCaller == false)
                {
                    Phase = PHASE.STAY;
                }
                else
                {
                    Phase = PHASE.SERACH;
                }
                
                break;
        }       
    }

    private void GenerateCube()
    {
        for (int x = -(gridWidth / 2); x < gridWidth - (gridWidth / 2); x++)
        {
            GameObject g = Instantiate(Prefab, new Vector3(x * 0.7f, 0 + 10, 0), Quaternion.identity) as GameObject;

            //生成したオブジェクとの親にこのオブジェクトを設定
            g.transform.parent = gameObject.transform;
        }
    }

    public void SetCubeData(int X, int Y, GameObject Obj)
    {
        Field[X, Y].Cube = Obj;
        Field[X, Y].Alive = true;
        Field[X, Y].Break = false;
    }

    private void CountBlocks(int X, int Y, CountType Type)
    {
        BlockCounter++;

        if (Field[X, Y].Cube != null)
        {
            switch (Type)
            {
                case CountType.X_Count:
                    if (X + 1 < gridWidth && Field[X + 1, Y].Cube != null && Field[X + 1, Y].Alive == true && Field[X + 1, Y].Cube.GetComponent<Block>().CubeName == Field[X, Y].Cube.GetComponent<Block>().CubeName)
                    {
                        CountBlocks(X + 1, Y, CountType.X_Count);
                        Debug.Log("X_BlockCounter:" + BlockCounter);
                    }
                    break;
                case CountType.Y_Count:
                    if (Y + 1 < gridHeight && Field[X, Y + 1].Cube != null && Field[X, Y + 1].Alive == true && Field[X, Y + 1].Cube.GetComponent<Block>().CubeName == Field[X, Y].Cube.GetComponent<Block>().CubeName)
                    {
                        CountBlocks(X, Y + 1, CountType.Y_Count);
                        Debug.Log("Y_BlockCounter:" + BlockCounter);
                    }
                    break;
                default:
                    break;
            }

            if (BlockCounter >= 3)
            {
                Debug.Log("Field[" + X + "," + Y + "]:" + Field[X, Y].Cube.GetComponent<Block>().CubeName);
                Field[X, Y].Break = true;
            }
        }
    }
}
