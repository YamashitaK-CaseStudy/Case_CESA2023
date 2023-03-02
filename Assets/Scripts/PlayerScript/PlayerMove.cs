using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

	[SerializeField] private float moveSpeed = 3.0f;
	[SerializeField] private float jumpPower = 3.0f;
	private CharacterController _characterController;
	private Transform _transform;
	private Vector3 moveVelocity;

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
	}
}
