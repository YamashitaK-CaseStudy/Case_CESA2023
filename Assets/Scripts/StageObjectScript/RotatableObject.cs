using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回転できるオブジェクトのスクリプト
public class RotatableObject : MonoBehaviour{

    [SerializeField] Vector3 _rotAxis;            // 回転軸の向きベクトル
    [SerializeField] Vector3 _axisCenterLocalPos; // 軸中心座標:ローカル座標軸で指定してください
    [SerializeField] float _axisLength;

    private Vector3 _axisCenterWorldPos;

    // Start is called before the first frame update
    void Start(){

        _rotAxis.Normalize();
        
        // オブジェクト固有の軸を可視化
        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        var lineRenderer = this.GetComponent<LineRenderer>();

        var selfPos = this.transform.position;  // 自身の座標を取得(中心)
        _axisCenterWorldPos = selfPos + _axisCenterLocalPos; // 軸座標を計算

        var axisHalfLength = _axisLength / 2;

        var rotAxisStartPos = _axisCenterWorldPos + ( axisHalfLength * _rotAxis);
        var rotAxisEndPos = _axisCenterWorldPos + ( - axisHalfLength * _rotAxis);

        var positions = new Vector3[]{
             rotAxisStartPos, // 開始点
             rotAxisEndPos    // 終了点
        };

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions); 
        
        //RotateSmallAxisSelf();
    }

    // 自身の軸でまわす小をする
    void RotateSmallAxisSelf() {
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
    void RotateSmallAxisExtern(Vector3 rotAxisExturn,Vector3 centerPos) {
        var tr = this.transform;

        // 回転のクォータニオン作成
        var rotQuat = Quaternion.AngleAxis(180, rotAxisExturn);

        // 円運動の位置計算
        var pos = tr.position;
        pos -= centerPos;
        pos = rotQuat * pos;
        pos += centerPos;

        tr.position = pos;

        // 向き更新
        tr.rotation = tr.rotation * rotQuat;

    }

    // Update is called once per frame
    void Update(){
       
    }


}
