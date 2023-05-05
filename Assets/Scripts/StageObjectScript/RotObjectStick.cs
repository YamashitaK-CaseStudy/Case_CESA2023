using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour{

    // �O��̉�]
    public int oldangleY = 0;
    public int oldangleX = 0;

    // public void StartStickRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle) {

    //     if (_isSpin || _isRotating) {
    //         return;
    //     }

    //     var playerComp = _playerTransform.GetComponent<Player>();

    //     playerComp.NotificationStartRotate();
    
    //     // �t���O�𗧂Ă�
    //     _isRotating = true;

    //     // �o�ߎ��Ԃ�������
    //     _elapsedTime = 0.0f;

    //     // ��]�̒��S��ݒ�
    //     _axisCenterWorldPos = rotCenter;

    //     // ��]����ݒ�
    //     _rotAxis = rotAxis;

    //     // ��]�I�t�Z�b�g�l���Z�b�g
    //     _angle = rotAngle;

    //     PlayPartical();

    // }

    public Vector3 _nowRotAxis;
   
    public void StartRotateX(Vector3 center, Vector3 axis, int angle, Transform playerTransform) {
        if (oldangleX == angle) {
            return;
        }

        // �v���C���[�̃g�����X�t�H�[����ێ�
        _playerTransform = playerTransform;

        var offset = angle - oldangleX;

        if (offset > 0) {
            _nowRotAxis = new Vector3(0, -1, 0);
        }
        else {
            _nowRotAxis = new Vector3(0, 1, 0);
        }

        StartRotate(center, axis, offset, playerTransform);
       // RotateAxis(center, axis, offset);

        oldangleX = angle;
    }

    public void StartRotateY(Vector3 center, Vector3 axis, int angle,Transform playerTransform) {
        if (oldangleY == angle) {
            return;
        }
        // �v���C���[�̃g�����X�t�H�[����ێ�
        _playerTransform = playerTransform;

        var Pos = playerTransform.position;
        var pPos = new Vector3(center.x,Pos.y,0);
        playerTransform.transform.position = pPos;

        var offset = angle - oldangleY;

        // ���v���
        if(offset > 0) {
            _nowRotAxis = new Vector3(0, 1, 0);
        }
        else {
            _nowRotAxis = new Vector3(0, -1, 0);
        }

        StartRotate(center, axis, offset, playerTransform);
        //StartStickRotate(center, axis, offset);
        //RotateAxis(center, axis, offset);

        oldangleY = angle;
        PlayPartical();
    }
}
