using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour {

    [SerializeField] private float _deadZone;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private AnimationCurve _accelerationMoveCurve;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _normalIdle_To_JumpWaitTime;

    private Rigidbody _rigidbody;

    private float _accelTime = 0.0f;
    private float _speedx = 0.0f;
    private float _speedy = 0.0f;
    private bool _isBall_To_Jump = false;
    private bool _isnormalIdle_To_Jump = false;
    private TimeMeasurement.Alarm _normalIdle_To_JumpAlarm;

    public float Speed_x {
        get { return _speedx; }
    }

    public float Speed_y {
        get { return _speedy; }
    }


    void StartMove() {

        TryGetComponent(out _rigidbody);

        // �ʏ�A�C�h����Ԃ���̃W�����v�̑҂����Ԍv���̒ǉ�
        _normalIdle_To_JumpAlarm = _timeMeasurement.AddArarm("NormalIdle_To_Jump", _normalIdle_To_JumpWaitTime);
    }

    // Update is called once per frame
    void UpdateMove() {

        Move();
        Jump();
    }

    // ���ړ�
    private void Move() {

        var value_x = Input.GetAxis("Horizontal");

        if (-_deadZone > value_x) {

            _accelTime += Time.deltaTime;
            _speedx = -_moveSpeed * _accelerationMoveCurve.Evaluate(_accelTime);
            transform.LookAt(transform.position + new Vector3(-1, 0, 0));

        }
        else if (value_x > _deadZone) {

            _accelTime += Time.deltaTime;
            _speedx = _moveSpeed * _accelerationMoveCurve.Evaluate(_accelTime);
            transform.LookAt(transform.position + new Vector3(1, 0, 0));
        }
        else {


            // ����
            _speedx = 0.0f;
            
            /*
            _accelTime = 0.0f;

            if (_speedx != 0.0) {

                if (_speedx > 0) {
                    _speedx -= 0.05f;

                    if (_speedx < 0) {
                        _speedx = 0.0f;
                    }
                }
                else if (_speedx < 0) {
                    _speedx += 0.05f;

                    if (_speedx > 0) {
                        _speedx = 0.0f;
                    }
                }
            }
             */
        }

        _rigidbody.velocity = new Vector3(_speedx, _rigidbody.velocity.y, 0);

        // �O����Ray���ǂɓ������Ă�����x�x�N�g�����ɂ���
        if (_frontrayCheck.IsFrontHit) {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        }

    }

    private void Jump() {

        Physics.gravity = new Vector3(0, _gravity, 0);

        if (_groundCheck.IsGround) {

            if (Input.GetButtonDown("Jump")) {


                Debug.Log("akaka");


                // ����Ƀu���b�N������΃W�����v���Ȃ�
                if (!_upperrayCheck.IsUpperHit) {

                    // �ʏ�A�C�h����Ԃ���̃W�����v
                    if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Normal_Idle")) {
                        _isnormalIdle_To_Jump = true;
                        _normalIdle_To_JumpAlarm.TimeStart = true;
                    }

                    // �{�[����Ԃ���̃W�����v
                    else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Ball")) {
                       
                        _isBall_To_Jump = true;
                        _rigidbody.AddForce(_jumpSpeed * Vector3.up, ForceMode.Impulse);
                    }
                }
            }
        }

        // �ʏ�A�C�h����Ԃ���̃A�j���[�V�����W�����v�̑҂�����
        if (_normalIdle_To_JumpAlarm.TimeEnd) {
            _normalIdle_To_JumpAlarm.TimeStart = false;
            _normalIdle_To_JumpAlarm.ResetTime();
            _rigidbody.AddForce(_jumpSpeed * Vector3.up, ForceMode.Impulse);
        }
    }
}
