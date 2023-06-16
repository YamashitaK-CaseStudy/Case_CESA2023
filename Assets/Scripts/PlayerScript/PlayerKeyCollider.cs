using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyCollider : MonoBehaviour
{
    [SerializeField, Header("���̎擾���������")] private bool _isGetKeyLimitOver;
    private KeyManager _keyManager = null;

    void Start(){

        _keyManager = GameObject.Find("Pf_KeyManager").GetComponent<KeyManager>();

        if(_keyManager == null) {

            Debug.Log("Pf_KeyManager�����݂��܂���");
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (!_isGetKeyLimitOver && _keyManager.GetPlayerKeyNum >= 1) {
            return;
        }

        if(other.transform.root.tag == "Key") {

            Debug.Log("���̎擾");
            var keyObj = other.transform.root.gameObject;
            _keyManager.PlayerGetKey(keyObj);
        }
    }
}
