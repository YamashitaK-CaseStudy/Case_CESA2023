using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class LiftSwich : MonoBehaviour {

    [SerializeField, Header("アニメータ")] private Animator _animator;
    [SerializeField, Header("管理するリフト")] private Lift _lift;

    private bool IsLiftUpdate = false;
    private LiftMove _liftMove;
    private CinemachineVirtualCamera _vCam;

    public Lift GetLift {
        get {return _lift; }
    }

    public enum LiftMove {
        Up, Down
    }

    private void Start() {

        _vCam = _lift.gameObject.GetComponent<LIftCamera>().GetVirtualCamera;
    }


    // 当たり判定の有効化・無効化
    [System.Obsolete]
    public void AllRotObjColliderChnage(LiftMove move) {

        // 全回転オブジェクト取得
        var _rotObj = GameObject.FindGameObjectsWithTag("RotateObject");

        // リフトで扱う当たり判定を取得
        foreach (var obj in _rotObj) {
            LiftColliderChange(obj, move);
        }

        LiftColliderChange(_lift.gameObject, move);
    }

    // 全リフトの当たり判定の取得
    void LiftColliderChange(GameObject obj, LiftMove move) {

        Transform children = obj.GetComponentInChildren<Transform>();
        //子要素がいなければ終了
        if (children.childCount == 0) {
            return;
        }
        foreach (Transform ob in children) {

            if (ob.name == "Pf_LiftCollider") {
                ChangeCollider(ob.gameObject, move);
            }

            LiftColliderChange(ob.gameObject, move);
        }
    }

    private void ChangeCollider(GameObject obj, LiftMove move) {

        for (int i = 0; i < obj.transform.childCount; i++) {
            var collider = obj.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
        }

        // 基準をあらかじめ入れておく
        var Collider1 = obj.transform.GetChild(0).gameObject.transform.position.y;
        var Collider2 = obj.transform.GetChild(1).gameObject.transform.position.y;
        var Collider3 = obj.transform.GetChild(2).gameObject.transform.position.y;
        var Collider4 = obj.transform.GetChild(3).gameObject.transform.position.y;

        float posValue = 0.0f;

        if (move == LiftMove.Up) {
            posValue = Mathf.Max(Collider1, Collider2, Collider3, Collider4);
        }
        else if (move == LiftMove.Down) {
            posValue = Mathf.Min(Collider1, Collider2, Collider3, Collider4);
        }

        for (int i = 0; i < obj.transform.childCount; i++) {
            var collider = obj.transform.GetChild(i).gameObject;

            if(Math.Round(posValue, 1, MidpointRounding.AwayFromZero) != Math.Round(collider.transform.position.y, 1, MidpointRounding.AwayFromZero)) {
                collider.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    // リフトの稼働時当たり判定の都合xフレーム待つ
    public IEnumerator WaitProces() {

        // 1フレーム処理を待ちます。
        yield return null;
        _vCam.Priority = 100;

        // 1フレーム処理を待ちます。
        yield return new WaitForSeconds(1.0f);

        _lift.LiftAction(_lift.GetLiftMove);
    }
}
