using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : RotatableObject
{
	[SerializeField] float speed = 1.0f;
	[SerializeField] Vector3 _boltAxis;
	
	private void Start(){
		// 自身の回転軸の向きを正規化しとく
		_boltAxis.Normalize();

		// まわす大の設定
		StartSettingSpin();
	}
	private void Update() {
		UpdateGimick();
		UpdateRotate();
		UpdateSpin();
	}
	private void UpdateGimick(){
		bool isforward = true;
		if(Input.GetKey("n")){
			isforward = true;
		}

		if(Input.GetKey("m")){
			isforward = false;
		}

		if(_isRotating){
			StartRotate();
			Vector3 axis = _boltAxis;
			BoltGimick(axis,isforward);
		}
	}
	public void BoltGimick(Vector3 axis, bool isforward){
		// 方向指定
		int dir = 1;
		if(!isforward) dir = -1;
		// 座標更新
		var _position = transform.position;
		_position.x += axis.x * speed * dir * Time.deltaTime;
		_position.y += axis.y * speed * dir * Time.deltaTime;

		transform.position = _position;
	}
}
