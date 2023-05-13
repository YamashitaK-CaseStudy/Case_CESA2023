using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour
{
	private GameObject _parentObj;
	private RotatableObject _parentRotObj;
	private RotHitCheckFloor[] _childObjHitCheckFloorComp;
	private bool _isHitFloor {get;set;}
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

	private void SetReflect(Vector3 center,Vector3 axis, int angle){
		_isReflectRot = true;
		_isHitFloor = false;
		// 必要なデータを格納する
		_refCenterPos 	= center;	// 中心座標
		_refAxis 		= axis;		// 軸
		_refAngle 		= -angle;		// 角度
	}

	private void SetChildHitCheckFloorFlg(bool flg){
		var size = 1.0f;
		if(!flg) size = 0.5f;
		// フラグとコライダーサイズを変更
		for(int i = 0; i < _childObjHitCheckFloorComp.Length; i++){
			_childObjHitCheckFloorComp[i]._isCheckHit = flg;
			_childObjHitCheckFloorComp[i].SetCollisonSize(size,_rotAxis,flg);
		}
	}

	private void SetChildCheckIntoChain(bool flg){
		// フラグとコライダーサイズを変更
		for(int i = 0; i < _childObjHitCheckFloorComp.Length; i++){
			_childObjChainComp[i].SetCheckInto(flg);
		}
	}

	private void SetChildCheckIntoFloor(bool flg){
		// フラグとコライダーサイズを変更
		for(int i = 0; i < _childObjHitCheckFloorComp.Length; i++){
			_childObjHitCheckFloorComp[i].SetCheckInto(flg);
		}
	}
}
