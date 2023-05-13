using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjObserver : MonoBehaviour
{
	RotatableObject _lastRotateRotObj = null;	// 最後に回転し始めたオブジェクトを格納する

	public void SetLastRotateRotObj(RotatableObject obj){
		_lastRotateRotObj = obj;
	}

	public RotatableObject GetLastRotateRotObj(){
		return _lastRotateRotObj;
	}
}
