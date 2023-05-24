using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotObjUnion : MonoBehaviour{

    static bool _isStartDoOnce = false;
    static RotObjUnionObtherber _obtherber = null;
    private RotatableObject _parentRotObjComp = null;
    [SerializeField]private Collider _unionColliderX;
    [SerializeField]private Collider _unionColliderY;
    [SerializeField]private Collider _unionColliderZ;
    private void Start() {

        if (!_isStartDoOnce) {
            _isStartDoOnce = true;
            _obtherber = GameObject.FindGameObjectWithTag("Observer").GetComponent<RotObjUnionObtherber>();
        }

        _parentRotObjComp = this.transform.root.gameObject.GetComponent<RotatableObject>();
    }

    private void OnTriggerEnter(Collider other) {
        // 当たったRotateObjectを取得
        GetRotateObject(other.gameObject);

        if(other.transform.root.gameObject.tag != "RotateObject") return;
        if(this.transform.root.gameObject.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
        if(_parentRotObjComp._isUnion)return;
        _parentRotObjComp._isUnion = true;
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag != "RotateObject") return;
        if(other.gameObject.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
        _parentRotObjComp.PreparationUinon();
        GetRotateObject(other.gameObject);
        Debug.Log("合体処理入るで");
    }

    // 回転オブジェクトの取得
    private void GetRotateObject(GameObject obj) {

        if (obj.transform.root.gameObject.tag == "RotateObject") {

            var rotcomp = obj.transform.root.gameObject.GetComponent<RotatableObject>();

            var _rotObj = obj.transform.root.gameObject;

            // オブザーバーに素材を送る
            _obtherber.AddunionMaterial(_rotObj);
            Debug.Log(obj);
            EffectGenerate(obj.gameObject);
            GameSoundManager.Instance.PlayGameSE(GameSESoundData.GameSE.Magnet);
        }
    }

    public void SetUnionCollider(bool flg){
        _unionColliderX.enabled = flg;
        _unionColliderY.enabled = flg;
        _unionColliderZ.enabled = flg;
    }

    // 合体が終了したことを知らせる
    public void FinishUnion(){

    }
}
