using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour {

	[SerializeField] int _rotSpeed = 15;

	/*
	GameObject _object;
	GameObject[] _cloneObj;
	GameObject _toSpinCloneObj;
	 */

	GameObject[] _cloneBaceObjs;	// Clone元のオブジェクトを格納しておく配列
	GameObject[,] _cloneObjs;		// Cloneしたオブジェクトの配列
	GameObject[] _toSpinCloneObjs;  // 回す用のクローン

	int _cloneNum = 0;

	protected void StartSettingSpin() {
		//_object = this.transform.Find("Object").gameObject;
	}

	public void StartSpin() {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// フラグを立てる
		_isSpin = true;

	}

	public void StartSpin(Vector3 rotCenter, Vector3 rotAxis) {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// RotableObjの子オブジェクトであるObject = Clone元をすべて取得
		var childNum = this.transform.childCount;
		_cloneBaceObjs = new GameObject[childNum];
		int itr = 0;
		foreach ( Transform childTransform in this.transform ) {
			_cloneBaceObjs[itr++] = childTransform.gameObject;
		}
		Debug.Log(itr);

		// 回転の中心を設定
		_axisCenterWorldPos = rotCenter;

		// 回転軸を設定
		_rotAxis = rotAxis;

		// フラグを立てる
		_isSpin = true;


		_cloneNum = _cloneBaceObjs.Length;

		_cloneObjs = new GameObject[_cloneNum,3];

		Debug.Log(_cloneBaceObjs.Length);

		/*
		_cloneObj = new GameObject[3];
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
		 */

		// オブジェクトを複製する
		// 3Dの位置関係的に90°・180°・270°の3つの複製が必要
		for ( int i = 0 ; i < _cloneNum ; i++ ) {

			for ( int j = 0 ; j < 3 ; j++ ) {
				_cloneObjs[i,j] = Instantiate(_cloneBaceObjs[i]) as GameObject;
				_cloneObjs[i,j].transform.parent			= _cloneBaceObjs[i].transform.parent;
				_cloneObjs[i,j].transform.localPosition	= _cloneBaceObjs[i].transform.localPosition;
				_cloneObjs[i,j].transform.localScale		= _cloneBaceObjs[i].transform.localScale;
				_cloneObjs[i,j].transform.rotation			= _cloneBaceObjs[i].transform.rotation;

				// 回す
				// 回転移動用のクォータニオン
				var rotQuat = Quaternion.AngleAxis(90 * (j + 1), _rotAxis);

				// 円運動の位置計算
				var tr = _cloneObjs[i,j].transform;
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

		_toSpinCloneObjs = new GameObject[_cloneNum];

		for ( int i = 0 ; i < _cloneNum ; i++ ) {
			// 回転する用のコピーを生成
			_toSpinCloneObjs[i] = Instantiate(_cloneBaceObjs[i]) as GameObject;
			_toSpinCloneObjs[i].transform.parent =			_cloneBaceObjs[i].transform.parent;
			_toSpinCloneObjs[i].transform.localPosition =	_cloneBaceObjs[i].transform.localPosition;
			_toSpinCloneObjs[i].transform.localScale =		_cloneBaceObjs[i].transform.localScale;
			_toSpinCloneObjs[i].transform.rotation =		_cloneBaceObjs[i].transform.rotation;

			// コリジョンを無効にする
			var cloneParentTransform = _toSpinCloneObjs[i].transform;

			// 子オブジェクト単位の処理
			// Cloneの子オブジェクトはPf_Partsが複数存在
			// Pf_PartについているBoxColliderとPf_Partの子オブジェクトの内のChainColliderを無効化する必要がある
			foreach ( Transform childTransform in cloneParentTransform ) {

				// ボックスコライダーを無効化する
				var childBoxCollider = childTransform.gameObject.GetComponent<BoxCollider>();
				childBoxCollider.enabled = false;

				// チェインコライダーのボックスコライダーを無効化する
				var childChineCollider = childTransform.Find("ChainCollider");
				var childChineColliderBoxCollider = childChineCollider.gameObject.GetComponent<BoxCollider>();
				childChineColliderBoxCollider.enabled = false;

			}
		}

		/*
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
		}
		 */
	}

	protected void UpdateSpin() {
		if ( _isSpin ) {
			// 現在フレームの回転を示す回転のクォータニオン作成
			var rotQuat = Quaternion.AngleAxis(_rotSpeed, _rotAxis);

			Debug.Log(_cloneNum);
			for ( int i = 0 ; i < _cloneNum ; i++ ) {
				// 円運動の位置計算
				var tr = _toSpinCloneObjs[i].transform;
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
}
