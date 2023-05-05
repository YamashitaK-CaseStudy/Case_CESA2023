using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class RotatableObject : MonoBehaviour
{

    private Transform _playerTransform = null;// �v���C���[�̃g�����X�t�H�[��

    public bool _isReservation = false;// �\��t���O
    public Vector3 _resePos { get; set; }   // �\����W
    public Vector3 _reseAxis { get; set; }  // �\��
    public int _reseAngle { get; set; }     // �\���]
    private float _oldAngle = 0.0f;
    private RotHitFloar[] _childComp;
    private float _polatAngle = 0.0f;
    private void StartSettingRot()
    {
        var child = this.transform.GetChild(0).gameObject;
        _childComp = new RotHitFloar[child.transform.childCount];
        for (int i = 0; i < child.transform.childCount; i++)
        {
            _childComp[i] = child.transform.GetChild(i).GetComponent<RotHitFloar>();
        }
    }

    public void StartRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle)
    {

        if (_isSpin || _isRotating)
        {
            return;
        }
        // ��]�̒��S��ݒ�
        _axisCenterWorldPos = rotCenter;
        _resePos = rotCenter;

        // ��]����ݒ�
        _rotAxis = rotAxis;
        _reseAxis = rotAxis;

        // ��]�I�t�Z�b�g�l���Z�b�g
        _angle = rotAngle;
        _reseAngle = rotAngle;
        // �t���O�𗧂Ă�
        _isRotating = true;

        // �p�x�ɂ��␳�l���v�Z����
        _polatAngle = _angle / 90;
        Debug.Log(_polatAngle);

        // �o�ߎ��Ԃ�������
        _elapsedTime = 0.0f;
        // �g���C���̋N��
        PlayPartical();
    }


    public void StartRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle, Transform playerTransform)
    {

        if (_isSpin || _isRotating)
        {
            return;
        }

        // ��]�̒��S��ݒ�
        _axisCenterWorldPos = rotCenter;
        _resePos = rotCenter;

        // ��]����ݒ�
        _rotAxis = rotAxis;
        _reseAxis = rotAxis;

        // ��]�I�t�Z�b�g�l���Z�b�g
        _angle = rotAngle;
        _reseAngle = rotAngle;

        // �g�����X�t�H�[�����i�[
        _playerTransform = playerTransform;

        // �t���O�𗧂Ă�
        _isRotating = true;

        // �o�ߎ��Ԃ�������
        _elapsedTime = 0.0f;

        // �p�x�ɂ��␳�l���v�Z����
        _polatAngle = _angle / 90;
        Debug.Log(_polatAngle);

        // �g���C���̋N��
        PlayPartical();
    }

    // �܂킷���̍X�V
    protected void UpdateRotate()
    {
        if (_doOnce)
        {
            _isRotateStartFream = false;
        }

        // ��]�����t���O
        if (_isRotating)
        {
            if (!_doOnce)
            {
                _isRotateStartFream = true;
                _doOnce = true;
            }

            // ���N�G�X�g�f���^�^�C�������߂�
            // ���N�G�X�g�f���^�^�C���F�f���^�^�C����1��]�ɕK�v�Ȏ��ԂŊ������l
            // ����̍��Z�l��1�ɂȂ�����,1��]�ɕK�v�Ȏ��Ԃ��o�߂������ƂɂȂ�
            float requiredDeltaTime = Time.deltaTime / (_rotRequirdTime * Math.Abs(_polatAngle));
            _elapsedTime += requiredDeltaTime;

            // �ڕW��]��*���N�G�X�g�f���^�^�C���ł��̃t���[���ł̉�]�p�x�����߂邱�Ƃ��ł���
            // ���N�G�X�g�f���^�^�C���̍��Z�l�����傤��1�ɂȂ�悤�ɕ␳��������Ƒ���]�ʂ͖ڕW��]�ʂƈ�v����
            bool isFinish = false;
            if (_elapsedTime >= 1)
            {
                _isRotating = false;
                _isRotateEndFream = true;
                requiredDeltaTime -= (_elapsedTime - 1); // �␳

                StopPartical();

                // �v���C���[�N���̉�]���𔻒�
                if (_playerTransform != null)
                {
                    var playerComp = _playerTransform.GetComponent<Player>();

                    // �v���C���[�ɉ�]�I���ʒm���΂�
                    playerComp.NotificationEndRotate();

                    // �o�O�h�~
                    _playerTransform = null;
                }
                isFinish = true;
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

            _oldAngle = _elapsedTime * _angle;
            // 90�x�i�ނ��ƂɊm�F�������Ă邩�ǂ������m�F����
            if(Math.Abs(_polatAngle) > 1){    // 90�x��1�ɂȂ�̂ł���ȏォ�ǂ����m�F
                if(_elapsedTime >= 1 / Math.Abs(_polatAngle)){
                    Debug.Log(_elapsedTime);
                    Debug.Log("90�x�o��");
                    isFinish = true;
                    _isRotating = false;

                    // �ً}��~���Ă���̂Ŋp�x�ɕ␳�������Ȃ��ƌ덷���o��
                    var tmpAngle = this.transform.eulerAngles;
                    // if(_rotAxis.x != 0){
                    //     tmpAngle.x = _polatAngle * 90;
                    // }else if(_rotAxis.y != 0){
                    //     tmpAngle.y = _polatAngle * 90;
                    // }
                    this.transform.eulerAngles = tmpAngle;
                    _polatAngle = 0.0f;
                }
            }

            if(isFinish){
                // �덷���C������
                var tmppos = new Vector3(0, 0, 0);
                tmppos.x = (float)Math.Round(this.transform.position.x, 0, MidpointRounding.AwayFromZero);
                tmppos.y = (float)Math.Round(this.transform.position.y, 0, MidpointRounding.AwayFromZero);
                tmppos.z = (float)Math.Round(this.transform.position.z, 0, MidpointRounding.AwayFromZero);
                this.transform.position = tmppos;

                _isRotating = false;
                _elapsedTime = 0.0f;
                _isRotateEndFream = true;

                CheckHitNotMoveObj();
            }
        }
        else
        {
            _doOnce = false;
            _isRotateEndFream = false;
        }
    }

    private void CheckHitNotMoveObj()
    {
        if (!_isReservation) return;
        _isReservation = false;
        StartRotate(_resePos, -_reseAxis, _reseAngle);
    }
}
