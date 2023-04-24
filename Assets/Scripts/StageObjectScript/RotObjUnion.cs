using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotObjUnion : MonoBehaviour{

    static bool _isStartDoOnce = false;
    static RotObjUnionObtherber _obtherber = null;

    private void Start() {

        if (!_isStartDoOnce) {
            _isStartDoOnce = true;
            _obtherber = GameObject.FindGameObjectWithTag("UnionObtherber").GetComponent<RotObjUnionObtherber>();
        }
    }

    private void OnTriggerEnter(Collider other) {

        // 当たったRotateObjectを取得
        GetRotateObject(other.gameObject);
        EffectGenerate(other.gameObject);
    }

    // 回転オブジェクトの取得
    private void GetRotateObject(GameObject obj) {

        if (obj.transform.root.gameObject.tag == "RotateObject") {

             var _rotObj = obj.transform.root.gameObject;

            // オブザーバーに素材を送る
            _obtherber.AddunionMaterial(_rotObj);
        }
    }
}
