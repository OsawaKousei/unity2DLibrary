using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * 仕様：
 * マウスのドラッグで線を描画
 * 使い方：
 * 任意のゲームオブジェクトにこのスクリプトをアタッチ
 * 注意事項：
 * 
 */
public class LineWriter : MonoBehaviour
{ 
    //線の色
    [SerializeField] Color lineColor;
    //線のマテリアル
    [SerializeField] Material lineMaterial;
    //線の太さ
    [Range(0.1f, 0.5f)]
    [SerializeField] float lineWidth;

    //描画可能領域設定
    [SerializeField] bool lineRestriction;
    [SerializeField] float right;
    [SerializeField] float left;
    [SerializeField] float bottom;
    [SerializeField] float top;

    //LineRdenerer型のリスト宣言
    private List<LineRenderer> lineRenderers;

    void Start()
    {
        //Listの初期化
        lineRenderers = new List<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //lineObjを生成し、初期化する
            addLineObject();
        }

        if (Input.GetMouseButton(0))
        {
            //マウスポインタがあるスクリーン座標を取得
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);

            //スクリーン座標をワールド座標に変換
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            if (!lineRestriction)
            {
                //線を描画
                addPositionDataToLineRendererList();
            }else if ((left <= worldPosition.x && worldPosition.x <= right) && (bottom <= worldPosition.y && worldPosition.y <= top))
            {
                //線を描画
                addPositionDataToLineRendererList();
            }

        }

        //デバッグ用にマウスのワールド座標を表示
        if (Input.GetMouseButtonDown(1))
        {
            //マウスポインタがあるスクリーン座標を取得
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);

            //スクリーン座標をワールド座標に変換
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Debug.Log("x:" + worldPosition.x + "y:" + worldPosition.y);
        }

    }

    //クリックしたら発動
    private void addLineObject()
    {
        //空のゲームオブジェクト作成
        GameObject lineObj = new GameObject();
        //オブジェクトの名前をStrokeに変更
        lineObj.name = "Stroke";
        //lineObjにLineRendereコンポーネント追加
        var lineRenderer = lineObj.AddComponent<LineRenderer>();
        //マテリアル追加
        lineRenderer.material = lineMaterial;
        //lineRendererリストにlineObjを追加
        lineRenderers.Add(lineObj.GetComponent<LineRenderer>());
        //lineObjを自身の子要素に設定
        lineObj.transform.SetParent(transform);
        //lineObj初期化処理
        initRenderers();
    }

    //lineObj初期化処理
    private void initRenderers()
    {
        //線をつなぐ点を0に初期化
        lineRenderers.Last().positionCount = 0;
        //色の初期化
        lineRenderers.Last().material.color = lineColor;
        //太さの初期化
        lineRenderers.Last().startWidth = lineWidth;
        lineRenderers.Last().endWidth = lineWidth;
    }

    private void addPositionDataToLineRendererList()
    {
        //マウスポインタがあるスクリーン座標を取得
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);

        //スクリーン座標をワールド座標に変換
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //ワールド座標をローカル座標に変換
        Vector3 localPosition = transform.InverseTransformPoint(worldPosition.x, worldPosition.y, -1.0f);

        //lineRenderersの最後のlineObjのローカルポジションを上記のローカルポジションに設定
        lineRenderers.Last().transform.localPosition = localPosition;

        //lineObjの線と線をつなぐ点の数を更新
        lineRenderers.Last().positionCount += 1;

        //LineRendererコンポーネントリストを更新
        lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount - 1, worldPosition);

        //あとから描いた線が上に来るように調整
        lineRenderers.Last().sortingOrder = lineRenderers.Count;
    }

    //全ての線を消去
    public void crearScreen()
    {
        foreach (LineRenderer line in lineRenderers)
        {
            Destroy(line);
        }
    }
}

