using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回転できるオブジェクトのスクリプト
public partial class RotatableObject : MonoBehaviour{

	[SerializeField] public float _rotRequirdTime = 0.075f;      // 1回転に必要な時間(sec)


	protected Vector3 _rotAxis;				// 自身の回転軸ベクトル
	public Vector3 _axisCenterWorldPos;     // 回転軸の中心のワールド座標
	protected float _elapsedTime = 0.0f;        // 回転開始からの経過時間
	protected int _angle;						// 回転角度
	public bool _isRotating = false;		// 回転してるかフラグ
	public bool _isSpining = false;			// 回転しているかフラグ
	public bool _isRotateStartFream = false;	// 回転し始めた1フレームを教えるフラグ
	public bool _isRotateEndFream = false;      // 回転が終了した1フレームを教えるフラグ
	protected bool _doOnce = false;
	protected GameObject _observer;

	//プロパティ。外部からのメンバ変数へのアクセスを定義するもの。ゲッターやセッターのようなもの。
	public float ProgressRate//進捗率という意味です。_elapsedTimeが単純に経過した時間ではなく全体時間で割った０～１の進捗率として扱われているためこの名前にしました。
	{
		get
		{
			return _elapsedTime;
		}
	}

	// Start is called before the first frame update
	void Start(){
		_observer = GameObject.FindWithTag("Observer");
		StartFunc();
		//StartSettingOtherHit();
		// 自身の回転軸の向きを正規化しとく
		_rotAxis.Normalize();
		ChainSettingStart();
		HitCheckFloorSettingStart();
		UnionSettingStart();
	}

	// デバッグ状況によってはUpdateに戻す

	// Update is called once per frame
	void FixedUpdate(){
		// ディレイ処理が入れば処理を考慮する
		if(_observer.GetComponent<HitStopController>()._isHitStop) return;

		HitChainUpdate();
		UpdateRotate();
		UpdateSpin();
		HitFloorUpdate();
	}


}
