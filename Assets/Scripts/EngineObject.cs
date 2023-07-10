using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EngineObject : MonoBehaviour
{
	bool _isActivat = false;
	float _rotSpeed = 2f;
	Vector3 _rotAxis;
	[SerializeField] GameObject StartUpTargetObject;
	void Update()
	{
		if(!_isActivat) return;

		var angle = _rotSpeed * Time.deltaTime;
		// なんか回転する処理
		// 現在フレームの回転を示す回転のクォータニオン作成
		var angleAxis = Quaternion.AngleAxis(angle, -_rotAxis);
		// 円運動の位置計算
		var tr = transform;
		var pos = tr.position;
		// クォータニオンを用いた回転は原点からのオフセットを用いる必要がある
		// _axisCenterWorldPosを任意軸の座標に変更すれば任意軸の回転ができる
		pos -= transform.root.transform.position;
		pos = angleAxis * pos;
		pos += transform.root.transform.position;
		tr.position = pos;
		// 向き更新
		tr.rotation = angleAxis * tr.rotation;

		if(_rotSpeed < 4028){
			_rotSpeed += _rotSpeed;
		}
	}

	public void StartUpEngine(Vector3 axis){
		_isActivat = true;
		_rotAxis = axis;
		Debug.Log(axis);

		// 起動させる
		StartUpTargetObject.GetComponent<RotatableObject>().StartSpin(this.transform.position,-_rotAxis);
	}
}
