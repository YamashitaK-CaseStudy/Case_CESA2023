using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RotObjCheckHitChain : MonoBehaviour
{
	private GameObject _parentObj;
	private RotatableObject _parentRotObj;
	private BoxCollider _thisColliderComp;
	public bool _isCheckHit { get; set; } 		// 当たり判定を判定するかどうか
	private bool _isCheckInto { get; set; }		// めり込みを確認
	GameObject _observer;
	[SerializeField] private VisualEffect _hitVFX;
	private void Start()
	{
		// 自分の親のオブジェクトを確保
		_parentObj = this.transform.root.gameObject;
		_parentRotObj = _parentObj.GetComponent<RotatableObject>();
		_thisColliderComp = this.GetComponent<BoxCollider>();
		// 当たり判定をとるかどうかの判定をとる
		_isCheckHit = false;
		_isCheckInto = false;

		_observer = GameObject.FindWithTag("Observer");
	}

	public void parentUpdate(){
		_parentObj = this.transform.root.gameObject;
		_parentRotObj = _parentObj.GetComponent<RotatableObject>();
	}

	// オブジェクトが当たったとき
	private void OnTriggerEnter(Collider other)
	{
		if(_observer.GetComponent<HitStopController>()._isHitStop) return;
		if(!_isCheckHit) return;
		if(other.gameObject.layer == LayerMask.NameToLayer("EngineBlade")) return;
		if (other.transform.root.gameObject.GetComponent<RotatableObject>()._isRotating) return;
		// RotateObjectのみと当たり判定を取る
		var comp = other.transform.root.GetComponent<RotatableObject>();
		// 自分の親に知らせる
		_parentRotObj.SetisHitChain(other.gameObject, comp, other.transform.position);
		// エフェクトを起動する
		_hitVFX.SendEvent("OnPlay");
	}
	private void OnTriggerStay(Collider other)
	{
		// 当たり判定を行うかを確認
		if(_isCheckHit) return;
		if(!_isCheckInto) return;
		// 当たってるオブジェクトと自分が回転しておらず触れ続けてる状態の時に処理を行う
		if(_parentRotObj._isRotating) return;
		// 触れている相手がRotObjかどうかを確認
		if(other.transform.root.gameObject.tag != "RotateObject")return;
		var otherRotComp = other.transform.root.gameObject.GetComponent<RotatableObject>();
		if(otherRotComp._isRotating) return;

		// 自分の親に知らせる
		Debug.Log("ChainCheck");
		_parentRotObj.SetisIntoChain(other.gameObject, otherRotComp, other.transform.position);
		_hitVFX.SendEvent("OnPlay");
		_isCheckInto = false;
	}
	public void SetCheckInto(bool flg){
		_isCheckInto = true;
	}
}