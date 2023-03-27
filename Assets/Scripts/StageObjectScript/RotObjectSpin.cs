using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour {

	GameObject _mesh;
	GameObject _collider;
	GameObject _object;

	protected void StartSettingSpin() {
		_object = this.transform.Find("Object").gameObject;
		


	}

	public void StartSpin() {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// フラグを立てる
		_isSpin = true;

		// オブジェクトを複製する
		var cloneObj = Instantiate(_object) as GameObject;
		cloneObj.transform.parent = _object.transform.parent;
		cloneObj.transform.localPosition = _object.transform.localPosition;
		cloneObj.transform.localScale = _object.transform.localScale;

		var cloneMesh = cloneObj.transform.Find("Mesh");

		var color = cloneMesh.GetComponent<MeshRenderer>().material.color;

		color.a = 0.5f;

		cloneMesh.GetComponent<MeshRenderer>().material.color = color;

		// 回す
		// 自身の回転軸で180°まわした座標が複製コリジョンの座標

		// 回転移動用のクォータニオン
		var rotQuat = Quaternion.AngleAxis(180,_rotAxis);

		// 円運動の位置計算
		var tr = cloneObj.transform;
		var pos = tr.position;

		// クォータニオンを用いた回転は原点からのオフセットを用いる必要がある
		// _axisCenterWorldPosを任意軸の座標に変更すれば任意軸の回転ができる
		pos -= _axisCenterWorldPos;
		pos = rotQuat * pos;
		pos += _axisCenterWorldPos;

		tr.position = pos;

		// 向き更新
		tr.rotation = tr.rotation * rotQuat;




		/*
		//----------------------------
		// 複製コリジョンの座標を設定
		//----------------------------
		//!{
		// 自身の回転軸で180°まわした座標が複製コリジョンの座標

		// 回転移動用のクォータニオン
		var rotQuat = Quaternion.AngleAxis(180,_rotAxis);

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

		 */
		//!}
	}

	public void StartSpin(Vector3 rotCenter, Vector3 rotAxis) {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// 回転の中心を設定
		_axisCenterWorldPos = rotCenter;

		// 回転軸を設定
		_rotAxis = rotAxis;

		// フラグを立てる
		_isSpin = true;

		// オブジェクトを複製する
		var cloneObj = Instantiate(_object) as GameObject;
		cloneObj.transform.parent = _object.transform.parent;
		cloneObj.transform.localPosition = _object.transform.localPosition;
		cloneObj.transform.localScale = _object.transform.localScale;


		// 回す
		// 自身の回転軸で180°まわした座標が複製コリジョンの座標

		// 回転移動用のクォータニオン
		var rotQuat = Quaternion.AngleAxis(180,_rotAxis);

		// 円運動の位置計算
		var tr = cloneObj.transform;
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

	protected void UpdateSpin() {
		if ( _isSpin ) {
			/*
			// 現在フレームの回転を示す回転のクォータニオン作成
			var rotQuat = Quaternion.AngleAxis(155, _rotAxis);

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
			 */

		}
	}
   
}
