using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarp : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.name == "flagEnter"){
			GameObject flag = GameObject.Find("flagExit");
			if(flag == null){
				Debug.Log("出口内や内科");
				return;
			}

			this.transform.position = flag.gameObject.transform.position;
		}
	}
}
