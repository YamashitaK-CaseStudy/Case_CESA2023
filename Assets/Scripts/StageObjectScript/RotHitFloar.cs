using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotHitFloar : MonoBehaviour
{
	GameObject _parentObj;
	RotatableObject _parentRotComp;
	// Start is called before the first frame update
	void Start()
	{
		_parentObj = this.transform.root.gameObject;
		_parentRotComp = _parentObj.transform.GetComponent<RotatableObject>();
		Debug.Log(_parentRotComp);
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.transform.root.name != "Floor") return;
		Debug.Log("これ床と当たってるやんけ");

		Vector3 pos = _parentRotComp._axisCenterWorldPos;
		Vector3 axis = _parentRotComp._nowRotAxis;
		int angle = 0;
		if(_parentRotComp.oldangleX != 0){
			angle = _parentRotComp.oldangleX;
		}else if(_parentRotComp.oldangleY != 0){
			angle = _parentRotComp.oldangleY;
		}
		Debug.Log(pos);
		Debug.Log(axis);
		Debug.Log(angle);
		_parentRotComp.SetReservation(pos,-axis,-angle);
	}
}
