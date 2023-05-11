using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotHitCheckFloor : MonoBehaviour
{
	private GameObject _parentObj;
	private RotatableObject _parentRotObj;
	public bool _isCheckHit { get; set; } 		// 当たり判定を判定するかどうか
	private void Start()
	{
		// 自分の親のオブジェクトを確保
		_parentObj = this.transform.root.gameObject;
		_parentRotObj = _parentObj.GetComponent<RotatableObject>();
		var _childNum = _parentObj.transform.GetChild(0).childCount;
		// 当たり判定をとるかどうかの判定をとる
		_isCheckHit = false;
	}
	// オブジェクトが当たったとき
	private void OnTriggerEnter(Collider other)
	{
		if(!_isCheckHit) return;
		Debug.Log("床のあたったで");
		// RotateObjectのみと当たり判定を取る
		var comp = other.transform.root.GetComponent<RotatableObject>();
		// 自分の親に知らせる
		_parentRotObj.SetisHitFloor();
	}

}
