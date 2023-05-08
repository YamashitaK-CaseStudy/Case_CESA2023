using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    [SerializeField] private GameObject _skeletalObj;
    [SerializeField] private float _idleCheneTime;   // �ʏ�A�C�h����Ԃɖ߂鎞��

    private Animator _animator;
    private TimeMeasurement.Alarm _idleChangeTimer;
    private TimeMeasurement.Alarm _xAxisRotWaitTimer;

    private void PlayerSkAnimationStart() {

        _animator = _skeletalObj.GetComponent<Animator>();
       
         _idleChangeTimer = _timeMeasurement.AddArarm("IdleChange", _idleCheneTime);
        _idleChangeTimer.TimeStart = true;

        _xAxisRotWaitTimer = _timeMeasurement.AddArarm("xRotWait", 1.5f);
    }

    private void PlayerSkAnimationUpdate() {

        // ���x��0�ȏ�
        var absMove = Mathf.Abs(Speed_x);
        if (absMove > 0) {

            _animator.SetBool("RunState", true);
            _animator.SetFloat("RunSpeed", absMove * 0.3f);
            _idleChangeTimer.ResetTime();
        }
        else {

            _animator.SetFloat("RunSpeed", 0);

            // ����Ƀu���b�N���Ȃ����A�C�h����ԂɕύX���鎞�Ԃ����Ă���ʏ�A�C�h����Ԃɖ߂�
            if (_idleChangeTimer.TimeEnd) {
                if (!_upperrayCheck.IsUpperHit) {
                    _animator.SetBool("RunState", false);
                    _animator.SetBool("UpBlock", false);
                }
                else if (_upperrayCheck.IsUpperHit) {
                    _animator.SetBool("RunState", false);
                    _animator.SetBool("UpBlock", true);
                }
            }
        }

        // �ʏ�A�C�h����Ԃ���W�����v�����Ƃ��̃A�j���[�V�������Đ�
        if (_isnormalIdle_To_Jump || _isBall_To_Jump) {
            _isnormalIdle_To_Jump = false;
            _isBall_To_Jump = false;

            Debug.Log("�W�����v����");
            _animator.SetTrigger("StartJump");
        }

        // y����]���̃A�j���[�V����
        if (_priortyAxis == priorityAxis.yAxisRot) {
            if (_bottomHitCheck.GetIsRotHit) {
                if (_bottomHitCheck.GetRotObj.GetComponent<RotatableObject>()._isRotateStartFream) {
                    _animator.SetBool("RunState", false);
                    _animator.SetTrigger("StartRot_Y");
                }
            }
        }

        // x����]���̃A�j���[�V����
        if (_priortyAxis == priorityAxis.xAxisRot) {
            if (_stricRotAngle.GetIsActicStick) {

                _animator.SetBool("StartRot_X", true);
                _xAxisRotWaitTimer.TimeStart = true;

                if (_xAxisRotWaitTimer.TimeEnd) {
                    _stricRotAngle._isActicDial_X = true;
                }
            }
            else {
                _animator.SetBool("StartRot_X", false);
                _xAxisRotWaitTimer.ResetTime();
                _xAxisRotWaitTimer.TimeStart = false;
                _stricRotAngle._isActicDial_X = false;
            }
        }
    }
}
