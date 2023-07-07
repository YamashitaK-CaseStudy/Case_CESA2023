using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjUnionObtherber : MonoBehaviour{

	// 回転オブジェクトの合体素材
	private List<GameObject> _unionMaterials = new List<GameObject>();
	private List<GameObject> _unionChildObj = new List<GameObject>();
	public bool _isUseUnion;
	GameObject _parentObj = null;
	Player _playerObj = null;
	HitStopController _ctrl;
	[SerializeField] private RotObjObserver _rotObserver = null;

	void Start(){
		_playerObj = GameObject.FindWithTag("Player").GetComponent<Player>();
	}

	void FixedUpdate() {
		// if(_ctrl == null){
		//     _ctrl = this.gameObject.GetComponent<HitStopController>();
		// }else{
		//     if(_ctrl._isHitStop) return;
		// }
		// if(!_isUseUnion) return;

		// if (_unionMaterials.Count < 2) {
		//     return;
		// }

		// // 合体オブジェクトの対象では無いオブジェクトを取り除く
		// for (int i = _unionMaterials.Count - 1; i >= 0; i--) {
		//     if (_unionMaterials[i].GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) {
		//         _unionMaterials.RemoveAt(i);
		//     }
		// }

		// if (_unionMaterials.Count < 2) {
		//     return;
		// }

		// // 回転オブジェクトが止まってるか確認
		// foreach (var rotobj in _unionMaterials) {

		//     if (rotobj.GetComponent<RotatableObject>()._isRotating) {
		//                 Debug.Log("回転してるんや...");
		//         return;
		//     }
		// }

		// // 合体元の回転オブジェクトの子供(Objects)を取得する
		// var basechildObjects = _unionMaterials[0].transform.GetChild(0).gameObject;

		// // 回転オブジェクトにぶら下がってるObjectオブジェクトを取得する
		// List<GameObject> objects = new List<GameObject>();
		// for (int i = 1; i < _unionMaterials.Count; i++) {

		//     for(int j = 0; j < _unionMaterials[i].transform.childCount; j++) {

		//         for (int k = 0; k < _unionMaterials[i].transform.GetChild(j).childCount; k++) {
		//             objects.Add(_unionMaterials[i].transform.GetChild(j).GetChild(k).gameObject);
		//         }
		//     }

		//     // 空になった回転オブジェクトを削除する
		//     Destroy(_unionMaterials[i].gameObject);
		// }

		// // 親を変更
		// foreach(var obj in objects) {
		//    obj.transform.parent = basechildObjects.transform;
		// }

		// // 合体した瞬間を
		// Debug.Log("合体した");
		// var rotObj = basechildObjects.transform.root.GetComponent<RotatableObject>();
		// rotObj.ChildCountUpdate();  // 子供の更新
		// rotObj.FinishUnion();       // 合体が終了したのを通知する
		// _rotObserver.SetLastRotateRotObj(rotObj);   // オブザーバーに最後に回転したオブジェクトとして登録

		// _unionMaterials.Clear();
	}

	// 合体素材の追加
	public void AddunionMaterial(GameObject material) {
		if(!_unionMaterials.Contains(material)){
			_unionMaterials.Add(material);
			for(int i = 0; i < _unionMaterials.Count; i++){
				Debug.Log("あたったやつ追加住んで");
				Debug.Log(_unionMaterials[i].name);
			}
			//ObjectUnion();
			Debug.Log("あとしょり");
			Debug.Log(_unionMaterials.Count);
		}
	}

	// 合体素材の追加
	public void AddUnionChildObj(GameObject material){
		if(!_unionChildObj.Contains(material)){
			_unionChildObj.Add(material);
		}
	}

	// 合体素材の親を設定する
	public void SetUnionParentObj(GameObject obj){
		if(obj.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
		_parentObj = obj;
	}

	// 合体処理
	// 回転が終了したタイミングで呼び出す
	public void Union(){
		// 合体させるべきオブジェクトがない場合処理を行わない
		if(_unionChildObj.Count <= 0 && _parentObj == null) return;

		// 念のために種別を確認し対象以外が検知された場合そのオブジェクトを取り除く
		for (int i = _unionChildObj.Count - 1; i >= 0; i--) {
			if (_unionChildObj[i].GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) {
				_unionChildObj.RemoveAt(i);
			}
		}

		// 合体させるべきオブジェクトがない場合処理を行わない
		if(_unionChildObj.Count <= 0 && _parentObj == null) return;

		// 回転オブジェクトにぶら下がってるObjectオブジェクトを取得する
		List<GameObject> objects = new List<GameObject>();
		for (int i = 0; i < _unionChildObj.Count; i++) {
			if(_parentObj.gameObject == _unionChildObj[i]) continue;
			for(int j = 0; j < _unionChildObj[i].transform.childCount; j++) {
				for(int k = 0; k < _unionChildObj[i].transform.GetChild(j).transform.childCount; k++){
					objects.Add(_unionChildObj[i].transform.GetChild(j).transform.GetChild(k).gameObject);
				}
			}
			// 空になった回転オブジェクトを削除する
			Destroy(_unionChildObj[i].gameObject);
		}

		// 親を変更
		foreach(var obj in objects) {
			obj.transform.parent = _parentObj.transform.GetChild(0).transform;
		}

		// 合体した瞬間を
		Debug.Log("合体した");
		var rotObj = _parentObj.transform.root.GetComponent<RotatableObject>();
		rotObj.ChildCountUpdate();	// 子供の更新
		rotObj.FinishUnion();		// 合体が終了したのを通知する
		rotObj.StartUpUnionFX();
		var playerAxisParts = _playerObj.GetAxisParts();
		foreach(var obj in objects){
			if(obj == playerAxisParts){
				_playerObj.BlockLoad(obj);
				break;
			}
		}
		DeleteData();
		_rotObserver.SetLastRotateRotObj(rotObj);   // オブザーバーに最後に回転したオブジェクトとして登録
		return;
	}


    public void DeleteData(){
        // 処理の最後に子供にするイブジェクトのデータを削除
		_unionChildObj.Clear();
		_parentObj = null;
    }
}