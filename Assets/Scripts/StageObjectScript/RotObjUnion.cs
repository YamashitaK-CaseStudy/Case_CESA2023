using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotObjUnion : MonoBehaviour{

    bool _isStartDoOnce = false;
    RotObjUnionObtherber _obtherber = null;
    RotObjObserver _obtherberRot = null;
    GameObject _contactObj = null;
    [SerializeField]private Collider _unionColliderX;
    [SerializeField]private Collider _unionColliderY;
    [SerializeField]private Collider _unionColliderZ;
    private void Start() {
        if (!_isStartDoOnce) {
            _isStartDoOnce = true;
            var tmpobs = GameObject.FindGameObjectWithTag("Observer");
            _obtherber = tmpobs.GetComponent<RotObjUnionObtherber>();
            _obtherberRot = tmpobs.GetComponent<RotObjObserver>();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.root.gameObject.tag != "RotateObject") return;
        if(this.transform.root.gameObject.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
        var otherComp = other.transform.root.gameObject.transform.GetComponent<RotatableObject>();
        if(this.transform.root.gameObject.transform.GetComponent<RotatableObject>()._isRotating)return;
        if(this.transform.root.gameObject != _obtherberRot.GetLastRotateRotObj().gameObject)return;
        SetEffectData(other.gameObject);
        if(otherComp._isUnion)return;

        Debug.Log("このオブジェクト送られた:" + this.transform.root.gameObject.name);
        Debug.Log("自分オブジェクト送られた:" + other.transform.root.gameObject.name);
        //　自分が回転している時だけ送る
        _obtherber.AddUnionChildObj(other.transform.root.gameObject);
        _obtherber.SetUnionParentObj(this.transform.root.gameObject);
        Debug.Log("あああ");
        otherComp._isUnion = true;
    }

    // private void OnTriggerStay(Collider other) {
    //     if(other.transform.root.tag != "RotateObject") return;
    //     if(other.transform.root.gameObject.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
    //     // if(other.transform.root.GetComponent<RotatableObject>()._isRotating) return;
    //     Debug.Log(other.name);
    //     _parentRotObjComp.PreparationUinon();
    //     GetRotateObject(other.gameObject);
    //     Debug.Log("ĚüéĹ");
    // }

    // ń]IuWFNgĚćž
    // private void GetRotateObject(GameObject obj) {
    //     Debug.Log(this.name);
    //     Debug.Log(obj.name);
    //     if (obj.transform.root.gameObject.tag == "RotateObject") {
    //         var rotcomp = obj.transform.root.gameObject.GetComponent<RotatableObject>();

    //         var _rotObj = obj.transform.root.gameObject;

    //         // IuU[o[ÉfŢđé
    //         //_obtherber.AddUnionChildObj(_rotObj);
    //         if(obj.transform.root.gameObject.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
    //         EffectGenerate(obj.gameObject);
    //         GameSoundManager.Instance.PlayGameSE(GameSESoundData.GameSE.Magnet);
    //     }
    // }

    public void StartUpFX(){
        if(_contactObj == null) return;
        Debug.Log("ああああ");
        EffectGenerate(_contactObj);
        _contactObj = null;
    }

    private void SetEffectData(GameObject contactObj){
        Debug.Log("データ格納");
        _contactObj = contactObj;
    }

    public void SetUnionCollider(bool flg){
        _unionColliderX.enabled = flg;
        _unionColliderY.enabled = flg;
        _unionColliderZ.enabled = flg;
    }
}
