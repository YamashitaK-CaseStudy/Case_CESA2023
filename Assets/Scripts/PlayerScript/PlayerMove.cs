using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour {

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpUpPower;
    [SerializeField] private float _jumpDownPower;
    [SerializeField] private float _deadZone;

    [SerializeField] private Vector3 _rayoffset;
    [SerializeField] private float _raydistance;

    private Rigidbody _rigidbody;
    private Vector3 _moveVelocity;
    public bool _startJump = false;

    // getter
    public Vector3 GetMoveVelocity {
        get { return _moveVelocity; }
        set { _moveVelocity = value; }
    }

    void StartMove() {
        Application.targetFrameRate = 120; //60FPS�ɐݒ�

        _rigidbody = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, _jumpDownPower, 0);
    }

    // Update is called once per frame
    void UpdateMove() {

        // ���s�ړ�
        var value_x = Input.GetAxis("Horizontal");

        if (-_deadZone > value_x || value_x > _deadZone) {
            _moveVelocity.x = value_x * _moveSpeed;
        }
        else {
            _moveVelocity.x = 0.0f;
        }

        // �����ύX
        transform.LookAt(transform.position + new Vector3(_moveVelocity.x, 0, 0));
        _rigidbody.velocity = new Vector3(_moveVelocity.x, _rigidbody.velocity.y, 0);

        // �W�����v
        if (IsGround()) {
            if (Input.GetButtonDown("Jump")) {
               // _rigidbody.AddForce(new Vector3(0, _jumpUpPower, 0));
                _rigidbody.velocity += new Vector3(0, _jumpUpPower, 0);

                _skAnimator.SetTrigger("StartJump");
            }
        }
        else {
            Debug.Log("��ׂȂ�");
        }

        IsFrontBlock();
       // Debug.Log(IsUpBlock());
    }

    // �O���Ƀu���b�N�����邩�m�F
    public bool IsFrontBlock() {

        bool isfront = false;

        bool isHit = false;
        RaycastHit hitInfo;

        // Ray�̃X�^�[�g�ʒu��ݒ�
        Vector3 rayPosition = transform.position + new Vector3(0, -0.25f, 0);
        Ray ray = new Ray(rayPosition, new Vector3(_moveVelocity.x, 0, 0).normalized * 0.4f);
        isHit = Physics.Raycast(ray, out hitInfo, 0.8f);
        Debug.DrawRay(rayPosition, new Vector3(_moveVelocity.x, 0, 0).normalized * 0.4f, Color.red);

        if (isHit) {
            if (hitInfo.collider.gameObject.layer != 8) {
                isfront = true;

                var moveVec = new Vector3(_moveVelocity.x, 0, 0);

                var dot = Vector3.Dot(moveVec.normalized, hitInfo.normal.normalized);

                Debug.Log(dot);

                if (dot == -1) {
                    _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
                }

                // Debug.Log(hitInfo.collider.gameObject.name);
            }
        }
        return isfront;
    }

    // �n�ʂ̊m�F
    public bool IsGround() {

        bool isground = false;

        bool isHit = false;
        RaycastHit hitInfo;

        // Ray�̃X�^�[�g�ʒu��ݒ�
        Vector3 rayPosition = transform.position;
        Ray ray = new Ray(rayPosition, Vector3.down);
        isHit = Physics.Raycast(ray, out hitInfo, 1.2f);
        Debug.DrawRay(rayPosition, Vector3.down * 1.2f, Color.red);

        if (isHit) {
            if (hitInfo.collider.gameObject.layer != 8 && hitInfo.collider.transform.root.tag != "Player") {
                isground = true;

              // Debug.Log(hitInfo.collider.gameObject.name);
            }
        }
        return isground;
    }

    // �^��Ƀu���b�N�����邩�m�F
    public bool IsUpBlock() {

        bool isUp = false;

        bool isHit = false;
        RaycastHit hitInfo;

        // Ray�̃X�^�[�g�ʒu��ݒ�
        Vector3 rayPosition = transform.position;
        Ray ray = new Ray(rayPosition, Vector3.up);
        isHit = Physics.Raycast(ray, out hitInfo, 0.5f);
        Debug.DrawRay(rayPosition, Vector3.up * 0.5f, Color.red);

        if (isHit) {
            if (hitInfo.collider.transform.root.tag != "Player") {
                isUp = true;

                // Debug.Log(hitInfo.collider.gameObject.name);
            }
        }
        return isUp;
    }
}
