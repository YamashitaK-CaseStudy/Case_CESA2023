using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour{

    private bool _isRotating = true;

    private RightStickRotAngle _stricRotAngle = null;
    private InputAction _blocklockButton = null;
    private InputAction _rotationSpinButton = null;

    // ���b�N���Ă��鎞�̃I�u�W�F�N�g
    private GameObject _lockRootObject = null;
    private GameObject _lockObjectParts = null;
    private bool _isLock = false;
    private BlockPriorty _blockPriorty;

    // ��]�I�u�W�F�N�g�̃R���|�[�l���g
    private RotatableObject _rotComp = null;
    private RotObjkinds _rotatbleKind = null;

    // �u���b�N�D��
    enum BlockPriorty {
        Bottom,Front,None
    }

    private void PlayerRotationStart() {

        // �E�X�e�B�b�N�R���|�l���g�擾
        _stricRotAngle = GetComponent<RightStickRotAngle>();

        // �e��{�^���̎擾
        _blocklockButton    = _playerInput.actions.FindAction("BlockLock");
        _rotationSpinButton = _playerInput.actions.FindAction("RotationSpin");

        // �v���C���[�̃��b�N�t���[���̐F�ύX
        LockFreamChangeColor(Color.red);
    }

    private void PlayerRotationUpdate() {

        // ���b�N�t���[���̉���
        LockFreamDisplay(_bottomHitCheck, _frontHitCheck);

        // ���b�N�{�^���������ꂽ��
        if (_blocklockButton.IsPressed()) {
            if (!_isLock) {
                Debug.Log("�u���b�N���b�N");
                _isLock = true;

                // �u���b�N�̗D��x�m�F
                BlockPriority(_bottomHitCheck, _frontHitCheck);

                // �e�A�j���[�V�����̑J��
                AnimatoinState(true);

                // �u���b�N�S���̃t���[������
                LockFreamMassSetActive(_lockRootObject, true);

                // ����̓����蔻��ray��L�΂�(�{���g�p��)
                _upperrayCheck.SetDistacne(1.5f);
            }
        }

        // ���b�N�{�^���������ꂽ��
        if (_blocklockButton.WasReleasedThisFrame()) {
            Debug.Log("�u���b�N���b�N����");
            _isLock = false;
            LockFreamMassSetActive(_lockRootObject, false);
            _stricRotAngle.yAxisDestroyDamiobj();
            AnimatoinState(false);
            _upperrayCheck.SetDistacne(0.7f);
        }

        if ((_isLock && _animCallBack.GetIsRotationValid)) {

            // ���̉�]�I�u�W�F�N�g���Q��
            if (_blockPriorty == BlockPriorty.Bottom) {

                // �u���b�N���Ƃɉ�]�̎�ނ��Ⴄ�̂�Kind���g���Ďd������
                // �ʏ�u���b�N����
                if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.NomalRotObject ||
                    _rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.UnionRotObject) {
                    Debug.Log("�ʏ��]����");

                    if (_rotComp._isRotateEndFream) {
                        Debug.Log("��]�I��");
                        _stricRotAngle.yAxisManyObjJude(_bottomHitCheck);
                    }

                    _stricRotAngle.StickRotY_Update();
                    _rotComp.StickRotateY(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);

                    if (_stricRotAngle._isOldAngleChinge) {

                        _animator.SetFloat("RotationSpeed", 6);
                        if(_rotComp.offsetRotAxis.y == 1) {
                            _animator.SetTrigger("Rotation_Y_Right");
                        }
                        else if(_rotComp.offsetRotAxis.y == -1) {
                            _animator.SetTrigger("Rotation_Y_Left");
                        }
                    }
                }

                // �{���g����
                else if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) {

                    Debug.Log("�{���g��]����");
                    _animator.SetFloat("RotationSpeed", 1);
                    if (!_upperrayCheck.IsUpperHit) {
                        _rotComp.StickRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);

                        if (_rotComp._isRotateStartFream) {
                            _animator.SetTrigger("Rotation_Y_Right");
                            Debug.Log("�{���g��]�͂���");
                        }
                    }
                }

                // ������]����
                else if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.SpinObject) {
                    Debug.Log("������]����");

                    if (_rotationSpinButton.WasPressedThisFrame()) {

                        if (_rotComp._isSpining) {
                            Debug.Log("������]�I��");
                            _rotComp.EndSpin();
                        }
                        else {
                            _rotComp.StartSpin(CompensateRotationAxis(_lockObjectParts.transform.position), Vector3.up);
                        }
                    }
                }
            }
            else if (_blockPriorty == BlockPriorty.Front) {

                // �u���b�N���Ƃɉ�]�̎�ނ��Ⴄ�̂�Kind���g���Ďd������
                // �ʏ�u���b�N����
                if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.NomalRotObject ||
                    _rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.UnionRotObject) {

                    _stricRotAngle.StickRotX_Update();
                    _rotComp.StickRotateY(CompensateRotationAxis(_frontHitCheck.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleX, this.transform);
                }

                // �{���g����
                else if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) {

                    if (!_backHItCheck.GetIsHit && _lockObjectParts.tag == "ScrewObj") {
                        _rotComp.StickRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleY, this.transform);
                    }
                }

                // ������]����
                else if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.SpinObject) {

                    if (_rotationSpinButton.WasPressedThisFrame()) {

                        if (_rotComp._isSpining) {
                            Debug.Log("������]�I��");
                            _rotComp.EndSpin();
                        }
                        else {
                            _rotComp.StartSpin(CompensateRotationAxis(_lockObjectParts.transform.position), Vector3.right);
                        }
                    }
                }
            }
        }
    }

    // ���b�N�\�ȉ�]�I�u�W�F�N�g����������
    private void LockFreamDisplay(RotObjHitCheck _bottom, RotObjHitCheck _front) {

        if (_bottom.GetRotPartsObj != null) {

            Debug.Log("�\���艺");
            _LockFreamObj.transform.position = _bottom.GetRotPartsObj.transform.position;
            _LockFreamObj.active = true;

            // �{���g�̌������������ł���Ή������Ȃ�
            var boltComp = _bottom.GetRotObj.GetComponent<Bolt>();
            if (boltComp != null && boltComp.upVectorInWorld == Bolt.UpVectorInWorld.HORIZONTAL) {
                _LockFreamObj.active = false;
            }
        }
        else if (_front.GetRotPartsObj != null) {

            Debug.Log("�\���荶�E");
            _LockFreamObj.transform.position = _front.GetRotPartsObj.transform.position;
            _LockFreamObj.active = true;

            // �{���g�̌������������ł���Ή������Ȃ�
            var boltComp = _front.GetRotObj.GetComponent<Bolt>();
            if (boltComp != null && boltComp.upVectorInWorld == Bolt.UpVectorInWorld.VERTICAL || 
                _front.GetRotPartsObj.tag == "ThreadObj") {
                _LockFreamObj.active = false;
            }
        }
        else {

            _LockFreamObj.active = false;
        }
    }

    // �������E�ǂ������Q�Ƃ��邩�֐�
    private void BlockPriority(in RotObjHitCheck _bottom, in RotObjHitCheck _front) {

        // ���ɃI�u�W�F�N�g�����邩�m�F
        if (_bottom.GetRotObj != null) {

            _blockPriorty = BlockPriorty.Bottom;
            _lockRootObject = _bottom.GetRotObj;
            _lockObjectParts = _bottom.GetRotPartsObj;
            _stricRotAngle.yAxisManyObjJude(_bottom);
            GetRotationComponent();
        }
        else if (_front.GetRotObj != null) {

            _blockPriorty = BlockPriorty.Front;
            _lockRootObject = _front.GetRotObj;
            _lockObjectParts = _front.GetRotPartsObj;
            GetRotationComponent();
        }
        else {

            _blockPriorty = BlockPriorty.None;
            _lockRootObject = null;
            _lockObjectParts = null;
        }
    }

    // �A�j���[�V�����̃X�e�[�g�֐�
    private void AnimatoinState(bool _isState) {

        switch (_isState) {
            case true:

                // ���ɉ�]�I�u�W�F�N�g������ꍇ
                if (_bottomHitCheck.GetRotObj != null) {

                    // ���Ƀu���b�N���Ȃ��ꍇ
                    if (!_upperrayCheck.IsUpperHit) {
                        _yBlockLock = true;
                    }
                    // ���Ƀu���b�N������ꍇ
                    else {
                        _yBlockUpperLock = true;
                    }

                    // ���W�␳
                    Pos_Correction(_bottomHitCheck.GetRotPartsObj.transform.position);
                }

                // ���E�ɉ�]�I�u�W�F�N�g������ꍇ
                else if(_frontHitCheck.GetRotObj != null) {

                    _xBlockLock = true;

                    // �{���g�̂˂������ł���ΑJ�ڂ��Ȃ�
                    if (_frontHitCheck.GetRotPartsObj.tag == "ThreadObj") {
                        _xBlockLock = false;
                    }
                }

                // ���ɉ�]�I�u�W�F�N�g�������ꍇ
                else {
                    // ���Ƀu���b�N���Ȃ��ꍇ
                    if (!_upperrayCheck.IsUpperHit) {
                        _yBlockLock = true;
                    }
                }

                break;

            case false:

                _yBlockLock = false;
                _xBlockLock = false;
                _yBlockUpperLock = false;
                _animCallBack.RotationInValid();

                break;
        }
    }

    private void LockFreamMassSetActive(GameObject _object, bool _displayflg) {

        if (_object == null) {
            return;
        }

        // �ΏۃI�u�W�F�N�g�̎q�I�u�W�F�N�g���`�F�b�N����
        foreach (Transform child in _object.transform) {

            // �q�I�u�W�F�N�g�̃A�N�e�B�u��؂�ւ���
            GameObject childObject = child.gameObject;

            if (childObject.tag == "LockFream") {
                childObject.SetActive(_displayflg);
            }

            // �ċA�I�ɑS�Ă̎q�I�u�W�F�N�g����������
            LockFreamMassSetActive(childObject, _displayflg);
        }
    }

    // ���b�N�t���[���̐F��ύX
    private void LockFreamChangeColor(Color color) {
        var mat = _LockFreamObj.GetComponent<MeshRenderer>().material;
        mat.SetColor("_Color", color);
    }

    // �v���C���[�̍��W�␳
    private void Pos_Correction(in Vector3 center) {

        var Pos = transform.position;
        var pPos = new Vector3(center.x, Pos.y, 0);
        transform.transform.position = pPos;
    }

    // ���W�␳�@Player�̂�}���R�s
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

    public void GetRotationComponent() {

        _rotComp = _lockRootObject.GetComponent<RotatableObject>();
        _rotatbleKind = _lockRootObject.GetComponent<RotObjkinds>();
    }

    public void NotificationStartRotate() {
        _isRotating = true;
    }

    public void NotificationEndRotate() {
        _isRotating = false;
    }
}


