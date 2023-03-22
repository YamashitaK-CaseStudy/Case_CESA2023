using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactUnion : MonoBehaviour {

    private ContactObtherber _obtherber;

    void Start() {

        // �I�u�U�[�o�[�I�u�W�F�N�g���擾����
        _obtherber = GameObject.Find("ContactObtherberObj").GetComponent<ContactObtherber>();
    }

    void Update() {
    }

    private void OnTriggerEnter(Collider other) {

        // ����������]�I�u�W�F�N�g�̈�ԏ�̐e���擾����
        GameObject parentobject = other.transform.root.gameObject;

        if (parentobject.tag == "RotateObject") {

            other.transform.parent.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.black;
            _obtherber.ContactCall(parentobject);
        }
    }
}
