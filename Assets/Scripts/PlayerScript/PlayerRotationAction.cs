using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    private StickRotAngle _stricRotAngle = null;
    private bool _isRotating = false;            // 回している最中かを判定するフラグ
    private priorityAxis _priortyAxis;

    private enum priorityAxis {
        yAxisRot, xAxisRot,None
    }

    private void PlayerRotationStart() {

        // 右スティックコンポネント取得
        _stricRotAngle = GetComponent<StickRotAngle>();

        // 各種ボタンの取得
        _rotationButton     = _playerInput.actions.FindAction("Rotation");
        _rotationSpinButton = _playerInput.actions.FindAction("RotationSpin");
    }

    private void PlayerRotationUpdate() {

        // 移動中、ジャンプ中は回転させない
        if(Mathf.Abs( _rigidbody.velocity.x) > 0 || !_groundCheck.IsGround) {
            return;
        }

        // 下に回転オブジェクトがある場合無条件で優先
        
        // 真下に回転オブジェクトがある時
        if (_bottomHitCheck.GetIsRotHit) {

            _priortyAxis = priorityAxis.yAxisRot;

            var rotatbleComp = _bottomHitCheck.GetRotObj.GetComponent<RotatableObject>();

            // スティック回転Y
            if (rotatbleComp._isRotateEndFream) {
                _stricRotAngle.yAxisManyObjJude(_bottomHitCheck);
            }

            _stricRotAngle.StickRotAngleY_Update();
            rotatbleComp.StartRotateY(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY,this.transform);
           
            // 通常軸回転
            if (_rotationButton.WasPressedThisFrame()) {
                rotatbleComp.StartRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, 90, this.transform);
            }

            // 高速回転
            if (_rotationSpinButton.WasPressedThisFrame()) {
                Debug.Log(_bottomHitCheck.GetRotObj);
                rotatbleComp.StartSpin(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up);
            }
        }

        // 前方に回転オブジェクトがある時
        else if (_frontHitCheck.GetIsRotHit) {

            _priortyAxis = priorityAxis.xAxisRot;

            var rotatbleComp = _frontHitCheck.GetRotObj.GetComponent<RotatableObject>();
            Debug.Log(_frontHitCheck.GetRotObj);


            if (rotatbleComp._isRotateEndFream) {
                _stricRotAngle.xAxisManyObjJude(_frontHitCheck);
            }

            // スティック回転X
            _stricRotAngle.StickRotAngleX_Update();
            rotatbleComp.StartRotateX(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleX, this.transform);

            // 通常軸回転
            if (_rotationButton.WasPressedThisFrame()) {
                rotatbleComp.StartRotate(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, 90,this.transform);
            }

            // 高速回転
            if (_rotationSpinButton.WasPressedThisFrame()) {
                rotatbleComp.StartSpin(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right);
            }
        }
        else {
            _priortyAxis = priorityAxis.None;
        }
    }

    public void NotificationStartRotate() {
        _isRotating = true;
    }

    public void NotificationEndRotate() {
        _isRotating = false;
    }
}
