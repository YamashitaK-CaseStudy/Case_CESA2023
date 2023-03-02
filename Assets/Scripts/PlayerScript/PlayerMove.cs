using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

	[SerializeField] private float moveSpeed = 3.0f;
	[SerializeField] private float jumpPower = 3.0f;
	private CharacterController _characterController;
	private Transform _transform;
	private Vector3 moveVelocity;

	[SerializeField] private float _deadRJ = 0.3f;

	// Start is called before the first frame update
	void StartMove(){
		_characterController = GetComponent<CharacterController>();
		_transform = transform;
	}

	// Update is called once per frame
	void UpdateMove(){
		//Debug.Log(_characterController.isGrounded ? "�n��ɂ���" : "�󒆂ɂ���");

		moveVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;
		_transform.LookAt(_transform.position + new Vector3(moveVelocity.x, 0.0f, moveVelocity.z));

		if ( _characterController.isGrounded ) {
			if ( Input.GetButtonDown("Jump") ) {
				Debug.Log("�W�����v");
				moveVelocity.y = jumpPower;
			}

		}
		else {
			moveVelocity.y += Physics.gravity.y * Time.deltaTime;
		}

		_characterController.Move(moveVelocity * Time.deltaTime);


		// �A�N�V�����̓��͊m�F

		// ��������
		// �E�X�e�B�b�N�̓��͒l��ݒ肳�ꂽ�f�b�h�]�[���Ɣ�r����
		// �c�����Ɖ��������ꂼ��ɕ]���l�� -����:-1 ���͖���:0 +����:1 �����蓖�Ă܂�
		// ���̌�,���̕]���l�����Ƃɕ����𔻕ʂ��܂�

		float inputRHorizon = Input.GetAxis("HorizontalRight");
		float inputRVertical = Input.GetAxis("VerticalRight");

		INPUT_DIRECTION inputDir = INPUT_DIRECTION.NONE;

		int evaluationHorizon = 0;		//���E���͂̕]���l
		int evaluationVertical = 0;     //�㉺���͂̕]���l

		if ( inputRHorizon > _deadRJ ) {
			evaluationHorizon = 1;
		}
		else if ( inputRHorizon < -_deadRJ ) {
			evaluationHorizon = -1;
		}
		else {
			evaluationHorizon = 0;
		}

		if ( inputRVertical > _deadRJ ) {
			evaluationVertical = 1;
		}
		else if ( inputRVertical < -_deadRJ ) {
			evaluationVertical = -1;
		}
		else {
			evaluationVertical = 0;
		}


		// ���ʂ��܂�
		if ( evaluationHorizon == -1 && evaluationVertical == 0 ) {
			inputDir = INPUT_DIRECTION.LEFT;
		}
		if ( evaluationHorizon == -1 && evaluationVertical == 1 ) {
			inputDir = INPUT_DIRECTION.L_UP;
		}
		if ( evaluationHorizon == -1 && evaluationVertical == -1 ) {
			inputDir = INPUT_DIRECTION.L_DOWN;
		}
		if ( evaluationHorizon == 0 && evaluationVertical == 0 ) {
			inputDir = INPUT_DIRECTION.NONE;
		}
		if ( evaluationHorizon == 0 && evaluationVertical == 1 ) {
			inputDir = INPUT_DIRECTION.UP;
		}
		if ( evaluationHorizon == 0 && evaluationVertical == -1 ) {
			inputDir = INPUT_DIRECTION.DOWN;
		}
		if ( evaluationHorizon == 1 && evaluationVertical == 0 ) {
			inputDir = INPUT_DIRECTION.RIGHT;
		}
		if ( evaluationHorizon == 1 && evaluationVertical == 1 ) {
			inputDir = INPUT_DIRECTION.R_UP;
		}
		if ( evaluationHorizon == 1 && evaluationVertical == -1 ) {
			inputDir = INPUT_DIRECTION.R_DOWN;
		}

		// ���͒l�ɉ����Ċ֐����Ă�
		if ( inputDir == INPUT_DIRECTION.NONE ) {
			return;
		}

		if ( _characterController.isGrounded ) {
			if ( inputDir == INPUT_DIRECTION.LEFT || inputDir == INPUT_DIRECTION.RIGHT ) {
				StartRollingJumpGround(inputDir);
			}
		}
		else {
			StartRollingJumpSky(inputDir);
		}
		

	}
}
