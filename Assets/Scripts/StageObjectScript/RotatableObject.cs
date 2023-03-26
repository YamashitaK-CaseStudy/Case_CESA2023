using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回転できるオブジェクトのスクリプト
public partial class RotatableObject : MonoBehaviour{

    [SerializeField] protected Vector3 _selfRotAxis;              // 自身の回転軸ベクトル
    [SerializeField] public Vector3 _axisCenterLocalPos;        // 軸中心座標:ローカル座標で指定してください
    [SerializeField] private float _axisLength;                 // TEST：軸の長さ
    [SerializeField] private float _rotRequirdTime = 1.0f;      // 1回転に必要な時間(sec)
    
    private float _elapsedTime = 0.0f;  // 経過時間

    private Vector3 _axisCenterWorldPos; // 回転軸の中心のワールド座標


    public bool _isRotating = false;   // 回転してるかフラグ
    private bool _isSpin = false;       // 回転しているかフラグ

    // Start is called before the first frame update
    void Start(){

        // 自身の回転軸の向きを正規化しとく
        _selfRotAxis.Normalize();

        // 軸の中心のワールド座標を計算
        CalkAxisWorldPos();

        // まわす大の設定
        StartSettingSpin();

    }

    // 自身の軸のワールド座標を計算する
    protected void CalkAxisWorldPos() {
        // オブジェクト固有の軸を可視化
        // LineRendererコンポーネントを取得
        var lineRenderer = this.GetComponent<LineRenderer>();

        // 軸をワールド座標刑に変換
        _axisCenterWorldPos = this.transform.TransformPoint(_axisCenterLocalPos); // 軸座標を計算

        var axisHalfLength = _axisLength / 2;

        var rotAxisStartPos = _axisCenterWorldPos + ( axisHalfLength * _selfRotAxis);
        var rotAxisEndPos = _axisCenterWorldPos + ( - axisHalfLength * _selfRotAxis);

        var positions = new Vector3[]{
             rotAxisStartPos, // 開始点
             rotAxisEndPos    // 終了点
        };

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);
    }

   

    // Update is called once per frame
    void Update(){
        UpdateRotate();
        UpdateSpin();      
    }


}
