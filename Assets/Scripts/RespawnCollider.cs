using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCollider : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] private GameObject _fadeObj;
	[SerializeField] private Vector3 _respawnPos;
	private bool _isHit = false;
	private bool _respawnWait = false;
	private GameObject _playerObj;
	private MaskFade _maskFadeComp;
	private float _requiredTime = 0;
	void Start()
	{
		var mesh = this.transform.GetComponent<MeshRenderer>();
		Color newColor = new Color(0,0,0,0);
		mesh.material.color = newColor;

		_maskFadeComp = _fadeObj.GetComponent<MaskFade>();
	}
	void Update(){
		if(!_isHit && !_respawnWait) return;

		// フェードアウトが終わってたら移動させる
		if(_maskFadeComp.IsFadeInWait() && !_respawnWait){
			// 飛ばす
			var pos = _respawnPos;
			//pos.y += 5;
			//pos.z += 4;
			_playerObj.transform.position = pos;
			_isHit = false;
			_respawnWait = true;
		}

		// 時間の測定
		if(_respawnWait){
			_requiredTime += Time.deltaTime;
		}

		// フェードインを始める
		if(_requiredTime >= _maskFadeComp.GetFadeWaitTime()){
			_maskFadeComp.StartFadeIn();
			//
			if(_playerObj.transform.position.z <= 0){
				var pos = _playerObj.transform.position;
				// pos.z -= 0.05f;
				_playerObj.transform.position = pos;
			}else{
				var pos = _playerObj.transform.position;
				pos.z = 0f;
				_playerObj.transform.position = pos;
				_requiredTime = 0;
				_respawnWait = false;
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.transform.root.tag != "Player") return;
		if(_isHit) return;
		_isHit = true;
		// オブジェクトを格納する
		_playerObj = other.transform.root.gameObject;
		var playerObjComp = _playerObj.GetComponent<Player>();
		// ダメージ処理
		playerObjComp.Damage();

		// ここにプレイヤーの停止処理を入れる


		if(playerObjComp.GetHP() <= 0){
			// ゲームオーバーの演出いれたい
			// リザルトに飛ぶ
			SuzumuraTomoki.SceneManager.LoadResult();
		} else{
			// Fade入れる
			_maskFadeComp.StartFadeOut();
		}
	}
}
