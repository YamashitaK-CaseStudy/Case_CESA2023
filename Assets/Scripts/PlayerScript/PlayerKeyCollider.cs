using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyCollider : MonoBehaviour
{
    [SerializeField, Header("Œ®‚Ìæ“¾ãŒÀ–³§ŒÀ")] private bool _isGetKeyLimitOver;
    private KeyManager _keyManager = null;

    void Start(){

        _keyManager = GameObject.Find("Pf_KeyManager").GetComponent<KeyManager>();

        if(_keyManager == null) {

            Debug.Log("Pf_KeyManager‚ª‘¶İ‚µ‚Ü‚¹‚ñ");
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (!_isGetKeyLimitOver && _keyManager.GetPlayerKeyNum >= 1) {
            return;
        }

        if(other.transform.root.tag == "Key") {

            Debug.Log("Œ®‚Ìæ“¾");
            var keyObj = other.transform.root.gameObject;
            _keyManager.PlayerGetKey(keyObj);
        }
    }
}
