using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回転できるオブジェクトのスクリプト
public class RotatableObject : MonoBehaviour{

    [SerializeField] Vector3 _rotAxis;            // 回転軸の向きベクトル
    [SerializeField] Vector3 _axisCenterLocalPos; // 軸中心座標:ローカル座標軸で指定してください
    [SerializeField] float _axisLength;

    

    // Start is called before the first frame update
    void Start(){
        
        // オブジェクト固有の軸を可視化
        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        var lineRenderer = this.GetComponent<LineRenderer>();


        var selfPos = this.transform.position;  // 自身の座標を取得(中心)
        var axisCenterWorldPos = selfPos + _axisCenterLocalPos; // 軸座標を計算

        var rotAxisStartPos = axisCenterWorldPos + ((_axisLength/2)*_rotAxis);
        var rotAxisEndPos= axisCenterWorldPos + (-(_axisLength/2)*_rotAxis);

        var positions = new Vector3[]{
             rotAxisStartPos, // 開始点
             rotAxisEndPos    // 終了点
        };

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
