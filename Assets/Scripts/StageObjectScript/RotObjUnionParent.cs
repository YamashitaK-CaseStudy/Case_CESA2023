using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour{
	private RotObjUnion[] _unionChildComp;
	void UnionSettingStart()
	{
		// Unionオブジェクト以外は処理を行わない
		if(this.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
		var tmpObject = this.transform.GetChild(0).gameObject;
		// 自分のパーツ分用意する
		_unionChildComp = new RotObjUnion[tmpObject.transform.childCount];
		for(int i = 0; i < tmpObject.transform.childCount; i++){
			// 子供のオブジェクトを確保
			var childObj = tmpObject.transform.GetChild(i);
			// 孫のオブジェクトから必要なコンポーネントのみを所得する
			for(int j = 0; j < childObj.transform.childCount; j++){
				var childCompObj = childObj.transform.GetChild(j).gameObject;
				// UnionColliderのみを所得する
				if(childCompObj.layer == LayerMask.NameToLayer("Union")){
					_unionChildComp[i] = childCompObj.GetComponent<RotObjUnion>();
					continue;	// 必要なオブジェクトを取れば孫を変更する
				}
			}
		}
	}

	public void PreparationUinon(){
		// 合体処理が入った時点で他の当たり判定を切る
		SetUnionChildCollider(false);
	}
	// 合体が終了したことを知らせる
	public void FinishUnion(){
		SetUnionChildCollider(false);
	}

	void SetUnionChildCollider(bool flg){
		// Unionオブジェクト以外は処理を行わない
		if(this.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
		for(int i = 0; i < _unionChildComp.Length;i++){
			if(_unionChildComp[i] == null) continue;
			_unionChildComp[i].SetUnionCollider(flg);
		}
	}
}
