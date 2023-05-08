using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour
{

	[SerializeField] int _rotSpeed = 15;

	private GameObject[] _cloneBaceObjs;        // Clone元のオブジェクトの配列
	private GameObject[,] _cloneObjs;           // Cloneしたオブジェクトの配列
	private GameObject[] _toSpinCloneObjs;  // 回す用のクローンの配列

	private List<GameObject> _originObjList;    // クローン元のオブジェクトリスト

	private int _originNum = 0;
	private int _listLength = 0;

	public void StartFunc(){
		_originObjList = new List<GameObject>();
	}

	public void StartSpin()
	{
		if (_isSpining || _isRotating)
		{
			return;
		}

		// フラグを立てる
		_isSpining = true;

	}

	public void StartSpin(Vector3 rotCenter, Vector3 rotAxis)
	{
		if (_isSpining || _isRotating)
		{
			return;
		}

		AddCloneOriginToList();

		// 回転の中心を設定
		_axisCenterWorldPos = rotCenter;

		// 回転軸を設定
		_rotAxis = rotAxis;

		// フラグを立てる
		_isSpining = true;

		// クローン用のGameObjectを確保
		// クローン1つに対して,90°・180°・270°用の3つ必要
		_cloneObjs = new GameObject[_originNum, 3];

		//Debug.Log(_cloneBaceObjs.Length);

		CreateClone();

		CreateCloneToSpin();
	}



	// 高速回転を終了
	public void EndSpin()
	{
		if (_isSpining == false || _isRotating == true)
		{
			return;
		}

		_isSpining = false;


		foreach (GameObject cloneObj in _cloneObjs)
		{
			Destroy(cloneObj);
		}


		foreach (GameObject toSpinCloneObj in _toSpinCloneObjs)
		{
			Destroy(toSpinCloneObj);
		}
	}

	// 高速回転の更新処理
	protected void UpdateSpin()
	{
		if (_isSpining)
		{

			// 現在フレームの回転を示す回転のクォータニオン作成
			var rotQuat = Quaternion.AngleAxis(_rotSpeed, _rotAxis);

			Debug.Log(_originNum);
			for (int i = 0; i < _originNum; i++)
			{
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
  
	// Clone元をすべて取得して配列に格納する処理
	// すでに追加済みの場合はスキップ
	// MEMO：磁石ギミックの兼ね合いで増(減)に対応
	protected void AddCloneOriginToList(){

		_originNum = this.transform.childCount; // 実際のクローン元の数
		_listLength = _originObjList.Count;  // 現時点のクローン元を格納している配列の長さを取得しとく

		Debug.Log(_listLength);

		// クローンの数に変更がなければ処理をスキップ
		if (_listLength == this.transform.childCount)
		{
			return;
		}
		// 始めて呼ばれた時
		else if (_listLength == 0)
		{
			foreach (Transform childTransform in this.transform)
			{
				_originObjList.Add(childTransform.gameObject);
			}
		}

		// クローンの数に変更があった場合
		// 数の減少については想定上起こりえないため考慮しない
		else if (_listLength > 0)
		{
			// 追加する数を計算
			var addObjNum = this.transform.childCount - _listLength;

			for (int itr = _originNum - addObjNum; itr < _originNum; itr++)
			{
				_originObjList.Add(this.transform.GetChild(itr).gameObject);
			}
		}
		else
		{
			Debug.Log("未定義の条件分岐：クローン元の数が前回の高速回転から減少しています");
		}

		return;

	}

	// クローン生成処理
	protected void CreateClone()
	{
		// クローン用のGameObjectを確保
		// クローン1つに対して,90°・180°・270°用の3つ必要
		_cloneObjs = new GameObject[_originNum, 3];

		// オブジェクトを複製する
		// 3Dの位置関係的に90°・180°・270°の3つの複製が必要
		for (int i = 0; i < _originNum; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				

				_cloneObjs[i, j] = Instantiate(_originObjList[i]) as GameObject;
				_cloneObjs[i, j].transform.parent = _originObjList[i].transform.parent;
				_cloneObjs[i, j].transform.localPosition = _originObjList[i].transform.localPosition;
				_cloneObjs[i, j].transform.localScale = _originObjList[i].transform.localScale;
				_cloneObjs[i, j].transform.rotation = _originObjList[i].transform.rotation;

				// 回す
				// 回転移動用のクォータニオン
				var rotQuat = Quaternion.AngleAxis(90 * (j + 1), _rotAxis);


				// 円運動の位置計算
				var tr = _cloneObjs[i, j].transform;
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

	// 回転用のクローンを生成
	private void CreateCloneToSpin()
	{
		// まわす用のクローンを追加で生成
		_toSpinCloneObjs = new GameObject[_originNum];

		for (int i = 0; i < _originNum; i++)
		{
			// 回転する用のコピーを生成
			_toSpinCloneObjs[i] = Instantiate(_originObjList[i]) as GameObject;
			_toSpinCloneObjs[i].transform.parent = _originObjList[i].transform.parent;
			_toSpinCloneObjs[i].transform.localPosition = _originObjList[i].transform.localPosition;
			_toSpinCloneObjs[i].transform.localScale = _originObjList[i].transform.localScale;
			_toSpinCloneObjs[i].transform.rotation = _originObjList[i].transform.rotation;


			// コリジョンを無効にする処理
			// Cloneの子オブジェクトはPf_Partsが複数存在
			// Pf_PartについているBoxColliderとPf_Partの子オブジェクトの内のChainColliderを無効化する必要がある
			foreach (Transform childTransform in _toSpinCloneObjs[i].transform)
			{

				// ボックスコライダーを無効化する
				var childBoxCollider = childTransform.gameObject.GetComponent<BoxCollider>();
				childBoxCollider.enabled = false;

				// チェインコライダーのボックスコライダーを無効化する
				var childChineCollider = childTransform.Find("ChainCollider");
				var childChineColliderBoxCollider = childChineCollider.gameObject.GetComponent<BoxCollider>();
				childChineColliderBoxCollider.enabled = false;

			}
		}
	}
}
