using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class StickRotAngle : MonoBehaviour {

    void Start() {

        // InputSystem���擾
        _playerInput = GetComponent<PlayerInput>();

        //// �_�C�A���̐ݒ�
        //for (int i = 0; i < 360 / _setAngleDial; i++) {
        //    _selectface.Add(new SelectFace(i * _setAngleDial, _toleranceangle));
        //}
    }

    // ��]�I�u�W�F�N�g��Y������
    public void StickRotAngleY_Update() {


            _stickDialAngle_Y = SettingDialAngle(GetAngleY());
    }

    // ��]�I�u�W�F�N�g��X������
    public void StickRotAngleX_Update() {

       
            _stickDialAngle_X = SettingDialAngle(GetAngleX());
    }
}
