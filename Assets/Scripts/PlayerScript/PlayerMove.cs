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
		Debug.Log(_characterController.isGrounded ? "地上にいる" : "空中にいる");

		moveVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;
		_transform.LookAt(_transform.position + new Vector3(moveVelocity.x, 0.0f, moveVelocity.z));

		if ( _characterController.isGrounded ) {
			if ( Input.GetButtonDown("Jump") ) {
				Debug.Log("ジャンプ");
				moveVelocity.y = jumpPower;
			}

		}
		else {
			moveVelocity.y += Physics.gravity.y * Time.deltaTime;
		}

		_characterController.Move(moveVelocity * Time.deltaTime);


		// アクションの入力確認

		// 処理説明
		// 右スティックの入力値を設定されたデッドゾーンと比較して
		// 縦方向と横方向それぞれに評価値を -方向:-1 入力無し:0 +方向:1 を割り当てます
		// その後,その評価値をもとに方向を判別します

		float inputRHorizon = Input.GetAxis("HorizontalRight");
		float inputRVertical = Input.GetAxis("VerticalRight");

		INPUT_DIRECTION inputDir = INPUT_DIRECTION.NONE;

		int evaluationHorizon = 0;		//左右入力の評価値
		int evaluationVertical = 0;     //上下入力の評価値

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


		// 判別します
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

		// 入力値に応じて関数を呼ぶ
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
