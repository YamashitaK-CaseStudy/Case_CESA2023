using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
	[SerializeField] private RespawnCollider colliderObj;
	[SerializeField] private Vector3 _respawnPos;
	private void Start(){
		var mesh = this.transform.GetComponent<MeshRenderer>();
		Color newColor = new Color(0,0,0,0);
		mesh.material.color = newColor;
	}
	private void OnTriggerEnter(Collider other) {
		Debug.Log("あたり");
		if(other.gameObject.transform.root.tag != "Player") return;
		Debug.Log("リスポーン設定");
		colliderObj.SetRespawnPosition(_respawnPos);
	}
}
