using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour{

    private StickRotAngle _stricRotAngle = null;
    private bool _isRotating = false;            // 回している最中かを判定するフラグ
    
    private InputAction _blocklockButton = null;
    private InputAction _rotationButton = null;
    private InputAction _rotationSpinButton = null;

    private void PlayerRotationStart() {

        // 右スティックコンポネント取得
        _stricRotAngle = GetComponent<StickRotAngle>();

        // 各種ボタンの取得
        _blocklockButton    = _playerInput.actions.FindAction("BlockLock");
        _rotationButton     = _playerInput.actions.FindAction("Rotation");
        _rotationSpinButton = _playerInput.actions.FindAction("RotationSpin");
    }

    private void PlayerRotationUpdate() {

        // 移動中、ジャンプ中は回転させない
        if (Mathf.Abs(_speedx) > 0 || !_groundCheck.IsGround) {
            return;
        }

        // ブロックのロック
        if (_blocklockButton.IsPressed()) {

            _speedx = 0.0f;

            // 下に回転オブジェクトがある場合無条件で優先
            if (_bottomHitCheck.GetRotObj != null && !_upperrayCheck.IsUpperHit) {
                Pos_Correction(_bottomHitCheck.GetRotPartsObj.transform.position);
                _yBlockLock = true;
            }
            else if(_bottomHitCheck.GetRotObj != null && _upperrayCheck.IsUpperHit) {
                Pos_Correction(_bottomHitCheck.GetRotPartsObj.transform.position);
                _yBlockUpperLock = true;
            }
            else if (_frontHitCheck.GetRotObj != null) {
                _xBlockLock = true;
            }
            else if(_bottomHitCheck.GetRotObj == null && _upperrayCheck.IsUpperHit) {
      
            }
            else {
                _yBlockLock = true;
            }
        }
        else {
            _yBlockLock = false;
            _xBlockLock = false;
            _yBlockUpperLock = false;
            _animCallBack.RotationInValid();
        }

        // 真下に回転オブジェクトがある時
        if(_animCallBack.GetIsRotationValid && _yBlockLock) {

            if(_bottomHitCheck.GetRotObj == null) {
                return;
            }

            var rotatbleComp = _bottomHitCheck.GetRotObj.GetComponent<RotatableObject>();

            // スティック回転Y
            if (!rotatbleComp._isRotating) {
                _stricRotAngle.yAxisManyObjJude(_bottomHitCheck);
            }

            _stricRotAngle.StickRotAngleY_Update();
            rotatbleComp.StartRotateY(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);

           
            // 通常軸回転
            if (_rotationButton.WasPressedThisFrame()) {
                rotatbleComp.StartRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, 90, this.transform);
            }

            // 高速回転
            if (_rotationSpinButton.WasPressedThisFrame()) {
                if (rotatbleComp._isSpining) {
                    Debug.Log("高速回転終了");
                    rotatbleComp.EndSpin();
                }
                else {
                    rotatbleComp.StartSpin(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up);
                }
            }
        }

        // 前方に回転オブジェクトがある時
        else if (_animCallBack.GetIsRotationValid && _xBlockLock) {

            if (_frontHitCheck.GetRotObj == null) {
                return;
            }
            var rotatbleComp = _frontHitCheck.GetRotObj.GetComponent<RotatableObject>();

            if (!rotatbleComp._isRotating) {
                _stricRotAngle.xAxisManyObjJude(_frontHitCheck);
            }

            // スティック回転X
            _stricRotAngle.StickRotAngleX_Update();
            rotatbleComp.StartRotateX(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleX, this.transform);

            // 通常軸回転
            if (_rotationButton.WasPressedThisFrame()) {
                rotatbleComp.StartRotate(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, 90, this.transform);
            }

            // 高速回転
            if (_rotationSpinButton.WasPressedThisFrame()) {
                if (rotatbleComp._isSpining) {
                    Debug.Log("高速回転終了");
                    rotatbleComp.EndSpin();
                }
                else {
                    rotatbleComp.StartSpin(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right);
                }
            }
        }
    }

    private void Pos_Correction(in Vector3 center) {

        var Pos = transform.position;
        var pPos = new Vector3(center.x, Pos.y, 0);
        transform.transform.position = pPos;
    }

    public void NotificationStartRotate() {
        _isRotating = true;
    }

    public void NotificationEndRotate() {
        _isRotating = false;
    }
}
