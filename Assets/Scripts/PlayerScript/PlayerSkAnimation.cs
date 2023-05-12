using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    [SerializeField] private GameObject _skeletalObj;
    [SerializeField] private PlayerskAnimationCallBack _animCallBack;

    private Animator _animator;

    // �A�j���[�V�������Ŏg���ϐ�
    private bool _yBlockLock = false, _yBlockUpperLock = false, _xBlockLock = false;
   
    private void PlayerSkAnimationStart() {

        _animator = _skeletalObj.GetComponent<Animator>();  
    }

    private void PlayerSkAnimationUpdate() {

        // y����]�J�n�A�j���[�V����
        if (_yBlockLock) {
            transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
            _animator.SetBool("StartRot_Y", true);
        }
        // ����Ƀu���b�N�����鎞�̉�]�J�n�A�j���[�V����
        else if (_yBlockUpperLock) {
            transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
            _animator.SetBool("StartRotUpper_Y", true);
        }
        // x����]�J�n�A�j���[�V����
        else if (_xBlockLock) {
            _animator.SetBool("StartRot_X", true);
        }
        else {
            _animator.SetBool("StartRot_Y", false);
            _animator.SetBool("StartRot_X", false);
            _animator.SetBool("StartRotUpper_Y", false);
        }

        // y����]���̃A�j���[�V����
        if (_bottomHitCheck.GetRotObj != null) {

            if (_bottomHitCheck.GetRotObj.GetComponent<RotatableObject>()._isRotateStartFream) {
                _animator.SetTrigger("Rotation_Y");
            }
        }


        // ���x��0�ȏ�
        var absMove = Mathf.Abs(_speedx);
        if (absMove > 0) {

            _animator.SetBool("RunState", true);
            _animator.SetFloat("RunSpeed", absMove * 0.3f);
        }
        else {

            _animator.SetBool("RunState", false);
            _animator.SetFloat("RunSpeed", 0);
        }

       
        // �W�����v���ɓ��Ƀu���b�N������������{�[����Ԃɂ���
        if(!_groundCheck.IsGround && _upperrayCheck.IsUpperHit) {
            _animator.SetTrigger("UpperHit");
        }
    }
}
