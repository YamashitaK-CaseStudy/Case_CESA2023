using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

	[SerializeField] private float moveSpeed = 3.0f;
	[SerializeField] private float jumpPower = 3.0f;
	private CharacterController _characterController;
	private Transform _transform;
	public Vector3 moveVelocity;

	private Animator _playerAnimator;


	// Start is called before the first frame update
	void StartMove(){
		_characterController = GetComponent<CharacterController>();
		_transform = transform;
	}

	// Update is called once per frame
	void UpdateMove(){

		float dedzone = 0.5f;
		var inputVarX = Input.GetAxis("Horizontal");
		if ( -dedzone < inputVarX && inputVarX < dedzone ) {
			//Debug.Log("Dedzone");
			moveVelocity.x = 0.0f;
		}
		else {
			moveVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;
		}


		_transform.LookAt(_transform.position + new Vector3(moveVelocity.x, 0.0f, moveVelocity.z));

		if ( _characterController.isGrounded ) {
			if ( Input.GetButtonDown("Jump") ) {
				//Debug.Log("ジャンプキーが押された");
				moveVelocity.y = jumpPower;
			}

		}
		else {
			moveVelocity.y += Physics.gravity.y * Time.deltaTime;
		}

		_characterController.Move(moveVelocity * Time.deltaTime);


	}
}
