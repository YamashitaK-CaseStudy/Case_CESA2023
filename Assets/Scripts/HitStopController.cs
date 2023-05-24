using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitStopController : MonoBehaviour
{
	// ヒットストップのマネージャー

	// Start is called before the first frame update
	[HideInInspector] public bool _isHitStop { get; set;}
	[SerializeField] float DelayTime = 1f;

	void Start(){
		Init();
	}

	void Init(){
		_isHitStop = false;
	}

	public void SetHitDelay(){
		DOVirtual.DelayedCall(DelayTime, ()=> ReleseHitDelay());
		_isHitStop = true;
	}

	void ReleseHitDelay(){
		_isHitStop = false;
	}
}
