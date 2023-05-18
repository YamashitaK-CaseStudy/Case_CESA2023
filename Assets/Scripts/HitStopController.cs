using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitStopController : MonoBehaviour
{
	// ヒットストップのマネージャー

	// Start is called before the first frame update
	[HideInInspector] public bool _isHitStop { get; set;}
	private int  elapsedFrames = 0;
	private bool _isCanMove;
	[SerializeField] float RotateObjHitStopFrame = 30f;
	[SerializeField] float DelayTime = 1f;
	void Start()
	{
		_isHitStop = false;
		_isCanMove = true;
		elapsedFrames = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if(_isHitStop){
			// 動けるフレームになれば処理を0に
			if(elapsedFrames == RotateObjHitStopFrame)	{
				//_isHitStop = false;
				elapsedFrames = 0;
				_isCanMove = true;
			}else {
				_isCanMove = false;
			}
			elapsedFrames++;
			Debug.Log(elapsedFrames);
		}
	}

	public void SetHitDelay(){
		Debug.Log("ディレイスタート");
		DOVirtual.DelayedCall(DelayTime, ()=> ReleseHitDelay());
		_isHitStop = true;
	}

	void ReleseHitDelay(){
		Debug.Log("ディレイ解除");
		_isHitStop = false;
		_isCanMove = true;
	}

	public bool isCanMove(){
		return _isHitStop;
	}

	public bool isHitStop(){
		return _isHitStop;
	}
}
