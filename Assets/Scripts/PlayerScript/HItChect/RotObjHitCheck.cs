using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjHitCheck : MonoBehaviour{

    // �I�u�W�F�N�g�̏��
    private GameObject _rotObj = null;
    private GameObject _rotPartsObj = null;

    private void OnTriggerEnter(Collider other) {

        if (other.transform.parent == null || other.transform.parent.parent == null) {
            return;
        }

        // ��]�I�u�W�F�N�g�̎擾
        GetRotateObject(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {

        if (other.transform.parent == null || other.transform.parent.parent == null) {
            return;
        }

        // ��]�I�u�W�F�N�g�̉��
        ReleseRotateObject(other.gameObject);
    }

    // ��]�I�u�W�F�N�g�̎擾
    private void GetRotateObject(GameObject obj) {

        if (obj.transform.parent.parent.gameObject.tag == "RotateObject") {

            Debug.Log("�����蔻�����" + obj);

            _rotObj = obj.transform.parent.parent.gameObject;
            _rotPartsObj = obj;
        }
    }

    // ��]�I�u�W�F�N�g�̉��
    private void ReleseRotateObject(GameObject obj) {

        if (_rotPartsObj == null) {
            return;
        }

        if (obj != _rotPartsObj.gameObject) {
            return;
        }

        if (obj.transform.parent.parent.gameObject.tag == "RotateObject") {

            Debug.Log("�����蔻�蔲��" + obj);

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
