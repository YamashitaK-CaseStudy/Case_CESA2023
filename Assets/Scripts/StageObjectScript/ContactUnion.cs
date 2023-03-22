using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactUnion : MonoBehaviour {

    private ContactObtherber _obtherber;
    private MeshRenderer _mesh;

    void Start() {

        // �I�u�U�[�o�[�I�u�W�F�N�g���擾����
        _obtherber = GameObject.Find("ContactObtherberObj").GetComponent<ContactObtherber>();

        // ���b�V�������_�[���擾����
        _mesh = this.transform.parent.parent.Find("Mesh").GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other) {

        // ���g�̍ŏ�ʂ̐e���擾
        GameObject parentobj =  this.transform.root.gameObject;

        if (other.gameObject.name == "BaceCollision") {
            if (parentobj.name != other.transform.root.name) {

                Debug.Log(other.name);

                // ���g�̐F��ύX����
                _mesh.material.color = Color.black;

                _obtherber.ContactCall(other.transform.root.gameObject);
            }
        }
    }
}
