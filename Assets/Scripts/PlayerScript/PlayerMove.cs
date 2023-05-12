using UnityEngine;

public partial class Player : MonoBehaviour {

    [SerializeField, Header("左スティックのデッドゾーン")] private float _deadZone;
    [SerializeField, Header("移動速度")] private float _moveSpeed;
    [SerializeField, Header("重力")] private float _gravity;
    [SerializeField, Header("ジャンプ力")] private float _jumpPower;
   
    private Rigidbody _rigidbody;
    private float _speedx;

 
    public float GetSpeedx {
        get { return _speedx; }
    }

    void StartMove() {

        TryGetComponent(out _rigidbody);
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

       
        var value_x = Input.GetAxis("Horizontal");
        var valueCeil = Mathf.Ceil(value_x);

        if (-_deadZone < value_x && value_x < _deadZone) {

            _speedx = 0.0f;
        }
        else {

            _speedx = valueCeil * _moveSpeed;
            transform.LookAt(new Vector3(transform.position.x + valueCeil, transform.position.y, 0), Vector3.up);
        }
     
        // 速度適応
        _rigidbody.velocity = new Vector3(_speedx, _rigidbody.velocity.y, 0);
    }

    private void Jump() {

        Physics.gravity = new Vector3(0, _gravity, 0);

        if (_groundCheck.IsGround) {

            if (Input.GetButtonDown("Jump")) {

                _rigidbody.AddForce(_jumpPower * Vector3.up, ForceMode.Impulse);
                _rigidbody.AddForce(_jumpPower * Vector3.up, ForceMode.Impulse);

                // 頭上にブロックがあればジャンプアニメーションしない
                if (!_upperrayCheck.IsUpperHit) {

                    _animator.SetTrigger("StartJump");
                }
            }
        }
    }
}
