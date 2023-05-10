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
	}

	private void OnTriggerEnter(Collider other) {
		if(other.transform.root.gameObject.name != "Floor") return;
		_parentRotComp._isReservation = true;
	}
}
