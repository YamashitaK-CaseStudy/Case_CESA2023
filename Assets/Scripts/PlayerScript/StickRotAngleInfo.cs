using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class StickRotAngle : MonoBehaviour {

    [SerializeField, Header("�f�b�h�]�[��")] private float _deadzone;

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

    // �J�����̐��ʂ��猩�ĉE������O�łǂ����ɃI�u�W�F�N�g��������
    private enum LeftRightFrontBack_ManyObj {
        Right,
        Left,
        Front,
        Back,
        Same
    }

    // �J�����񐳖ʂ��猩�ď㉺��O�łǂ����ɃI�u�W�F�N�g��������
    private enum UpDownFrontBack_ManyObj {
        Up,
        Down,
        Front,
        Back,
        Same
    }

    //------------------------------------------------------------------------------------------------------//
    //------------------------------------------------------------------------------------------------------//

    // Y����]�� �E������O�łǂ����ɃI�u�W�F�N�g�����������肷��
    public void LRFB_Many_Jude(RotObjHitCheck _hitcheck) {

        // ����Ă�u���b�N�̍��W���擾����
        var rideObj_x = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.x);
        var rideObj_z = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.z);

        // �u���b�N���܂Ƃ߂Ă�e���擾����
        var objects = _hitcheck.GetRotObj.transform.Find("Object").gameObject;

        // �u���b�N�̐�
        int rightnum = 0, leftnum = 0, frontnum = 0, backnum = 0;

        // �u���b�N�̐����肷��
        for (int i = 0; i < objects.transform.childCount; i++) {

            var parts_x = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.x);
            var parts_z = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.z);

            // Debug.Log(parts_x);

            // x������
            if (rideObj_x != parts_x) {

                if (rideObj_x < parts_x) {
                    rightnum++;
                }
                else {
                    leftnum++;
                }
            }

            // z������
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

    // �v���C���[�̈ʒu�ɂ��X�e�B�b�N�̊p�x���擾����
    private int GetAngleY() {

        var value = _playerInput.actions["RotaionSelect"].ReadValue<Vector2>();

        if ((value.x < -_deadzone || _deadzone < value.x || value.y < -_deadzone || _deadzone < value.y)) {

            // �E
            if (_lrfb_Manyobj == LeftRightFrontBack_ManyObj.Right) {

                _stickAngle_Y = (int)(Mathf.Atan2(-value.y, value.x) * Mathf.Rad2Deg);
            }
            // ��
            else if (_lrfb_Manyobj == LeftRightFrontBack_ManyObj.Left) {

                _stickAngle_Y = (int)(Mathf.Atan2(value.y, -value.x) * Mathf.Rad2Deg);
            }
            // ��
            else if (_lrfb_Manyobj == LeftRightFrontBack_ManyObj.Back) {

                _stickAngle_Y = (int)(Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg);
            }
            // ��O
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

    // Y����]�� �E������O�łǂ����ɃI�u�W�F�N�g�����������肷��
    public void UDFB_Many_Jude(RotObjHitCheck _hitcheck) {

        // ����Ă�u���b�N�̍��W���擾����
        var rideObj_y = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.y);
        var rideObj_z = (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.z);

        // �u���b�N���܂Ƃ߂Ă�e���擾����
        var objects = _hitcheck.GetRotObj.transform.Find("Object").gameObject;

        // �u���b�N�̐�
        int upnum = 0, downnum = 0, frontnum = 0, backnum = 0;

        // �w�����I�u�W�F�N�g���E���������肷��
        if(transform.position.x < (int)Mathf.Round(_hitcheck.GetRotPartsObj.transform.position.x)) {
            _playerLR_obj = 1;
        }
        else {
            _playerLR_obj = -1;
        }

        // �u���b�N�̐����肷��
        for (int i = 0; i < objects.transform.childCount; i++) {

            var parts_y = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.y);
            var parts_z = (int)Mathf.Round(objects.transform.GetChild(i).transform.position.z);

            // x������
            if (rideObj_y != parts_y) {

                if (rideObj_y < parts_y) {
                    upnum++;
                }
                else {
                    downnum++;
                }
            }

            // z������
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

   
    // �v���C���[�̈ʒu�ɂ��X�e�B�b�N�̊p�x���擾����
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
                Debug.Log("����ȊO");
            }
        }

        if (_stickAngle_X < 0) {
            _stickAngle_X += 360;
        }

        return _stickAngle_X;
    }

    //------------------------------------------------------------------------------------------------------//
    //------------------------------------------------------------------------------------------------------//

    // �X�e�B�b�N�̊p�x���_�C�A���ɐU�蕪����
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