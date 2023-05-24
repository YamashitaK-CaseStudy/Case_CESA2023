using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour{

    private GameObject _hitObj = null;
    private bool _isGround = false;

    public bool IsGround {
        get { return _isGround; }
    }

    private void OnTriggerEnter(Collider other) {

        _hitObj = other.gameObject;
        //if (other.transform.root.gameObject.layer == 13) {

        //    _isGround = true;
        //}
        //else {
        //    _isGround = false;
        //}
    }

    private void OnTriggerStay(Collider other) {

        //if(other.transform.root.gameObject.layer == 13) {

        //    _isGround = true;
        //}
        //else {
        //    _isGround = false;
        //}

        _hitObj = other.gameObject;
    }

    private void OnTriggerExit(Collider other) {

        _hitObj = null;
        //_isGround = false;
    }

    private void Update() {

        if(_hitObj != null && _hitObj.transform.root.gameObject.layer == 13) {

            Debug.Log("â∫ÇÃÉuÉçÉbÉN" + _hitObj);
            _isGround = true;
        }
        else {
            _isGround = false;
        }
    }
}
