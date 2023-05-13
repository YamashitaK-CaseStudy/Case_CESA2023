using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour {

    [SerializeField, Header("左スティックのデッドゾーン")] private float _deadZone;
    [SerializeField, Header("移動速度")] private float _moveSpeed;
    [SerializeField, Header("重力")] private float _gravity;
    [SerializeField, Header("ジャンプ力")] private float _jumpPower;

    private InputAction _jumpButton;
    private Rigidbody _rigidbody;
    private float _speedx;

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
        if (_xBlockLock || _yBlockLock || _animCallBack.GetIsRotationAnimPlay) {
            return;
        }

        Move();
        Jump();
    }

    // 横移動
    private void Move() {

        var value_x = _playerInput.actions["Move"].ReadValue<Vector2>().x;
        var valueCeil = Mathf.Ceil(value_x);

        if (-_deadZone < value_x && value_x < _deadZone) {

            _speedx = 0.0f;
        }
        else {

            _speedx = valueCeil * _moveSpeed;
            transform.LookAt(new Vector3(transform.position.x + value_x, transform.position.y, 0), Vector3.up);
        }
     
        // 速度適応
        _rigidbody.velocity = new Vector3(_speedx, _rigidbody.velocity.y, 0);

        // 壁刷りを防ぐ
        if (_frontrayCheck.IsFrontHit) {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        }
    }

    private void Jump() {

        Physics.gravity = new Vector3(0, _gravity, 0);

        if (_groundCheck.IsGround) {

            if (_jumpButton.WasPressedThisFrame()) {

                // 頭上にブロックがあればジャンプアニメーションしない
                if (!_upperrayCheck.IsUpperHit) {
                    _animator.SetTrigger("StartJump");

                    if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Normal_Idle")) {

                        //_bigJump = true;
                    }
                    else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Ball")) {

                        _rigidbody.AddForce(_jumpPower * Vector3.up, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
