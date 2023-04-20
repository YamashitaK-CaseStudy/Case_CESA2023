using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjUnion : MonoBehaviour{

    static bool _isStartDoOnce = false;
    static RotObjUnionObtherber _obtherber = null;

    private void Start() {

        if (!_isStartDoOnce) {
            _isStartDoOnce = true;
            _obtherber = GameObject.FindGameObjectWithTag("UnionObtherber").GetComponent<RotObjUnionObtherber>();
        }
    }

    private void OnTriggerEnter(Collider other) {

        // ��������RotateObject���擾
        GetRotateObject(other.gameObject);
    }

    // ��]�I�u�W�F�N�g�̎擾
    private void GetRotateObject(GameObject obj) {

        if (obj.transform.root.gameObject.tag == "RotateObject") {

             var _rotObj = obj.transform.root.gameObject;

            // �I�u�U�[�o�[�ɑf�ނ𑗂�
            _obtherber.AddunionMaterial(_rotObj);
        }
    }
}