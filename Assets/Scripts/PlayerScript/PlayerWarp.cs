using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
	bool isWarp = false;
	float time;
	GameObject fade;
	GameObject flag;
	// Start is called before the first frame update
	void StartWarp()
	{
		fade = GameObject.Find("Pf_Fade");
		flag = GameObject.Find("flagExit");
	}

	// Update is called once per frame
	void UpdateWarp()
	{
		if(isWarp){
			time += Time.deltaTime;
			if(fade.GetComponent<Fade>().FinishFadeIn()){
				Debug.Log("ワープはじめました");
				Warp();
				isWarp = false;
			}
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.name == "flagEnter"){
			if(flag == null){
				Debug.Log("出口内や内科");
				return;
			}
			fade.GetComponent<Fade>().FadeInStart();
			isWarp = true;
		}
	}

	private void Warp(){
		Debug.Log("Warp");
		Vector3 tmp = flag.transform.position;
		gameObject.transform.position = new Vector3(tmp.x,tmp.y,0.0f);
		Debug.Log(this.transform.position);
	}
}
