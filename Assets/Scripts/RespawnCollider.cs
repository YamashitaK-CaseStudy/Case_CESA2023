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
		var mesh = this.transform.GetComponent<MeshRenderer>();
		Color newColor = new Color(0,0,0,0);
		mesh.material.color = newColor;
	}

	public void SetRespawnPosition(Vector3 pos){
		_respawnPos = pos;
		Debug.Log(_respawnPos);
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.transform.root.tag != "Player") return;
		Debug.Log("リスポーン");
		// 今のリスポーンポイントに飛ばす
		var Comp = other.gameObject.GetComponent<CharacterController>();
		// キャラクターコントローラーを一度切る
		if(Comp != null){
			Comp.enabled = false;
		}
		// 飛ばす
		other.transform.root.gameObject.transform.position = _respawnPos;
		// キャラクターコントローラーを起動する
		if(Comp != null){
			other.gameObject.GetComponent<CharacterController>().enabled = true;
		}

		// ダメージ処理
		other.transform.root.GetComponent<Player>().Damage();
	}
}
