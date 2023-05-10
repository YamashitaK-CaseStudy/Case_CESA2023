using UnityEngine;

public partial class Player : MonoBehaviour {

    [SerializeField, Header("���X�e�B�b�N�̃f�b�h�]�[��")] private float _deadZone;
    [SerializeField, Header("�ړ����x")] private float _moveSpeed;
    [SerializeField, Header("�d��")] private float _gravity;
    [SerializeField, Header("�W�����v��")] private float _jumpPower;
   
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

        // �X�e�B�b�N���͓���Ă邩��]�A�j���[�V�������Đ�����Ă�Ԃ͓����Ȃ�
        if (_xBlockLock || _yBlockLock || _animCallBack.GetIsRotationAnimPlay) {
            return;
        }

        Move();
        Jump();
    }

    // ���ړ�
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
     
        // ���x�K��
        _rigidbody.velocity = new Vector3(_speedx, _rigidbody.velocity.y, 0);
    }

    private void Jump() {

        Physics.gravity = new Vector3(0, _gravity, 0);

        if (_groundCheck.IsGround) {

            if (Input.GetButtonDown("Jump")) {

                _rigidbody.AddForce(_jumpPower * Vector3.up, ForceMode.Impulse);
                _rigidbody.AddForce(_jumpPower * Vector3.up, ForceMode.Impulse);

                // ����Ƀu���b�N������΃W�����v�A�j���[�V�������Ȃ�
                if (!_upperrayCheck.IsUpperHit) {

                    _animator.SetTrigger("StartJump");
                }
            }
        }
    }
}
