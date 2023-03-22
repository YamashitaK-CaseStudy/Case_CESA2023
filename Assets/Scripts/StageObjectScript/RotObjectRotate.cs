using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour {

    // ���g�̎��ł܂킷�����J�n
    public void StartRotate() {
        if ( _isSpin || _isRotating ) {
            return;
        }

        // �t���O�𗧂Ă�
        _isRotating = true;

        // �o�ߎ��Ԃ�������
        _elapsedTime = 0.0f;
    }


    // �܂킷���̍X�V
    void UpdateRotate()
    {
        // ��]�����t���O
        if ( _isRotating ) {

            // ���N�G�X�g�f���^�^�C�������߂�
            // ���N�G�X�g�f���^�^�C���F�f���^�^�C����1��]�ɕK�v�Ȏ��ԂŊ������l
            // ����̍��Z�l��1�ɂȂ�����,1��]�ɕK�v�Ȏ��Ԃ��o�߂������ƂɂȂ�
            float requiredDeltaTime = Time.deltaTime/_rotRequirdTime;
            _elapsedTime += requiredDeltaTime;

            // �ڕW��]��*���N�G�X�g�f���^�^�C���ł��̃t���[���ł̉�]�p�x�����߂邱�Ƃ��ł���
            // ���N�G�X�g�f���^�^�C���̍��Z�l�����傤��1�ɂȂ�悤�ɕ␳��������Ƒ���]�ʂ͖ڕW��]�ʂƈ�v����
            if ( _elapsedTime >= 1 ) {
                _isRotating = false;
                requiredDeltaTime -= ( _elapsedTime - 1 ); // �␳
            }

            // ���݃t���[���̉�]��������]�̃N�H�[�^�j�I���쐬
            var angleAxis = Quaternion.AngleAxis(180 * requiredDeltaTime, _selfRotAxis);

            // �~�^���̈ʒu�v�Z
            var tr = transform;
            var pos = tr.position;
            
            // �N�H�[�^�j�I����p������]�͌��_����̃I�t�Z�b�g��p����K�v������
            // _axisCenterWorldPos��C�ӎ��̍��W�ɕύX����ΔC�ӎ��̉�]���ł���
            pos -= _axisCenterWorldPos;
            pos = angleAxis * pos;
            pos += _axisCenterWorldPos;

            tr.position = pos;

            // �����X�V
            tr.rotation = tr.rotation * angleAxis;
        }
    }
}