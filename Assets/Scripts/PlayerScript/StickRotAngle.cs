using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class StickRotAngle : MonoBehaviour {

    [SerializeField, Header("デッドゾーン")] private float _deadzone;
    private PlayerInput _playerInput;

    private int _stickDialAngle_Y, _stickDialAngle_X;
    private bool _isActicDial_Y = false, _isActicDial_X = false;
    public int _stickAngle_Y, _stickAngle_X;
    private LeftRightFrontBack_ManyObj _yAxisManyObj;

    public bool _isDamiObjCreate = false;
    public GameObject _damiObject;


    // Getter
    public int GetStickDialAngleY {
        get { return _stickDialAngle_Y; }
    }

    // Getter
    public int GetStickDialAngleX {
        get { return _stickDialAngle_X; }
    }


    // y軸の4方向を示す
    private enum LeftRightFrontBack_ManyObj {
        Right, Left, Front, Back
    }

    private void Start() {

        // InputSystemを取得
        _playerInput = GetComponent<PlayerInput>();
    }


    // y回転の更新
    public void StickRotY_Update() {
        _stickDialAngle_Y = SettingDialAngle(GetAngleY());
    }

    // x回転の更新
    public void StickRotX_Update() {
        _stickDialAngle_X = GetAngleX();
    }

    private int GetAngleY() {

        var value = _playerInput.actions["RotaionSelect"].ReadValue<Vector2>();

        if ((value.x < -_deadzone || _deadzone < value.x || value.y < -_deadzone || _deadzone < value.y)) {

            // 右
            if (_yAxisManyObj == LeftRightFrontBack_ManyObj.Right) {

                _stickAngle_Y = (int)(Mathf.Atan2(-value.y, value.x) * Mathf.Rad2Deg);
            }
            // 左
            else if (_yAxisManyObj == LeftRightFrontBack_ManyObj.Left) {

                _stickAngle_Y = (int)(Mathf.Atan2(value.y, -value.x) * Mathf.Rad2Deg);
            }
            // 奥
            else if (_yAxisManyObj == LeftRightFrontBack_ManyObj.Back) {

                _stickAngle_Y = (int)(Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg);
            }
            // 手前
            else if (_yAxisManyObj == LeftRightFrontBack_ManyObj.Front) {

                _stickAngle_Y = (int)(Mathf.Atan2(-value.x, -value.y) * Mathf.Rad2Deg);
            }
        }
      
        if (_stickAngle_Y < 0) {
            _stickAngle_Y += 360;
        }

        return _stickAngle_Y;
    }

    private int GetAngleX() {

        var value = _playerInput.actions["RotaionSelect"].ReadValue<Vector2>();

        int angle = 0;

        if (value.y < -_deadzone) {
            if (!_isActicDial_X) {
                angle = -90;
                _isActicDial_X = true;
            }
            else {
                _stickDialAngle_X = 0;
            }
        }
        else if (_deadzone < value.y) {
            if (!_isActicDial_X) {
                angle = 90;
                _isActicDial_X = true;
            }
            else {
                _stickDialAngle_X = 0;
            }
        }
        else {
            _isActicDial_X = false;
            _stickDialAngle_X = 0;
        }

        return _stickDialAngle_X + angle;
    }

    // スティックの角度をダイアルに振り分ける
    private int SettingDialAngle(int angle) {

        int dialAngle = 0;

        if (angle >= 45 && angle < 135) {

            dialAngle = 90;
        }
        else if (angle >= 135 && angle < 225) {

            dialAngle = 180;
        }
        else if (angle >= 225 && angle < 315) {

            dialAngle = -90;
        }
        else {

            dialAngle = 0;
        }

        return dialAngle;
    }

    public void yAxisManyObjJude(RotObjHitCheck _hitcheck) {

        // NULL回避
        if (_hitcheck.GetRotObj == null) {
            return;
        }

        // 乗ってるブロックの座標を取得する
        var rideObj_x = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.x);
        var rideObj_y = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.y);
        var rideObj_z = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.z);

        // ブロックをまとめてる親を取得する
        var objects = _hitcheck.GetRotObj.transform.Find("Object").gameObject;

        // ブロックの数
        int rightnum = 0, leftnum = 0, frontnum = 0, backnum = 0;

        // ブロックの数判定する
        for (int i = 0; i < objects.transform.childCount; i++) {

            var parts_x = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.x);
            var parts_z = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.z);

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

        var max = Mathf.Max(rightnum, leftnum, backnum, frontnum);

        if (max == rightnum) {
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Right;
        }
        else if (max == leftnum) {
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Left;
        }
        else if (max == backnum) {
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Back;
        }
        else if (max == frontnum) {
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Front;
        }

        if (!_isDamiObjCreate) {
            _damiObject = yAxisCreateDamiObj(_yAxisManyObj, new Vector3(rideObj_x, rideObj_y, rideObj_z), objects.transform);
            _isDamiObjCreate = true;
        }

        RotatableObject rotbleobj = _hitcheck.GetRotObj.GetComponent<RotatableObject>();

        _stickAngle_Y = 0;
        rotbleobj.oldangleY = 0;

        Debug.Log("角度リセット");
    }

    // 座標補正　Playerのやつマルコピ
    private Vector3 CompensateRotationAxis(in Vector3 AXIS) {
        return new Vector3(RoundOff(AXIS.x), RoundOff(AXIS.y), RoundOff(AXIS.z));
    }

    private int RoundOff(float value) {
        int valueInt = (int)value;

        if (value - valueInt < 0.5f) {
            return valueInt;
        }

        return ++valueInt;
    }

    private GameObject yAxisCreateDamiObj(LeftRightFrontBack_ManyObj dir, Vector3 ridePos, Transform objectObj) {

        var damiobj = new GameObject("stickDamiObj");
        damiobj.tag = "DamiObject";

        switch (dir) {
            case LeftRightFrontBack_ManyObj.Right:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.right);
                break;

            case LeftRightFrontBack_ManyObj.Left:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.left);
                break;

            case LeftRightFrontBack_ManyObj.Back:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.forward);
                break;

            case LeftRightFrontBack_ManyObj.Front:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.back);
                break;
        }

        damiobj.transform.parent = objectObj;
        return damiobj;
    }

    /*
    private PlayerInput _playerInput;
    private LeftRightFrontBack_ManyObj _yAxisManyObj;
    private UpDownFrontBack_ManyObj _xAxisManyObj;

    int _playerLR_obj = 1;
    public int _stickAngle_Y, _stickAngle_X;
    private int _stickDialAngle_Y, _stickDialAngle_X;

    public bool _isDamiObjCreate = false;
    public GameObject _damiObject;

    public bool _isActicStick;
    public bool _isActicDial_X { get; set; }
    public bool _isActicDial_Y { get; set; }

    public bool _isActivStickOneFream = false;

    // y軸の4方向を示す
    private enum LeftRightFrontBack_ManyObj {
        Right, Left, Front, Back
    }

    // x軸の4方向を示す
    private enum UpDownFrontBack_ManyObj {
        Up, Down, Front, Back
    }

    // Getter
    public int GetStickDialAngleY {
        get { return _stickDialAngle_Y; }
    }

    // Getter
    public int GetStickDialAngleX {
        get { return _stickDialAngle_X; }
    }

    // Getter
    public bool GetIsActicStick {
        get { return _isActicStick; }
    }

    //--------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------//

    public void yAxisManyObjJude(RotObjHitCheck _hitcheck) {

        // NULL回避
        if (_hitcheck.GetRotObj == null) {
            return;
        }

        // 乗ってるブロックの座標を取得する
        var rideObj_x = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.x);
        var rideObj_y = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.y);
        var rideObj_z = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.z);

        // ブロックをまとめてる親を取得する
        var objects = _hitcheck.GetRotObj.transform.Find("Object").gameObject;

        // ブロックの数
        int rightnum = 0, leftnum = 0, frontnum = 0, backnum = 0;

        // ブロックの数判定する
        for (int i = 0; i < objects.transform.childCount; i++) {

            var parts_x = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.x);
            var parts_z = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.z);

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

        var max = Mathf.Max(rightnum, leftnum, backnum, frontnum);

        if (max == rightnum) {
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Right;
        }
        else if (max == leftnum) {
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Left;
        }
        else if (max == backnum) {
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Back;
        }
        else if (max == frontnum) {
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Front;
        }

        if (!_isDamiObjCreate) {
            _damiObject = yAxisCreateDamiObj(_yAxisManyObj, new Vector3(rideObj_x, rideObj_y, rideObj_z), objects.transform);
            _isDamiObjCreate = true;
        }

        RotatableObject rotbleobj = _hitcheck.GetRotObj.GetComponent<RotatableObject>();

        _stickAngle_Y = 0;
        rotbleobj.oldangleY = 0;

        Debug.Log("角度リセット");
    }

    // プレイヤーの位置によるスティックの角度を取得する
    private int GetAngleY() {

        var value = _playerInput.actions["RotaionSelect"].ReadValue<Vector2>();

        if ((value.x < -_deadzone || _deadzone < value.x || value.y < -_deadzone || _deadzone < value.y)) {

            _isActicStick = true;

            // 右
            if (_yAxisManyObj == LeftRightFrontBack_ManyObj.Right) {

                _stickAngle_Y = (int)(Mathf.Atan2(-value.y, value.x) * Mathf.Rad2Deg);
            }
            // 左
            else if (_yAxisManyObj == LeftRightFrontBack_ManyObj.Left) {

                _stickAngle_Y = (int)(Mathf.Atan2(value.y, -value.x) * Mathf.Rad2Deg);
            }
            // 奥
            else if (_yAxisManyObj == LeftRightFrontBack_ManyObj.Back) {

                _stickAngle_Y = (int)(Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg);
            }
            // 手前
            else if (_yAxisManyObj == LeftRightFrontBack_ManyObj.Front) {

                _stickAngle_Y = (int)(Mathf.Atan2(-value.x, -value.y) * Mathf.Rad2Deg);
            }
        }
        else {
            _isActicStick = false;
        }

        if (_stickAngle_Y < 0) {
            _stickAngle_Y += 360;
        }

        return _stickAngle_Y;
    }

    public void xAxisManyObjJude(RotObjHitCheck _hitcheck) {

        // NULL回避
        if (_hitcheck.GetRotObj == null) {
            Debug.Log("NULL回避");
            return;
        }

        // 乗ってるブロックの座標を取得する
        var rideObj_x = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.x);
        var rideObj_y = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.y);
        var rideObj_z = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.z);

        // ブロックをまとめてる親を取得する
        var objects = _hitcheck.GetRotObj.transform.Find("Object").gameObject;

        // ブロックの数
       int upnum = 0, downnum = 0, frontnum = 0, backnum = 0;

        // 指したオブジェクトが右が左か判定する
        if (transform.position.x < rideObj_x) {
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

        var max = Mathf.Max(upnum, downnum, frontnum, backnum);

        if (max == upnum) {
            _xAxisManyObj = UpDownFrontBack_ManyObj.Up;
        }
        else if (max == downnum) {
            _xAxisManyObj = UpDownFrontBack_ManyObj.Down;
        }
        else if (max == frontnum) {
            _xAxisManyObj = UpDownFrontBack_ManyObj.Front;
        }
        else if (max == backnum) {
            _xAxisManyObj = UpDownFrontBack_ManyObj.Back;
        }

        if (!_isDamiObjCreate) {
            _damiObject = xAxisCreateDamiObj(_xAxisManyObj, new Vector3(rideObj_x, rideObj_y, rideObj_z), objects.transform);
            _isDamiObjCreate = true;
        }

        RotatableObject rotbleobj = _hitcheck.GetRotObj.GetComponent<RotatableObject>();

        _stickAngle_X = 0;
        rotbleobj.oldangleX = 0;
    }

    private bool DoOnce = false;

    // プレイヤーの位置によるスティックの角度を取得する
    private int GetAngleX() {

        var value = _playerInput.actions["RotaionSelect"].ReadValue<Vector2>();

        if (value.y < -_deadzone) {

            if (!DoOnce) {
                _isActicDial_X = true;
                _stickDialAngle_X -= 90;
                DoOnce = true;
            }
            else {
                _isActicDial_X = false;
                _stickDialAngle_X = 0;
            }
        }
        else if (_deadzone < value.y) {

            if (!DoOnce) {
                _isActicDial_X = true;
                _stickDialAngle_X += 90;
                DoOnce = true;
            }
            else {
                _isActicDial_X = false;
                _stickDialAngle_X = 0;
            }
           
        }
        else {
            DoOnce = false;
            _stickDialAngle_X = 0;
        }

        Debug.Log("角度"+ _stickDialAngle_X) ;

        return _stickAngle_X;
    }

    //--------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------//

    // スティックの角度をダイアルに振り分ける
    private int SettingDialAngle(int angle) {

        int dialAngle = 0;

        if (angle >= 45 && angle < 135) {
       
            dialAngle = 90;
        }
        else if (angle >= 135 && angle < 225) {
           
            dialAngle = 180;
        }
        else if (angle >= 225 && angle < 315) {
       
            dialAngle = -90;
        }
        else {
        
            dialAngle = 0;
        }

        return dialAngle;
    }

    // 座標補正　Playerのやつマルコピ
    private Vector3 CompensateRotationAxis(in Vector3 AXIS) {
        return new Vector3(RoundOff(AXIS.x), RoundOff(AXIS.y),RoundOff(AXIS.z));
    }

    private int RoundOff(float value) {
        int valueInt = (int)value;dddddddddddddd

        if (value - valueInt < 0.5f) {
            return valueInt;
        }

        return ++valueInt;
    }

    private GameObject yAxisCreateDamiObj(LeftRightFrontBack_ManyObj dir, Vector3 ridePos,  Transform objectObj) {

        var damiobj = new GameObject("stickDamiObj");
        damiobj.tag = "DamiObject";

        switch (dir) {
            case LeftRightFrontBack_ManyObj.Right:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.right);
                break;

            case LeftRightFrontBack_ManyObj.Left:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.left);
                break;

            case LeftRightFrontBack_ManyObj.Back:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.forward);
                break;

            case LeftRightFrontBack_ManyObj.Front:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.back);
                break;
        }

        damiobj.transform.parent = objectObj;
        return damiobj;
    }

    private GameObject xAxisCreateDamiObj(UpDownFrontBack_ManyObj dir, Vector3 ridePos, Transform objectObj) {

        var damiobj = new GameObject("stickDamiObj");
        damiobj.tag = "DamiObject";
        switch (dir) {
            case UpDownFrontBack_ManyObj.Up:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.up);
                break;

            case UpDownFrontBack_ManyObj.Down:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.down);
                break;

            case UpDownFrontBack_ManyObj.Back:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.forward);
                break;

            case UpDownFrontBack_ManyObj.Front:
                damiobj.transform.position = CompensateRotationAxis(ridePos + Vector3.back);
                break;
        }

        damiobj.transform.parent = objectObj;
        return damiobj;
    }
    */
}

