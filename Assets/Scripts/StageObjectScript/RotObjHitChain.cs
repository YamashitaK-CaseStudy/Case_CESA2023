using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjHitChain : MonoBehaviour
{
	private GameObject _parentObj;
	private RotatableObject _parentRotObj;
	private GameObject[] _broObj {get; set;}	// 兄弟オブジェクト
	private int _childNum = 0;
	[SerializeField] private int _HitChainAngle = 180;
	private void Start(){
		//Debug.Log("初期設定");
		_parentObj = this.transform.root.gameObject;
		_parentRotObj = _parentObj.GetComponent<RotatableObject>();
		_childNum = _parentObj.transform.GetChild(0).childCount;
		_broObj = new GameObject[_childNum];

		for(int i = 0 ; i < _childNum; i++){
			_broObj[i] = _parentObj.transform.GetChild(0).GetChild(i).gameObject;
		}
	}
	// オブジェクトが当たったとき
	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.layer != 3) return;
		// 回転してるオブジェクトのときのみ処理
		if(!_parentRotObj._isRotating) return;
		// RotateObjectのみと当たり判定を取る
		if(other.transform.root.gameObject.CompareTag("RotateObject")) {

			Debug.Log(other.gameObject);
			// コンポーネントを確認
			var comp = other.transform.root.GetComponent<RotatableObject>();
			GameObject chainObj = null;
			chainObj = other.gameObject;
			var compChain = chainObj.transform.GetComponent<RotObjHitChain>();
			if(compChain == null) return;

			// 必要な軸と中心座標を確保
			// 相手の軸を自分の軸の逆方向を渡す
			comp._nowRotAxis = -_parentRotObj._nowRotAxis;
			var centerPos = compChain.MostFarObjPos(comp._nowRotAxis, other.transform.position);
			// 回転処理
			comp.StartRotate(centerPos,comp._nowRotAxis,_HitChainAngle);
		}
	}

	public Vector3 MostFarObjPos(Vector3 axis, Vector3 mypos){
		float fardis = 0;	// 距離測定
		int farnum = 0;		// 一番遠いオブジェクトのID
		for(int i = 0; i < _childNum; i++){
			float dis = 0;
			var broObjPos = _broObj[i].transform.position;
			dis += Mathf.Abs((mypos.x - broObjPos.x) * axis.y);
			dis += Mathf.Abs((mypos.y - broObjPos.y) * axis.x);

			if(fardis < dis){
				fardis = dis;
				farnum = i;
			}
		}
		return _broObj[farnum].transform.position;
	}
}
