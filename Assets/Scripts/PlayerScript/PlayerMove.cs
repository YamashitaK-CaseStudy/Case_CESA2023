using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour {

    [SerializeField, Header("���X�e�B�b�N�̃f�b�h�]�[��")] private float _deadZone;
    [SerializeField, Header("�ړ����x")] private float _moveSpeed;
    [SerializeField, Header("�d��")] private float _gravity;
    [SerializeField, Header("�W�����v��")] private float _jumpPower;
    [SerializeField, Header("�W�����v�J�[�u")] private AnimationCurve _jumpCurve;
    [SerializeField, Header("�W�����v���̈ړ������W��")] private float _jumpNowMoveDecay;

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

        // �X�e�B�b�N���͓���Ă邩��]�A�j���[�V�������Đ�����Ă�Ԃ͓����Ȃ�
        if (_xBlockLock || _yBlockLock || _yBlockUpperLock || _animCallBack.GetIsRotationAnimPlay) {
           // Debug.Log("����");
            return;
        }

        Move();
        Jump();
    }

    // ���ړ�
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

        // ���x�K��
        _rigidbody.velocity = new Vector3(_speedx, _rigidbody.velocity.y, 0);

        // �Ǎ����h��
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

            //Debug.Log("�����Ă�");

            if (_jumpButton.WasPressedThisFrame()) {

                _isJumpflg = true;

               // Debug.Log("�W�����v�{�^�������ꂽ");
                _rigidbody.AddForce(_jumpPower * Vector3.up, ForceMode.Impulse);

                if (!_upperrayCheck.IsUpperHit) {

                    // �҂�����
                    _animator.SetTrigger("StartJump");
                    _jumpCurveSpeed = 0.0f;

                    // JumpSE�Đ�
                    Debug.Log("�W�����vSE");
                    PlayerSoundManager.Instance.PlayPlayerSE(PlayerSESoundData.PlayerSE.Jump);
                }
            }
        }
        else {
            // �W�����v�{�^���������Ď���󒆂ɔ�񂾎�
            if (_isJumpflg) {
                _isJumpNowflg = true;
                _isJumpflg = false;
            }

            // �n�ʂɂ��Ă鎞�͉��G�t�F�N�g��~
           
        }

       // Debug.Log("�W�����v��" + _isJumpNowflg);
    }
}
