using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjHitCheck : MonoBehaviour{

    // オブジェクトの情報
    private GameObject _rotObj = null;
    private GameObject _rotPartsObj = null;

    // 回転オブジェクトに触れてるかフラグ
    private bool _isRotHit = false;

    // 当たった回転オブジェクトが変更されたときフラグ
    private bool _ischangeRotHit = false;

    private void OnTriggerEnter(Collider other) {

        // 回転オブジェクトの取得
        GetRotateObject(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {

        if(_rotPartsObj == null) {
            return;
        }

        if (other.gameObject != _rotPartsObj.gameObject) {
            return;
        }

        _rotObj = null;
        _rotPartsObj = null;
        _isRotHit = false;
    }

    public void InitChangeRotHit() {
        _ischangeRotHit = false;
    }

    // 回転オブジェクトの取得
    private void GetRotateObject(GameObject obj) {

        if (obj.transform.root.gameObject.tag == "RotateObject") {

            _isRotHit = true;
            _ischangeRotHit = true;

            _rotObj = obj.transform.root.gameObject;
            _rotPartsObj = obj;
        }
        else {

            _isRotHit = false;
        }
    }

    // Getter
    public  GameObject GetRotObj {
        get { return _rotObj; }
    }

    public GameObject GetRotPartsObj {
        get { return _rotPartsObj; }
    }

    public ref bool GetIsRotHit {
        get { return ref _isRotHit; }
    }

    public bool GetIsChangeRotHit {
        get {return _ischangeRotHit; }
    }

}
