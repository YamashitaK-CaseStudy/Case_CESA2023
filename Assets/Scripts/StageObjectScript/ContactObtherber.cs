using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class ContactObtherber : MonoBehaviour
{
    private List<GameObject> _unionmaterials = new List<GameObject>();
    private GameObject _selectObj;

    void Start() {
    }

    void Update(){

        // ２つ以上合体するオブジェクトがリストの中にあれば実行する
        if (2 <= _unionmaterials.Count) {

            // プレイヤーの選択していたオブジェクトを取得
            _selectObj = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()._selectGameObject;

            // 新しくオブジェクトを作成する
            GameObject unionobj = Instantiate((GameObject)Resources.Load("Pf_UnionObj"), _selectObj.transform.position, Quaternion.identity);

            unionobj.tag = "RotateObject";
            unionobj.transform.position = _selectObj.transform.position;

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
