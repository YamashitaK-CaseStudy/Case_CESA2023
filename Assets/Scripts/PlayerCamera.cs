using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour{

	// �I�t�Z�b�g
	[SerializeField] public Vector3 cameraOffset = new Vector3(0.0f,0.0f,0.0f);
	[SerializeField]private string targetName = "Player";

	private Transform target;

	// Start is called before the first frame update
	void Start(){
		target = GameObject.Find(targetName).transform;
	}


    // Update is called once per frame
    void LateUpdate(){
        this.transform.position = target.transform.position + cameraOffset;
        this.transform.LookAt(target);
    }

}
