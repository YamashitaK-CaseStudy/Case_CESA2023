using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : RotatableObject
{
	[SerializeField] private float _translationSpeed = 1.0f;
	[SerializeField] private float _speedMagnificationSpinning = 2.0f;
	private float _translationSpeedSpinning;

	private void Start(){
		// まわす大の設定
		StartSettingSpin();
		_translationSpeedSpinning = _translationSpeed * _speedMagnificationSpinning;
	}
	private void Update() {
        if (_isRotating)
        {
			UpdateRotate();
			UpdatePosition(_translationSpeed);
        }
		else if (_isSpin)
		{
			UpdateSpin();
			UpdatePosition(_translationSpeedSpinning);
		}
	}
	
	public void UpdatePosition(float speed){
		// 座標更新
		var position = transform.position;
		position.x += -_rotAxis.x * speed * Time.deltaTime;
		position.y += -_rotAxis.y * speed * Time.deltaTime;

		transform.position = position;
	}
}
