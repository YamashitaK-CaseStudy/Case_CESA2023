using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour {

	GameObject _mesh;
	GameObject _collider;

	private float _spinSpeed = 2; // 単位回転に必要な時間[sec]

	protected void StartSettingSpin() {
		_mesh = this.transform.Find("Mesh").gameObject;
		_collider = this.transform.Find("Collider").gameObject;
		
	}

	public void StartSpin() {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// フラグを立てる
		_isSpin = true;

		//----------------------------
		// 複製コリジョンの座標を設定
		//----------------------------
		//!{
		// 自身の回転軸で180°まわした座標が複製コリジョンの座標

		// 回転移動用のクォータニオン
		var rotQuat = Quaternion.AngleAxis(180,_selfRotAxis);

		// 円運動の位置計算
		var pretence = _collider.transform.Find("PretenceCollison").gameObject;
		var tr = pretence.transform;
		var pos = tr.position;

		// クォータニオンを用いた回転は原点からのオフセットを用いる必要がある
		// _axisCenterWorldPosを任意軸の座標に変更すれば任意軸の回転ができる
		pos -= _axisCenterWorldPos;
		pos = rotQuat * pos;
		pos += _axisCenterWorldPos;

		tr.position = pos;

		// 向き更新
		tr.rotation = tr.rotation * rotQuat;

		//!}
	}

	protected void UpdateSpin() {
		if ( _isSpin ) {

			// 現在フレームの回転を示す回転のクォータニオン作成
			var rotQuat = Quaternion.AngleAxis(155, _selfRotAxis);

			// 円運動の位置計算
			var tr = _mesh.transform;
			var pos = tr.position;

			// クォータニオンを用いた回転は原点からのオフセットを用いる必要がある
			// _axisCenterWorldPosを任意軸の座標に変更すれば任意軸の回転ができる
			pos -= _axisCenterWorldPos;
			pos = rotQuat * pos;
			pos += _axisCenterWorldPos;

			tr.position = pos;

			// 向き更新
			tr.rotation = tr.rotation * rotQuat;

		}
	}
   
}
