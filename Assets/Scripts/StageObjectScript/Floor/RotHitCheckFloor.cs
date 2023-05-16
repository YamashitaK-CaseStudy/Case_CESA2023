using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotHitCheckFloor : MonoBehaviour
{
	private GameObject _parentObj;
	private RotatableObject _parentRotObj;
	private BoxCollider _thisColliderComp;
	public bool _isCheckHit { get; set; }       // 当たり判定を判定するかどうか
	private bool _isCheckInto { get; set; }     // めり込んでいるかどうか
	private void Start()
	{
		// 自分の親のオブジェクトを確保
		_parentObj = this.transform.root.gameObject;
		_parentRotObj = _parentObj.GetComponent<RotatableObject>();
		_thisColliderComp = this.GetComponent<BoxCollider>();
		// 当たり判定をとるかどうかの判定をとる
		_isCheckHit = false;
		_isCheckInto = false;
	}
	public void parentUpdate(){
		_parentObj = this.transform.root.gameObject;
		_parentRotObj = _parentObj.GetComponent<RotatableObject>();
	}
	// オブジェクトが当たったとき
	private void OnTriggerEnter(Collider other)
	{
		if (!_isCheckHit) return;
		// 回転オブジェクトに当たった場合は反射させない
		if (other.transform.root.gameObject.tag == "RotateObject") return;
		// 自分の親に知らせる
		_parentRotObj.SetisHitFloor();
	}

	private void OnTriggerStay(Collider other)
	{
		if (_isCheckHit) return;
		if (!_isCheckInto) return;
		// 回転オブジェクトに当たった場合は反射させない
		if (other.transform.root.gameObject.tag == "RotateObject") return;
		// めり込んだ場合を考慮する
		_isCheckInto = false;
		Debug.Log("Check");
		// 自分の親に知らせる
		_parentRotObj.SetisHitFloor();
	}

	public void SetCollisonSize(float size, Vector3 axiz, bool flg)
	{
		var tmpSize = _thisColliderComp.size;
		if (flg)
		{
			if (Mathf.Abs(axiz.x) == 1.0f)
			{
				tmpSize.y = size;
				tmpSize.z = size;
			}
			else if (Mathf.Abs(axiz.y) == 1.0f)
			{
				tmpSize.x = size;
				tmpSize.z = size;
			}
		}
		else
		{ //　判定を切ってるときはColliderのサイズを普段サイズに
			tmpSize.x = size;
			tmpSize.y = size;
			tmpSize.z = size;
		}
		_thisColliderComp.size = tmpSize;
	}

	public void SetCheckInto(bool flg){
		_isCheckInto = flg;
	}
}
