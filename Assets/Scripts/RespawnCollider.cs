using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCollider : MonoBehaviour
{
	// Start is called before the first frame update
	private Vector3 _respawnPos;
	void Start()
	{
		_respawnPos = new Vector3(0,0,0);
	}

	public void SetRespawnPosition(Vector3 pos){
		_respawnPos = pos;
		Debug.Log(_respawnPos);
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag != "Player") return;
		// 今のリスポーンポイントに飛ばす
		var Comp = other.gameObject.GetComponent<CharacterController>();
		// キャラクターコントローラーを一度切る
		Comp.enabled = false;
		other.transform.root.gameObject.transform.position = _respawnPos;
		// キャラクターコントローラーを起動する
		other.gameObject.GetComponent<CharacterController>().enabled = true;
		// ダメージ処理
		other.transform.GetComponent<Player>().Damage();
	}
}
