using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCollider : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] private GameObject _fadeObj;
	[SerializeField] private Vector3 _respawnPos;
	[SerializeField] private float _waitTime;
	private bool _isHit = false;
	private GameObject _playerObj;
	private MaskFade _maskFadeComp;
	void Start()
	{
		var mesh = this.transform.GetComponent<MeshRenderer>();
		Color newColor = new Color(0,0,0,0);
		mesh.material.color = newColor;

		_maskFadeComp = _fadeObj.GetComponent<MaskFade>();
	}
	void Update(){
		if(!_isHit) return;

		// FadeInが終わってたら移動させる
		if(_maskFadeComp.IsFadeInWait()){
			// 飛ばす
			_playerObj.transform.position = _respawnPos;
			_isHit = false;
			_maskFadeComp.StartFadeIn();
			Debug.Log(_isHit);
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
		if(playerObjComp.GetHP() == 0){
			// リザルトに飛ぶ
			SuzumuraTomoki.SceneManager.LoadResult();
		} else{
			// Fade入れる
			_maskFadeComp.StartFadeOut();
		}
	}
}
