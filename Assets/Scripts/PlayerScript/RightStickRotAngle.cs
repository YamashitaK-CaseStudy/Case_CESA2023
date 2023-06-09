using UnityEngine;
using UnityEngine.InputSystem;

public class RightStickRotAngle : MonoBehaviour
{

    [SerializeField, Header("デッドゾーン")] private float _deadzone;
    private PlayerInput _playerInput;

    private int _stickAngle_Y, _stickDialAngle_Y, _stickDialAngle_X;
    private int _oldStickDialAngle_Y = 0;
    private bool _isActicDial_X = false;
    public bool _isOldAngleChinge { get; set; } = false;
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

        int angleY = SettingDialAngle(GetAngleY());
        _stickDialAngle_Y = angleY;

        if (_oldStickDialAngle_Y == angleY) {
            _isOldAngleChinge = false;
            return;
        }
        else {
            if (angleY != 0) {
                Debug.Log("必要な時のみ更新" + angleY);
                _isOldAngleChinge = true;
            }

            _oldStickDialAngle_Y = angleY;
            _stickDialAngle_Y = _oldStickDialAngle_Y;
        }
    }

    // x回転の更新
    public void StickRotX_Update() {

        _stickDialAngle_X = GetAngleX();
    }

    private int GetAngleY() {

        var value = _playerInput.actions["RotaionSelect"].ReadValue<Vector2>();
        if (value.magnitude > _deadzone) {

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

  
    // スティックの角度をダイアルに振り分ける
    private int SettingDialAngle(int angle) {

        if (angle >= 45 && angle < 135) {
            return 90;
        }
        else if (angle >= 135 && angle < 225) {
            return -180;
        }
        else if (angle >= 225 && angle < 315) {
            return -90;
        }
        else {
            return 0;
        }
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
            Debug.Log("右基準");
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Right;
        }
        else if (max == leftnum) {
            Debug.Log("左基準");
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Left;
        }
        else if (max == backnum) {
            Debug.Log("後基準");
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Back;
        }
        else if (max == frontnum) {
            Debug.Log("前基準");
            _yAxisManyObj = LeftRightFrontBack_ManyObj.Front;
        }

        var _rotComp = _hitcheck.GetRotObj.GetComponent<RotatableObject>();
        _rotComp._oldAngleY = 0;
        _stickAngle_Y = 0;

        if (!_isDamiObjCreate) {
            _damiObject = yAxisCreateDamiObj(_yAxisManyObj, new Vector3(rideObj_x, rideObj_y, rideObj_z), objects.transform);
            _isDamiObjCreate = true;
        }

        Debug.Log("基準角度" + _stickAngle_Y);
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

    public void yAxisDestroyDamiobj() {
        _isDamiObjCreate = false;
        Destroy(_damiObject);
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

    private bool _isStickSpin = false;
    private float _spinAxis = 0;
    private float _oldspinAxis = 0;

    // 回転スタート
    public bool GetIsStickSpin {
        get { return _isStickSpin; }
    }

    // 回転時の軸
    public float GetSpinAxis {
        get { return _spinAxis; }
    }

    public void StickSpinUpdate() {

        var value = _playerInput.actions["RotaionSelect"].ReadValue<Vector2>();

        // スティックが倒されたとき
        // 右回り
        if (value.x < -_deadzone) {

            _spinAxis = -1;
        }
        // 左回り
        else if (_deadzone < value.x) {

            _spinAxis = 1;
        }

        if (_oldspinAxis == _spinAxis) {
            _isStickSpin = false;
        }
        else {
            _isStickSpin = true;
        }


        _oldspinAxis = _spinAxis;
    }
}
