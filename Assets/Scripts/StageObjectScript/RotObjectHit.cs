using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour
{
	private GameObject _parentObj;
	private Vector3[] _broObj {get; set;}	// 兄弟オブジェクト
	private int _childNum = 0;
	[SerializeField] private int _HitChainAngle = 180;
	private void StartSettingOtherHit(){
		Debug.Log("初期設定");
		_parentObj = this.transform.root.gameObject;
		_childNum = _parentObj.transform.GetChild(0).childCount;
		_broObj = new Vector3[_childNum];
		UpdateChilePos();
	}
	// オブジェクトが当たったとき
	private void OnTriggerEnter(Collider other) {
		// 回転してるオブジェクトのときは処理しない
		if(!_isRotating) return;
		// RotateObjectのみと当たり判定を取る
		if(other.transform.root.gameObject.CompareTag("RotateObject")) {
			Debug.Log(other.transform.root.gameObject);
			// コンポーネントを確認
			var comp = other.transform.root.GetComponent<RotatableObject>();
			// 必要な軸と中心座標を確保
			// 相手の軸を自分の軸の逆方向を渡す
			comp._nowRotAxis = -_nowRotAxis;
			var centerPos = comp.MostFarObjPos(comp._nowRotAxis, other.transform.position);
			// 回転する
			Debug.Log(centerPos);
			comp.StartRotate(centerPos,comp._nowRotAxis,_HitChainAngle);
		}
	}

	public Vector3 MostFarObjPos(Vector3 axis, Vector3 mypos){
		Vector3 result =new Vector3(0,0,0);
		float fardis = 0;	// 距離測定
		int farnum = 0;		// 一番遠いオブジェクトのID
		Debug.Log("当たった場所");
		Debug.Log(mypos);
		for(int i = 0; i < _childNum; i++){
			float dis = 0;
			dis += Mathf.Abs((mypos.x - _broObj[i].x) * axis.y);
			dis += Mathf.Abs((mypos.y - _broObj[i].y) * axis.x);

			if(fardis < dis){
				fardis = dis;
				farnum = i;
			}
		}
		result = _broObj[farnum];
		Debug.Log("一番遠い");
		Debug.Log(result);
		return result;
	}

	private void UpdateChilePos(){
		// 自分が持っている子供を全部検索保持
		for(int i = 0; i < _childNum; i++){
			_broObj[i] = _parentObj.transform.GetChild(0).GetChild(i).transform.position;
		}
	}
}
