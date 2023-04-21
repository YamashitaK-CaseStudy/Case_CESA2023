using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour{

    // 前回の回転
    public int oldangleY = 0;
    public int oldangleX = 0;


    public void StartStickRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle) {

        if (_isSpin || _isRotating) {
            return;
        }

        // フラグを立てる
        _isRotating = true;

        // 経過時間を初期化
        _elapsedTime = 0.0f;

        // 回転の中心を設定
        _axisCenterWorldPos = rotCenter;

        // 回転軸を設定
        _rotAxis = rotAxis;

        // 回転オフセット値をセット
        _angle = rotAngle;
    }



    //private void RotateAxis(Vector3 center, Vector3 axis, int angle) {

    //    // 現在フレームの回転を示す回転のクォータニオン作成
    //    var angleAxis = Quaternion.AngleAxis(angle, axis);

    //    // 円運動の位置計算
    //    var tr = transform;
    //    var pos = tr.position;

    //    // クォータニオンを用いた回転は原点からのオフセットを用いる必要がある
    //    // _axisCenterWorldPosを任意軸の座標に変更すれば任意軸の回転ができる
    //    pos -= center;
    //    pos = angleAxis * pos;
    //    pos += center;

    //    tr.position = pos;

    //    // 向き更新
    //    tr.rotation = angleAxis * tr.rotation;
    //}

    public Vector3 _nowRotAxis;
   
    public void StartRotateX(Vector3 center, Vector3 axis, int angle) {

        if (oldangleX == angle) {
            return;
        }

        var offset = angle - oldangleX;

        if (offset > 0) {
            _nowRotAxis = new Vector3(0, -1, 0);
        }
        else {
            _nowRotAxis = new Vector3(0, 1, 0);
        }

        StartStickRotate(center, axis, offset);
       // RotateAxis(center, axis, offset);

        oldangleX = angle;
    }

    public void StartRotateY(Vector3 center, Vector3 axis, int angle) {

        if (oldangleY == angle) {
            return;
        }

        var offset = angle - oldangleY;

        // 時計回り
        if(offset > 0) {
            _nowRotAxis = new Vector3(0, 1, 0);
        }
        else {
            _nowRotAxis = new Vector3(0, -1, 0);
        }

        StartStickRotate(center, axis, offset);
        //RotateAxis(center, axis, offset);

        oldangleY = angle;
    }
}
