using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour{

    // 前回の回転
    public int oldangleY = 0;
    public int oldangleX = 0;

    // public void StartStickRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle) {

    //     if (_isSpin || _isRotating) {
    //         return;
    //     }

    //     var playerComp = _playerTransform.GetComponent<Player>();

    //     playerComp.NotificationStartRotate();
    
    //     // フラグを立てる
    //     _isRotating = true;

    //     // 経過時間を初期化
    //     _elapsedTime = 0.0f;

    //     // 回転の中心を設定
    //     _axisCenterWorldPos = rotCenter;

    //     // 回転軸を設定
    //     _rotAxis = rotAxis;

    //     // 回転オフセット値をセット
    //     _angle = rotAngle;

    //     PlayPartical();

    // }

    public Vector3 _nowRotAxis;
   
    public void StartRotateX(Vector3 center, Vector3 axis, int angle, Transform playerTransform) {
        if (oldangleX == angle) {
            return;
        }

        // プレイヤーのトランスフォームを保持
        _playerTransform = playerTransform;

        var offset = angle - oldangleX;

        if (offset > 0) {
            _nowRotAxis = new Vector3(0, -1, 0);
        }
        else {
            _nowRotAxis = new Vector3(0, 1, 0);
        }

        StartRotate(center, axis, offset, playerTransform);
       // RotateAxis(center, axis, offset);

        oldangleX = angle;
    }

    public void StartRotateY(Vector3 center, Vector3 axis, int angle,Transform playerTransform) {
        if (oldangleY == angle) {
            return;
        }
        // プレイヤーのトランスフォームを保持
        _playerTransform = playerTransform;

        var Pos = playerTransform.position;
        var pPos = new Vector3(center.x,Pos.y,0);
        playerTransform.transform.position = pPos;

        var offset = angle - oldangleY;

        // 時計回り
        if(offset > 0) {
            _nowRotAxis = new Vector3(0, 1, 0);
        }
        else {
            _nowRotAxis = new Vector3(0, -1, 0);
        }

        StartRotate(center, axis, offset, playerTransform);
        //StartStickRotate(center, axis, offset);
        //RotateAxis(center, axis, offset);

        oldangleY = angle;
        PlayPartical();
    }
}
