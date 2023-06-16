using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour {

    [SerializeField, Header("左スティックのデッドゾーン")] private float _deadZone;
    [SerializeField, Header("移動速度")] private float _moveSpeed;
    [SerializeField, Header("重力")] private float _gravity;
    [SerializeField, Header("ジャンプ力")] private float _jumpPower;
    [SerializeField, Header("ジャンプカーブ")] private AnimationCurve _jumpCurve;
    [SerializeField, Header("ジャンプ中の移動減衰係数")] private float _jumpNowMoveDecay;

    private InputAction _jumpButton;
    private Rigidbody _rigidbody;
    private float _speedx;
    private bool _isJumpflg,_isJumpNowflg;

    private float _jumpCurveSpeed = 0.0f;
    
    public float GetSpeedx {
        get { return _speedx; }
    }

    void StartMove() {

        TryGetComponent(out _rigidbody);

        _jumpButton = _playerInput.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void UpdateMove() {

        // スティック入力入れてるかつ回転アニメーションが再生されてる間は動けない
        if (_xBlockLock || _yBlockLock || _yBlockUpperLock || _animCallBack.GetIsRotationAnimPlay) {
           // Debug.Log("無効");
            return;
        }

        Move();
        Jump();
    }

    // 横移動
    private void Move() {

        var value_x = _playerInput.actions["Move"].ReadValue<Vector2>().x;

        float speed = 0.0f;

        if (-_deadZone > value_x) {

            speed = -_moveSpeed;
            transform.LookAt(new Vector3(transform.position.x + value_x, transform.position.y, 0), Vector3.up);
        }
        else if (value_x > _deadZone) {

            speed = _moveSpeed;
            transform.LookAt(new Vector3(transform.position.x + value_x, transform.position.y, 0), Vector3.up);
        }
        else {
            speed = 0.0f;
        }

        if (!_groundCheck.IsGround && _isJumpNowflg) {
            _speedx = speed * _jumpNowMoveDecay;
        }
        else {
            _speedx = speed;
        }

        // 速度適応
        _rigidbody.velocity = new Vector3(_speedx, _rigidbody.velocity.y, 0);

        // 壁刷りを防ぐ
        if (_frontrayCheck.IsFrontHit) {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        }
    }

    private void Jump() {

        _jumpCurveSpeed += Time.deltaTime;
        _animator.SetFloat("IdleJumpSpeed", _jumpCurve.Evaluate(_jumpCurveSpeed));
       
        Physics.gravity = new Vector3(0, _gravity, 0);

        if (_groundCheck.IsGround) {

            _animator.SetBool("UpperHit", false);
            _isJumpNowflg = false;

            //Debug.Log("浮いてる");

            if (_jumpButton.WasPressedThisFrame()) {

                _isJumpflg = true;

               // Debug.Log("ジャンプボタン押された");
                _rigidbody.AddForce(_jumpPower * Vector3.up, ForceMode.Impulse);

                if (!_upperrayCheck.IsUpperHit) {

                    // 待ち処理
                    _animator.SetTrigger("StartJump");
                    _jumpCurveSpeed = 0.0f;

                    // JumpSE再生
                    Debug.Log("ジャンプSE");
                    PlayerSoundManager.Instance.PlayPlayerSE(PlayerSESoundData.PlayerSE.Jump);
                }
            }
        }
        else {
            // ジャンプボタンを押して自ら空中に飛んだ時
            if (_isJumpflg) {
                _isJumpNowflg = true;
                _isJumpflg = false;
            }

            // 地面についてる時は煙エフェクト停止
           
        }

       // Debug.Log("ジャンプ中" + _isJumpNowflg);
    }
}
