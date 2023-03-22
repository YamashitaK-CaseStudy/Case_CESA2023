using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactUnion : MonoBehaviour {

    private ContactObtherber _obtherber;
    private MeshRenderer _mesh;

    void Start() {

        // オブザーバーオブジェクトを取得する
        _obtherber = GameObject.Find("ContactObtherberObj").GetComponent<ContactObtherber>();

        // メッシュレンダーを取得する
        _mesh = this.transform.parent.parent.Find("Mesh").GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other) {

        // 自身の最上位の親を取得
        GameObject parentobj =  this.transform.root.gameObject;

        if (other.gameObject.name == "BaceCollision") {
            if (parentobj.name != other.transform.root.name) {

                Debug.Log(other.name);

                // 自身の色を変更する
                _mesh.material.color = Color.black;

                _obtherber.ContactCall(other.transform.root.gameObject);
            }
        }
    }
}
