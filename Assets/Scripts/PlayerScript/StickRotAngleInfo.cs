using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class StickRotAngle : MonoBehaviour {

    [SerializeField, Header("デッドゾーン")] private float _deadzone;

    private PlayerInput _playerInput;

    private LeftRightFrontBack_ManyObj _lrfb_Manyobj;
    private UpDownFrontBack_ManyObj    _udfb_Manyobj;
 
    private int _playerLR_obj;
    private int _stickAngle_Y;
    private int _stickAngle_X;
    public int _stickDialAngle_Y;
    private int _stickDialAngle_X;

    public bool _isStickActiv { set; get; } = false;
    
    // Getter
    public int GetStickDialAngleY {
        get { return _stickDialAngle_Y; }
    }

    public int GetStickDialAngleX {
        get { return _stickDialAngle_X; }
    }

    // カメラの正面から見て右左奥手前でどっちにオブジェクトが多いか
    private enum LeftRightFrontBack_ManyObj {
        Right,
        Left,
        Front,
        Back,
        Same
    }

    // カメラん正面から見て上下手前でどっちにオブジェクトが多いか
    private enum UpDownFrontBack_ManyObj {
        Up,
        Down,
        Front,
        Back,
        Same
    }

    //------------------------------------------------------------------------------------------------------//
    //------------------------------------------------------------------------------------------------------//

    // Y軸回転で 右左奥手前でどっちにオブジェクトが多いか判定する
    public void LRFB_Many_Jude(RotObjHitCheck _hitcheck) {

        // 乗ってるブロックの座標を取得する
        var rideObj_x = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.x);
        var rideObj_z = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.z);

        // ブロックをまとめてる親を取得する
        var objects = _hitcheck.GetRotObj.transform.Find("Object").gameObject;

        // ブロックの数
        int rightnum = 0, leftnum = 0, frontnum = 0, backnum = 0;

        // ブロックの数判定する
        for (int i = 0; i < objects.transform.childCount; i++) {

            var parts_x = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.x);
            var parts_z = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.z);

            // Debug.Log(parts_x);

            // x軸判定
            if (rideObj_x != parts_x) {

                if (rideObj_x < parts_x) {
                    rightnum++;
                }
                else {
                    leftnum++;
                }
            }

            // z軸判定
            if (rideObj_z != parts_z) {

                if (rideObj_z < parts_z) {
                    backnum++;
                }
                else {
                    frontnum++;
                }
            }
        }

        var max = Mathf.Max(leftnum, rightnum, backnum, frontnum);

        if (max == leftnum) {
            _lrfb_Manyobj = LeftRightFrontBack_ManyObj.Left;
        }
        else if (max == rightnum) {
            _lrfb_Manyobj = LeftRightFrontBack_ManyObj.Right;
        }
        else if (max == backnum) {
            _lrfb_Manyobj = LeftRightFrontBack_ManyObj.Back;
        }
        else if (max == frontnum) {
            _lrfb_Manyobj = LeftRightFrontBack_ManyObj.Front;
        }

        RotatableObject rotbleobj = _hitcheck.GetRotObj.GetComponent<RotatableObject>();

        _stickAngle_Y = 0;
        rotbleobj.oldangleY = 0;
    }

    // プレイヤーの位置によるスティックの角度を取得する
    private int GetAngleY() {

        var value = _playerInput.actions["RotaionSelect"].ReadValue<Vector2>();

        if ((value.x < -_deadzone || _deadzone < value.x || value.y < -_deadzone || _deadzone < value.y)) {

            // 右
            if (_lrfb_Manyobj == LeftRightFrontBack_ManyObj.Right) {

                _stickAngle_Y = (int)(Mathf.Atan2(-value.y, value.x) * Mathf.Rad2Deg);
            }
            // 左
            else if (_lrfb_Manyobj == LeftRightFrontBack_ManyObj.Left) {

                _stickAngle_Y = (int)(Mathf.Atan2(value.y, -value.x) * Mathf.Rad2Deg);
            }
            // 奥
            else if (_lrfb_Manyobj == LeftRightFrontBack_ManyObj.Back) {

                _stickAngle_Y = (int)(Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg);
            }
            // 手前
            else if (_lrfb_Manyobj == LeftRightFrontBack_ManyObj.Front) {

                _stickAngle_Y = (int)(Mathf.Atan2(-value.x, -value.y) * Mathf.Rad2Deg);
            }
        }

        if (_stickAngle_Y < 0) {
            _stickAngle_Y += 360;
        }

        return _stickAngle_Y;
    }

    //------------------------------------------------------------------------------------------------------//
    //------------------------------------------------------------------------------------------------------//

    // Y軸回転で 右左奥手前でどっちにオブジェクトが多いか判定する
    public void UDFB_Many_Jude(RotObjHitCheck _hitcheck) {

        // 乗ってるブロックの座標を取得する
        var rideObj_y = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.y);
        var rideObj_z = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.z);

        // ブロックをまとめてる親を取得する
        var objects = _hitcheck.GetRotObj.transform.Find("Object").gameObject;

        // ブロックの数
        int upnum = 0, downnum = 0, frontnum = 0, backnum = 0;

        // 指したオブジェクトが右が左か判定する
        if(transform.position.x < (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.x)) {
            _playerLR_obj = 1;
        }
        else {
            _playerLR_obj = -1;
        }

        // ブロックの数判定する
        for (int i = 0; i < objects.transform.childCount; i++) {

            var parts_y = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.y);
            var parts_z = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.z);

            // x軸判定
            if (rideObj_y != parts_y) {

                if (rideObj_y < parts_y) {
                    upnum++;
                }
                else {
                    downnum++;
                }
            }

            // z軸判定
            if (rideObj_z != parts_z) {

                if (rideObj_z < parts_z) {
                    backnum++;
                }
                else {
                    frontnum++;
                }
            }
        }

        var max = Mathf.Max(upnum, downnum, backnum, frontnum);

        if (max == upnum) {
            _udfb_Manyobj = UpDownFrontBack_ManyObj.Up;
        }
        else if (max == downnum) {
            _udfb_Manyobj = UpDownFrontBack_ManyObj.Down;
        }
        else if (max == backnum) {
            _udfb_Manyobj = UpDownFrontBack_ManyObj.Back;
        }
        else if (max == frontnum) {
            _udfb_Manyobj = UpDownFrontBack_ManyObj.Front;
        }

        RotatableObject rotbleobj = _hitcheck.GetRotObj.GetComponent<RotatableObject>();

        _stickAngle_X = 0;
        rotbleobj.oldangleX = 0;
    }

   
    // プレイヤーの位置によるスティックの角度を取得する
    private int GetAngleX() {

        var value = _playerInput.actions["RotaionSelect"].ReadValue<Vector2>();

        if ((value.x < -_deadzone || _deadzone < value.x || value.y < -_deadzone || _deadzone < value.y)) {

            if (_udfb_Manyobj == UpDownFrontBack_ManyObj.Up) {

                _stickAngle_X = (int)(Mathf.Atan2(-value.x * _playerLR_obj, value.y) * Mathf.Rad2Deg);
            }
            else if (_udfb_Manyobj == UpDownFrontBack_ManyObj.Back ) {

                _stickAngle_X = (int)(Mathf.Atan2(-value.y, -value.x * _playerLR_obj) * Mathf.Rad2Deg);
            }
            else if (_udfb_Manyobj == UpDownFrontBack_ManyObj.Front) {

                _stickAngle_X = (int)(Mathf.Atan2(value.y, value.x * _playerLR_obj) * Mathf.Rad2Deg);
            }
            else if (_udfb_Manyobj == UpDownFrontBack_ManyObj.Down ) {

                _stickAngle_X = (int)(Mathf.Atan2(value.x * _playerLR_obj, -value.y) * Mathf.Rad2Deg);
            }
            else {
                Debug.Log("それ以外");
            }
        }

        if (_stickAngle_X < 0) {
            _stickAngle_X += 360;
        }

        return _stickAngle_X;
    }

    //------------------------------------------------------------------------------------------------------//
    //------------------------------------------------------------------------------------------------------//

    // スティックの角度をダイアルに振り分ける
    private int SettingDialAngle(int angle) {

        int dialAngle = 0;

        if (angle >= 45 && angle < 135) {
            dialAngle = 90;
        }
        else if (angle >= 135 && angle < 225) {
            dialAngle = - 180;
        }
        else if (angle >= 225 && angle < 315) {
            dialAngle = - 90;
        }
        else {
            dialAngle = 0;
        }

        var angle180 = (int)(Mathf.Repeat(dialAngle + 180, 360) - 180);

        return angle180;
    }
}