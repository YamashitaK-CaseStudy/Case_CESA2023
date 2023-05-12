using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour
{
	GameObject[] _childObj;
	GameObject _chainObj;	// 回転させるオブジェクト
	RotatableObject _chainObjComp;	// 回転させるオブジェクト
	RotObjCheckHitChain[] _childObjChainComp;
	float otherObjLength = 0;
	private bool _isHitChain {get;set;}
	Vector3 _rotPosOther;
	[SerializeField] private int _HitChainAngle = 180;
	void ChainSettingStart(){
		int childnum = this.gameObject.transform.GetChild(0).childCount;
		_childObj = new GameObject[childnum];
		_childObjChainComp = new RotObjCheckHitChain[childnum];
		// 子供のオブジェクトを確保
		for(int i = 0; i < childnum; i++){
			// 子供のオブジェクトを確保
			_childObj[i] = this.gameObject.transform.GetChild(0).transform.GetChild(i).gameObject;
			// 孫のオブジェクトからChainColliderを探す
			for(int j = 0; j < _childObj[i].transform.childCount; j++){
				var obj = _childObj[i].transform.GetChild(j).gameObject;
				// レイヤー確認してChainのオブジェクトを探す
				if(obj.layer == LayerMask.NameToLayer("Chain")){
					// 中から必要なコンポーネントを持ってくる
					_childObjChainComp[i] = obj.GetComponent<RotObjCheckHitChain>();
				}
			}
		}

		_isHitChain = false;
	}

	void HitChainUpdate(){
		if(!_isHitChain) return;

		_isHitChain = false;
		otherObjLength = 0;

		// 相手のオブジェクト基準で位置を計測
		var centPos = _chainObjComp.MostFarObjPos(_nowRotAxis, _rotPosOther);
		// 相手のオブジェクトを回転させる
		_chainObjComp.StartRotateChain(centPos, _chainObjComp._nowRotAxis, _HitChainAngle);
 	}

	public void SetisHitChain(GameObject other, RotatableObject otherRotComp, Vector3 hitPos){
		_isHitChain = true;
		// 当たり判定のセット
		_chainObj = other;
		_chainObjComp = otherRotComp;
		_chainObjComp._nowRotAxis = -_nowRotAxis;

		// 自分の回転座標から一番近い場所かどうかを確認
		if(otherObjLength == 0){
			// 距離を取っておく
			otherObjLength += Mathf.Abs((_axisCenterWorldPos.x - hitPos.x) * _rotAxis.y);
			otherObjLength += Mathf.Abs((_axisCenterWorldPos.y - hitPos.y) * _rotAxis.x);
			// 相手を回転させる座標を確保
			_rotPosOther = hitPos;
		}else{
			float distance = 0;
			distance += Mathf.Abs((_axisCenterWorldPos.x - hitPos.x) * _rotAxis.y);
			distance += Mathf.Abs((_axisCenterWorldPos.y - hitPos.y) * _rotAxis.x);
			if(otherObjLength > distance){
				otherObjLength = distance;
				// 相手を回転させる座標を確保
				_rotPosOther = hitPos;
			}
		}
	}

	// 子供の位置を
	public Vector3 MostFarObjPos(Vector3 axis, Vector3 mypos)
	{
		float fardis = 0;	// 距離測定
		int farnum = 0;		// 一番遠いオブジェクトのID
		for (int i = 0; i < _childObj.Length; i++)
		{
			float dis = 0;
			var broObjPos = _childObj[i].transform.position;
			dis += Mathf.Abs((mypos.x - broObjPos.x) * axis.y);
			dis += Mathf.Abs((mypos.y - broObjPos.y) * axis.x);

			if (fardis < dis)
			{
				fardis = dis;
				farnum = i;
			}
		}
		return _childObj[farnum].transform.position;
	}

	private void SetChildHitCheckChainFlg(bool flg){
		for(int i = 0; i < _childObjChainComp.Length; i++){
			_childObjChainComp[i]._isCheckHit = flg;
		}
	}
}
