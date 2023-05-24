using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour{

    private bool _isRotating = true;

    private StickRotAngle _stricRotAngle = null;
   
    private InputAction _blocklockButton = null;
    private InputAction _rotationButton = null;
    private InputAction _rotationSpinButton = null;

    // ���b�N���Ă��鎞�̃I�u�W�F�N�g
    private GameObject _lockObject = null;
    private bool _isLock = false;
    private BlockPriorty _blockPriorty;

    // �u���b�N�D��
    enum BlockPriorty {
        Bottom,Front,None
    }

    private void PlayerRotationStart() {

        // �E�X�e�B�b�N�R���|�l���g�擾
        _stricRotAngle = GetComponent<StickRotAngle>();

        // �e��{�^���̎擾
        _blocklockButton    = _playerInput.actions.FindAction("BlockLock");
        _rotationButton     = _playerInput.actions.FindAction("Rotation");
        _rotationSpinButton = _playerInput.actions.FindAction("RotationSpin");

        // �t���[���̐F��ԐF�ɂ���
        LockFreamChangeColor(Color.red);
    }

    private void PlayerRotationUpdate() {

        // ���b�N�t���[���̉���
        LockFreamDisplay(_bottomHitCheck, _frontHitCheck);

        // �ړ����A�W�����v���͉�]�����Ȃ�
        if (Mathf.Abs(_speedx) > 0 || !_groundCheck.IsGround) {
            return;
        }

        if (_blocklockButton.IsPressed()) {
            if (!_isLock) {

                // �u���b�N�̗D��x�m�F
                _lockObject = BlockPriority(_bottomHitCheck, _frontHitCheck);

                // �A�j���[�V�����̑J��
                AnimatoinState(true);

                // �u���b�N�S���̃t���[������
                LockFreamMassSetActive(_lockObject,true);

                _isLock = true;
                Debug.Log("�u���b�N���b�N");
            }
        }
        
        // �u���b�N���b�N����
        if (_blocklockButton.WasReleasedThisFrame()) {

            AnimatoinState(false);
            LockFreamMassSetActive(_lockObject,false);
            _isLock = false;
            Debug.Log("�u���b�N���b�N����");
        }

        // Lock��
        if ((_isLock && _animCallBack.GetIsRotationValid) || _yBlockLock || _yBlockUpperLock || _xBlockLock) {

            // �ǂ����ɂ��������ĂȂ���Δ�����
            if (_lockObject == null) {
                Debug.Log("�u���b�N���m���ĂȂ�");
                return;
            }

            var rotatbleComp = _lockObject.GetComponent<RotatableObject>();
            var rotatbleKind = _lockObject.GetComponent<RotObjkinds>();

            // ���̉�]�I�u�W�F�N�g���Q��
            if (_blockPriorty == BlockPriorty.Bottom) {

                Debug.Log("���I�u�W�F�N�g�擾��");

                // �u���b�N���Ƃɉ�]�̎�ނ��Ⴄ�̂�Kind���g���Ďd������
                // �ʏ�̉��F�u���b�N�Ǝ��΃u���b�N�̓X�e�B�b�N�ł̑���ɂčs��
                if(rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.NomalRotObject || rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.UnionRotObject) {

                    // �X�e�B�b�N�ł̉�]
                    if (rotatbleComp._isRotateEndFream) {
                        _stricRotAngle.yAxisManyObjJude(_bottomHitCheck);
                    }
                    _stricRotAngle.StickRotY_Update();
                    rotatbleComp.StickRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);

                    if (rotatbleComp._isRotateStartFream && (rotatbleComp.offsetRotAxis.y == 1)) {
                        _animator.SetTrigger("Rotation_Y_Right");
                    }
                    else if (rotatbleComp._isRotateStartFream && (rotatbleComp.offsetRotAxis.y == -1)) {
                        _animator.SetTrigger("Rotation_Y_Left");
                    }
                }

                // �{���g�u���b�N����
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) {

                    _stricRotAngle.StickRotY_Update();
                    rotatbleComp.StickRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);
                }

                // ������]�̓X�e�B�b�N�̉E���|�������]�Ŏn�܂�
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.SpinObject) {

                    // R3�������ݎ��̏���
                    if (_rotationSpinButton.WasPressedThisFrame()) {

                        if (rotatbleComp._isSpining) {
                            Debug.Log("������]�I��");
                            rotatbleComp.EndSpin();
                        }
                        else {

                            rotatbleComp.StartSpin(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.up);
                        }
                    }
                }
            }

            // ���E�̉�]�I�u�W�F�N�g���Q��
            else if (_blockPriorty == BlockPriorty.Front) {

                Debug.Log("���E�I�u�W�F�N�g�擾��");

                // �u���b�N���Ƃɉ�]�̎�ނ��Ⴄ�̂�Kind���g���Ďd������
                // �ʏ�̉��F�u���b�N�Ǝ��΃u���b�N�̓X�e�B�b�N�ł̑���ɂčs��
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.NomalRotObject || rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.UnionRotObject) {

                    // �X�e�B�b�N��]X
                    _stricRotAngle.StickRotX_Update();
                    rotatbleComp.StickRotate(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleX, this.transform);
                }

                // ������]�̓X�e�B�b�N�̉������݂ɂčs��
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.SpinObject) {

                    // R3�������ݎ��̏���
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

    // �������E�ǂ������Q�Ƃ��邩�֐�
    private GameObject BlockPriority(in RotObjHitCheck _bottom, in RotObjHitCheck _front) {

        // ���ɃI�u�W�F�N�g�����邩�m�F
        if (_bottom.GetRotObj != null) {

            _blockPriorty = BlockPriorty.Bottom;
            return _bottom.GetRotObj;
        }
        else if (_front.GetRotObj != null) {
            _blockPriorty = BlockPriorty.Front;
            return _front.GetRotObj;
        }
        else {
            _blockPriorty = BlockPriorty.None;
            return null;
        }
    }

    // �A�j���[�V�����̃X�e�[�g�֐�
    private void AnimatoinState(bool stateflg) {

        switch (stateflg) {
            case true:

                // ���ɉ�]�I�u�W�F�N�g�����肩���̏�Ƀu���b�N���Ȃ��ꍇ�̃A�j���[�V����
                if (_bottomHitCheck.GetRotObj != null && !_upperrayCheck.IsUpperHit) {

                    Debug.Log(" ���ɉ�]�I�u�W�F�N�g�����肩���̏�Ƀu���b�N���Ȃ��ꍇ�̃A�j���[�V����");
                    Pos_Correction(_bottomHitCheck.GetRotPartsObj.transform.position);
                    _yBlockLock = true;
                }
                // ���ɉ�]�I�u�W�F�N�g�����肩���̏�Ƀu���b�N������ꍇ�̃A�j���[�V����
                else if (_bottomHitCheck.GetRotObj != null && _upperrayCheck.IsUpperHit) {

                    Pos_Correction(_bottomHitCheck.GetRotPartsObj.transform.position);
                    _yBlockUpperLock = true;
                }
                // ���E�ɉ�]�I�u�W�F�N�g���������ꍇ�̃A�j���[�V����
                else if (_frontHitCheck.GetRotObj != null) {

                    // ���E�Ƀ{���g������ꍇ��Y�̃A�j���[�V�����̍Đ�������
                    if (_frontHitCheck.GetRotObj.GetComponent<RotObjkinds>()._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) {
                        _yBlockLock = true;
                    }
                    else {
                        _xBlockLock = true;
                    }
                }
                // ���ɉ�]�I�u�W�F�N�g�����������Ƀu���b�N������ꍇ�̃A�j���[�V����
                else if (_bottomHitCheck.GetRotObj == null && _upperrayCheck.IsUpperHit) {

                }
                // ���ɉ�]�I�u�W�F�N�g���������Ƀu���b�N���Ȃ��ꍇ�̃A�j���[�V����
                else {
                    _yBlockLock = true;
                }

                break;

            case false:

                _yBlockLock = false;
                _xBlockLock = false;
                _yBlockUpperLock = false;
                _animCallBack.RotationInValid();
                _isLock = false;

                break;
        }
    }

    // ���b�N�t���[���̐F��ύX
    private void LockFreamChangeColor(Color color) {
        var mat = _LockFreamObj.GetComponent<MeshRenderer>().material;
        mat.SetColor("_Color", color);
    }

    // ���b�N�\�Ȏ��̂ݕ\��
    private void LockFreamDisplay(RotObjHitCheck _bottom,RotObjHitCheck _front) {
        if(_bottom.GetRotObj != null) {
            _LockFreamObj.transform.position = _bottom.GetRotPartsObj.transform.position;
            _LockFreamObj.active = true;
        }
        else if(_front.GetRotObj != null) {
            _LockFreamObj.transform.position = _front.GetRotPartsObj.transform.position;
            _LockFreamObj.active = true;
        }
        else {
            _LockFreamObj.active = false;
        }
    }

    private void LockFreamMassSetActive(GameObject _object, bool _displayflg) {

        // �ΏۃI�u�W�F�N�g�̎q�I�u�W�F�N�g���`�F�b�N����
        foreach (Transform child in _object.transform) {

            // �q�I�u�W�F�N�g�̃A�N�e�B�u��؂�ւ���
            GameObject childObject = child.gameObject;

            if(childObject.tag == "LockFream") {
                childObject.SetActive(_displayflg);
            }

            // �ċA�I�ɑS�Ă̎q�I�u�W�F�N�g����������
            LockFreamMassSetActive(childObject, _displayflg);
        }
    }
}


