using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotHitFloar : MonoBehaviour
{
	GameObject _parentObj;
	RotatableObject _parentRotComp;
	GameObject _hitCheckCollider;
	// Start is called before the first frame update
	void Start()
	{
		_parentObj = this.transform.root.gameObject;
		_parentRotComp = _parentObj.transform.GetComponent<RotatableObject>();
		// コライダーを用意する
		for(int i = 0; i < this.transform.childCount; i++){
			var tmpObj = this.transform.GetChild(i);
			if(tmpObj.name == "HitCheckCollider"){
				_hitCheckCollider = tmpObj.gameObject;
				break;
			}
		}
		_hitCheckCollider.SetActive(false);
		Debug.Log(_parentRotComp);
	}

	public void ChecktoRotate(Vector3 pos, Vector3 axis, int angle){
		_hitCheckCollider.SetActive(true);
		_hitCheckCollider.transform.RotateAround(pos, axis, angle);
	}

	public void InitCheckCollider(){
		_hitCheckCollider.transform.position = this.transform.position;
		_hitCheckCollider.transform.eulerAngles = this.transform.eulerAngles;
		_hitCheckCollider.SetActive(false);
	}

	private void OnTriggerEnter(Collider other) {
		if(other.transform.root.gameObject.name != "Floor") return;
		_parentRotComp.ForcedStopRotate();
	}
}
