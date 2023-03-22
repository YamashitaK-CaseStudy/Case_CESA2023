using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回転できるオブジェクトのスクリプト
public class RotatableObject : MonoBehaviour{

    [SerializeField] private Vector3 _selfRotAxis;              // 自身の回転軸ベクトル
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
     

    }

    // 軸の座標を計算する
    void CalkAxisWorldPos() {
        // オブジェクト固有の軸を可視化
        // LineRendererコンポーネントを取得
        var lineRenderer = this.GetComponent<LineRenderer>();

        // 軸をワールド座標刑に変換
        _axisCenterWorldPos = transform.TransformPoint(_axisCenterLocalPos); // 軸座標を計算

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

    // 自身の軸でまわす小をする
    public void RotateSmallAxisSelf() {
        if ( _isSpin || _isRotating ) {
            return;
        }
       
        // フラグを立てる
        _isRotating = true;

        _elapsedTime = 0.0f;
    }

    // 外部の軸でまわす小をする
    public void RotateSmallAxisExtern(Vector3 centerPos,Vector3 rotAxis) {
        // 回転の中心座標と自身の座標間でベクトルをとる
        var tmpVec = centerPos - this.transform.position; 

        // Z軸正方向と外積を取って回転軸を求める
      

        var tr = this.transform;

        // 回転のクォータニオン作成
        var rotQuat = Quaternion.AngleAxis(180, rotAxis);

        var dirQuat = Quaternion.AngleAxis(180,Vector3.up);


        // 円運動の位置計算
        var pos = tr.position;
        pos -= centerPos;
        pos = rotQuat * pos;
        pos += centerPos;

        tr.position = pos;

        // 向き更新
        tr.rotation = tr.rotation * dirQuat;
  
        CalkAxisWorldPos();
    }

    // 自身の座標でまわす大をする
    public void SpinAxisSelf() {
        _isSpin = true;
    }

    // 外部の軸でまわす大をする
    public void SpinAxisExturn(Vector3 spinCenterPos,Vector3 spinAxisVec) {
         
    }

    // Update is called once per frame
    void Update(){
        // 回転中かフラグ
        if ( _isRotating ) {

            // リクエストデルタタイムを求める
            // リクエストデルタタイム：デルタタイムを1回転に必要な時間で割った値
            // これの合算値が1になった時,1回転に必要な時間が経過したことになる
            float requiredDeltaTime = Time.deltaTime/_rotRequirdTime;
            _elapsedTime += requiredDeltaTime;

            // 目標回転量*リクエストデルタタイムでそのフレームでの回転角度を求めることができる
            // リクエストデルタタイムの合算値がちょうど1になるように補正をかけると総回転量は目標回転量と一致する
            if ( _elapsedTime >= 1 ) {
                _isRotating = false;
                requiredDeltaTime -= ( _elapsedTime - 1 ); // 補正
			}

            // 現在フレームの回転を示す回転のクォータニオン作成
            var angleAxis = Quaternion.AngleAxis(180 * requiredDeltaTime, _selfRotAxis);

            // 円運動の位置計算
            var tr = transform;
            var pos = tr.position;

            pos -= _axisCenterWorldPos;
            pos = angleAxis * pos;
            pos += _axisCenterWorldPos;

            tr.position = pos;

            tr.rotation = tr.rotation * angleAxis;
        }

        // 回っているかフラグ
        if ( _isSpin ) {
        }
    }


}
