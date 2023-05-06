using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    private StickRotAngle _stricRotAngle = null;
    private bool _isRotating = false;            // �񂵂Ă���Œ����𔻒肷��t���O
    private priorityAxis _priortyAxis;

    private enum priorityAxis {
        yAxisRot, xAxisRot,None
    }

    private void PlayerRotationStart() {

        // �E�X�e�B�b�N�R���|�l���g�擾
        _stricRotAngle = GetComponent<StickRotAngle>();

        // �e��{�^���̎擾
        _rotationButton     = _playerInput.actions.FindAction("Rotation");
        _rotationSpinButton = _playerInput.actions.FindAction("RotationSpin");
    }

    private void PlayerRotationUpdate() {

        // �ړ����A�W�����v���͉�]�����Ȃ�
        if(Mathf.Abs( _rigidbody.velocity.x) > 0 || !_groundCheck.IsGround) {
            return;
        }

        // ���ɉ�]�I�u�W�F�N�g������ꍇ�������ŗD��
        
        // �^���ɉ�]�I�u�W�F�N�g�����鎞
        if (_bottomHitCheck.GetIsRotHit) {

            _priortyAxis = priorityAxis.yAxisRot;

            var rotatbleComp = _bottomHitCheck.GetRotObj.GetComponent<RotatableObject>();

            // �X�e�B�b�N��]Y
            if (rotatbleComp._isRotateEndFream) {
                _stricRotAngle.yAxisManyObjJude(_bottomHitCheck);
            }

            _stricRotAngle.StickRotAngleY_Update();
            rotatbleComp.StartRotateY(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY,this.transform);
           
            // �ʏ펲��]
            if (_rotationButton.WasPressedThisFrame()) {
                rotatbleComp.StartRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, 90, this.transform);
            }

            // ������]
            if (_rotationSpinButton.WasPressedThisFrame()) {
                Debug.Log(_bottomHitCheck.GetRotObj);
                rotatbleComp.StartSpin(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up);
            }
        }

        // �O���ɉ�]�I�u�W�F�N�g�����鎞
        else if (_frontHitCheck.GetIsRotHit) {

            _priortyAxis = priorityAxis.xAxisRot;

            var rotatbleComp = _frontHitCheck.GetRotObj.GetComponent<RotatableObject>();
            Debug.Log(_frontHitCheck.GetRotObj);


            if (rotatbleComp._isRotateEndFream) {
                _stricRotAngle.xAxisManyObjJude(_frontHitCheck);
            }

            // �X�e�B�b�N��]X
            _stricRotAngle.StickRotAngleX_Update();
            rotatbleComp.StartRotateX(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleX, this.transform);

            // �ʏ펲��]
            if (_rotationButton.WasPressedThisFrame()) {
                rotatbleComp.StartRotate(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, 90,this.transform);
            }

            // ������]
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
