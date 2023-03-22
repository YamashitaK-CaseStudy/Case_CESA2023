using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactUnion : MonoBehaviour {

    private ContactObtherber _obtherber;

    void Start() {

        // オブザーバーオブジェクトを取得する
        _obtherber = GameObject.Find("ContactObtherberObj").GetComponent<ContactObtherber>();
    }

    void Update() {
    }

    private void OnTriggerEnter(Collider other) {

        // 当たった回転オブジェクトの一番上の親を取得する
        GameObject parentobject = other.transform.root.gameObject;

        if (parentobject.tag == "RotateObject") {

            other.transform.parent.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.black;
            _obtherber.ContactCall(parentobject);
        }
    }
}
