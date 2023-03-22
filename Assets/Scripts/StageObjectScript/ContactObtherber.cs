using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class ContactObtherber : MonoBehaviour
{
    private List<GameObject> _unionmaterials = new List<GameObject>();
    Vector3 localpos = new Vector3(0, 0, 0);
    Vector3 selectpos = new Vector3(0, 0, 0);

    void Start() {
    }

    void Update(){

        var _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player._selectGameObject.GetComponent<RotatableObject>()._isRotating == false) {
            selectpos = _player._selectGameObject.transform.position;
            localpos = _player._selectGameObject.GetComponent<RotatableObject>()._axisCenterLocalPos;
        }

        // ２つ以上合体するオブジェクトがリストの中にあれば実行する
        if (2 <= _unionmaterials.Count) {

            // 新しくオブジェクトを作成する
            GameObject unionobj = Instantiate((GameObject)Resources.Load("Pf_RootObj"), selectpos, Quaternion.identity);

            unionobj.tag = "RotateObject";
            unionobj.transform.position = localpos + selectpos;

            foreach (var obj in _unionmaterials) {

                // 作成した親に子供としてリストの中のゲームオブジェクトアタッチする
                obj.transform.SetParent(unionobj.transform);

                // タグを付け替える
                obj.tag = "Untagged";
            }

            // リストの中身を空にする
            _unionmaterials.Clear();
        }
    }

    public void ContactCall(in GameObject collision) {

        if(_unionmaterials.Contains(collision) == false) {
            _unionmaterials.Add(collision);
        }
    }
}
