using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class RotatableObject : MonoBehaviour {

    private Transform _playerTransform = null;// �v���C���[�̃g�����X�t�H�[��

    // ���g�̎��ł܂킷�����J�n
    public void StartRotate() {
        if ( _isSpin || _isRotating ) {
            return;
        }

        // �t���O�𗧂Ă�
        _isRotating = true;

        // �o�ߎ��Ԃ�������
        _elapsedTime = 0.0f;

        // �g���C���̋N��
        PlayPartical();
    }

    public void StartRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle) {

        if ( _isSpin || _isRotating ) {
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

        // �g���C���̋N��
        PlayPartical();
    }

   
    public void StartRotate(Vector3 rotCenter, Vector3 rotAxis,int rotAngle,Transform playerTransform) {
        
        if ( _isSpin || _isRotating ) {
            return;
        }

        // �g�����X�t�H�[�����i�[
        _playerTransform = playerTransform;

        // �t���O�𗧂Ă�
        _isRotating = true;

        // �o�ߎ��Ԃ�������
        _elapsedTime = 0.0f;

        // �덷���C������
        var pos = new Vector3(0,0,0);
        pos.x = (float)Math.Round(this.transform.position.x, 0, MidpointRounding.AwayFromZero);
        pos.y = (float)Math.Round(this.transform.position.y, 0, MidpointRounding.AwayFromZero);
        pos.z = (float)Math.Round(this.transform.position.z, 0, MidpointRounding.AwayFromZero);
        this.transform.position = pos;

        // ��]�̒��S��ݒ�
        _axisCenterWorldPos = rotCenter;
        
        // ��]����ݒ�
        _rotAxis = rotAxis;

        // ��]�I�t�Z�b�g�l���Z�b�g
        _angle = rotAngle;

        // �g���C���̋N��
        PlayPartical();
    }

    // �܂킷���̍X�V
    protected void UpdateRotate()
    {
        if (_doOnce) {
            _isRotateStartFream = false;
        }

        // ��]�����t���O
        if ( _isRotating ) {

            if (!_doOnce) {
                _isRotateStartFream = true;
                _doOnce = true;
            }
        
            // ���N�G�X�g�f���^�^�C�������߂�
            // ���N�G�X�g�f���^�^�C���F�f���^�^�C����1��]�ɕK�v�Ȏ��ԂŊ������l
            // ����̍��Z�l��1�ɂȂ�����,1��]�ɕK�v�Ȏ��Ԃ��o�߂������ƂɂȂ�
            float requiredDeltaTime = Time.deltaTime/_rotRequirdTime;
            _elapsedTime += requiredDeltaTime;

            // �ڕW��]��*���N�G�X�g�f���^�^�C���ł��̃t���[���ł̉�]�p�x�����߂邱�Ƃ��ł���
            // ���N�G�X�g�f���^�^�C���̍��Z�l�����傤��1�ɂȂ�悤�ɕ␳��������Ƒ���]�ʂ͖ڕW��]�ʂƈ�v����
            if ( _elapsedTime >= 1 ) {
                //Debug.Log("��ԏI��");
                _isRotating = false;
                requiredDeltaTime -= ( _elapsedTime - 1 ); // �␳

                _isRotateEndFream = true;
                StopPartical();

                // �v���C���[�N���̉�]���𔻒�
                if ( _playerTransform != null ) {
                    Debug.Log(_playerTransform.gameObject);
                
                    var playerComp = _playerTransform.GetComponent<Player>();

                    if ( playerComp == null ) {
                        Debug.Log("���肦�Ȃ��b");
                    }

                    // �v���C���[�ɉ�]�I���ʒm���΂�
                    playerComp.NotificationEndRotate();

                    // �o�O�h�~
                    _playerTransform = null;
                }

            }

            // ���݃t���[���̉�]��������]�̃N�H�[�^�j�I���쐬
            var angleAxis = Quaternion.AngleAxis(_angle * requiredDeltaTime, _rotAxis);

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
            tr.rotation = angleAxis * tr.rotation;
        }
        else {
            _doOnce = false;
            _isRotateEndFream = false;
        }
    }


}
