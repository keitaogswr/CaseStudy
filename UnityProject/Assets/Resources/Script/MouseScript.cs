using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public GameObject effectPrefab;     // エフェクトのPrefab

    private Vector3 nowMousePosition;   // 現在のマウス位置
    private Vector3 oldMousePosition;   // 過去のマウス位置
    private Quaternion quaternion;      // クオータニオン

    // Startの前に行われる処理
    private void Awake()
    {
        // 破棄を無効化
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        nowMousePosition = new Vector3(0, 0, 0);
        oldMousePosition = nowMousePosition;
        quaternion = new Quaternion();
        quaternion = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        oldMousePosition = nowMousePosition;
        nowMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // クリック時   エフェクト生成
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(effectPrefab, new Vector3(nowMousePosition.x, nowMousePosition.y, -1 ), quaternion);
        }
    }
    // マウス位置取得
    public Vector3 GetMousePosition()
    {
        return nowMousePosition;
    }

    // 過去のマウス位置取得
    public Vector3 GetOldMousePosition()
    {
        return oldMousePosition;
    }
}
