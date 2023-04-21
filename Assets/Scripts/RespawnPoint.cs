using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private RespawnCollider colliderObj;
    [SerializeField] private Vector3 _respawnPos;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag != "Player") return;
        colliderObj.SetRespawnPosition(_respawnPos);
    }
}
