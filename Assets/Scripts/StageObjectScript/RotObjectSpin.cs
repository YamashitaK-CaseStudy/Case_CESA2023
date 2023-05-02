using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour {

	[SerializeField] int _rotSpeed = 15;

	GameObject _object;
	GameObject[] _cloneObj;
	GameObject _toSpinCloneObj;

	protected void StartSettingSpin() {
		_object = this.transform.Find("Object").gameObject;
	}

	public void StartSpin() {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// フラグを立てる
		_isSpin = true;

		/*
		// オブジェクトを複製する
		var cloneObj = Instantiate(_object) as GameObject;
		cloneObj.transform.parent = _object.transform.parent;
		cloneObj.transform.localPosition = _object.transform.localPosition;
		cloneObj.transform.localScale = _object.transform.localScale;
		cloneObj.transform.rotation = _object.transform.rotation;

		 */
		// 回す
		// 自身の回転軸で180°まわした座標が複製コリジョンの座標

		/*
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
		//tr.rotation =  rotQuat * tr.rotation;

		 */



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

		_cloneObj = new GameObject[3];

		// オブジェクトを複製する
		// 3Dの位置関係的に90°・180°・270°の3つの複製が必要
		for (int i = 0; i < 3; i++){
			_cloneObj[i] = Instantiate(_object) as GameObject;
			_cloneObj[i].transform.parent = _object.transform.parent;
			_cloneObj[i].transform.localPosition = _object.transform.localPosition;
			_cloneObj[i].transform.localScale = _object.transform.localScale;
			_cloneObj[i].transform.rotation = _object.transform.rotation;

			// 回す
			// 回転移動用のクォータニオン
			var rotQuat = Quaternion.AngleAxis(90 * (i + 1), _rotAxis);

			// 円運動の位置計算
			var tr = _cloneObj[i].transform;
			var pos = tr.position;

			// クォータニオンを用いた回転は原点からのオフセットを用いる必要がある
			// _axisCenterWorldPosを任意軸の座標に変更すれば任意軸の回転ができる
			pos -= _axisCenterWorldPos;
			pos = rotQuat * pos;
			pos += _axisCenterWorldPos;

			tr.position = pos;

			// 向き更新
			tr.rotation = rotQuat * tr.rotation;
		}

		// 回転する用のコピーを生成
		_toSpinCloneObj = Instantiate(_object) as GameObject;
		_toSpinCloneObj.transform.parent = _object.transform.parent;
		_toSpinCloneObj.transform.localPosition = _object.transform.localPosition;
		_toSpinCloneObj.transform.localScale = _object.transform.localScale;
		_toSpinCloneObj.transform.rotation = _object.transform.rotation;

		// コリジョンを無効にする
		var parentTransform = _toSpinCloneObj.transform;

		// 子オブジェクト単位の処理
		// Cloneの子オブジェクトはPf_Partsが複数存在
		// Pf_PartについているBoxColliderとPf_Partの子オブジェクトの内のChainColliderを無効化する必要がある
		foreach (Transform childTransform in parentTransform){

			// ボックスコライダーを無効化する
			var childBoxCollider = childTransform.gameObject.GetComponent<BoxCollider>();
			childBoxCollider.enabled = false;

			// チェインコライダーのボックスコライダーを無効化する
			var childChineCollider = childTransform.Find("ChainCollider");
			var childChineColliderBoxCollider = childChineCollider.gameObject.GetComponent<BoxCollider>();
			childChineColliderBoxCollider.enabled = false;

			// レンダラーを無効にする
			var childMesh = childTransform.Find("P_RotateObject");
			var childMeshRenderer = childMesh.GetComponent<MeshRenderer>();
			childMeshRenderer.enabled = false;

		}



	}



	protected void UpdateSpin() {
		if ( _isSpin ) {
			
			// 現在フレームの回転を示す回転のクォータニオン作成
			var rotQuat = Quaternion.AngleAxis(_rotSpeed, _rotAxis);

			// 円運動の位置計算
			var tr = _toSpinCloneObj.transform;
			var pos = tr.position;

			// クォータニオンを用いた回転は原点からのオフセットを用いる必要がある
			// _axisCenterWorldPosを任意軸の座標に変更すれば任意軸の回転ができる
			pos -= _axisCenterWorldPos;
			pos = rotQuat * pos;
			pos += _axisCenterWorldPos;

			tr.position = pos;

			// 向き更新
			tr.rotation = rotQuat * tr.rotation;


		}
	}
   
}
