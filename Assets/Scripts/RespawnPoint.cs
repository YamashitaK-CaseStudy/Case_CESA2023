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
		if(other.gameObject.tag != "Player") return;
		colliderObj.SetRespawnPosition(_respawnPos);
	}
}
