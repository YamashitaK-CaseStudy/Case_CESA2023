using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerKeyCollider : MonoBehaviour
{
    [SerializeField, Header("鍵の取得上限無制限")] private bool _isGetKeyLimitOver;
    private KeyManager _keyManager = null;

    void Start(){

        _keyManager = GameObject.Find("Pf_KeyManager").GetComponent<KeyManager>();

        if(_keyManager == null) {

            Debug.Log("Pf_KeyManagerが存在しません");
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (!_isGetKeyLimitOver && _keyManager.GetPlayerKeyNum >= 1) {
            return;
        }

        if(other.transform.tag == "Key") {

            PlayerSoundManager.Instance.PlayPlayerSE(PlayerSESoundData.PlayerSE.GetKey);
           
            Debug.Log("鍵の取得");
            var keyObj = other.transform.gameObject;
            _keyManager.PlayerGetKey(keyObj);
        }
    }
}
