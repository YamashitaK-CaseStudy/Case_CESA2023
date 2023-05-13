using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjObserver : MonoBehaviour
{
	RotatableObject _lastRotateRotObj;	// 最後に回転し始めたオブジェクトを格納する
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	public void SetLastRotateRotObj(RotatableObject obj){
		_lastRotateRotObj = obj;
	}
	public RotatableObject GetLastRotateRotObj(){
		return _lastRotateRotObj;
	}
}
