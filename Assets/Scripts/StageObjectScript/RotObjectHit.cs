using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour
{
	private GameObject _parentObj;
	private Vector3[] _broObj;	// 兄弟オブジェクト
	private void StartSettingOtherHit(){
		Debug.Log("初期設定");
		_parentObj = this.transform.root.gameObject;
		int childnum = _parentObj.transform.GetChild(0).childCount;
		_broObj = new Vector3[childnum];
		for(int i = 0; i < childnum; i++){
			_broObj[i] = _parentObj.transform.GetChild(0).GetChild(i).transform.position;
			Debug.Log(_broObj[i]);
		}
	}
	// オブジェクトが当たったとき
	private void OnTriggerEnter(Collider other) {
		if(other.transform.root.gameObject.CompareTag("RotateObject")) {
			Debug.Log("あたり");
			Debug.Log(other.transform.root.gameObject);
		var comp = other.transform.root.GetComponent<RotatableObject>();
		// var axis = comp._rotAxis;
		var axis = new Vector3(0,1,0);
		var centerPos = MostFarObjPos(other.transform.root.gameObject,axis);
		//中で壊れるオブジェクトか回転するオブジェクト化取るにはどうするべきか...
		StartRotate(centerPos,axis);
		}
	}

	private Vector3 MostFarObjPos(GameObject obj, Vector3 axis){
		Vector3 result =new Vector3(0,0,0);
		int childnum = _parentObj.transform.GetChild(0).childCount;
		float fardis = 0;
		int farnum = 0;
		for(int i = 0; i < childnum; i++){
			float dis = 0;
			dis += Mathf.Abs(_broObj[i].x) * axis.x;
			dis += Mathf.Abs(_broObj[i].y) * axis.y;
			dis += Mathf.Abs(_broObj[i].z) * axis.z;

			if(fardis < dis){
				fardis = dis;
				farnum = i;
			}
		}
		result = _broObj[farnum];
		Debug.Log(farnum);
		return result;
	}

}
