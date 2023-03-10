using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回転できるオブジェクトのスクリプト
public class RotatableObject : MonoBehaviour{

    [SerializeField] Vector3 _rotAxis;            // 回転軸の向きベクトル
    [SerializeField] Vector3 _axisCenterLocalPos; // 軸中心座標:ローカル座標で指定してください
    [SerializeField] float _axisLength;

    private Vector3 _axisCenterWorldPos;

    private Quaternion _rotQuat;  // 回転のクオータニオン
    private Quaternion _baceQuat; // 回転はじめのクオータニオン

    private bool _isSpin = false; // 回転しているかフラグ

    // Start is called before the first frame update
    void Start(){

        _rotAxis.Normalize();

        // 軸の中心のワールド座標を計算
        CalkAxisWorldPos();
     

    }


    void CalkAxisWorldPos() {
        // オブジェクト固有の軸を可視化
        // LineRendererコンポーネントを取得
        var lineRenderer = this.GetComponent<LineRenderer>();

        // 軸をワールド座標刑に変換
        _axisCenterWorldPos = transform.TransformPoint(_axisCenterLocalPos); // 軸座標を計算

        var axisHalfLength = _axisLength / 2;

        var rotAxisStartPos = _axisCenterWorldPos + ( axisHalfLength * _rotAxis);
        var rotAxisEndPos = _axisCenterWorldPos + ( - axisHalfLength * _rotAxis);

        var positions = new Vector3[]{
             rotAxisStartPos, // 開始点
             rotAxisEndPos    // 終了点
        };

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);
    }

    // 自身の軸でまわす小をする
    public void RotateSmallAxisSelf() {
        var tr = this.transform;
        
        // 回転のクォータニオン作成
        var rotQuat = Quaternion.AngleAxis(180, _rotAxis);
     

        // 円運動の位置計算
        var pos = tr.position;
        pos -= _axisCenterWorldPos;
        pos = rotQuat * pos;
        pos += _axisCenterWorldPos;

        tr.position = pos;

        // 向き更新
        tr.rotation = tr.rotation * rotQuat;
         
        
    }

    // 外部の軸でまわす小をする
    public void RotateSmallAxisExtern(Vector3 centerPos) {
        // 回転の中心座標と自身の座標間でベクトルをとる
        var tmpVec = centerPos - this.transform.position; 

        // Z軸正方向と外積を取って回転軸を求める
        var rotAxis = Vector3.Cross(centerPos, tmpVec);

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

    public void SpinAxisSelf() {
        _isSpin = true;

        // 回転のクォータニオン作成
        _rotQuat = Quaternion.AngleAxis(180, _rotAxis);
    }

    public void SpinAxisExturn(Vector3 spinCenterPos) {
    
    
        
    }

    // Update is called once per frame
    void Update(){
        if ( _isSpin ) {
            var tr = this.transform;

            // 円運動の位置計算
            var pos = tr.position;
            pos -= _axisCenterWorldPos;
            pos = _rotQuat * pos;
            pos += _axisCenterWorldPos;

            tr.position = pos;

            // 向き更新
            tr.rotation = tr.rotation * _rotQuat;
        }
    }


}
