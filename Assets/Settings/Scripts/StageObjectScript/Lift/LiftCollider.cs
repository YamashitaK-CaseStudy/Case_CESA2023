using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftCollider : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerStay(Collider other) {

        var myobj = this.transform.root.gameObject;
        var youObj = other.transform.root.gameObject;

        // 自身がリフトで相手が回転オブジェクトである場合
        if (myobj.tag == "Lift" && youObj.tag == "RotateObject") {

            // リフトの情報を相手の回転オブジェクトに伝える
            youObj.GetComponent<LiftWithMoveInfo>().LiftObj = myobj;

            // リフトに回転オブジェクトを追加する
            myobj.GetComponent<Lift>().AddMoveRotObj(youObj);
        }

        // 自身が回転オブジェクトで相手が回転オブジェクトである場合
        else if (myobj.tag == "RotateObject" && youObj.tag == "RotateObject") {

            // 自分がリフトの情報を持っていたら回転オブジェクトに伝える
            var myhavelift = myobj.GetComponent<LiftWithMoveInfo>().LiftObj;
            if (myhavelift != null) {
                youObj.GetComponent<LiftWithMoveInfo>().LiftObj = myhavelift;
                myhavelift.GetComponent<Lift>().AddMoveRotObj(youObj);
            }
        }

        // 自身が回転オブジェクトで相手が不動オブジェクトである場合
        else if (myobj.tag == "RotateObject" && youObj.name == "Floor") {

            // 自身が不動オブジェクトにあたったら止めるフラグを立てる
            myobj.GetComponent<LiftWithMoveInfo>().IsStopMove = true;
            Debug.Log("自身が回転オブジェクトで相手が不動オブジェクトである場合");
        }

        // この状態はあまりない
        else if (myobj.tag == "Lift" && youObj.name == "Floor") {

        }
    }
}
