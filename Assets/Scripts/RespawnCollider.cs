using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RespawnCollider : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] private GameObject _fadeObj;
	[SerializeField] private Vector3 _respawnPos;
	[SerializeField] private float _waitTimeBeforeFade;
	[SerializeField] private Cinemachine.CinemachineVirtualCamera _respawnVCam;
	private Cinemachine.CinemachineVirtualCamera _playerVCam;
	private bool _isHit = false;
	private bool _respawnWait = false;
	private GameObject _playerObj;
	private Player _playerObjComp;
	private MaskFade _maskFadeComp;
	private Vector3 _playerDeadpoint;
	private float _requiredTime = 0;
	private bool _waitFade = false;
	private Vector3 _VCamAngle;
	void Start()
	{
		this.GetComponent<MeshRenderer>().enabled = false;

		_maskFadeComp = _fadeObj.GetComponent<MaskFade>();

		_playerObj = GameObject.FindWithTag("Player");
		_playerObjComp = _playerObj.GetComponent<Player>();

		_playerVCam = GameObject.Find("VCam_PlayerHoming").GetComponent<Cinemachine.CinemachineVirtualCamera>();
		_VCamAngle = _playerVCam.transform.eulerAngles;
	}
	void Update(){
		// 当たった後のみ処理を行う
		if(!_isHit) return;

		// Fade入る前の演出と後の演出を管理
		if(_waitFade){
			UpdateFadeBefore();	// フェード入る前の処理
		}else{
			UpdateFade();		// フェード入った後の処理
		}
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.transform.root.tag != "Player") return;
		if(_isHit) return;
		_isHit = true;
		_waitFade = true;
		_requiredTime = 0;

		// プレイヤーが死んだ場所を設定
		_playerDeadpoint = _playerObj.transform.position;
		// ダメージ処理
		_playerObjComp.Damage();
		// ここにプレイヤーの停止処理を入れる
		SuzumuraTomoki.SceneManager.playerInput.Disable();

		// ここで爆発アニメーション入れて
		_playerObjComp.GetAnimator.SetTrigger("GameOver");

		_playerVCam.Follow = null;
		_playerVCam.LookAt = _playerObj.transform;
	}

	void UpdateFade(){
		// フェードアウトが終わってたら移動させる
		if(!_respawnWait){
			// 飛ばす
			var pos = _respawnPos;
			// 飛ばす座標を目標地点からちょっと奥にするように
			pos.y += 5;
			pos.z += 4;
			_playerObj.transform.position = pos;
			_playerVCam.LookAt = null;
			_playerVCam.transform.eulerAngles = _VCamAngle;
			_playerVCam.Follow = _playerObj.transform;

			// ここでボールに戻ってほしい（完全に暗転してる最中の処理）


			// フラグを管理
			_respawnWait = true;
			// VCam
			_respawnVCam.Priority = 11;
		}

		// 時間の測定
		if(_respawnWait){
			_requiredTime += Time.deltaTime;
			Debug.Log(_requiredTime);
		}

		// フェードインを始める
		if(_requiredTime >= _maskFadeComp.GetFadeWaitTime()){
			_maskFadeComp.StartFadeIn();

			if(_playerObj.transform.position.z > 0){
				var pos = _playerObj.transform.position;
				pos.z -= 3f * Time.deltaTime;
				_playerObj.transform.position = pos;


				// ここでボールころころ処理入れてほしい


			}else{
				// ここで通常アイドル状態に戻す(落とされるタイミング)


				// プレイヤーの入力を始める
				SuzumuraTomoki.SceneManager.playerInput.Enable();
				// Vカメラ
				_respawnVCam.Priority = 9;

				// プレイヤーの座標を補正する
				var pos = _playerObj.transform.position;
				pos.z = 0f;
				_playerObj.transform.position = pos;

				//　使用した変数を初期化していく
				_requiredTime = 0;
				_respawnWait = false;
				_isHit = false;
			}
		}
	}

	void UpdateFadeBefore(){
		bool isFinishEffect = false;

		// アニメーションとか設定
		// 経過時間を計算
		_requiredTime += Time.deltaTime;

		// 設定された時間を超過したら
		if(_requiredTime >= _waitTimeBeforeFade) isFinishEffect = true;

		// フェードアウト中はプレイヤーを動かさない
		if(_maskFadeComp.IsFadeInWait()){
			_requiredTime = 0;
			_waitFade = false;
		}else{
			_playerObj.transform.position = _playerDeadpoint;
		}

		if(!isFinishEffect) return;
		if(_playerObjComp.GetHP() <= 0){
			// ゲームオーバーの演出いれたい
			// リザルトに飛ぶ
			SuzumuraTomoki.SceneManager.LoadResult();
		} else{
			// Fade入れる
			_maskFadeComp.StartFadeOut();
		}
	}
}
