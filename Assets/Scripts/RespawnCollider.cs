using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCollider : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] private Vector3 _respawnPos;
	void Start()
	{
		var mesh = this.transform.GetComponent<MeshRenderer>();
		Color newColor = new Color(0,0,0,0);
		mesh.material.color = newColor;
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.transform.root.tag != "Player") return;
		Debug.Log("リスポーン");
		// 今のリスポーンポイントに飛ばす
		var Comp = other.gameObject.GetComponent<CharacterController>();
		// 飛ばす
		other.transform.root.gameObject.transform.position = _respawnPos;

		// ダメージ処理
		other.transform.root.GetComponent<Player>().Damage();
	}
}
