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
        if (other.transform.parent == null)//対処療法的でよくない
        {
            return;//床などは無視
        }
        if (other.transform.parent.parent == null)//対処療法的でよくない
        {
            return;//床などは無視
        }
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

        if (obj.transform.parent.parent.gameObject.tag == "RotateObject") {

            _isRotHit = true;
            _ischangeRotHit = true;

            _rotObj = obj.transform.parent.parent.gameObject;
            _rotPartsObj = obj;
        }
        else {

            //_isRotHit = false;
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
