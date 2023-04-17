using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour{

    private void RotateAxis(Vector3 center, Vector3 axis, int angle) {

        // ���݃t���[���̉�]��������]�̃N�H�[�^�j�I���쐬
        var angleAxis = Quaternion.AngleAxis(angle, axis);

        // �~�^���̈ʒu�v�Z
        var tr = transform;
        var pos = tr.position;

        // �N�H�[�^�j�I����p������]�͌��_����̃I�t�Z�b�g��p����K�v������
        // _axisCenterWorldPos��C�ӎ��̍��W�ɕύX����ΔC�ӎ��̉�]���ł���
        pos -= center;
        pos = angleAxis * pos;
        pos += center;

        tr.position = pos;

        // �����X�V
        tr.rotation = angleAxis * tr.rotation;
    }


    // �O��̉�]
    public int oldangleY = 0;
    public int oldangleX = 0;

    public void StartRotateX(Vector3 center, Vector3 axis, int angle) {

        if (oldangleX == angle) {
            return;
        }

        var offset = angle - oldangleX;

        RotateAxis(center, axis, offset);

        oldangleX = angle;
    }

    public void StartRotateY(Vector3 center, Vector3 axis, int angle) {

        if (oldangleY == angle) {
            return;
        }

        var offset = angle - oldangleY;

        RotateAxis(center, axis, offset);

        oldangleY = angle;
    }
}
