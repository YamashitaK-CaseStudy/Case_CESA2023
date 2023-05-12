using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitStopController : MonoBehaviour
{
	// ヒットストップのマネージャー

	// Start is called before the first frame update
	private bool _isHitStop;
	[SerializeField] float RotateObjHitStopFrame = 30f;
	[SerializeField]
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(!_isHitStop){
			if(Input.GetButtonDown("Jump")){
				SetHitDelay();
				DOVirtual.DelayedCall(3f, ()=> ReleseHitDelay());
			}
		}
	}

	void SetHitDelay(){
		Debug.Log("ディレイスタート");
	}

	void ReleseHitDelay(){
		Debug.Log("ディレイ解除");
	}
}
