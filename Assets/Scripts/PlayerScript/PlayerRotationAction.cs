using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour{

    private StickRotAngle _stricRotAngle = null;
    private bool _isRotating = false;            // �񂵂Ă���Œ����𔻒肷��t���O
    
    private InputAction _blocklockButton = null;
    private InputAction _rotationButton = null;
    private InputAction _rotationSpinButton = null;

    private void PlayerRotationStart() {

        // �E�X�e�B�b�N�R���|�l���g�擾
        _stricRotAngle = GetComponent<StickRotAngle>();

        // �e��{�^���̎擾
        _blocklockButton    = _playerInput.actions.FindAction("BlockLock");
        _rotationButton     = _playerInput.actions.FindAction("Rotation");
        _rotationSpinButton = _playerInput.actions.FindAction("RotationSpin");
    }

    private void PlayerRotationUpdate() {

        // �ړ����A�W�����v���͉�]�����Ȃ�
        if (Mathf.Abs(_speedx) > 0 || !_groundCheck.IsGround) {
            return;
        }

        // �u���b�N�̃��b�N
        if (_blocklockButton.IsPressed()) {

            _speedx = 0.0f;

            // ���ɉ�]�I�u�W�F�N�g������ꍇ�������ŗD��
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

        // �^���ɉ�]�I�u�W�F�N�g�����鎞
        if(_animCallBack.GetIsRotationValid && _yBlockLock) {

            if(_bottomHitCheck.GetRotObj == null) {
                return;
            }

            var rotatbleComp = _bottomHitCheck.GetRotObj.GetComponent<RotatableObject>();

            // �X�e�B�b�N��]Y
            if (!rotatbleComp._isRotating) {
                _stricRotAngle.yAxisManyObjJude(_bottomHitCheck);
            }

            _stricRotAngle.StickRotAngleY_Update();
            rotatbleComp.StartRotateY(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);

           
            // �ʏ펲��]
            if (_rotationButton.WasPressedThisFrame()) {
                rotatbleComp.StartRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, 90, this.transform);
            }

            // ������]
            if (_rotationSpinButton.WasPressedThisFrame()) {
                if (rotatbleComp._isSpining) {
                    Debug.Log("������]�I��");
                    rotatbleComp.EndSpin();
                }
                else {
                    rotatbleComp.StartSpin(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up);
                }
            }
        }

        // �O���ɉ�]�I�u�W�F�N�g�����鎞
        else if (_animCallBack.GetIsRotationValid && _xBlockLock) {

            if (_frontHitCheck.GetRotObj == null) {
                return;
            }
            var rotatbleComp = _frontHitCheck.GetRotObj.GetComponent<RotatableObject>();

            if (!rotatbleComp._isRotating) {
                _stricRotAngle.xAxisManyObjJude(_frontHitCheck);
            }

            // �X�e�B�b�N��]X
            _stricRotAngle.StickRotAngleX_Update();
            rotatbleComp.StartRotateX(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleX, this.transform);

            // �ʏ펲��]
            if (_rotationButton.WasPressedThisFrame()) {
                rotatbleComp.StartRotate(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, 90, this.transform);
            }

            // ������]
            if (_rotationSpinButton.WasPressedThisFrame()) {
                if (rotatbleComp._isSpining) {
                    Debug.Log("������]�I��");
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
