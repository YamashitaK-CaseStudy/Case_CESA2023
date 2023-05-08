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
    private TimeMeasurement.Alarm landingStiffnessTime;
    private bool _isLandingStiffness = false;
    private bool _isJumpButton = false;

    public float Speed_x {
        get { return _speedx; }
    }

    public float Speed_y {
        get { return _speedy; }
    }


    void StartMove() {

        TryGetComponent(out _rigidbody);

        // 通常アイドル状態からのジャンプの待ち時間計測の追加
        _normalIdle_To_JumpAlarm = _timeMeasurement.AddArarm("NormalIdle_To_Jump", _normalIdle_To_JumpWaitTime);

        // 着地時の硬直時間計測
        landingStiffnessTime = _timeMeasurement.AddArarm("LandingStiffness", 0.1f);
    }

    // Update is called once per frame
    void UpdateMove() {

        // x軸アニメーション中は動けないようにする
        if (_animator.GetBool("StartRot_X")) {
            return;
        }

        Move();
        Jump();
    }

    // 横移動
    private void Move() {

        // 回転させている間は動かない
        if ( _isRotating ) {
            //return;
        }
        
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
            // 減速
            // 慣性は無し
            _speedx = 0.0f;
            
        }

        _rigidbody.velocity = new Vector3(_speedx, _rigidbody.velocity.y, 0);

        // 前方のRayが壁に当たっていたらxベクトルを零にする
        if (_frontrayCheck.IsFrontHit) {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        }

    }

    private void Jump() {

        Physics.gravity = new Vector3(0, _gravity, 0);

        if (_groundCheck.IsGround) {

            if (Input.GetButtonDown("Jump")) {

                // 頭上にブロックがあればジャンプしない
                if (!_upperrayCheck.IsUpperHit) {

                    // 通常アイドル状態からのジャンプ
                    if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Normal_Idle")) {
                        _isnormalIdle_To_Jump = true;
                        _normalIdle_To_JumpAlarm.TimeStart = true;
                    }

                    // ボール状態からのジャンプ
                    else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Ball")) {

                        _isJumpButton = true;
                        _isBall_To_Jump = true;
                        _rigidbody.AddForce(_jumpSpeed * Vector3.up, ForceMode.Impulse);
                    }
                }
            }
        }
        

        // 通常アイドル状態からのアニメーションジャンプの待ち時間
        if (_normalIdle_To_JumpAlarm.TimeEnd) {
            _normalIdle_To_JumpAlarm.TimeStart = false;
            _normalIdle_To_JumpAlarm.ResetTime();
            _rigidbody.AddForce(_jumpSpeed * Vector3.up, ForceMode.Impulse);
        }
    }
}
