using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class Player : MonoBehaviour{

    [SerializeField] private GameObject _skeletalObj;
    [SerializeField] private PlayerskAnimationCallBack _animCallBack;
    [SerializeField] private VisualEffect _jumpEffectRight,_jumpEffectLeft;
    [SerializeField] private VisualEffect _moveEffect;

    private Animator _animator;
    private bool _jumpEffectdoOnce = false;
    private List<GameObject> _MoveEffectList = new List<GameObject>();

    public Animator GetAnimator {
        get { return _animator; }
    }

    // �A�j���[�V�������Ŏg���ϐ�
    private bool _yBlockLock = false, _yBlockUpperLock = false, _xBlockLock = false;
   
    private void PlayerSkAnimationStart() {
       
        _animator = _skeletalObj.GetComponent<Animator>();
    }

    GameObject _lastCloneMoveEffect = null;

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

        // �W�����v���ɓ��Ƀu���b�N������������{�[����Ԃɂ���
        if (!_groundCheck.IsGround && _upperrayCheck.IsUpperHit) {
            _animator.SetTrigger("UpperHit");
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

       
        //Debug.Log("���x" + Math.Abs(_speedx));
        // �ړ����̃G�t�F�N�g�N��
        if (Math.Abs(_speedx) > 0 && _groundCheck.IsGround) {
            // �v���C���[�����]������������
            var moveEffect = Instantiate(_moveEffect.gameObject, _moveEffect.transform.position, transform.rotation);
            moveEffect.GetComponent<VisualEffect>().SendEvent("PlayEffect");
            moveEffect.GetComponent<EffectEndDestroy>().EffctStopTimerStart();
        }

        //Debug.Log("�G�t�F�N�g�̌�" + _MoveEffectList.Count);

        // �W�����v���G�t�F�N�g����
        if (_animCallBack.GetIsJumpEffectPlay) {
         
            if (!_jumpEffectdoOnce) {
                Debug.Log("������");
                //_jumpEffect.SendEvent("StopEffect");
                //_jumpEffect.SendEvent("PlayEffect");

                var jumpEffectL = Instantiate(_jumpEffectLeft.gameObject,  _jumpEffectLeft.transform.position,  _jumpEffectLeft.transform.rotation);
                var jumpEffectR = Instantiate(_jumpEffectRight.gameObject, _jumpEffectRight.transform.position, _jumpEffectRight.transform.rotation);
                jumpEffectL.GetComponent<VisualEffect>().SendEvent("PlayEffect");
                jumpEffectR.GetComponent<VisualEffect>().SendEvent("PlayEffect");
                jumpEffectL.GetComponent<EffectEndDestroy>().EffctStopTimerStart();
                jumpEffectR.GetComponent<EffectEndDestroy>().EffctStopTimerStart();

                _jumpEffectdoOnce = true;
            }
        }
        else {
            //_jumpEffect.SendEvent("StopEffect");
            _jumpEffectdoOnce = false;
        }
    }
}
