using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour{

    private void RotateAxis(Vector3 center, Vector3 axis, int angle) {

        // 現在フレームの回転を示す回転のクォータニオン作成
        var angleAxis = Quaternion.AngleAxis(angle, axis);

        // 円運動の位置計算
        var tr = transform;
        var pos = tr.position;

        // クォータニオンを用いた回転は原点からのオフセットを用いる必要がある
        // _axisCenterWorldPosを任意軸の座標に変更すれば任意軸の回転ができる
        pos -= center;
        pos = angleAxis * pos;
        pos += center;

        tr.position = pos;

        // 向き更新
        tr.rotation = angleAxis * tr.rotation;
    }


    // 前回の回転
    public int oldangleY = 0;
    public int oldangleX = 0;

    public void StartRotateX(Vector3 center, Vector3 axis, int angle) {

        if (oldangleX == angle) {
            return;
        }

        var offset = angle - oldangleX;

        RotateAxis(center, axis, offset);

        oldangleX = angle;
    }

    public void StartRotateY(Vector3 center, Vector3 axis, int angle) {

        if (oldangleY == angle) {
            return;
        }

        var offset = angle - oldangleY;

        RotateAxis(center, axis, offset);

        oldangleY = angle;
    }
}
