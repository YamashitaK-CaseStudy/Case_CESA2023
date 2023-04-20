using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour{

    // �O��̉�]
    public int oldangleY = 0;
    public int oldangleX = 0;


    public void StartStickRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle) {

        if (_isSpin || _isRotating) {
            return;
        }

        // �t���O�𗧂Ă�
        _isRotating = true;

        // �o�ߎ��Ԃ�������
        _elapsedTime = 0.0f;

        // ��]�̒��S��ݒ�
        _axisCenterWorldPos = rotCenter;

        // ��]����ݒ�
        _rotAxis = rotAxis;

        // ��]�I�t�Z�b�g�l���Z�b�g
        _angle = rotAngle;
    }



    //private void RotateAxis(Vector3 center, Vector3 axis, int angle) {

    //    // ���݃t���[���̉�]��������]�̃N�H�[�^�j�I���쐬
    //    var angleAxis = Quaternion.AngleAxis(angle, axis);

    //    // �~�^���̈ʒu�v�Z
    //    var tr = transform;
    //    var pos = tr.position;

    //    // �N�H�[�^�j�I����p������]�͌��_����̃I�t�Z�b�g��p����K�v������
    //    // _axisCenterWorldPos��C�ӎ��̍��W�ɕύX����ΔC�ӎ��̉�]���ł���
    //    pos -= center;
    //    pos = angleAxis * pos;
    //    pos += center;

    //    tr.position = pos;

    //    // �����X�V
    //    tr.rotation = angleAxis * tr.rotation;
    //}

    public Vector3 _nowRotAxis;
   
    public void StartRotateX(Vector3 center, Vector3 axis, int angle) {

        if (oldangleX == angle) {
            return;
        }

        var offset = angle - oldangleX;

        if (offset > 0) {
            _nowRotAxis = new Vector3(0, -1, 0);
        }
        else {
            _nowRotAxis = new Vector3(0, 1, 0);
        }

        StartStickRotate(center, axis, offset);
       // RotateAxis(center, axis, offset);

        oldangleX = angle;
    }

    public void StartRotateY(Vector3 center, Vector3 axis, int angle) {

        if (oldangleY == angle) {
            return;
        }

        var offset = angle - oldangleY;

        // ���v���
        if(offset > 0) {
            _nowRotAxis = new Vector3(0, 1, 0);
        }
        else {
            _nowRotAxis = new Vector3(0, -1, 0);
        }

        StartStickRotate(center, axis, offset);
        //RotateAxis(center, axis, offset);

        oldangleY = angle;
    }
}
