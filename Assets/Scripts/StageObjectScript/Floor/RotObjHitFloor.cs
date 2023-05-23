using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour
{
	private GameObject _parentObj;
	private RotatableObject _parentRotObj;
	private RotHitCheckFloor[] _childObjHitCheckFloorComp;
	protected bool _isHitFloor {get;set;}
	private bool _isReflectRot = false; // 反射を行うかどうか
	private Vector3 _refCenterPos;
	private Vector3 _refAxis;
	private int _refAngle;

	private void HitCheckFloorSettingStart()
	{
		_isHitFloor = false;
		int childnum = this.gameObject.transform.GetChild(0).childCount;
		_childObj = new GameObject[childnum];
		_childObjHitCheckFloorComp = new RotHitCheckFloor[childnum];
		Debug.Log(childnum);
		Debug.Log(_childObjHitCheckFloorComp);
		// 子供のオブジェクトを確保
		for(int i = 0; i < childnum; i++){
			// 子供のオブジェクトを確保
			_childObj[i] = this.gameObject.transform.GetChild(0).transform.GetChild(i).gameObject;
			// 孫のオブジェクトからChainColliderを探す
			for(int j = 0; j < _childObj[i].transform.childCount; j++){
				var obj = _childObj[i].transform.GetChild(j).gameObject;
				// レイヤー確認してChainのオブジェクトを探す
				if(obj.layer == LayerMask.NameToLayer("Acala")){
					// 中から必要なコンポーネントを持ってくる
					_childObjHitCheckFloorComp[i] = obj.GetComponent<RotHitCheckFloor>();
				}
			}
		}
		Debug.Log("ここで確認");
		Debug.Log(_childObjHitCheckFloorComp.Length);
	}

	public void ChildCountUpdate(){
		// 一回空っぽにする
		System.Array.Resize(ref _childObj, 0);
		System.Array.Resize(ref _childObjHitCheckFloorComp, 0);
		System.Array.Resize(ref _childObjChainComp, 0);
		// 子供のサイズを設定
		int childnum = this.gameObject.transform.GetChild(0).childCount;
		int childcountNum = 0;

		// 子供のオブジェクトを確保
		for(int i = 0; i < childnum; i++){
			// 孫のオブジェクトを確保
			var tmpObj = this.gameObject.transform.GetChild(0).transform.GetChild(i).gameObject;
			// ダミーオブジェクトの場合はループをいったん飛ばす
			if(tmpObj.tag == "DamiObject") continue;
			// 配列のサイズを変更
			System.Array.Resize(ref _childObj, _childObj.Length + 1);
			System.Array.Resize(ref _childObjHitCheckFloorComp, _childObjHitCheckFloorComp.Length + 1);
			System.Array.Resize(ref _childObjChainComp, _childObjChainComp.Length + 1);

			// 孫オブジェクトを格納
			_childObj[childcountNum] = tmpObj;
			// 孫のオブジェクトの子供からColliderを検索
			for(int j = 0; j < _childObj[childcountNum].transform.childCount; j++){
				var obj = _childObj[childcountNum].transform.GetChild(j).gameObject;
				// レイヤー確認してAcalaのオブジェクトを探す
				if(obj.layer == LayerMask.NameToLayer("Acala")){
					// 中から必要なコンポーネントを持ってくる
					_childObjHitCheckFloorComp[childcountNum] = obj.GetComponent<RotHitCheckFloor>();
				}
				// レイヤー確認してChainのオブジェクトを探す
				if(obj.layer == LayerMask.NameToLayer("Chain")){
					// 中から必要なコンポーネントを持ってくる
					_childObjChainComp[childcountNum] = obj.GetComponent<RotObjCheckHitChain>();
				}
			}
			childcountNum++;
		}

		for(int i = 0; i < childcountNum; i++){
			_childObjHitCheckFloorComp[i].parentUpdate();
			_childObjChainComp[i].parentUpdate();
		}
	}

	private void HitFloorUpdate(){
		if(!_isReflectRot) return;
		if(_isRotating) return;
		// 当たったときのフラグを切っておく
		_isReflectRot = false;
		StartRotateReflect(_refCenterPos,_refAxis,_refAngle);
	}

	public void SetisHitFloor(){
		_isHitFloor = true;
	}

	protected void SetReflect(Vector3 center,Vector3 axis, int angle){
		_isReflectRot = true;
		_isHitFloor = false;
		// 必要なデータを格納する
		_refCenterPos 	= center;	// 中心座標
		_refAxis 		= axis;		// 軸
		_refAngle 		= -angle;		// 角度
	}

	protected void SetChildHitCheckFloorFlg(bool flg){
		if(this.GetComponent<RotObjkinds>()._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) return;
		var size = 1.0f;
		if(!flg) size = 0.5f;
		// フラグとコライダーサイズを変更
		Debug.Log(this.name);
		Debug.Log(_childObjHitCheckFloorComp);
		for(int i = 0; i < _childObjHitCheckFloorComp.Length; i++){
			if(_childObjHitCheckFloorComp[i] == null) continue;
			_childObjHitCheckFloorComp[i]._isCheckHit = flg;
			_childObjHitCheckFloorComp[i].SetCollisonSize(size,_rotAxis,flg);
		}
	}

	protected void SetChildCheckIntoFloor(bool flg){
		if(this.GetComponent<RotObjkinds>()._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) return;
		// フラグとコライダーサイズを変更
		for(int i = 0; i < _childObjHitCheckFloorComp.Length; i++){
			if(_childObjHitCheckFloorComp[i] == null) continue;
			_childObjHitCheckFloorComp[i].SetCheckInto(flg);
		}
	}
}
