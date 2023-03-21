using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour {

	GameObject _mesh;
	GameObject _collider;

	void StartSettingSpin() {
		_mesh = this.transform.Find("Mesh").gameObject;
		_collider = this.transform.Find("Collider").gameObject;
		
	}


	void UpdateSpin() {
	
	}
   
}
