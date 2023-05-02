using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour
{
	GameObject _otherobjcol;
	private void StartSettingOtherHit(){
		_otherobjcol = this.transform.Find("OtherObjCollison").gameObject;
	}
	// オブジェクトが当たったとき
	private void OnTriggerEnter(Collider other) {
		if(!other.gameObject.CompareTag("RotObjRotate")) return;	// RotateObject
		if(!other.gameObject.transform.root.gameObject.GetComponent<RotatableObject>()._isRotating) return;
		// 中で壊れるオブジェクトか回転するオブジェクト化取るにはどうするべきか...
		StartRotate();
	}

}
