using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCheck : MonoBehaviour{

    // �������Ă��邩�̃`�F�b�N
    private bool _isHit = false;

    public bool GetIsHit {
        get { return _isHit; }
    }

    private void OnTriggerEnter(Collider other) {
        _isHit = true;
    }

    private void OnTriggerExit(Collider other) {
        _isHit = false;
    }
}
