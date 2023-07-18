using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskFade : MonoBehaviour
{
	// Start is called before the first frame update
	bool _fadeIn, _fadeOut;
	bool _fadeInWait, _fadeOutWait;
	float _min = 0f,_max = 25f;
	float _elapsedTime = 0.0f;
	[SerializeField] float _fadeOutTime = 5;
	[SerializeField] float _fadeInTime = 3;
	[SerializeField] float _fadeWaitTime = 1;
	private RectTransform _unmaskObjTrans;
	void Start()
	{
		_fadeIn = false;
		_fadeOut = false;
		_fadeInWait = false;
		_fadeOutWait = true;

		for(int i = 0; i < this.transform.childCount; i++){
			var tmpObj = this.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject;
			if(tmpObj.name == "UnMask"){
				_unmaskObjTrans = tmpObj.GetComponent<RectTransform>();
				break;
			}
		}
	}


	// Update is called once per frame
	void Update()
	{
		if(_fadeIn || _fadeOut){
			if(_fadeIn) FadeIn();
			if(_fadeOut) FadeOut();
		}
	}
	void FadeIn(){
		// 時間を計算
		_elapsedTime += Time.deltaTime;
		// 必要時間からこのFで大きくなる量を計算する
		float size = _max / _fadeInTime / (1 / Time.deltaTime);

		Vector3 tmpSize = new Vector3(1,1,1);
		// スケールに加算をする
		_unmaskObjTrans.localScale += tmpSize * size;
		if(_elapsedTime >= _fadeInTime){
			// 最後の補正をかける
			_unmaskObjTrans.localScale = _max * new Vector3(1,1,1);
			// fadeIn待ち状態にする
			_fadeOutWait = true;
			_fadeIn = false;
			_elapsedTime = 0f;
		}
	}

	void FadeOut(){
		// 時間を計算
		_elapsedTime += Time.deltaTime;
		// 必要時間からこのFで大きくなる量を計算する
		float size = _max / _fadeOutTime / (1 / Time.deltaTime);

		Vector3 tmpSize = new Vector3(1,1,1);
		// スケールに加算をする
		_unmaskObjTrans.localScale -= tmpSize * size;
		if(_elapsedTime >= _fadeOutTime){
			// 最後の補正をかける
			_unmaskObjTrans.localScale = _min * new Vector3(1,1,1);
			// fadeIn待ち状態にする
			_fadeInWait = true;
			_fadeOut = false;
			_elapsedTime = 0f;
		}
	}

	public void StartFadeIn(){
		if(!_fadeInWait) return;
		_fadeIn = true;
		_fadeInWait = false;
	}
	public void StartFadeOut(){
		if(!_fadeOutWait) return;
		_fadeOut = true;
		_fadeOutWait = false;
	}
	// 待ち状態を確認する
	public bool IsFadeInWait(){
		return _fadeInWait;
	}

	public bool IsFadeOutWait(){
		return _fadeOutWait;
	}

	public float GetFadeWaitTime(){
		return _fadeWaitTime;
	}
}
