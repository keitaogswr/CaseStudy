using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct FIELD
{
    public bool Alive;
    public bool Break;
    public Vector2 Pos;
    public GameObject Cube;

    public string CubeName;
};

public enum PHASE
{
    STAY = 0,
    R_PUSH,
    L_PUSH,
    SP_PUSH,
    SERACH,
    VANISH,
    SLIDE,
    DROP,
    GENERATE,
};

public struct ARM
{
    public Vector3 Pos;
    public Vector3 StartPos;
    public Vector3 EndPos;
    public GameObject Arm;
}


public enum CountType
{
    Y_Count = 0,
    X_Count,
};

public class GameMain : MonoBehaviour {

    //検索対象のマテリアル名
    public string[] CubeMats = { "Red", "Blue", "Green", "Yellow" };
    // , "Purple"

    public int gridWidth;       //オブジェクトの横配列量
    public int gridHeight;      //オブジェクトの縦配列量
    public float MoveSpeed;     //オブジェクトの動く速度
    public GameObject Prefab;   //配置するオブジェク
    public GameObject NextBlock;//UI用の次のブロック
    public GameObject NextBlock1;//UI用の次のブロック
    public GameObject NextBlock2;//UI用の次のブロック

    public GameObject Arm_R;
    public GameObject Arm_L;

    public GameObject VanishEffect; //ブロック破壊時に出すエフェクト

    //更新周期
    public float span;

    //ゲームのフェイズ
    public PHASE Phase;

    //GameObject[,] Blocks = new GameObject[5, 10];

    //フレームの変数
    float time = 0.0f;
    float PushTime = 0;
    float SlideTime = 0;
    float ArmTime = 0;
    bool ArmTurn = false;

    //ゲームのフィールド管理用配列
    public FIELD[,] Field = new FIELD[7,7];

    //ブロックの連立数を計測するための変数
    int BlockCounter = 0;

    //連鎖を起こしたい
    bool VanishCaller = false;

    //各処理を一回のみ処理させ、状態遷移まで待機にするフラグ
    bool Action = false;

    //差し込み用ブロックの待機列
    private string[] NextBlocks = new string[4];

    //タップした場所の変数入れ
    private int TapPoint_X;
    private int TapPoint_Y;

    ARM[] ARM_L = new ARM[7];
    ARM[] ARM_R = new ARM[7];

    private berScript Ber;
    private Timer Timer;
    private int vanishCount;
    public int addTimeVanishCount = 10;
	public float addTime = 0.5f;



    private Vector2 tapPosition;
    private bool tapped;
    public const float flickLength = 20;
    public const float flickAngle = 30;



    void Start()
    {
        Phase = PHASE.STAY;

        Ber = GameObject.Find("Ber").GetComponent<berScript>();
        Timer = GameObject.Find("Timer").GetComponent<Timer>();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //Debug.Log(x);
                Field[x, y].Pos = new Vector2(x - (gridWidth / 2), y);

                //GameObject g = Instantiate(Prefab, new Vector3(x - (gridWidth / 2), y + 1, 0), Quaternion.identity) as GameObject;

                GameObject g = RandColorCreateBlock(new Vector3(x - (gridWidth / 2), y, 0));

                //生成したオブジェクとの親にこのオブジェクトを設定
                g.transform.parent = gameObject.transform;

                SetCubeData(x, y, g);
            }
        }

        //各列毎に右左アームをセットで生成
        for (int y = 0;y < gridHeight;y++)
        {
            GameObject g = Instantiate(Arm_L, new Vector3(-4.5f,y,0), Quaternion.identity) as GameObject;
            ARM_L[y].Arm = g;
            ARM_L[y].StartPos = ARM_L[y].Arm.GetComponent<Transform>().localPosition;
            ARM_L[y].EndPos = ARM_L[y].Arm.GetComponent<Transform>().localPosition;

            g.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
            g.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);

            //生成したオブジェクとの親にこのオブジェクトを設定
            g.transform.parent = GameObject.Find("Canvas").transform;

            g = Instantiate(Arm_R, new Vector3(4.5f, y, 0), Quaternion.identity) as GameObject;
            ARM_R[y].Arm = g;
            ARM_R[y].StartPos = ARM_R[y].Arm.GetComponent<Transform>().localPosition;
            ARM_R[y].EndPos = ARM_R[y].Arm.GetComponent<Transform>().localPosition;

            g.GetComponent<RectTransform>().pivot = new Vector2(1,0.5f);

            //生成したオブジェクとの親にこのオブジェクトを設定
            g.transform.parent = GameObject.Find("Canvas").transform;
        }

        for(int i = 0;i < 4; i++)
        {
            NextBlocks[i] = CubeMats[Random.Range(0, CubeMats.Length)];
        }

        //挿入用ブロックにマテリアルを設定
        NextBlock.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[0]) as Material;
        NextBlock1.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[1]) as Material;
        NextBlock2.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[2]) as Material;
    }

    // Update is called once per frame
    void Update()
    {
        //int Cube_Cnt = 0;
        int x, y;
        Rigidbody2D Rigid;
        FIELD SlideWork;
        FIELD ClashWork;

        switch (Phase)
        {
            //待機
            case PHASE.STAY:

                vanishCount = 0;

                for (x = 0; x < gridWidth; x++)
                {
                    for (y = 0; y < gridHeight; y++)
                    {
                        if (Field[x, y].Cube != null)
                        {
                            //動けなくしていた部分をもとに戻す
                            Rigid = Field[x, y].Cube.GetComponent<Rigidbody2D>();
                            //解除
                            Rigid.constraints = RigidbodyConstraints2D.None;
                            //再設定
                            Rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
                            Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

                            Field[x, y].CubeName = Field[x, y].Cube.GetComponent<Block>().CubeName;
                        }
                    }
                }

                if ( Input.GetMouseButtonDown(0) )
                {
                    tapPosition = Input.mousePosition;
                    tapped = true;
                }
                else if ( Input.GetMouseButtonUp(0) )
                {
                    if ( ! tapped )
                    {
                        break;
                    }

                    tapped = false;

                    var tapPoint = Camera.main.ScreenToWorldPoint(tapPosition);
                    var collition2d = Physics2D.OverlapPoint(tapPoint);

                    if (!collition2d)
                    {
                        //２Dのあたり判定に重なっていない
                        break;
                    }

                    var hit = Physics2D.Raycast(tapPoint, -Vector2.up);
                    if (!hit)
                    {
                        //レイを飛ばして当たったオブジェクトがない
                        break;
                    }

                    Vector2 dir = ( Vector2 )Input.mousePosition - tapPosition;
                    
                    if (Ber.GetFillAmount() != 1 && dir.sqrMagnitude < flickLength * flickLength)
                    {
                        // ゲージマックスでなく、フリックとみなさない距離は無視
                        break;
                    }

                    dir.Normalize();



                    int Col_X = Mathf.RoundToInt(hit.collider.gameObject.transform.position.x + 3);
                    int Col_Y = Mathf.RoundToInt(hit.collider.gameObject.transform.position.y);

                    //int Col_X = Mathf.RoundToInt(collition2d.transform.position.x + 3);
                    //int Col_Y = Mathf.RoundToInt(collition2d.transform.position.y);

                    //オブジェクトが配置されている配列のXとY
                    TapPoint_X = Col_X;
                    TapPoint_Y = Col_Y;

                    //レンジアウト防止
                    if (TapPoint_X > gridWidth || TapPoint_X < 0)
                    {
                        break;
                    }
                    if (TapPoint_Y > gridHeight || TapPoint_Y < 0)
                    {
                        break;
                    }

                    if (Ber.GetFillAmount() == 1)
                    {
                        // スーパーアーム

                        Phase = PHASE.SP_PUSH;
                    }
                    else if (Vector2.Angle(dir, Vector2.right) < flickAngle)
                    {
                        // 右へフリック

                        Phase = PHASE.R_PUSH;
                    }
                    else if (Vector2.Angle(dir, Vector2.left) < flickAngle)
                    {
                        // 左へフリック

                        Phase = PHASE.L_PUSH;
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    var tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    var collition2d = Physics2D.OverlapPoint(tapPoint);

                    Instantiate(VanishEffect, new Vector3(tapPoint.x, tapPoint.y, -1), Quaternion.identity);

                    //２Dのあたり判定に重なっていたら
                    if (collition2d)
                    {
                        //レイを飛ばして当たったオブジェクトがあるなら
                        var hit = Physics2D.Raycast(tapPoint, -Vector2.up);
                        if (hit)
                        {
                            int Col_X = Mathf.RoundToInt(hit.collider.gameObject.transform.position.x + 3);
                            int Col_Y = Mathf.RoundToInt(hit.collider.gameObject.transform.position.y);

                            //オブジェクトが配置されている配列のXとY
                            TapPoint_X = Col_X;
                            TapPoint_Y = Col_Y;
                        }
                    }

                    Debug.Log("クリック！");
                    Field[(int)TapPoint_X, (int)TapPoint_Y].Cube.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[0]) as Material;
                    Field[(int)TapPoint_X, (int)TapPoint_Y].Cube.GetComponent<Block>().CubeName = NextBlocks[0];
                    //Phase = PHASE.PUSH;
                }
                break;

            //差し込み時の押し出し処理
            case PHASE.R_PUSH:

                if (Action != true)
                {
                    if (TapPoint_X + 1 < gridWidth)
                    {
                        if (TapPoint_Y + 1 < gridHeight)
                        {
                            for (int i = 0; i < gridWidth; i++)
                            {
                                //挿入するブロックの上のラインがあるのなら動けなくする
                                Rigid = Field[i, TapPoint_Y + 1].Cube.GetComponent<Rigidbody2D>();
                                Rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
                            }
                        }

                        //入れ替え用に変数を避難させる
                        SlideWork = Field[(int)TapPoint_X, (int)TapPoint_Y];

                        for (int i = TapPoint_X + 1; i < gridWidth; i++)
                        {
                            //右隣りのブロック情報をコピー
                            ClashWork = Field[i, TapPoint_Y];
                            //書き換え
                            Field[i, TapPoint_Y] = SlideWork;

                            //Field[i, TapPoint_Y].Cube.GetComponent<Transform>().localPosition += new Vector3(1, 0, 0);
                            Field[i, TapPoint_Y].Cube.GetComponent<Block>().SetStartPos(Field[i, TapPoint_Y].Cube.GetComponent<Transform>().localPosition);
                            Field[i, TapPoint_Y].Cube.GetComponent<Block>().SetMovePos(Field[i, TapPoint_Y].Cube.GetComponent<Transform>().localPosition += new Vector3(1, 0, 0));

                            SlideWork = ClashWork;
                        }

                        //はじき出されるオブジェクトに力を加える
                        Rigid = SlideWork.Cube.GetComponent<Rigidbody2D>();
                        Rigid.constraints = RigidbodyConstraints2D.None;
                        Rigid.AddForce(new Vector2(10, 0));
                        SlideWork.Cube.GetComponent<Transform>().localPosition += new Vector3(0.99f, 0, 0);
                        SlideWork.Cube.GetComponent<Transform>().localScale = new Vector3(0.9f, 0.9f, 0.5f);
                        Destroy(SlideWork.Cube, 1);

                        //挿入用オブジェクトのインスタンス化
                        //GameObject g = Instantiate(Prefab, new Vector3(TapPoint_X - (gridWidth / 2), TapPoint_Y, 0), Quaternion.identity) as GameObject;
                        GameObject g = Instantiate(Prefab, NextBlock.GetComponent<Transform>().localPosition + new Vector3(0, 0, 3), Quaternion.identity) as GameObject;

                        g.GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0.3f);

                        //生成したオブジェクとの親にこのオブジェクトを設定
                        g.transform.parent = gameObject.transform;

                        //Fieldにデータをセット
                        SetCubeData(TapPoint_X, TapPoint_Y, g);

                        g.GetComponent<Block>().SetStartPos(NextBlock.GetComponent<Transform>().localPosition);
                        g.GetComponent<Block>().SetMovePos(new Vector3(TapPoint_X - (gridWidth / 2), TapPoint_Y, 0));

                        g.GetComponent<Rigidbody2D>().simulated = false;

                        if (TapPoint_Y + 1 < gridHeight)
                        {
                            for (int i = 0; i < gridWidth; i++)
                            {
                                //動けなくしていた部分をもとに戻す
                                Rigid = Field[i, TapPoint_Y + 1].Cube.GetComponent<Rigidbody2D>();
                                //解除
                                Rigid.constraints = RigidbodyConstraints2D.None;
                                //再設定
                                Rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
                                Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                            }
                        }
                    }
                    //挿入したブロックに色を付ける
                    Field[(int)TapPoint_X, (int)TapPoint_Y].Cube.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[0]) as Material;
                    Field[(int)TapPoint_X, (int)TapPoint_Y].Cube.GetComponent<Block>().CubeName = NextBlocks[0];

                    Action = true;
                }


                //移動の線形補間部分
                for (x = 0; x < gridWidth; x++)
                {
                    if (Field[x, TapPoint_Y].Alive != false)
                    {
                        //Moveに何か入っているならば動かす
                        if (Field[x, TapPoint_Y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                        {
                            Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition = Vector3.Lerp(Field[x, TapPoint_Y].Cube.GetComponent<Block>().StartPos, Field[x, TapPoint_Y].Cube.GetComponent<Block>().MovedPos, PushTime);
                            if (PushTime == 1)
                            {
                                Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.y), 0);
                                Field[x, TapPoint_Y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));
                            }
                        }
                    }
                }


                if (Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                {
                    Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Transform>().localPosition = Vector3.Lerp(Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Block>().StartPos, Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Block>().MovedPos, PushTime * 2);
                    if (PushTime == 1)
                    {
                        Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.y), 0);
                        Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));
                    }
                }

                //タイムの加算
                time += Time.deltaTime;
                PushTime += MoveSpeed;

                //ArmTime += 0.25f;

                //押し込みの時間の制限
                if (PushTime > 1)
                {
                    PushTime = 1;
                }

                if (time > span)
                {
                    //挿入待ちブロックの色を更新
                    NextBlocks[0] = NextBlocks[1];

                    //配列をずらす
                    for (int i = 1; i < 3; i++)
                    {
                        NextBlocks[i] = NextBlocks[i + 1];
                    }

                    //最後尾にランダムでカラーの名前を入れておく
                    NextBlocks[3] = CubeMats[Random.Range(0, CubeMats.Length)];

                    //挿入用ブロックにマテリアルを設定
                    NextBlock.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[0]) as Material;
                    NextBlock1.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[1]) as Material;
                    NextBlock2.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[2]) as Material;

                    Field[(int)TapPoint_X, (int)TapPoint_Y].Cube.GetComponent<Transform>().localScale = new Vector3(1, 1, 0.3f);
                    Field[(int)TapPoint_X, (int)TapPoint_Y].Cube.GetComponent<Rigidbody2D>().simulated = true;

                    for (x = 0; x < gridWidth; x++)
                    {
                        if (Field[x, TapPoint_Y].Alive != false)
                        {
                            //Moveに何か入っているならば動かす
                            if (Field[x, TapPoint_Y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                            {
                                Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition = Vector3.Lerp(Field[x, TapPoint_Y].Cube.GetComponent<Block>().StartPos, Field[x, TapPoint_Y].Cube.GetComponent<Block>().MovedPos, 1);

                                Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.y), 0);
                                Field[x, TapPoint_Y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));
                            }
                        }
                    }

                    time = 0;
                    PushTime = 0;

                    Phase = PHASE.SERACH;
                    Action = false;
                }
                break;

            //差し込み時の押し出し処理
            case PHASE.L_PUSH:

                if (Action != true)
                {
                    if (TapPoint_X - 1 >= 0)
                    {
                        if (TapPoint_Y + 1 < gridHeight)
                        {
                            for (int i = 0; i < gridWidth; i++)
                            {
                                //挿入するブロックの上のラインがあるのなら動けなくする
                                Rigid = Field[i, TapPoint_Y + 1].Cube.GetComponent<Rigidbody2D>();
                                Rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
                            }
                        }

                        //入れ替え用に変数を避難させる
                        SlideWork = Field[(int)TapPoint_X, (int)TapPoint_Y];

                        for (int i = TapPoint_X - 1; i >= 0; i--)
                        {
                            //左隣りのブロック情報をコピー
                            ClashWork = Field[i, TapPoint_Y];
                            //書き換え
                            Field[i, TapPoint_Y] = SlideWork;

                            //Field[i, TapPoint_Y].Cube.GetComponent<Transform>().localPosition += new Vector3(1, 0, 0);
                            Field[i, TapPoint_Y].Cube.GetComponent<Block>().
                                SetStartPos(Field[i, TapPoint_Y].Cube.GetComponent<Transform>().localPosition);
                            Field[i, TapPoint_Y].Cube.GetComponent<Block>().
                                SetMovePos(Field[i, TapPoint_Y].Cube.GetComponent<Transform>().localPosition += new Vector3(-1, 0, 0));

                            SlideWork = ClashWork;
                        }

                        //はじき出されるオブジェクトに力を加える
                        Rigid = SlideWork.Cube.GetComponent<Rigidbody2D>();
                        Rigid.constraints = RigidbodyConstraints2D.None;
                        Rigid.AddForce(new Vector2(-10, 0));
                        SlideWork.Cube.GetComponent<Transform>().localPosition += new Vector3(-0.99f, 0, 0);
                        SlideWork.Cube.GetComponent<Transform>().localScale = new Vector3(0.9f, 0.9f, 0.5f);
                        Destroy(SlideWork.Cube, 1);

                        //挿入用オブジェクトのインスタンス化
                        //GameObject g = Instantiate(Prefab, new Vector3(TapPoint_X - (gridWidth / 2), TapPoint_Y, 0), Quaternion.identity) as GameObject;
                        GameObject g = Instantiate(Prefab, NextBlock.GetComponent<Transform>().localPosition + new Vector3(0, 0, 3), Quaternion.identity) as GameObject;

                        g.GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0.3f);

                        //生成したオブジェクとの親にこのオブジェクトを設定
                        g.transform.parent = gameObject.transform;

                        //Fieldにデータをセット
                        SetCubeData(TapPoint_X, TapPoint_Y, g);

                        g.GetComponent<Block>().SetStartPos(NextBlock.GetComponent<Transform>().localPosition);
                        g.GetComponent<Block>().SetMovePos(new Vector3(TapPoint_X - (gridWidth / 2), TapPoint_Y, 0));

                        g.GetComponent<Rigidbody2D>().simulated = false;

                        if (TapPoint_Y + 1 < gridHeight)
                        {
                            for (int i = 0; i < gridWidth; i++)
                            {
                                //動けなくしていた部分をもとに戻す
                                Rigid = Field[i, TapPoint_Y + 1].Cube.GetComponent<Rigidbody2D>();
                                //解除
                                Rigid.constraints = RigidbodyConstraints2D.None;
                                //再設定
                                Rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
                                Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                            }
                        }
                    }
                    //挿入したブロックに色を付ける
                    Field[(int)TapPoint_X, (int)TapPoint_Y].Cube.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[0]) as Material;
                    Field[(int)TapPoint_X, (int)TapPoint_Y].Cube.GetComponent<Block>().CubeName = NextBlocks[0];

                    Action = true;
                }


                //移動の線形補間部分
                for (x = 0; x < gridWidth; x++)
                {
                    if (Field[x, TapPoint_Y].Alive != false)
                    {
                        //Moveに何か入っているならば動かす
                        if (Field[x, TapPoint_Y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                        {
                            Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition =
                                Vector3.Lerp(Field[x, TapPoint_Y].Cube.GetComponent<Block>().StartPos, Field[x, TapPoint_Y].Cube.GetComponent<Block>().MovedPos, PushTime);

                            if (PushTime == 1)
                            {
                                Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.y), 0);
                                Field[x, TapPoint_Y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));
                            }
                        }
                    }
                }


                if (Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                {
                    Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Transform>().localPosition = Vector3.Lerp(Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Block>().StartPos, Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Block>().MovedPos, PushTime * 2);
                    if (PushTime == 1)
                    {
                        Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.y), 0);
                        Field[TapPoint_X, TapPoint_Y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));
                    }
                }

                //タイムの加算
                time += Time.deltaTime;
                PushTime += MoveSpeed;

                //ArmTime += 0.25f;

                //押し込みの時間の制限
                if (PushTime > 1)
                {
                    PushTime = 1;
                }

                if (time > span)
                {
                    //挿入待ちブロックの色を更新
                    NextBlocks[0] = NextBlocks[1];

                    //配列をずらす
                    for (int i = 1; i < 3; i++)
                    {
                        NextBlocks[i] = NextBlocks[i + 1];
                    }

                    //最後尾にランダムでカラーの名前を入れておく
                    NextBlocks[3] = CubeMats[Random.Range(0, CubeMats.Length)];

                    //挿入用ブロックにマテリアルを設定
                    NextBlock.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[0]) as Material;
                    NextBlock1.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[1]) as Material;
                    NextBlock2.GetComponent<Renderer>().material = Resources.Load("Materials/" + NextBlocks[2]) as Material;

                    Field[(int)TapPoint_X, (int)TapPoint_Y].Cube.GetComponent<Transform>().localScale = new Vector3(1, 1, 0.3f);
                    Field[(int)TapPoint_X, (int)TapPoint_Y].Cube.GetComponent<Rigidbody2D>().simulated = true;

                    for (x = 0; x < gridWidth; x++)
                    {
                        if (Field[x, TapPoint_Y].Alive != false)
                        {
                            //Moveに何か入っているならば動かす
                            if (Field[x, TapPoint_Y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                            {
                                Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition = Vector3.Lerp(Field[x, TapPoint_Y].Cube.GetComponent<Block>().StartPos, Field[x, TapPoint_Y].Cube.GetComponent<Block>().MovedPos, 1);

                                Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[x, TapPoint_Y].Cube.GetComponent<Transform>().localPosition.y), 0);
                                Field[x, TapPoint_Y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));
                            }
                        }
                    }

                    time = 0;
                    PushTime = 0;

                    Phase = PHASE.SERACH;
                    Action = false;
                }
                break;

            //ブロックが連なりを検出する処理
            case PHASE.SP_PUSH:

                Ber.ResetAmount();

                if (Action == false)
                {
                    for (x = 0; x < gridWidth; x++)
                    {
                        
                        Field[x, TapPoint_Y].Break = true;
                        Field[x, TapPoint_Y].Cube.GetComponent<Renderer>().material = Resources.Load("Materials/" + "Break" + Field[x, TapPoint_Y].Cube.GetComponent<Block>().CubeName) as Material;

                    }
                    Action = true;
                }

                time += Time.deltaTime;

                if (time > span)
                {
                    time = 0;


                    Phase = PHASE.SERACH;
                    Action = false;
                }
                break;
            case PHASE.SERACH:
                Debug.Log("サーチフェイズ");

                VanishCaller = false;

                if (Action == false)
                {
                    for (x = 0; x < gridWidth; x++)
                    {
                        for (y = 0; y < gridHeight; y++)
                        {
                            //横方向に連続しているか
                            BlockCounter = 0;
                            CountBlocks(x, y, CountType.X_Count);

                            //縦方向に連続しているか
                            BlockCounter = 0;
                            CountBlocks(x, y, CountType.Y_Count);
                        }
                    }
                    Action = true;
                }

                time += Time.deltaTime;

                if (time > span)
                {
                    time = 0;

                    
                    Phase = PHASE.VANISH;
                    Action = false;
                }

                //Phase = PHASE.VANISH;
                break;
            //ブロックの消去処理
            case PHASE.VANISH:
                Debug.Log("ヴァニッシュフェイズ");
                if (Action == false)
                {
                    for (x = 0; x < gridWidth; x++)
                    {
                        for (y = 0; y < gridHeight; y++)
                        {
                            if (Field[x, y].Cube != null)
                            {
                                //動けなくしていた部分をもとに戻す
                                Rigid = Field[x, y].Cube.GetComponent<Rigidbody2D>();
                                //解除
                                Rigid.constraints = RigidbodyConstraints2D.FreezeAll;
                                
                            }
                        }
                    }

                    //ブロックの削除処理
                    for (x = 0; x < gridWidth; x++)
                    {
                        for (y = 0; y < gridHeight; y++)
                        {
                            if (Field[x, y].Break == true)
                            {
                                GameObject g = Instantiate(VanishEffect, new Vector3(x - 3, y, -1), Quaternion.identity) as GameObject;

                                var Ps = g.GetComponent<ParticleSystem>();
                                Ps.GetComponent<Renderer>().material = Field[x, y].Cube.GetComponent<Renderer>().material;

                                Destroy(Field[x, y].Cube, span);
                                Field[x, y].Alive = false;
                                Field[x, y].Break = false;

                                VanishCaller = true;

                                //Debug.Log("X" + x + "Y" + y);

                                Ber.SetAddMode(60, 0.001f);

                                if (vanishCount >= addTimeVanishCount)
                                {
                                    Timer.AddTimeSecond(addTime);
                                }
                            }
                        }
                    }
                    Action = true;
                }

                time += Time.deltaTime;

                if (time > span)
                {
                    time = 0;

                    if (VanishCaller == true)
                    {
                        Phase = PHASE.SLIDE;
                        Action = false;
                    }
                    else
                    {
                        Phase = PHASE.STAY;
                        Action = false;
                    }
                }
                
                break;
            //ブロックのずらし処理
            case PHASE.SLIDE:
                int SlideCnt = 0;
                int SlideEmpCnt = 0;
                

                if (Action == false)
                {
                    for (y = 0; y < gridHeight; y++)
                    {
                        //右半分
                        for (x = 3; x < gridHeight; x++)
                        {
                            //フィールドに空きを見つけた
                            if (Field[x, y].Alive == false)
                            {
                                SlideCnt++;
                                //空き領域より上にブロックが存在するか探す
                                for (SlideEmpCnt = x + 1; SlideEmpCnt < gridWidth; SlideEmpCnt++)
                                {
                                    if (Field[SlideEmpCnt, y].Alive == true)
                                    {
                                        break;
                                    }
                                    //空き領域までの数をカウント
                                    SlideCnt++;
                                }
                                //存在しない場合はカウントが不必要になるので0へ
                                if (SlideEmpCnt == gridHeight)
                                {
                                    SlideCnt = 0;
                                }

                                if (SlideCnt > 0)
                                {
                                    for (int i = SlideEmpCnt; SlideEmpCnt < gridWidth; SlideEmpCnt++)
                                    {
                                        
                                        //Debug.Log("移動してる行：" + EmpCnt + "下がった量:" + DropCnt);
                                        Field[SlideEmpCnt - SlideCnt, y] = Field[SlideEmpCnt, y];

                                        if (Field[SlideEmpCnt - SlideCnt, y].Alive != false || Field[SlideEmpCnt - SlideCnt, y].Cube != null)
                                        {
                                            //Field[SlideEmpCnt - SlideCnt, y].Cube.GetComponent<Transform>().localPosition -= new Vector3(SlideCnt, 0, 0);
                                            Field[SlideEmpCnt - SlideCnt, y].Cube.GetComponent<Block>().SetStartPos(Field[SlideEmpCnt - SlideCnt, y].Cube.GetComponent<Transform>().localPosition);
                                            Field[SlideEmpCnt - SlideCnt, y].Cube.GetComponent<Block>().SetMovePos(new Vector3((SlideEmpCnt - SlideCnt) - 3, y, 0));
                                            //Field[SlideEmpCnt - SlideCnt, y].Cube.GetComponent<Block>().SetMovePos(Field[SlideEmpCnt - SlideCnt, y].Cube.GetComponent<Transform>().localPosition - new Vector3(SlideCnt, 0, 0));
                                        }
                                        //アームの移動距離を設定
                                        //ARM_R[y].EndPos = ARM_R[y].Arm.GetComponent<Transform>().localPosition - new Vector3(SlideCnt, 0, 0);

                                        ARM_R[y].Arm.GetComponent<ArmScript>().OnArmAdd(SlideCnt, (int)(span * 100));

                                        Field[SlideEmpCnt, y].Cube = null;
                                        Field[SlideEmpCnt, y].Alive = false;
                                    }
                                    SlideCnt = 0;
                                }
                            }
                        }

                        //左半分
                        for (x = 3; x >= 0; x--)
                        {
                            //フィールドに空きを見つけた
                            if (Field[x, y].Alive == false)
                            {
                                SlideCnt++;
                                //空き領域より上にブロックが存在するか探す
                                for (SlideEmpCnt = x - 1; SlideEmpCnt >= 0; SlideEmpCnt--)
                                {
                                    if (Field[SlideEmpCnt, y].Alive == true)
                                    {
                                        break;
                                    }
                                    //空き領域までの数をカウント
                                    SlideCnt++;
                                }
                                //存在しない場合はカウントが不必要になるので0へ
                                if (SlideEmpCnt == (gridWidth/2))
                                {
                                    SlideCnt = 0;
                                }

                                if (SlideCnt > 0)
                                {
                                    for (int i = SlideEmpCnt; SlideEmpCnt >= 0; SlideEmpCnt--)
                                    {
                                        //Debug.Log("移動してる行：" + EmpCnt + "下がった量:" + DropCnt);
                                        Field[SlideEmpCnt + SlideCnt, y] = Field[SlideEmpCnt, y];
                                        //Field[SlideEmpCnt + SlideCnt, y].Cube.GetComponent<Transform>().localPosition += new Vector3(SlideCnt, 0, 0);
                                        if (Field[SlideEmpCnt + SlideCnt, y].Alive != false || Field[SlideEmpCnt + SlideCnt, y].Cube != null)
                                        {
                                            Field[SlideEmpCnt + SlideCnt, y].Cube.GetComponent<Block>().SetStartPos(Field[SlideEmpCnt + SlideCnt, y].Cube.GetComponent<Transform>().localPosition);
                                            //Field[SlideEmpCnt + SlideCnt, y].Cube.GetComponent<Block>().SetMovePos(Field[SlideEmpCnt + SlideCnt, y].Cube.GetComponent<Transform>().localPosition + new Vector3(SlideCnt, 0, 0));
                                            Field[SlideEmpCnt + SlideCnt, y].Cube.GetComponent<Block>().SetMovePos(new Vector3((SlideEmpCnt + SlideCnt) - 3, y, 0));
                                        }

                                        //アームの移動距離をセット
                                        ARM_L[y].Arm.GetComponent<ArmScript>().OnArmAdd(SlideCnt, (int)(span * 100));

                                        Field[SlideEmpCnt, y].Cube = null;
                                        Field[SlideEmpCnt, y].Alive = false;
                                    }
                                    SlideCnt = 0;
                                }
                            }
                        }
                    }
                    Action = true;
                }

                time += Time.deltaTime;
                SlideTime += MoveSpeed;
                //ArmTime += 0.25f;

                if(SlideTime > 1)
                {
                    SlideTime = 1;
                }

                //if(ArmTime > 1 && !ArmTurn)
                //{
                //    //ArmTime = 1;
                //    ArmTime = 0;
                //
                //    for (y = 0; y < gridHeight; y++)
                //    {
                //        ARM_L[y].Arm.GetComponent<Transform>().localPosition = Vector3.Lerp(ARM_L[y].StartPos, ARM_L[y].EndPos, 1);
                //        ARM_R[y].Arm.GetComponent<Transform>().localPosition = Vector3.Lerp(ARM_R[y].StartPos, ARM_R[y].EndPos, 1);
                //    }
                //    ArmTurn = true;
                //}
                
                for (y = 0; y < gridHeight; y++)
                {
                    for (x = 0; x < gridWidth; x++)
                    {
                        if (Field[x, y].Alive != false)
                        {
                            //Moveに何か入っているならば動かす
                            if (Field[x, y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                            {
                                Field[x, y].Cube.GetComponent<Transform>().localPosition = Vector3.Lerp(Field[x, y].Cube.GetComponent<Block>().StartPos, Field[x, y].Cube.GetComponent<Block>().MovedPos, SlideTime);
                                if(SlideTime == 1)
                                {
                                    Field[x, y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[x, y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[x, y].Cube.GetComponent<Transform>().localPosition.y), 0);
                                    Field[x, y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));
                                }
                            }
                        }
                    }
                }

                //アームの位置を更新
                //for(y = 0;y < gridHeight;y++)
                //{
                //    if (!ArmTurn)
                //    {
                //        ARM_L[y].Arm.GetComponent<Transform>().localPosition = Vector3.Lerp(ARM_L[y].StartPos, ARM_L[y].EndPos, ArmTime);
                //        ARM_R[y].Arm.GetComponent<Transform>().localPosition = Vector3.Lerp(ARM_R[y].StartPos, ARM_R[y].EndPos, ArmTime);
                //    }
                //    else
                //    {
                //        ARM_L[y].Arm.GetComponent<Transform>().localPosition = Vector3.Lerp(ARM_L[y].EndPos, ARM_L[y].StartPos, ArmTime);
                //        ARM_R[y].Arm.GetComponent<Transform>().localPosition = Vector3.Lerp(ARM_R[y].EndPos, ARM_R[y].StartPos, ArmTime);
                //
                //        if(ArmTime >= 1)
                //        {
                //            ARM_L[y].EndPos = ARM_L[y].StartPos;
                //            ARM_R[y].EndPos = ARM_R[y].StartPos;
                //        }
                //    }
                //}


                if (time > span)
                {
                    time = 0;

                    //アームを初期位置に戻す
                    //for (y = 0; y < gridHeight; y++)
                    //{
                    //    ARM_L[y].Arm.GetComponent<Transform>().localPosition = Vector3.Lerp(ARM_L[y].EndPos, ARM_L[y].StartPos, 1);
                    //    ARM_R[y].Arm.GetComponent<Transform>().localPosition = Vector3.Lerp(ARM_R[y].EndPos, ARM_R[y].StartPos, 1);
                    //
                    //    ARM_L[y].EndPos = ARM_L[y].StartPos;
                    //    ARM_R[y].EndPos = ARM_R[y].StartPos;
                    //    
                    //}

                    if (VanishCaller == true)
                    {
                        Phase = PHASE.DROP;
                        SlideTime = 0;
                        ArmTime = 0;
                        ArmTurn = false;
                        Action = false;

                        for (y = 0; y < gridHeight; y++)
                        {
                            for (x = 0; x < gridWidth; x++)
                            {
                                if (Field[x, y].Alive != false)
                                {
                                    //Moveに何か入っているならば動かす
                                    if (Field[x, y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                                    {
                                        Field[x, y].Cube.GetComponent<Transform>().localPosition = Vector3.Lerp(Field[x, y].Cube.GetComponent<Block>().StartPos, Field[x, y].Cube.GetComponent<Block>().MovedPos, 1);
                                        
                                        Field[x, y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[x, y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[x, y].Cube.GetComponent<Transform>().localPosition.y), 0);
                                        Field[x, y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));
                                        
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Phase = PHASE.STAY;
                        Action = false;
                    }
                }

                break;
            //ブロックの落下処理
            case PHASE.DROP:

                Debug.Log("ドロップフェイズ");

                int DropCnt = 0;
                int EmpCnt = 0;
                if (Action == false)
                {
                    for (x = 0; x < gridWidth; x++)
                    {
                        for (y = 0; y < gridHeight; y++)
                        {
                            //フィールドに空きを見つけた
                            if (Field[x, y].Alive == false)
                            {
                                DropCnt++;
                                //空き領域より上にブロックが存在するか探す
                                for (EmpCnt = y + 1; EmpCnt < gridHeight; EmpCnt++)
                                {
                                    if (Field[x, EmpCnt].Alive == true)
                                    {
                                        break;
                                    }
                                    //空き領域までの数をカウント
                                    DropCnt++;
                                }
                                //存在しない場合はカウントが不必要になるので0へ
                                if (EmpCnt == gridHeight)
                                {
                                    DropCnt = 0;
                                }

                                if (DropCnt > 0)
                                {
                                    for (int i = EmpCnt; EmpCnt < gridHeight; EmpCnt++)
                                    {
                                        //Debug.Log("移動してる行：" + EmpCnt + "下がった量:" + DropCnt);
                                        Field[x, EmpCnt - DropCnt] = Field[x, EmpCnt];

                                        if (Field[x, EmpCnt - DropCnt].Cube != null)
                                        {
                                            Field[x, EmpCnt - DropCnt].Cube.GetComponent<Block>().SetStartPos(Field[x, EmpCnt - DropCnt].Cube.GetComponent<Transform>().localPosition);
                                            Field[x, EmpCnt - DropCnt].Cube.GetComponent<Block>().SetMovePos(new Vector3(x - 3,EmpCnt - DropCnt,0));
                                        }
                                        Field[x, EmpCnt].Cube = null;
                                        Field[x, EmpCnt].Alive = false;
                                    }
                                    DropCnt = 0;
                                }
                            }
                        }
                    }
                    Action = true;

                }
                time += Time.deltaTime;

                SlideTime += MoveSpeed;

                if (SlideTime > 1)
                {
                    SlideTime = 1;
                }

                for (x = 0; x < gridWidth; x++)
                {
                    for (y = 0; y < gridHeight; y++)
                    {
                        if (Field[x, y].Alive != false)
                        {
                            //Moveに何か入っているならば動かす
                            if (Field[x, y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                            {
                                Field[x, y].Cube.GetComponent<Transform>().localPosition = Vector3.Lerp(Field[x, y].Cube.GetComponent<Block>().StartPos, Field[x, y].Cube.GetComponent<Block>().MovedPos, SlideTime);
                                if (SlideTime == 1)
                                {
                                    Field[x, y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[x, y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[x, y].Cube.GetComponent<Transform>().localPosition.y), 0);
                                    Field[x, y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));
                                }
                            }
                        }
                    }
                }

                if (time > span)
                {
                    time = 0;
                    SlideTime = 0;
                    Phase = PHASE.GENERATE;
                    Action = false;

                    for (y = 0; y < gridHeight; y++)
                    {
                        for (x = 0; x < gridWidth; x++)
                        {
                            if (Field[x, y].Alive != false)
                            {
                                //Moveに何か入っているならば動かす
                                if (Field[x, y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                                {
                                    Field[x, y].Cube.GetComponent<Transform>().localPosition = Vector3.Lerp(Field[x, y].Cube.GetComponent<Block>().StartPos, Field[x, y].Cube.GetComponent<Block>().MovedPos, 1);

                                    Field[x, y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[x, y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[x, y].Cube.GetComponent<Transform>().localPosition.y), 0);
                                    Field[x, y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));

                                }
                            }
                        }
                    }
                }
               
                break;
            //ブロック生成フェイズ
            case PHASE.GENERATE:

                Debug.Log("生成フェイズ");

                //生成したブロックの数
                int GenerateCnt = 0;

                if (Action == false)
                {
                    //空き領域の確認とそこにブロックの生成
                    for (x = 0; x < gridWidth; x++)
                    {
                        for (y = 0; y < gridHeight; y++)
                        {
                            if (Field[x, y].Cube == null)
                            {
                                //ブロックのインスタンス化
                                //GameObject g = Instantiate(Prefab, new Vector3(x - 3, y + GenerateCnt, 0), Quaternion.identity) as GameObject;
                                GameObject g = RandColorCreateBlock(new Vector3(x - 3, y + GenerateCnt, 0));

                                //生成したオブジェクとの親にこのオブジェクトを設定
                                g.transform.parent = gameObject.transform;

                                SetCubeData(x, y, g);

                                Field[x, y].Cube.GetComponent<Block>().SetStartPos(Field[x, y].Cube.GetComponent<Transform>().localPosition);
                                Field[x, y].Cube.GetComponent<Block>().SetMovePos(new Vector3(x - 3, y, 0));

                                GenerateCnt++;
                            }
                        }
                        //横軸を移動したので生成した数をリセット
                        GenerateCnt = 0;
                    }
                    Action = true;
                }
                time += Time.deltaTime;

                SlideTime += MoveSpeed;

                if (SlideTime > 1)
                {
                    SlideTime = 1;
                }

                for (x = 0; x < gridWidth; x++)
                {
                    for (y = 0; y < gridHeight; y++)
                    {
                        if (Field[x, y].Alive != false)
                        {
                            //Moveに何か入っているならば動かす
                            if (Field[x, y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                            {
                                Field[x, y].Cube.GetComponent<Transform>().localPosition = Vector3.Lerp(Field[x, y].Cube.GetComponent<Block>().StartPos, Field[x, y].Cube.GetComponent<Block>().MovedPos, SlideTime);
                                if (SlideTime == 1)
                                {
                                    Field[x, y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[x, y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[x, y].Cube.GetComponent<Transform>().localPosition.y), 0);
                                    Field[x, y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));
                                }
                            }
                        }
                    }
                }

                if (time > span)
                {
                    for (y = 0; y < gridHeight; y++)
                    {
                        for (x = 0; x < gridWidth; x++)
                        {
                            if (Field[x, y].Alive != false)
                            {
                                //Moveに何か入っているならば動かす
                                if (Field[x, y].Cube.GetComponent<Block>().MovedPos != new Vector3(100, 0, 0))
                                {
                                    Field[x, y].Cube.GetComponent<Transform>().localPosition = Vector3.Lerp(Field[x, y].Cube.GetComponent<Block>().StartPos, Field[x, y].Cube.GetComponent<Block>().MovedPos, 1);

                                    Field[x, y].Cube.GetComponent<Transform>().localPosition = new Vector3(Field[x, y].Cube.GetComponent<Transform>().localPosition.x, Mathf.Round(Field[x, y].Cube.GetComponent<Transform>().localPosition.y), 0);
                                    Field[x, y].Cube.GetComponent<Block>().SetMovePos(new Vector3(100, 0, 0));
                                }
                            }
                        }
                    }
                    if (VanishCaller == false)
                    {
                        Phase = PHASE.STAY;
                        SlideTime = 0;
                        time = 0;
                        Action = false;
                    }
                    else
                    {
                        Debug.Log("もっかい探索");
                        Phase = PHASE.SERACH;
                        SlideTime = 0;
                        time = 0;
                        Action = false;
                        for (x = 0; x < gridWidth; x++)
                        {
                            for (y = 0; y < gridHeight; y++)
                            {
                                if (Field[x, y].Cube != null)
                                {
                                    //動けなくしていた部分をもとに戻す
                                    Rigid = Field[x, y].Cube.GetComponent<Rigidbody2D>();
                                    //解除
                                    //Rigid.constraints = RigidbodyConstraints2D.None;
                                    //再設定
                                    Rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
                                    Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                                }
                            }
                        }
                    }
                }
                break;
        }
    }

    //フィールドにオブジェクト情報をセット
    public void SetCubeData(int X, int Y, GameObject Obj)
    {
        Field[X, Y].Cube = Obj;
        Field[X, Y].Alive = true;
        Field[X, Y].Break = false;
    }

    //ランダムカラーでのブロック生成
    private GameObject RandColorCreateBlock(Vector3 Pos)
    {
        //インスタンス化
        GameObject g = Instantiate(Prefab, Pos, Quaternion.identity) as GameObject;

        //ランダムで色を決める
        g.GetComponent<Block>().CubeName = CubeMats[Random.Range(0, CubeMats.Length)];

        //決めた色のマテリアルを取得
        Material CubeMat = Resources.Load("Materials/" + g.GetComponent<Block>().CubeName) as Material;

        //Debug.Log(CubeName);

        //マテリアルの貼り付け
        g.GetComponent<Renderer>().material = CubeMat;

        //生成したオブジェクトを返す
        return g;
    }

    //ブロックが連なっているかどうかの確認用関数、再帰処理でBlockCounterが3以上になればフラグを立てる
    private void CountBlocks(int X, int Y, CountType Type)
    {
        BlockCounter++;

        //ブロックが存在しているなら
        if (Field[X, Y].Cube != null)
        {
            switch (Type)
            {
                case CountType.X_Count:
                    if (X + 1 < gridWidth && Field[X + 1, Y].Cube != null && Field[X + 1, Y].Alive == true && Field[X + 1, Y].Cube.GetComponent<Block>().CubeName == Field[X, Y].Cube.GetComponent<Block>().CubeName)
                    {
                        CountBlocks(X + 1, Y, CountType.X_Count);
                    }
                    break;
                case CountType.Y_Count:
                    //次の配列が配列外になっていない。かつ配列内のオブジェクトデータが入っている。ヴァニッシュされていない。現在の検索位置と同じ色のブロックなら　正
                    if (Y + 1 < gridHeight && Field[X, Y + 1].Cube != null && Field[X, Y + 1].Alive == true && Field[X, Y + 1].Cube.GetComponent<Block>().CubeName == Field[X, Y].Cube.GetComponent<Block>().CubeName)
                    {
                        CountBlocks(X, Y + 1, CountType.Y_Count);
                    }
                    break;
                default:
                    break;
            }

            if (BlockCounter >= 3)
            {
                Field[X, Y].Break = true;
                Field[X ,Y].Cube.GetComponent<Renderer>().material = Resources.Load("Materials/" + "Break" + Field[X,Y].Cube.GetComponent<Block>().CubeName) as Material;
                Field[X, Y].Cube.GetComponent<Rigidbody2D>().simulated = false;
                GameObject.Find("PointManager").GetComponent<PointManager>().AddPoint(100);
                vanishCount++;
            }
        }
    }
}
