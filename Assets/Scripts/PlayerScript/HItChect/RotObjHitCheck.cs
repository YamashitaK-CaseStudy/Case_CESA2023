using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjHitCheck : MonoBehaviour{

    // オブジェクトの情報
    private GameObject _rotObj = null;
    private GameObject _rotPartsObj = null;

    private void OnTriggerEnter(Collider other) {

        if (other.transform.parent == null || other.transform.parent.parent == null) {
            return;
        }

        // 回転オブジェクトの取得
        GetRotateObject(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {

        if (other.transform.parent == null || other.transform.parent.parent == null) {
            return;
        }

        // 回転オブジェクトの解放
        ReleseRotateObject(other.gameObject);
    }

    // 回転オブジェクトの取得
    private void GetRotateObject(GameObject obj) {

        if (obj.transform.parent.parent.gameObject.tag == "RotateObject") {

            Debug.Log("当たり判定入り" + obj);

            _rotObj = obj.transform.parent.parent.gameObject;
            _rotPartsObj = obj;
        }
    }

    // 回転オブジェクトの解放
    private void ReleseRotateObject(GameObject obj) {

        if (_rotPartsObj == null) {
            return;
        }

        if (obj != _rotPartsObj.gameObject) {
            return;
        }

        if (obj.transform.parent.parent.gameObject.tag == "RotateObject") {

            Debug.Log("当たり判定抜け" + obj);

            if (_rotPartsObj != null) {

                _rotObj = null;
                _rotPartsObj = null;
            }
        }
    }

    // Getter
    public GameObject GetRotObj {
        get { return _rotObj; }
    }

    public GameObject GetRotPartsObj {
        get { return _rotPartsObj; }
    }
}
