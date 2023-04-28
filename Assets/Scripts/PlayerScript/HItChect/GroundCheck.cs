using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour{

    private bool _isGround = false;

    public bool IsGround {
        get { return _isGround; }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.root.gameObject.layer == 13) {

            _isGround = true;
        }
        else {
            _isGround = false;
        }
    }

    private void OnTriggerStay(Collider other) {
        
        if(other.transform.root.gameObject.layer == 13) {

            _isGround = true;
        }
        else {
            _isGround = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        _isGround = false;
    }
}
